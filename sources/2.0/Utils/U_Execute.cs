using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.Xml;
using System.Xml.Linq;
using Livrable2.Properties;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NSModel;
using NSServer;
using NSViewModel;
using static System.Windows.Forms.AxHost;
using Formatting = Newtonsoft.Json.Formatting;
using MessageBox = System.Windows.Forms.MessageBox;

namespace NSUtils;

public class U_Execute
{
    [ThreadStatic] private static List<string>? prio;
    [ThreadStatic] private static List<string>? pasprio;
    [ThreadStatic] private static int total;
    [ThreadStatic] private static int threadIndex;
    [ThreadStatic] private static string fileName;
    [ThreadStatic] private static string targetFilePath;
    [ThreadStatic] private static string directoryTarget;
    [ThreadStatic] private static int NbFilesLeftToDo;
    private readonly List<bool> pasprioencour = new();

    public List<int> indexes { get; } = new();

    public void StartThread(M_SaveJob _oSaveJobs, M_Model _oModel, VM_ViewModel _oViewModel, Server server)
    {
        List<string> tempPrio = new();
        List<string>? tempNotPrio = new();

        foreach (var dirPath in Directory.GetDirectories(_oSaveJobs.Get_saveJobSourceDirectory(), "*",
                     SearchOption.AllDirectories))
            Directory.CreateDirectory(dirPath.Replace(_oSaveJobs.Get_saveJobSourceDirectory(),
                _oSaveJobs.Get_saveJobDestinationDirectory()));
        foreach (var file in Directory.GetFiles(_oSaveJobs.Get_saveJobSourceDirectory(), "*.*",
                     SearchOption.AllDirectories))
            if (_oModel._extensionPriorityRegex.IsMatch(file))
                tempPrio.Add(file);
            else
                tempNotPrio.Add(file);


        pasprioencour.Add(tempPrio.Count == 0);
        var threadIndex = pasprioencour.Count - 1;
        _oSaveJobs._state = "active";
        _oSaveJobs._progress = 0;
        _oSaveJobs.resumeThread();
        var t = new Thread(() =>
        {
            ThreadContent(_oViewModel, _oSaveJobs, _oModel, tempPrio, tempNotPrio, threadIndex, server);
        });
        t.Start();
        _oSaveJobs.RunningThread = t;

        indexes.Add(_oSaveJobs.Get_index());
        // send notification to enable the pause button
        Application.Current.Dispatcher.BeginInvoke(() => { _oViewModel.setupObsCollection(); });
    }

    public void ThreadContent(VM_ViewModel _oViewModel, M_SaveJob _oSaveJobs, M_Model _oModel, List<string> prioEntrant,
        List<string> pasprioEntrant, int index, Server server)
    {
        void progessCalc(int total, int prioEntrant, int pasprioEntrant)
        {
            //only multiply by 10 to reduce the number of refreshes
            var p = (int)Math.Round((total - (float)(prioEntrant + pasprioEntrant)) / total * 10f);

            if (_oSaveJobs._progress < p * 10)
            {
                _oSaveJobs.Set_progress(p * 10);
                Application.Current.Dispatcher.BeginInvoke(() =>
                {
                    _oViewModel.setupObsCollection();
                    if (server.Get_socket() != null)
                    {
                        server.SendProgressToClient(_oSaveJobs.Get_index(), _oSaveJobs.Get_progress(),
                            _oSaveJobs.Get_state());
                    }
                });
            }
        }

        bool copy(string newPath)
        {
            var proceed = true;

            var noExecutionIfRunning = "CalculatorApp";
            var processes = Process.GetProcesses();
            foreach (var process in processes)
            {
                if (noExecutionIfRunning == process.ProcessName)
                {
                    proceed = false;
                }
            }

            if (proceed)
            {
                var state = "active";
                Debug.WriteLine(_oSaveJobs.ThreadPaused);
                if (!_oSaveJobs.ThreadPaused.WaitOne(0))
                {
                    _oSaveJobs.WriteJSON(_oModel.Get_workFile(), "paused", NbFilesLeftToDo, _oSaveJobs._progress);
                    Application.Current.Dispatcher.BeginInvoke(() => { _oViewModel.setupObsCollection(); });
                    _oSaveJobs.ThreadPaused.WaitOne();
                }


                var startCopyTime = DateTime.Now;

                fileName = Path.GetFileName(newPath);
                targetFilePath = newPath.Replace(_oSaveJobs.Get_saveJobSourceDirectory(),
                    _oSaveJobs.Get_saveJobDestinationDirectory());
                directoryTarget = targetFilePath.Replace(fileName, null);


                try
                {
                    if (_oModel._extensionToCryptRegex.IsMatch(newPath))
                    {
                        var myProcess = Process.Start("Resources\\Cryptosoft.exe",
                            newPath + " " + targetFilePath + " " + "azertyui");
                        myProcess.WaitForExit();
                        myProcess.Close();
                    }
                    else
                    {
                        File.Copy(newPath, targetFilePath, _oSaveJobs.Get_saveJobType() == 1);
                    }

                    NbFilesLeftToDo -= 1;
                    progessCalc(total, prioEntrant.Count, pasprioEntrant.Count);
                    _oSaveJobs.WriteJSON(_oModel.Get_workFile(), state, NbFilesLeftToDo,
                        _oSaveJobs.Get_progress());
                    var endCopyTime = DateTime.Now;
                    var copyTime = endCopyTime - startCopyTime;
                    WriteLog(_oModel, _oSaveJobs.Get_saveJobName(), _oModel.Get_logFile(), fileName, newPath,
                        targetFilePath, directoryTarget, copyTime);
                    return true;
                }
                catch (UnauthorizedAccessException e)
                {
                    Debug.WriteLine("permission issue " + e);
                    return true;
                }
                catch (Newtonsoft.Json.JsonReaderException e)
                {
                    Debug.WriteLine("log issue :" + e);
                    return true;
                }
                catch (Exception e)
                {
                    Debug.WriteLine("unknown issue :" + e);
                    return true;
                }
            }
            else
            {
                _oSaveJobs._state = $"{Resources.pleaseCloseCalculatator}";
                Application.Current.Dispatcher.BeginInvoke(() => { _oViewModel.setupObsCollection(); });
                return false;
                //MessageBox.Show($"{Resources.pleaseCloseCalculatator}");
            }
        }

        prio = prioEntrant;
        pasprio = pasprioEntrant;
        threadIndex = index;
        total = prioEntrant.Count + pasprioEntrant.Count;
        NbFilesLeftToDo = total;

        try
        {
            while (NbFilesLeftToDo > 0)
            {
                while (!pasprioencour.ToArray().All(e => e))
                {
                    var test = !pasprioencour.ToArray().All(e => e);
                    foreach (var s in prio.ToArray())
                    {
                        if (copy(s))
                        {
                            prio.Remove(s);
                        }
                    }

                    if (prio.Count == 0) pasprioencour[threadIndex] = true;
                }

                while (pasprio.Count > 0)
                {
                    foreach (var s in pasprio.ToArray())
                    {
                        if (copy(s))
                        {
                            pasprio.Remove(s);
                        }
                    }

                    if (!pasprioencour.ToArray().All(e => e)) break;
                }
            }

            progessCalc(total, prioEntrant.Count, pasprioEntrant.Count);
            _oSaveJobs.WriteJSON(_oModel.Get_workFile(), "done", NbFilesLeftToDo, _oSaveJobs.Get_progress());
        }
        catch (ThreadInterruptedException e)
        {
            _oSaveJobs.WriteJSON(_oModel.Get_workFile(), "aborted", NbFilesLeftToDo, 0);
            prio.Clear();
            pasprio.Clear();
        }

        Application.Current.Dispatcher.BeginInvoke(() => { _oViewModel.setupObsCollection(); });
        _oSaveJobs.RunningThread = null;
    }


    public void WriteLog(M_Model M, string JobName, string JsonLogPath, string fileName, string fileSourcePath,
        string fileDestPath, string directoryTarget, TimeSpan copyTime)
    {
        var pathDirectoryLog = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\EasySave\Log";
        Directory.CreateDirectory(pathDirectoryLog);
        Directory.CreateDirectory(pathDirectoryLog + @"\" + DateTime.Now.ToString("ddMMyyyy"));
        var logFileName = @"\" + DateTime.Now.ToString("ddMMyyyy") + @$"\log_{JobName.Replace(" ", "_")}" + ".json";
        var logXmlFileName = pathDirectoryLog + logFileName.Replace(".json", ".xml");

        var pathLog = pathDirectoryLog + logFileName;

        if (!File.Exists(logXmlFileName))
            using (var X = XmlWriter.Create(logXmlFileName, new XmlWriterSettings { Indent = true }))
            {
                X.WriteStartElement("logs");
                X.WriteEndElement();
                X.Flush();
            }

        M.Set_logFile(pathLog);
        if (!File.Exists(M.Get_logFile()))
        {
            var initLogFile = "{\n\t\"logs\": []\n}";
            File.WriteAllText(M.Get_logFile(), initLogFile);
        }


        long size;
        //Get fileinfo
        try
        {
            var fileInfo = new FileInfo(fileSourcePath);
            size = fileInfo.Length;
        }
        catch (Exception)
        {
            var fileInfo = new FileInfo(fileSourcePath + "\\" + fileName);
            size = fileInfo.Length;
        }

        //Get JSON file's content
        var allLog = JObject.Parse(File.ReadAllText(pathLog));

        var sb = new StringBuilder();
        var sw = new StringWriter(sb);
        JsonWriter writer = new JsonTextWriter(sw)
        {
            Formatting = Formatting.Indented
        };
        writer.WriteStartObject();
        writer.WritePropertyName("Name");
        writer.WriteValue(fileName);
        writer.WritePropertyName("FileSource");
        writer.WriteValue(fileSourcePath);
        writer.WritePropertyName("FileTarget");
        writer.WriteValue(fileDestPath);
        writer.WritePropertyName("destPath");
        writer.WriteValue(directoryTarget);
        writer.WritePropertyName("FileSize");
        writer.WriteValue(size);
        writer.WritePropertyName("FileTransferTime");
        writer.WriteValue(copyTime);
        writer.WritePropertyName("DateTime");
        writer.WriteValue(DateTime.Now.ToString("HH:mm:ss dd/MM/yyyy"));
        writer.WriteEndObject();

        //Convert object JsonWriter to string
        var json = sb.ToString();

        //Convert string to JObject
        var newLog = JObject.Parse(json);

        //Get JObject "logs" of Json Lof file
        var arrayLogs = (JArray)allLog["logs"];

        //Add newLog to allLog
        arrayLogs.Add(newLog);

        //Convert object JObject to string
        var newLogFile = allLog.ToString();

        //Write newLogFile string to log JSON file
        File.WriteAllText(JsonLogPath, newLogFile);


        //Write log to xml file
        var xmlDoc = XDocument.Load(pathLog.Replace(".json", ".xml"));
        var parentElement = new XElement("log");
        parentElement.Add(new XElement("Name", $"{fileName}"));
        parentElement.Add(new XElement("FileSource", $"{fileSourcePath}"));
        parentElement.Add(new XElement("FileTarget", $"{fileDestPath}"));
        parentElement.Add(new XElement("DestPath", $"{directoryTarget}"));
        parentElement.Add(new XElement("FileSize", $"{size}"));
        parentElement.Add(new XElement("FileTransferTime", $"{copyTime}"));
        parentElement.Add(new XElement("DateTime", DateTime.Now.ToString("HH:mm:ss dd/MM/yyyy")));

        var rootElement = xmlDoc.Element("logs");
        rootElement?.Add(parentElement);

        xmlDoc.Save(pathLog.Replace(".json", ".xml"));
    }
}
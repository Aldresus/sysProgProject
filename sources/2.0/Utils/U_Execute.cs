﻿using Livrable2.Properties;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NSModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using System.Xml.Linq;
using static System.Windows.Forms.AxHost;
using Livrable2;

namespace NSUtils
{
    public class U_Execute
    {
        
        [ThreadStatic] static List<string>? prio;
        [ThreadStatic] static List<string>? pasprio;
        [ThreadStatic] static int total;
        [ThreadStatic] static int threadIndex;
        [ThreadStatic] static string fileName;
        [ThreadStatic] static string targetFilePath;
        [ThreadStatic] static string directoryTarget;
        [ThreadStatic] static int NbFilesLeftToDo;
        List<int> progress = new() { };
        public List<int> indexes { get; } = new() { };
        List<bool> pasprioencour = new() { };
        public List<Thread> threads { get; set; } = new();

        public U_Execute()
        {
        }
        public void StartThread(M_SaveJob _oSaveJobs, M_Model _oModel)
        {

            bool proceed = true;
            string noExecutionIfRunning = "CalculatorApp";
            var processes = System.Diagnostics.Process.GetProcesses();
            foreach (var process in processes)
            {
                if (noExecutionIfRunning == process.ProcessName)
                {
                    proceed = false;
                }
            }
            if (proceed)
            {


                List<string> tempPrio = new() { };
                List<string>? tempNotPrio = new() { };

                foreach (string dirPath in Directory.GetDirectories(_oSaveJobs.Get_saveJobSourceDirectory(), "*", SearchOption.AllDirectories))
                {
                    Directory.CreateDirectory(dirPath.Replace(_oSaveJobs.Get_saveJobSourceDirectory(), _oSaveJobs.Get_saveJobDestinationDirectory()));
                }
                foreach (string file in Directory.GetFiles(_oSaveJobs.Get_saveJobSourceDirectory(), "*.*", SearchOption.AllDirectories))
                {
                    if (_oModel._extensionPriorityRegex.IsMatch(file))
                    {
                        tempPrio.Add(file);
                    }
                    else
                    {
                        tempNotPrio.Add(file);
                    }
                }


                pasprioencour.Add(tempPrio.Count == 0);
                int threadIndex = pasprioencour.Count - 1;

                var t = new Thread(() => { ThreadContent(_oSaveJobs, _oModel, tempPrio, tempNotPrio, threadIndex); });
                t.Start();
                threads.Add(t);
                progress.Add(0);
                indexes.Add(_oSaveJobs.Get_index());
            }
            else
            {
                System.Windows.Forms.MessageBox.Show($"{Resources.pleaseCloseCalculatator}");
            }


        }

        public void ThreadContent(M_SaveJob _oSaveJobs, M_Model _oModel, List<string> prioEntrant, List<string> pasprioEntrant, int index)
        {

            static int progessCalc(int total, int prioEntrant, int pasprioEntrant)
            {
                
                return (int)Math.Round(((float)total - (float)(prioEntrant + pasprioEntrant)) / (float)total * 100f);
            }
            void copy(string newPath)
            {
                string state = "active";
                DateTime startCopyTime = DateTime.Now;

                fileName = Path.GetFileName(newPath);
                targetFilePath = newPath.Replace(_oSaveJobs.Get_saveJobSourceDirectory(), _oSaveJobs.Get_saveJobDestinationDirectory());
                directoryTarget = targetFilePath.Replace(fileName, null);



                if (_oModel._extensionToCryptRegex.IsMatch(newPath))
                {
                    Process myProcess = Process.Start("Resources\\Cryptosoft.exe", newPath + " " + targetFilePath + " " + "azertyui");
                    myProcess.WaitForExit();
                    myProcess.Close();
                }
                else
                {
                    File.Copy(newPath, targetFilePath, _oSaveJobs.Get_saveJobType() == 1);
                }
                NbFilesLeftToDo -= 1;
                progress[threadIndex] = progessCalc(total, prioEntrant.Count, pasprioEntrant.Count);
                _oSaveJobs.WriteJSON(_oModel.Get_workFile(), state, NbFilesLeftToDo, progress[threadIndex]);
                DateTime endCopyTime = DateTime.Now;
                TimeSpan copyTime = endCopyTime - startCopyTime;
                WriteLog(_oModel.Get_logFile(), fileName, newPath, targetFilePath, directoryTarget, copyTime);
                
            }


            prio = prioEntrant;
            pasprio = pasprioEntrant;
            threadIndex = index;
            total = prioEntrant.Count + pasprioEntrant.Count;
            NbFilesLeftToDo = total;


            while (NbFilesLeftToDo>0)
            {

                while (!pasprioencour.ToArray().All((e) => e))
                {

                    bool test = !pasprioencour.ToArray().All((e) => e);
                    foreach (var s in prio.ToArray())
                    {
                        copy(s);
                        prio.Remove(s);

                    }
                    if (prio.Count == 0)
                    {
                        pasprioencour[threadIndex] = true;
                    }
                }
                while (pasprio.Count > 0)
                {
                    foreach (var s in pasprio.ToArray())
                    {
                        copy(s);
                        pasprio.Remove(s);
                    }
                    if (!pasprioencour.ToArray().All((e) => e))
                    {
                        break;
                    }
                }

            }
            _oSaveJobs.WriteJSON(_oModel.Get_workFile(), "inactive", NbFilesLeftToDo, progress[threadIndex]);

        }

        public void Execute(M_SaveJob SaveJob, string FileLogPath, string FileStatePath)
        {

            /*var t = new Thread(() =>
            {
                string sourcePath = SaveJob.Get_saveJobSourceDirectory();
                string targetPath = SaveJob.Get_saveJobDestinationDirectory();
                bool isFullSave = (SaveJob.Get_saveJobType() == 1) ? true : false;

                // Check if the source directory exists.
                if (System.IO.Directory.Exists(sourcePath))
                {
                    //Now Create all of the directories


                    //Copy all the files & Replaces any files with the same name

                    foreach (string newPath in AllFiles)
                    {

                    }
                    
                }
            });
            t.Start();
            System.Windows.Forms.MessageBox.Show($"{Resources.executed}");*/
        }

        public void WriteLog(string JsonLogPath, string fileName, string fileSourcePath, string fileDestPath, string directoryTarget, TimeSpan copyTime)
        {
            long size;
            //Get fileinfo
            try
            {
                FileInfo fileInfo = new FileInfo(fileSourcePath);
                size = fileInfo.Length;
            }
            catch (Exception)
            {
                FileInfo fileInfo = new FileInfo(fileSourcePath + "\\" + fileName);
                size = fileInfo.Length;
            }
            //Get JSON file's content
            JObject allLog = JObject.Parse(File.ReadAllText(JsonLogPath));

            StringBuilder sb = new StringBuilder();
            StringWriter sw = new StringWriter(sb);
            JsonWriter writer = new JsonTextWriter(sw)
            {
                Formatting = Newtonsoft.Json.Formatting.Indented
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
            string json = sb.ToString();

            //Convert string to JObject
            JObject newLog = JObject.Parse(json);

            //Get JObject "logs" of Json Lof file
            JArray arrayLogs = (JArray)allLog["logs"];

            //Add newLog to allLog
            arrayLogs.Add(newLog);

            //Convert object JObject to string
            string newLogFile = allLog.ToString();

            //Write newLogFile string to log JSON file
            File.WriteAllText(JsonLogPath, newLogFile);


            //Write log to xml file
            var xmlDoc = XDocument.Load(JsonLogPath.Replace(".json", ".xml"));
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

            xmlDoc.Save(JsonLogPath.Replace(".json", ".xml"));
        }
    }
}
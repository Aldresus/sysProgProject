﻿using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NSModel;
using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows;
using System.Xml.Linq;

namespace NSUtils
{
    public class U_Execute
    {
        public M_Model _oModel;

        public U_Execute(M_Model M)
        {
            this._oModel = M;
        }

        public void Execute(M_SaveJob SaveJob, string FileLogPath, string FileStatePath)
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
                string sourcePath = SaveJob.Get_saveJobSourceDirectory();
                string targetPath = SaveJob.Get_saveJobDestinationDirectory();
                bool isFullSave = (SaveJob.Get_saveJobType() == 1) ? true : false;

                // Check if the source directory exists.
                if (System.IO.Directory.Exists(sourcePath))
                {
                    string state = "active";
                    int total = SaveJob.Get_totalNbFile();
                    int NbFilesLeftToDo = total;
                    float progress = 0;
                    //Now Create all of the directories
                    foreach (string dirPath in Directory.GetDirectories(sourcePath, "*", SearchOption.AllDirectories))
                    {
                        Directory.CreateDirectory(dirPath.Replace(sourcePath, targetPath));
                    }

                    //Copy all the files & Replaces any files with the same name
                    foreach (string newPath in Directory.GetFiles(sourcePath, "*.*", SearchOption.AllDirectories))
                    {
                        DateTime startCopyTime = DateTime.Now;
                        if (_oModel._extensionToCryptRegex.IsMatch(newPath))
                        {
                            Process myProcess = Process.Start("Resources\\Cryptosoft.exe", newPath + " " + newPath.Replace(sourcePath, targetPath) + " " + "azertyui");
                            myProcess.WaitForExit();
                            myProcess.Close();
                        }
                        else
                        {
                            File.Copy(newPath, newPath.Replace(sourcePath, targetPath), isFullSave);
                        }
                        string fileName = newPath.Replace(sourcePath, null);
                        NbFilesLeftToDo -= 1;
                        progress = (int)Math.Round((((float)total - (float)NbFilesLeftToDo) / (float)total) * 100.0f);
                        SaveJob.WriteJSON(FileStatePath, state, NbFilesLeftToDo, (int)progress);
                        DateTime endCopyTime = DateTime.Now;
                        TimeSpan copyTime = endCopyTime - startCopyTime;
                        WriteLog(FileLogPath, fileName, newPath, newPath.Replace(sourcePath, targetPath), sourcePath, copyTime);
                    }
                    state = "inactive";
                    SaveJob.WriteJSON(FileStatePath, state, NbFilesLeftToDo, (int)progress);
                }

            }
            else
            {
                MessageBox.Show($"{noExecutionIfRunning} is running and forbids execution.");
            }

        }

        public void WriteLog(string JsonLogPath, string fileName, string fileSourcePath, string fileDestPath, string directorySource, TimeSpan copyTime)
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
            JsonWriter writer = new JsonTextWriter(sw);
            writer.Formatting = Newtonsoft.Json.Formatting.Indented;
            writer.WriteStartObject();
            writer.WritePropertyName("Name");
            writer.WriteValue(fileName);
            writer.WritePropertyName("FileSource");
            writer.WriteValue(fileSourcePath);
            writer.WritePropertyName("FileTarget");
            writer.WriteValue(fileDestPath);
            writer.WritePropertyName("destPath");
            writer.WriteValue(directorySource);
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
            parentElement.Add(new XElement("DestPath", $"{directorySource}"));
            parentElement.Add(new XElement("FileSize", $"{size}"));
            parentElement.Add(new XElement("FileTransferTime", $"{copyTime}"));
            parentElement.Add(new XElement("DateTime", DateTime.Now.ToString("HH:mm:ss dd/MM/yyyy")));

            var rootElement = xmlDoc.Element("logs");
            rootElement?.Add(parentElement);

            xmlDoc.Save(JsonLogPath.Replace(".json", ".xml"));
        }
    }
}
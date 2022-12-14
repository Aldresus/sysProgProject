//Class Model
//Description : This class is used to write log file and to move files about different save.

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NSUtils;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Linq;

namespace NSModel
{
    public class M_Model
    {
        private List<M_SaveJob> _listSaveJob = new List<M_SaveJob>();
        public U_Execute utilExecute { get; } = new U_Execute();
        private string _logFile;
        private string _workFile;
        private dynamic _language;
        private List<string> _extensionToCrypt { get; set; } = new List<string>();
        private List<string> _extensionPriority { get; set; } = new List<string>();
        public Regex _extensionToCryptRegex { get; set; }
        public Regex _extensionPriorityRegex { get; set; }

        //Constructor
        public M_Model()
        {
            string pathDirectoryLog = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments).ToString() +
                                      @"\EasySave\Log";
            if (!Directory.Exists(pathDirectoryLog))
            {
                Directory.CreateDirectory(pathDirectoryLog);
            }

            string logFileName = @"\log" + DateTime.Now.ToString("ddMMyyyy") + ".json";
            string logXmlFileName = pathDirectoryLog + logFileName.Replace(".json", ".xml");

            string pathLog = pathDirectoryLog + logFileName;

            if (!File.Exists(logXmlFileName))
            {
                using (XmlWriter X = XmlWriter.Create(logXmlFileName, new XmlWriterSettings { Indent = true }))
                {
                    X.WriteStartElement("logs");
                    X.WriteEndElement();
                    X.Flush();
                }
            }

            Set_logFile(pathLog);
            if (!File.Exists(Get_logFile()))
            {
                string initLogFile = "{\n\t\"logs\": []\n}";
                File.WriteAllText(Get_logFile(), initLogFile);
            }

            string pathDirectoryState = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments).ToString() +
                                        @"\EasySave";
            string pathState = pathDirectoryState + @"\State.json";
            Set_workFile(pathState);
            if (!File.Exists(Get_workFile()))
            {
                //Write json string to JSON file
                File.WriteAllText(Get_workFile(),
                    "{\n\"lang\": \"en\",\n\"extToCrypt\": [], \n\"extPrio\": [], \n\"State\": []\n}");
            }

            JObject objJSON = JObject.Parse(File.ReadAllText(Get_workFile()));

            int identationIndex = 0;
            foreach (JObject i in objJSON["State"])
            {
                _listSaveJob.Add(new M_SaveJob(i["Name"].ToString(), i["SourceFilePath"].ToString(),
                    i["TargetFilePath"].ToString(), i["Type"].Value<int>(), i["State"].ToString(), 0, identationIndex));
                identationIndex += 1;
            }

            //Get extensions to crypt in json file
            foreach (string i in objJSON["extToCrypt"])
            {
                _extensionToCrypt.Add(i);
            }

            //Set _extensionToCryptRegex
            Set_extensionToCryptRegex();

            //Get extensions to prioritize in json file
            foreach (string y in objJSON["extPrio"])
            {
                _extensionPriority.Add(y);
            }

            //Set _extensionPrioRegex
            Set_extensionPriorityRegex();
        }

        public void WriteLanguage(string language)
        {
            JObject objJSON = JObject.Parse(File.ReadAllText(Get_workFile()));
            objJSON["lang"] = language;

            //Convert object JObject to string
            string json = objJSON.ToString();

            //Write json string to JSON file
            File.WriteAllText(Get_workFile(), json);
        }

        //Getter _listSaveJob
        public List<M_SaveJob> Get_listSaveJob()
        {
            return _listSaveJob;
        }

        //Getter selected SaveJob
        public M_SaveJob GetSelectedSaveJob(int value)
        {
            return _listSaveJob[value];
        }

        //Setter _listSaveJob
        public void Set_ListSaveJob(List<M_SaveJob> values)
        {
            _listSaveJob = values;
        }

        //Setter selected SaveJob
        public void SetSelectedSaveJob(int value, M_SaveJob saveJob)
        {
            try
            {
                _listSaveJob[value] = saveJob;
            }
            catch (Exception e)
            {
                Get_listSaveJob().Add(saveJob);
            }
        }

        //Getter _logFile
        public string Get_logFile()
        {
            return _logFile;
        }

        //Setter _logFile
        public void Set_logFile(string value)
        {
            _logFile = value;
        }

        //Getter _workFile
        public string Get_workFile()
        {
            return _workFile;
        }

        //Setter _workFile
        public void Set_workFile(string value)
        {
            _workFile = value;
        }

        //Getter _language
        public dynamic Get_language()
        {
            return _language;
        }

        //Setter _language
        public void Set_language(dynamic value)
        {
            _language = value;
        }

        //Getter _extensionToCrypt
        public List<string> Get_extensionToCrypt()
        {
            return _extensionToCrypt;
        }

        //Add _extensionToCrypt
        public void Add_extensionToCrypt(string value)
        {
            _extensionToCrypt.Add(value);
        }

        //Remove _extensionToCrypt
        public void Remove_extensionToCrypt(int index)
        {
            _extensionToCrypt.RemoveAt(index);
        }

        public void Edit_extensionToCrypt(int index, string value)
        {
            _extensionToCrypt[index] = value;
        }

        public List<string> Get_extensionPriority()
        {
            return _extensionPriority;
        }

        //Add _extensionToCrypt
        public void Add_extensionPriority(string value)
        {
            _extensionPriority.Add(value);
        }

        //Remove _extensionToCrypt
        public void Remove_extensionPriority(int index)
        {
            _extensionPriority.RemoveAt(index);
        }

        public void Edit_extensionPriority(int index, string value)
        {
            _extensionPriority[index] = value;
        }

        public void InstanceNewSaveJob(string _saveJobName, string _saveJobSourceDirectory,
            string _saveJobDestinationDirectory, int _saveJobType, string _state, int _progress, int index)
        {
            _listSaveJob.Add(new M_SaveJob(_saveJobName, _saveJobSourceDirectory, _saveJobDestinationDirectory,
                _saveJobType, "idle", _progress, index));
        }

        public void RemoveSaveJob(int index)
        {
            List<M_SaveJob> oldListSaveJob = Get_listSaveJob();
            oldListSaveJob.RemoveAt(index);
            _listSaveJob = oldListSaveJob;

            JObject objJSON = JObject.Parse(File.ReadAllText(Get_workFile()));

            JArray arrayStates = (JArray)objJSON["State"];

            arrayStates.RemoveAt(index);

            //Convert object JObject to string
            string finalState = objJSON.ToString();

            //Write json string to JSON file
            File.WriteAllText(Get_workFile(), finalState);
        }

        public void AddExtensionToCryptState(string extension)
        {
            JObject objJSON = JObject.Parse(File.ReadAllText(Get_workFile()));
            JArray arrayExtToCrypt = (JArray)objJSON["extToCrypt"];
            arrayExtToCrypt.Add(extension);
            //Convert object JObject to string
            string modifiedExtToCrypt = objJSON.ToString();

            //Write json string to JSON file
            File.WriteAllText(Get_workFile(), modifiedExtToCrypt);
        }

        public void RemoveExtensionToCryptState(int index)
        {
            JObject objJSON = JObject.Parse(File.ReadAllText(Get_workFile()));
            JArray arrayExtToCrypt = (JArray)objJSON["extToCrypt"];
            arrayExtToCrypt.RemoveAt(index);
            //Convert object JObject to string
            string modifiedExtToCrypt = objJSON.ToString();

            //Write json string to JSON file
            File.WriteAllText(Get_workFile(), modifiedExtToCrypt);
        }

        public void EditExtensionToCryptState(int index, string value)
        {
            JObject objJSON = JObject.Parse(File.ReadAllText(Get_workFile()));
            JArray arrayExtToCrypt = (JArray)objJSON["extToCrypt"];
            arrayExtToCrypt[index] = value;
            //Convert object JObject to string
            string modifiedExtToCrypt = objJSON.ToString();

            //Write json string to JSON file
            File.WriteAllText(Get_workFile(), modifiedExtToCrypt);
        }

        public void Set_extensionToCryptRegex()
        {
            string result;
            if (_extensionToCrypt.Count() > 0)
            {
                result = String.Join("|", _extensionToCrypt.ToArray());
            }
            else
            {
                result = "jesuisvide";
            }

            string regex = @$"\b({result})\b";
            _extensionToCryptRegex = new Regex(regex);
        }

        public void AddExtensionPriorityState(string extension)
        {
            JObject objJSON = JObject.Parse(File.ReadAllText(Get_workFile()));
            JArray arrayExtPrio = (JArray)objJSON["extPrio"];
            arrayExtPrio.Add(extension);
            //Convert object JObject to string
            string modifiedExtPrio = objJSON.ToString();

            //Write json string to JSON file
            File.WriteAllText(Get_workFile(), modifiedExtPrio);
        }

        public void RemoveExtensionPriorityState(int index)
        {
            JObject objJSON = JObject.Parse(File.ReadAllText(Get_workFile()));
            JArray arrayExtPrio = (JArray)objJSON["extPrio"];
            arrayExtPrio.RemoveAt(index);
            //Convert object JObject to string
            string modifiedExtPrio = objJSON.ToString();

            //Write json string to JSON file
            File.WriteAllText(Get_workFile(), modifiedExtPrio);
        }

        public void EditExtensionPriorityState(int index, string value)
        {
            JObject objJSON = JObject.Parse(File.ReadAllText(Get_workFile()));
            JArray arrayExtPrio = (JArray)objJSON["extPrio"];
            arrayExtPrio[index] = value;
            //Convert object JObject to string
            string modifiedExtPrio = objJSON.ToString();

            //Write json string to JSON file
            File.WriteAllText(Get_workFile(), modifiedExtPrio);
        }

        public void Set_extensionPriorityRegex()
        {
            string result;
            if (_extensionPriority.Count() > 0)
            {
                result = String.Join("|", _extensionPriority.ToArray());
            }
            else
            {
                result = "jesuisvide";
            }

            string regex = @$"\b({result})\b";
            _extensionPriorityRegex = new Regex(regex);
        }
    }
}
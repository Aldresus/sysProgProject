//Class Model
//Description : This class is used to write log file and to move files about different save.
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Reflection;
using System.Xml;
using System.Xml.Linq;
using System.Text.RegularExpressions;

namespace NSModel {
    public class M_Model
    {
        private List<M_SaveJob> _listSaveJob = new List<M_SaveJob>();
        private string _logFile;
        private string _workFile;
        private dynamic _language;
        private List<string> _extensionToCrypt { get; set; } = new List<string>();
        private Regex _extensionToCryptRegex;

        //Constructor
        public M_Model()
        {
            string pathDirectoryLog = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments).ToString() + @"\EasySave\Log";
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

            this.Set_logFile(pathLog);
            if (!File.Exists(this.Get_logFile()))
            {
                string initLogFile = "{\n\t\"logs\": []\n}";
                File.WriteAllText(this.Get_logFile(), initLogFile);
            }
           
            string pathDirectoryState = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments).ToString() + @"\EasySave";
            string pathState = pathDirectoryState + @"\State.json";
            this.Set_workFile(pathState);
            if (!File.Exists(this.Get_workFile()))
            {

                //Write json string to JSON file
                File.WriteAllText(this.Get_workFile(), "{\n\"lang\": \"en\",\n\"extToCrypt\": [], \n\"State\": []\n}");

            }

            JObject objJSON = JObject.Parse(File.ReadAllText(this.Get_workFile()));

            int identationIndex = 0;
            foreach (JObject i in objJSON["State"])
            {
                this._listSaveJob.Add(new M_SaveJob(i["Name"].ToString(), i["SourceFilePath"].ToString(), i["TargetFilePath"].ToString(), i["Type"].Value<int>(), i["State"].ToString(), i["TotalFilesToCopy"].Value<int>(), i["TotalFilesSize"].Value<int>(), identationIndex));
                identationIndex += 1;
            }

            //Parse language
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = "Livrable2.Locales.locales.json";
            Stream stream = assembly.GetManifestResourceStream(resourceName);
            StreamReader reader = new StreamReader(stream);
             this._language = JObject.Parse(reader.ReadToEnd())[objJSON["lang"].ToString()];

            //Get extensions to crypt in json file
            foreach (string i in objJSON["extToCrypt"])
            {
                this._extensionToCrypt.Add(i);
            }

            //Set _extensionToCryptRegex
            this.Set_extensionToCryptRegex();
        }

        public void WriteLanguage(string language) {
            JObject objJSON = JObject.Parse(File.ReadAllText(this.Get_workFile()));
            objJSON["lang"] = language;

            //Convert object JObject to string
            string json = objJSON.ToString();

            //Write json string to JSON file
            File.WriteAllText(this.Get_workFile(), json);
        }

        //Getter and Setter

        //Getter _listSaveJob
        public List<M_SaveJob> Get_listSaveJob()
        {
            return this._listSaveJob;
        }

        //Getter selected SaveJob
        public M_SaveJob GetSelectedSaveJob(int value)
        {
            return this._listSaveJob[value];
        }

        //Setter _listSaveJob
        public void Set_ListSaveJob(List<M_SaveJob> values)
        {
            this._listSaveJob = values;
        }

        //Setter selected SaveJob
        public void SetSelectedSaveJob(int value, M_SaveJob saveJob)
        {
            try
            {
                this._listSaveJob[value] = saveJob;
            }
            catch (Exception e)
            {
                this.Get_listSaveJob().Add(saveJob);
            }
}

        //Getter _logFile
        public string Get_logFile()
        {
            return this._logFile;
        }

        //Setter _logFile
        public void Set_logFile(string value)
        {
            this._logFile = value;
        }

        //Getter _workFile
        public string Get_workFile()
        {
            return this._workFile;
        }

        //Setter _workFile
        public void Set_workFile(string value)
        {
            this._workFile = value;
        }

        //Getter _language
        public dynamic Get_language()
        {
            return this._language;
        }

        //Setter _language
        public void Set_language(dynamic value)
        {
            this._language = value;
        }

        //Getter _extensionToCrypt
        public List<string> Get_extensionToCrypt()
        {
            return this._extensionToCrypt;
        }

        //Add _extensionToCrypt
        public void Add_extensionToCrypt(string value)
        {
            this._extensionToCrypt.Add(value);
        }

        //Remove _extensionToCrypt
        public void Remove_extensionToCrypt(int index)
        {
            this._extensionToCrypt.RemoveAt(index);
        }

        public void Edit_extensionToCrypt(int index, string value)
        {
            this._extensionToCrypt[index] = value;
        }

        public void InstanceNewSaveJob(string _saveJobName, string _saveJobSourceDirectory, string _saveJobDestinationDirectory, int _saveJobType, string _state, int index)
        {
            this._listSaveJob.Add(new M_SaveJob(_saveJobName, _saveJobSourceDirectory, _saveJobDestinationDirectory, _saveJobType, _state, index));
        }

        public void RemoveSaveJob(int index)
        {
            List<M_SaveJob> oldListSaveJob = this.Get_listSaveJob();
            oldListSaveJob.RemoveAt(index);
            this._listSaveJob = oldListSaveJob;

            JObject objJSON = JObject.Parse(File.ReadAllText(this.Get_workFile()));

            JArray arrayStates = (JArray)objJSON["State"];
            
            arrayStates.RemoveAt(index);

            //Convert object JObject to string
            string finalState = objJSON.ToString();

            //Write json string to JSON file
            File.WriteAllText(this.Get_workFile(), finalState);
        }

        public void AddExtensionToCryptState(string extension)
        {
            JObject objJSON = JObject.Parse(File.ReadAllText(this.Get_workFile()));
            JArray arrayExtToCrypt = (JArray)objJSON["extToCrypt"];
            arrayExtToCrypt.Add(extension);
            //Convert object JObject to string
            string modifiedExtToCrypt = objJSON.ToString();

            //Write json string to JSON file
            File.WriteAllText(this.Get_workFile(), modifiedExtToCrypt);
        }

        public void RemoveExtensionToCryptState(int index)
        {

            JObject objJSON = JObject.Parse(File.ReadAllText(this.Get_workFile()));
            JArray arrayExtToCrypt = (JArray)objJSON["extToCrypt"];
            arrayExtToCrypt.RemoveAt(index);
            //Convert object JObject to string
            string modifiedExtToCrypt = objJSON.ToString();

            //Write json string to JSON file
            File.WriteAllText(this.Get_workFile(), modifiedExtToCrypt);
        }

        public void EditExtensionToCryptState(int index, string value)
        {
            JObject objJSON = JObject.Parse(File.ReadAllText(this.Get_workFile()));
            JArray arrayExtToCrypt = (JArray)objJSON["extToCrypt"];
            arrayExtToCrypt[index] = value;
            //Convert object JObject to string
            string modifiedExtToCrypt = objJSON.ToString();

            //Write json string to JSON file
            File.WriteAllText(this.Get_workFile(), modifiedExtToCrypt);
        }

        public void Set_extensionToCryptRegex()
        {
            var result = String.Join("|", this._extensionToCrypt.ToArray());
            string regex = @$"\b({result})\b";
            this._extensionToCryptRegex = new Regex(regex);
        }
    }
}
//Class Model
//Description : This class is used to write log file and to move files about different save.
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Reflection;

namespace NSModel {
    public class M_Model
    {
        private List<M_SaveJob> _listSaveJob = new List<M_SaveJob>();
        private string _logFile;
        private string _workFile;
        private dynamic _language;

        //Constructor
        public M_Model()
        {
            string pathDirectoryLog = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments).ToString() + @"\EasySave\Log";
            if (!Directory.Exists(pathDirectoryLog))
            {
                Directory.CreateDirectory(pathDirectoryLog);
            }

            string logFileName = @"\log" + DateTime.Now.ToString("ddMMyyyy") + ".json";
            string pathLog = pathDirectoryLog + logFileName;

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
                StringBuilder sb = new StringBuilder();
                StringWriter sw = new StringWriter(sb);
                JsonWriter writer = new JsonTextWriter(sw);
                writer.Formatting = Formatting.Indented;
                writer.WriteStartObject();
                writer.WritePropertyName("Name");
                writer.WriteValue("");
                writer.WritePropertyName("SourceFilePath");
                writer.WriteValue("");
                writer.WritePropertyName("TargetFilePath");
                writer.WriteValue("");
                writer.WritePropertyName("State");
                writer.WriteValue("");
                writer.WritePropertyName("Type");
                writer.WriteValue(0);
                writer.WritePropertyName("TotalFilesToCopy");
                writer.WriteValue(0);
                writer.WritePropertyName("TotalFilesSize");
                writer.WriteValue(0);
                writer.WritePropertyName("NbFilesLeftToDo");
                writer.WriteValue(0);
                writer.WritePropertyName("Progression");
                writer.WriteValue(0);
                writer.WriteEndObject();

                //Create start State Json file
                string startJson = "{\n\"lang\": \"en\",\n\"State\": [\n";

                //Convert object JObject to string
                string json = sb.ToString();

                string file = json;
                for (int i = 0; i < 4; i++)
                {
                    file += ",\n" + json;
                }

                //Create end State Json file
                string endJson = "\n]\n}";

                //Write json string to JSON file
                File.WriteAllText(this.Get_workFile(), startJson + file + endJson);

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
            var resourceName = "Livrable1.Locales.locales.json";
            Stream stream = assembly.GetManifestResourceStream(resourceName);
            StreamReader reader = new StreamReader(stream);
             this._language = JObject.Parse(reader.ReadToEnd())[objJSON["lang"].ToString()];
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
    }
}
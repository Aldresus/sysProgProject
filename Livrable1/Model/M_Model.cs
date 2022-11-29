//Class Model
//Description : This class is used to write log file and to move files about different save.
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace NSModel {
    public class M_Model
    {
        private List<M_SaveJob> _listSaveJob = new List<M_SaveJob>();
        private string _logFile;
        private string _workFile;
        private string _language;

        //Constructor
        public M_Model()
        {
            string pathApp = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments).ToString() + @"\Livrable1\Log";
            if (!Directory.Exists(pathApp))
            {
                Directory.CreateDirectory(pathApp);
            }
            //TEST
            Console.WriteLine(Directory.Exists(pathApp));
            Console.ReadLine();
            string pathLog = pathApp + @"\Log.json";
            this.Set_logFile(pathLog);
            if (!File.Exists(this.Get_logFile()))
            {
                string initLogFile = "{\n\t\"logs\": []\n}";
                File.WriteAllText(this.Get_logFile(), initLogFile);
            }
            

            string pathState = pathApp + @"\state.json";
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
                writer.WriteValue("");
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

            
            for (int i = 0; i < 5; i++)
            {
                this._listSaveJob.Add(new M_SaveJob(objJSON["State"][i]["Name"].ToString(), objJSON["State"][i]["SourceFilePath"].ToString(), objJSON["State"][i]["TargetFilePath"].ToString(), objJSON["State"][i]["Type"].ToString(), objJSON["State"][i]["State"].ToString(), objJSON["State"][i]["TotalFilesToCopy"].Value<int>(), objJSON["State"][i]["TotalFilesSize"].Value<int>()));
            }
            //TEST
            foreach (M_SaveJob saveJob in this._listSaveJob)
            {
                foreach (KeyValuePair<string, dynamic> i in saveJob.Read())
                {
                    Console.WriteLine($"{i.Key} : {i.Value}");
                }

            }
            //TODO : Parse language
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
        public string Get_language()
        {
            return this._language;
        }

        //Setter _language
        public void Set_language(string value)
        {
            this._language = value;
        }

        public void WriteJSON(int index) {
            //Get JSon file's content
            JObject objJSON = JObject.Parse(File.ReadAllText(this.Get_workFile()));

            //Get Selected SaveJob
            M_SaveJob saveJobSelected = GetSelectedSaveJob(index);

            //Edit Name
            objJSON["State"][index]["Name"] = saveJobSelected.Get_saveJobName();
            //Edit SourceFilePath
            objJSON["State"][index]["SourceFilePath"] = saveJobSelected.Get_saveJobSourceDirectory();
            //Edit DestinationFilePath
            objJSON["State"][index]["TargetFilePath"] = saveJobSelected.Get_saveJobDestinationDirectory();
            //Edit State
            objJSON["State"][index]["State"] = saveJobSelected.Get_state();
            //Edit saveJobType
            objJSON["State"][index]["Type"] = saveJobSelected.Get_saveJobType();
            //Edit TotalFilesToCopy
            objJSON["State"][index]["TotalFilesToCopy"] = saveJobSelected.Get_totalNbFile();
            //Edit TotalFilesSize
            objJSON["State"][index]["TotalFilesSize"] = saveJobSelected.Get_totalSizeFile();
            //Edit NbFilesLeftToDo
            objJSON["State"][index]["NbFilesLeftToDo"] = 30;
            //Edit Progression
            objJSON["State"][index]["Progression"] = 0;

            //Convert object JObject to string
            string json = objJSON.ToString();

            //Write json string to JSON file
            File.WriteAllText(this.Get_workFile(), json);
        }

        //TODO : d�placer la m�thode dans M_SaveJob et modifier les WriteValue n�cessaires.
       /* public void WriteLog(int index) 
        {
            //Get JSON file's content
            JObject allLog = JObject.Parse(File.ReadAllText(this.Get_logFile()));

            //Get Selected SaveJob
            M_SaveJob saveJobSelected = GetSelectedSaveJob(index);

            StringBuilder sb = new StringBuilder();
            StringWriter sw = new StringWriter(sb);
            JsonWriter writer = new JsonTextWriter(sw);
            writer.Formatting = Formatting.Indented;
            writer.WriteStartObject();
            writer.WritePropertyName("Name");
            writer.WriteValue(saveJobSelected.Get_saveJobName());
            writer.WritePropertyName("FileSource");
            writer.WriteValue(saveJobSelected.Get_file().Get_fileSource());
            writer.WritePropertyName("FileTarget");
            writer.WriteValue(saveJobSelected.Get_file().Get_fileDestination());
            writer.WritePropertyName("destPath");
            writer.WriteValue(saveJobSelected.Get_saveJobDestinationDirectory());
            writer.WritePropertyName("FileSize");
            writer.WriteValue(saveJobSelected.Get_file().Get_fileSize());
            writer.WritePropertyName("FileTransferTime");
            writer.WriteValue(0.2);
            writer.WritePropertyName("time");
            writer.WriteValue(saveJobSelected.Get_file().Get_time());
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
            File.WriteAllText(this.Get_logFile(), newLogFile);
        }*/
    }
}
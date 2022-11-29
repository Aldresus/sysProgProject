//Class Model
//Description : This class is used to write log file and to move files about different save.
using System;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace NSModel {
    public class M_Model
    {
        private List<M_SaveJob> _listSaveJob;
        private string _logFile;
        private string _workFile;
        private string _language;

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
            objJSON["SaveJob"][0]["Name"] = saveJobSelected.Get_saveJobName();
            //Edit SourceFilePath
            objJSON["SaveJob"][0]["SourceFilePath"] = saveJobSelected.Get_file().Get_fileSource();
            //Edit DestinationFilePath
            objJSON["SaveJob"][0]["TargetFilePath"] = saveJobSelected.Get_file().Get_fileDestination();
            //Edit State
            objJSON["SaveJob"][0]["State"] = saveJobSelected.Get_state();
            //Edit TotalFilesToCopy
            objJSON["SaveJob"][0]["TotalFilesToCopy"] = saveJobSelected.Get_totalNbFile();
            //Edit TotalFilesSize
            objJSON["SaveJob"][0]["TotalFilesSize"] = saveJobSelected.Get_totalSizeFile();
            //Edit NbFilesLeftToDo
            objJSON["SaveJob"][0]["NbFilesLeftToDo"] = 30;
            //Edit Progression
            objJSON["SaveJob"][0]["Progression"] = 0;

            //Convert object JObject to string
            string json = objJSON.ToString();

            //Write json string to JSON file
            File.WriteAllText(this.Get_workFile(), json);
        }

        public void WriteLog(int index) 
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
        }
    }
}
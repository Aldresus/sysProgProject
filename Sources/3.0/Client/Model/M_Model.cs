//Class Model
//Description : This class is used to write log file and to move files about different save.
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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
        private string _logFile;
        private string _workFile;
        private dynamic _language;

        //Constructor
        public M_Model(string workfile)
        {
            this._workFile = workfile;
            JObject objJSON = JObject.Parse(Get_workFile());

            int identationIndex = 0;
            foreach (JObject i in objJSON["State"])
            {
                _listSaveJob.Add(new M_SaveJob(i["Name"].ToString(), i["SourceFilePath"].ToString(), i["TargetFilePath"].ToString(), i["Type"].Value<int>(), i["State"].ToString(), i["TotalFilesToCopy"].Value<int>(), i["TotalFilesSize"].Value<int>(), identationIndex));
                identationIndex += 1;
            }

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

        public void InstanceNewSaveJob(string _saveJobName, string _saveJobSourceDirectory, string _saveJobDestinationDirectory, int _saveJobType, string _state, int index)
        {
            _listSaveJob.Add(new M_SaveJob(_saveJobName, _saveJobSourceDirectory, _saveJobDestinationDirectory, _saveJobType, _state, index));
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

    }
}
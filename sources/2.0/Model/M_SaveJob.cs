//Class SaveJob
//Description : This class is used to store information and save the files

using Livrable2;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NSUtils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace NSModel
{
    public class M_SaveJob
    {
        public string _saveJobName { get; set; }
        public string _saveJobSourceDirectory { get; set; }
        public string _saveJobDestinationDirectory { get; set; }
        public int _saveJobType { get; set; }
        private string _state;
        private int _totalNbFile;
        private int _totalSizeFile;
        private int _index;
        private int _NbFilesLeftToDo;
        public int _progress { get; set; }

        // default Constructor
        public M_SaveJob()
        {
        }

        //Constructor (Set all attributes when object instantiation except _totalNbFile, _totalSizeFile)
        public M_SaveJob(string _saveJobName, string _saveJobSourceDirectory, string _saveJobDestinationDirectory, int _saveJobType, string _state, int _progress, int index)
        {
            this.Set_saveJobName(_saveJobName);
            this.Set_saveJobSourceDirectory(_saveJobSourceDirectory);
            this.Set_saveJobDestinationDirectory(_saveJobDestinationDirectory);
            this.Set_saveJobType(_saveJobType);
            this.Set_state(_state);
            this.Set_totalNbFile(CalculateFolderNB(_saveJobSourceDirectory));
            this.Set_totalSizeFile((int)CalculateFolderSize(_saveJobSourceDirectory));
            this.Set_progress(_progress);
            this.Set_index(index);
        }
       

        public void Execute(MainWindow MW, M_SaveJob SaveJob, string logFilePath, string stateFilePath, M_Model M)
        {

            M.utilExecute.StartThread(SaveJob, M, MW);
        }

        //Getter and Setter
        
        //Getter _savedJobName
        public string Get_saveJobName()
        {
            return _saveJobName;
        }

        //Setter _savedJobName
        public void Set_saveJobName(string value)
        {
            _saveJobName = value;
        }

        //Getter _saveJobSourceDirectory
        public string Get_saveJobSourceDirectory()
        {
            return _saveJobSourceDirectory;
        }

        //Setter _saveJobSourceDirectory
        public void Set_saveJobSourceDirectory(string value)
        {
            _saveJobSourceDirectory = value;
        }

        //Getter _saveJobDestinationDirectory
        public string Get_saveJobDestinationDirectory()
        {
            return _saveJobDestinationDirectory;
        }

        //Setter _saveJobDestinationDirectory
        public void Set_saveJobDestinationDirectory(string value)
        {
            _saveJobDestinationDirectory = value;
        }

        //Getter _saveJobType
        public int Get_saveJobType()
        {
            return _saveJobType;
        }

        //Setter _saveJobType
        public void Set_saveJobType(int value)
        {
            this._saveJobType = value;
        }

        //Getter _state
        public string Get_state()
        {
            return _state;
        }

        //Setter _state
        public void Set_state(string value)
        {
            _state = value;
        }

        //Getter _totalNbFile
        public int Get_totalNbFile()
        {
            return _totalNbFile;
        }

        //Setter _totalNbFile
        public void Set_totalNbFile(int value)
        {
            _totalNbFile = value;
        }

        //Getter _totalSizeFile
        public int Get_totalSizeFile()
        {
            return _totalSizeFile;
        }

        //Setter _totalSizeFile
        public void Set_totalSizeFile(int value)
        {
            _totalSizeFile = value;
        }

        //Getter _index
        public int Get_index()
        {
            return _index;
        }

        //Setter _index
        public void Set_index(int value)
        {
            _index = value;
        }

        // Getter _NbFilesLeftToDo
        public int Get_NbFilesLeftToDo()
        {
            return _NbFilesLeftToDo;
        }

        // Setter _NbFilesLeftToDo
        public void Set_NbFilesLeftToDo(int value)
        {
            _NbFilesLeftToDo = value;
        }
        
        // Getter _progress
        public int Get_progress()
        {
            return _progress;
        }

        // Setter _progress
        public void Set_progress(int value)
        {
            _progress = value;
        }

        
        //Edit attributes of object M_SaveJob
        public void Update(string _saveJobName, string _saveJobSourceDirectory, string _saveJobDestinationDirectory, int _saveJobType, string _state, int _totalNbFile, int _totalSizeFile)
        {
            //Edit attributes
            this.Set_saveJobName(_saveJobName);
            this.Set_saveJobSourceDirectory(_saveJobSourceDirectory);
            this.Set_saveJobDestinationDirectory(_saveJobDestinationDirectory);
            this.Set_saveJobType(_saveJobType);
            this.Set_state(_state);
            this.Set_totalNbFile(_totalNbFile);
            this.Set_totalSizeFile(_totalSizeFile);
        }

        public void Update(string _saveJobName, string _saveJobSourceDirectory, string _saveJobDestinationDirectory, int _saveJobType)
        {
            //Edit attributes
            this.Set_saveJobName(_saveJobName);
            this.Set_saveJobSourceDirectory(_saveJobSourceDirectory);
            this.Set_saveJobDestinationDirectory(_saveJobDestinationDirectory);
            this.Set_saveJobType(_saveJobType);
        }

        //Get all attributes of object M_SaveJob
        public Dictionary<string, dynamic> Read()
        {
            //Create a dictionary to store all attributes of object M_SaveJob
            Dictionary<string, dynamic> values = new Dictionary<string, dynamic>();
            //Add all attributes of object M_SaveJob to dictionary
            values.Add("_saveJobName", this.Get_saveJobName());
            values.Add("_saveJobSourceDirectory", this.Get_saveJobSourceDirectory());
            values.Add("_saveJobDestinationDirectory", this.Get_saveJobDestinationDirectory());
            values.Add("_saveJobType", this.Get_saveJobType());
            values.Add("_state", this.Get_state());
            values.Add("_totalNbFile", this.Get_totalNbFile());
            values.Add("_totalSizeFile", this.Get_totalSizeFile());
            //Return dictionary that contains all attributes of object M_SaveJob
            return values;
        }

        public void WriteJSON(string JsonStatePath)
        {
            //Get JSon file's content
            JObject objJSON = JObject.Parse(File.ReadAllText(JsonStatePath));

            Set_totalNbFile(CalculateFolderNB(Get_saveJobSourceDirectory()));
            Set_totalSizeFile((int)CalculateFolderSize(Get_saveJobSourceDirectory()));

            try
            {
                //Edit Name
                objJSON["State"][this.Get_index()]["Name"] = this.Get_saveJobName();
                //Edit SourceFilePath
                objJSON["State"][this.Get_index()]["SourceFilePath"] = this.Get_saveJobSourceDirectory();
                //Edit DestinationFilePath
                objJSON["State"][this.Get_index()]["TargetFilePath"] = this.Get_saveJobDestinationDirectory();
                //Edit State
                objJSON["State"][this.Get_index()]["State"] = this.Get_state();
                //Edit saveJobType
                objJSON["State"][this.Get_index()]["Type"] = this.Get_saveJobType();
                //Edit TotalFilesToCopy
                objJSON["State"][this.Get_index()]["TotalFilesToCopy"] = this.Get_totalNbFile();
                //Edit TotalFilesSize
                objJSON["State"][this.Get_index()]["TotalFilesSize"] = this.Get_totalSizeFile();
                //Edit NbFilesLeftToDo
                objJSON["State"][this.Get_index()]["NbFilesLeftToDo"] = this.Get_NbFilesLeftToDo();
                //Edit Progression
                objJSON["State"][this.Get_index()]["Progression"] = this.Get_progress();
            }
            catch (Exception e)
            {
                StringBuilder sb = new StringBuilder();
                StringWriter sw = new StringWriter(sb);
                JsonWriter writer = new JsonTextWriter(sw);
                writer.Formatting = Formatting.Indented;
                writer.WriteStartObject();
                writer.WritePropertyName("Name");
                writer.WriteValue(this.Get_saveJobName());
                writer.WritePropertyName("SourceFilePath");
                writer.WriteValue(this.Get_saveJobSourceDirectory());
                writer.WritePropertyName("TargetFilePath");
                writer.WriteValue(this.Get_saveJobDestinationDirectory());
                writer.WritePropertyName("State");
                writer.WriteValue(this.Get_state());
                writer.WritePropertyName("Type");
                writer.WriteValue(this.Get_saveJobType());
                writer.WritePropertyName("TotalFilesToCopy");
                writer.WriteValue(this.Get_totalNbFile());
                writer.WritePropertyName("TotalFilesSize");
                writer.WriteValue(this.Get_totalSizeFile());
                writer.WritePropertyName("NbFilesLeftToDo");
                writer.WriteValue(this.Get_NbFilesLeftToDo());
                writer.WritePropertyName("Progression");
                writer.WriteValue(this.Get_progress());
                writer.WriteEndObject();

                //Convert object JsonWriter to string
                string json = sb.ToString();

                //Convert string to JObject
                JObject newState = JObject.Parse(json);

                //Get JObject "logs" of Json Lof file
                JArray arrayStates = (JArray)objJSON["State"];

                //Add newLog to allLog
                arrayStates.Add(newState);
            }

            //Convert object JObject to string
            string finalState = objJSON.ToString();

            //Write json string to JSON file
            File.WriteAllText(JsonStatePath, finalState);
        }
        

        //method which is called when the save job is running
        public void WriteJSON(string JsonStatePath, string state, int FilesLeft, int progress)
        {
            //Get JSon file's content
            JObject objJSON = JObject.Parse(File.ReadAllText(JsonStatePath));

            Set_state(state);
            Set_NbFilesLeftToDo(FilesLeft);
            Set_progress(progress);
            
            //Edit State
            objJSON["State"][this.Get_index()]["State"] = this.Get_state();
            //Edit NbFilesLeftToDo
            objJSON["State"][this.Get_index()]["NbFilesLeftToDo"] = this.Get_NbFilesLeftToDo();
            //Edit Progression
            objJSON["State"][this.Get_index()]["Progression"] = this.Get_progress();

            //Convert object JObject to string
            string json = objJSON.ToString();

            //Write json string to JSON file
            File.WriteAllText(JsonStatePath, json);
        }

        public long CalculateFolderSize(string folder)
        {
            long folderSize = 0;
            try
            {
                //Checks if the path is valid or not
                if (!Directory.Exists(folder))
                    return folderSize;
                else
                {
                    try
                    {
                        foreach (string file in Directory.GetFiles(folder))
                        {
                            if (File.Exists(file))
                            {
                                FileInfo finfo = new FileInfo(file);
                                folderSize += finfo.Length;
                            }
                        }

                        foreach (string dir in Directory.GetDirectories(folder))
                            folderSize += CalculateFolderSize(dir);
                    }
                    catch (NotSupportedException e)
                    {
                    }
                }
            }
            catch (UnauthorizedAccessException e)
            {
            }
            return folderSize;
        }

        public int CalculateFolderNB(string source)
        {
            try
            {
                return Directory.GetFiles(source, "*", SearchOption.AllDirectories).Length;
            }
            catch (Exception)
            {
                return 0;
            }
        }

        ~M_SaveJob()
        {
            //Destructor
        }
    }
}
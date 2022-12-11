//Class SaveJob
//Description : This class is used to store information and save the files

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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
        private int _progress;

        // default Constructor
        public M_SaveJob()
        {
        }

        //Constructor (Set all attributes when object instantiation except _totalNbFile, _totalSizeFile)
        public M_SaveJob(string _saveJobName, string _saveJobSourceDirectory, string _saveJobDestinationDirectory, int _saveJobType, string _state, int index)
        {
            this.Set_saveJobName(_saveJobName);
            this.Set_saveJobSourceDirectory(_saveJobSourceDirectory);
            this.Set_saveJobDestinationDirectory(_saveJobDestinationDirectory);
            this.Set_saveJobType(_saveJobType);
            this.Set_state(_state);
            this.Set_totalNbFile(CalculateFolderNB(_saveJobSourceDirectory));
            this.Set_totalSizeFile((int)CalculateFolderSize(_saveJobSourceDirectory));
            this.Set_index(index);
        }
        
        //Constructor (Set all attributes when object instantiation)
        public M_SaveJob(string _saveJobName, string _saveJobSourceDirectory, string _saveJobDestinationDirectory, int _saveJobType, string _state, int _totalNbFile, int _totalSizeFile, int index)
        {
            this.Set_saveJobName(_saveJobName);
            this.Set_saveJobSourceDirectory(_saveJobSourceDirectory);
            this.Set_saveJobDestinationDirectory(_saveJobDestinationDirectory);
            this.Set_saveJobType(_saveJobType);
            this.Set_state(_state);
            this.Set_totalNbFile(CalculateFolderNB(_saveJobSourceDirectory));
            this.Set_totalSizeFile((int)CalculateFolderSize(_saveJobSourceDirectory));
            this.Set_index(index);
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
            _saveJobType = value;
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

        public void Update(string _saveJobName, string _saveJobSourceDirectory, string _saveJobDestinationDirectory, int _saveJobType)
        {
            //Edit attributes
            this.Set_saveJobName(_saveJobName);
            this.Set_saveJobSourceDirectory(_saveJobSourceDirectory);
            this.Set_saveJobDestinationDirectory(_saveJobDestinationDirectory);
            this.Set_saveJobType(_saveJobType);
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
// Class MovingFile
// Description: This class is used to store information about the file that will move.

using System;
using System.ComponentModel.DataAnnotations;

namespace NSModel
{
    public class M_MovingFile
    {
        private string? _fileSource;
        private string? _fileDestination;
        private int _fileSize;
        private DateTime _time;

        //Getter _fileSource
        public string Get_fileSource()
        {
            return _fileSource;
        }

        //Setter _fileSource
        public void Set_fileSource(string value)
        {
            _fileSource = value;
        }

        //Getter _fileDestination
        public string Get_fileDestination()
        {
            return _fileDestination;
        }

        //Setter _fileDestination
        public void Set_fileDestination(string value)
        {
            _fileDestination = value;
        }

        //Getter _fileSize
        public int Get_fileSize()
        {
            return _fileSize;
        }

        //Setter _fileSize
        public void Set_fileSize(int value)
        {
            _fileSize = value;
        }

        //Getter _time
        public DateTime Get_time()
        {
            return _time;
        }

        //Setter _time
        public void Set_time(DateTime value)
        {
            _time = value;
        }
    }

}
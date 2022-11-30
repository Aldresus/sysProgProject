//Class Strategy
//Description : This Class is used to select the correct type of save to execute via the strategy design pattern

using System.Collections.Generic;
using System;
using NSUtils;
using NSViews;
using System.Diagnostics.Tracing;

namespace NSModel
{
    public interface IStrategy
    {
        public void Execute(string source, string destination, string FileLogPath, M_Model M);
    }

    class DiffentialSave : IStrategy
    {
        public void Execute(string source, string destination, string FileLogPath, M_Model M)
        {
           
            U_Execute utilExecute = new U_Execute(M);
            utilExecute.Execute(source, destination, false, FileLogPath);
        }
    }


    class FullSave : IStrategy
    {
        public void Execute(string source, string destination, string FileLogPath, M_Model M)
        {
            
            U_Execute utilExecute = new U_Execute(M);
            utilExecute.Execute(source, destination, true, FileLogPath);
        }
    }
}
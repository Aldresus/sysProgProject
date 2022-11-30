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
        public void Execute(M_SaveJob SaveJob, string FileLogPath, string FileStatePath);
    }

    class DiffentialSave : IStrategy
    {
        public void Execute(M_SaveJob SaveJob, string FileLogPath, string FileStatePath)
        {
            Console.WriteLine("Diff strategy selected");
            U_Execute utilExecute = new U_Execute();
            utilExecute.Execute(SaveJob, FileLogPath, FileStatePath);
        }
    }

    
    class FullSave : IStrategy
    {
        public void Execute(M_SaveJob SaveJob, string FileLogPath, string FileStatePath)
        {
            Console.WriteLine("Full strategy selected");
            U_Execute utilExecute = new U_Execute();
            utilExecute.Execute(SaveJob, FileLogPath, FileStatePath);
        }
    }
}
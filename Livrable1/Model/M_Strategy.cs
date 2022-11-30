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
        public void Execute(string source, string destination, string FileLogPath);
    }

    class DiffentialSave : IStrategy
    {
        public void Execute(string source, string destination, string FileLogPath)
        {
            Console.WriteLine("Diff strategy selected");
            U_Execute utilExecute = new U_Execute();
            utilExecute.Execute(source, destination, false, FileLogPath);
        }
    }


    class FullSave : IStrategy
    {
        public void Execute(string source, string destination, string FileLogPath)
        {
            Console.WriteLine("Full strategy selected");
            U_Execute utilExecute = new U_Execute();
            utilExecute.Execute(source, destination, true, FileLogPath);
        }
    }
}
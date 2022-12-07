//Class Strategy
//Description : This Class is used to select the correct type of save to execute via the strategy design pattern

using System.Collections.Generic;
using System;
using NSUtils;
using System.Diagnostics.Tracing;

namespace NSModel
{
    public interface IStrategy
    {
        public void Execute(M_SaveJob SaveJob, string FileLogPath, string FileStatePath, M_Model M);
    }

    class DiffentialSave : IStrategy
    {
        public void Execute(M_SaveJob SaveJob, string FileLogPath, string FileStatePath, M_Model M)
        {
            U_Execute utilExecute = new U_Execute(M);
            utilExecute.Execute(SaveJob, FileLogPath, FileStatePath);

        }
    }

    class FullSave : IStrategy
    {
        public void Execute(M_SaveJob SaveJob, string FileLogPath, string FileStatePath, M_Model M)
        {
            U_Execute utilExecute = new U_Execute(M);
            utilExecute.Execute(SaveJob, FileLogPath, FileStatePath);
        }
    }
}
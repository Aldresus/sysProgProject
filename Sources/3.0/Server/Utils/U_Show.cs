using NSModel;
using System;
using System.Collections.Generic;
namespace NSUtils
{
    public class U_Show
    {
        private M_Model _oModel;

        public U_Show(M_Model M)
        {
            this._oModel = M;
        }

        public void ShowJobs(List<M_SaveJob> listSaveJob)
        {
            int i = 1;
            foreach (M_SaveJob saveJob in listSaveJob)
            {
                if (!(saveJob.Get_saveJobName() == ""))
                {
                    Console.WriteLine($"{i} -  {_oModel.Get_language().name.ToString()} : {saveJob.Get_saveJobName()} {_oModel.Get_language().source.ToString()} : {saveJob.Get_saveJobSourceDirectory()} {_oModel.Get_language().destinationFolder.ToString()} : {saveJob.Get_saveJobDestinationDirectory()} {_oModel.Get_language().type.ToString()} : {saveJob.Get_saveJobType()}");
                }
                i++;

            }
        }
    }
}
using NSModel;
using System;
using System.Collections.Generic;
namespace NSUtils
{
    public class U_Show
    {
        public void ShowJobs(List<M_SaveJob> listSaveJob)
        {
            int i = 1;
            foreach (M_SaveJob saveJob in listSaveJob)
            {
                if (!(saveJob.Get_saveJobName() == ""))
                {
                    Console.WriteLine($"{i} -  name : {saveJob.Get_saveJobName()} source : {saveJob.Get_saveJobSourceDirectory()} destination : {saveJob.Get_saveJobDestinationDirectory()} type : {saveJob.Get_saveJobType()}");
                }
                i++;

            }
        }
    }
}
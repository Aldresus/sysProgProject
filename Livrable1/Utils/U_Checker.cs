using NSModel;
using System;
using System.Collections.Generic;
namespace NSUtils
{
    public class U_Checker
    {
        public int CheckAnyJobs(List<M_SaveJob> listSaveJob)
        {
            int i = 0;
            foreach (M_SaveJob saveJob in listSaveJob)
            {
                if (!(saveJob.Get_saveJobName() == ""))
                {
                    i++;
                }

            }
            return i;
        }
        public int GetEmptyJobIndex(List<M_SaveJob> listSaveJob)
        {
            int i = 0;
            foreach (M_SaveJob saveJob in listSaveJob)
            {
                Console.WriteLine(saveJob.Get_saveJobName());
                Console.WriteLine(i);
                Console.WriteLine(saveJob.Get_saveJobName() == "");
                Console.WriteLine(" ");
                
                if (saveJob.Get_saveJobName() == "")
                {
                    return i;
                    
                }
                i++;
            }
            Console.ReadLine();
            return -1; // should never happen
        }
    }
}
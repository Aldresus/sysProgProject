using NSModel;
using System;
using System.Collections.Generic;
using System.Linq;

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
            return listSaveJob.Count(); // should never happen
        }
        public bool CheckStringInput(string promptText, bool canBeEmpty)
        {

            {

                if (promptText.Length == 0 && canBeEmpty)
                {
                    return true;
                }
                else if (promptText.Length > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            }
        }
    }
}
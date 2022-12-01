using NSModel;
using NSUtils;
using NSViewModel;
using System;

namespace NSViews
{
    public class V_Delete
    {
        private VM_ViewModel _oViewModel;

        public V_Delete(VM_ViewModel VM)
        {
            _oViewModel = VM;
            M_Model M = VM.Get_Model();


            U_Reader Reader = new U_Reader(M);
            U_Show Show = new U_Show(M);
            U_Checker Checker = new U_Checker();


            if (M.Get_listSaveJob().Count() > 0)
            {
                bool validInput = false;



                while (!validInput)
                {

                    Console.Clear();
                    Console.WriteLine(M.Get_language().availableJobs.ToString());
                    Show.ShowJobs(M.Get_listSaveJob());
                    List<int> indexes = Reader.ReadMany($"{M.Get_language().enterJobIndexToDelete.ToString()}, {M.Get_language().enterZeroToAbort.ToString()}");
                    indexes.Sort();
                    if (indexes[0] == 0)
                    {
                        break;
                    }
                    else
                    {
                        int compt = 1;
                        for (int i = 0; i < indexes.Count(); i++)
                        {
                            Console.Write($"{indexes[i]} ");
                            if (i == 0) { }
                            else
                            {
                                if (indexes[i - 1] < indexes[i])
                                {

                                    indexes[i] -= compt;
                                    compt++;
                                }

                            }
                        }

                        foreach (int i in indexes)
                        {
                            int jobIndex = i - 1;

                            List<M_SaveJob> listSaveJob = M.Get_listSaveJob();

                            M.RemoveSaveJob(jobIndex);
                        }

                        Console.Write(M.Get_language().deleted.ToString());
                        validInput = true;

                        Reader.PressAnyKeyToContinue(M.Get_language().pressAnyToContinue.ToString());
                    }
                }
            }
            else
            {

                Console.WriteLine(M.Get_language().noJob.ToString());
                Reader.PressAnyKeyToContinue(M.Get_language().pressAnyToContinue.ToString());

            }
        }
    }
}

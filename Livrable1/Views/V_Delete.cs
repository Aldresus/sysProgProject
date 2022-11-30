using Newtonsoft.Json.Linq;
using NSModel;
using NSUtils;
using NSViewModel;
using System.Reflection;
using System.Xml.Linq;

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


            if (Checker.CheckAnyJobs(M.Get_listSaveJob()) > 0)
            {
                bool validInput = false;



                while (!validInput)
                {

                    Console.Clear();
                    Console.WriteLine(M.Get_language().availableJobs.ToString());
                    Show.ShowJobs(M.Get_listSaveJob());
                    List<int> indexes = Reader.ReadMany($"{M.Get_language().enterJobIndexToDelete.ToString()}, {M.Get_language().enterZeroToAbort.ToString()}");
                    if (indexes[0] == 0)
                    {
                        break;
                    }
                    else {                        
                        foreach (int i in indexes)
                        {
                            Console.Write($"{i} ");

                            int jobIndex = i - 1;
                            M.Get_listSaveJob()[jobIndex].Set_saveJobName("");
                            M.Get_listSaveJob()[jobIndex].Set_saveJobSourceDirectory("");
                            M.Get_listSaveJob()[jobIndex].Set_saveJobDestinationDirectory("");
                            M.Get_listSaveJob()[jobIndex].Set_saveJobType(0);
                            M.Get_listSaveJob()[jobIndex].Set_state("");
                            M.Get_listSaveJob()[jobIndex].Set_totalNbFile(0);
                            M.Get_listSaveJob()[jobIndex].Set_totalSizeFile(0);

                            M.GetSelectedSaveJob(jobIndex).WriteJSON(M.Get_workFile());
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

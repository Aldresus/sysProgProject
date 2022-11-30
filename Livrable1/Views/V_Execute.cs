using Newtonsoft.Json.Linq;
using NSModel;
using NSUtils;
using NSViewModel;
using System.Reflection;

namespace NSViews
{
    public class V_Execute
    {
        private VM_ViewModel _oViewModel;

        public V_Execute(VM_ViewModel VM)
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
                    else
                    {
                        foreach (int i in indexes)
                        {
                            Console.Write($"{i} ");
                            M.Get_listSaveJob()[i-1].Execute(M.Get_listSaveJob()[i - 1], M.Get_logFile(), M.Get_workFile(), M);
                        }
                        Console.Write(M.Get_language().executed.ToString());
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
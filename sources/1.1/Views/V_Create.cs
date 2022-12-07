using Newtonsoft.Json.Linq;
using NSModel;
using NSUtils;
using NSViewModel;

namespace NSViews
{
    public class V_Create
    {
        private VM_ViewModel _oViewModel;

        public V_Create(VM_ViewModel VM)
        {
            _oViewModel = VM;
            M_Model M = VM.Get_Model();

            U_Reader Reader = new U_Reader(M);
            U_Show Show = new U_Show(M);
            U_Checker Checker = new U_Checker();
  
            if (Checker.CheckAnyJobs(M.Get_listSaveJob()) < 10)
            {
                Console.Clear();

                string name = Reader.ReadString($"{M.Get_language().enterJobName.ToString()}, {M.Get_language().enterExitToAbort.ToString()}", false);

                if (name != "exit")
                {

                    //print save types
                    Console.WriteLine($"1 - {M.Get_language().fullSave.ToString()}");
                    Console.WriteLine($"2 - {M.Get_language().differentialSave.ToString()}");

                    int type = Reader.ReadInt(M.Get_language().enterJobType.ToString());
                    string source = Reader.ReadPath(M.Get_language().enterJobSource.ToString(), false);
                    string dest = Reader.ReadPath(M.Get_language().enterJobDestination.ToString(), true);

                    //show created job
                    Console.WriteLine($"\n{M.Get_language().jobCreated.ToString()}");
                    Console.WriteLine($"{M.Get_language().name.ToString()}: {name} ");
                    Console.WriteLine($"{M.Get_language().sourceFolder.ToString()}: {source} ");
                    Console.WriteLine($"{M.Get_language().destinationFolder.ToString()}: {dest} ");
                    Console.WriteLine($"{M.Get_language().type.ToString()}: {type}");

                    int jobIndex = Checker.GetEmptyJobIndex(M.Get_listSaveJob());
                    M.InstanceNewSaveJob(name, source, dest, type, "idle", jobIndex);
                    M.GetSelectedSaveJob(jobIndex).WriteJSON(M.Get_workFile());
                    Reader.PressAnyKeyToContinue(M.Get_language().pressAnyToContinue.ToString());
                }
            }
            else
            {
                {
                    Console.WriteLine(M.Get_language().maxJobReached.ToString());
                    Reader.PressAnyKeyToContinue(M.Get_language().pressAnyToContinue.ToString());
                }
            }
        }
    }
}

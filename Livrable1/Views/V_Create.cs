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
            this._oViewModel = VM;
            M_Model M = VM.Get_Model();

            U_Reader Reader = new U_Reader();
            U_Show Show = new U_Show();
            U_Checker Checker = new U_Checker();
            
          //TODO check if there is less than 5 save jobs
            if (Checker.CheckAnyJobs(M.Get_listSaveJob()) < 5)
            {
                Console.Clear();

                string name = Reader.ReadString(M.Get_language().enterJobName.ToString(), false);
                //print types
                Console.WriteLine("1 - type 1"); //placeholder
                Console.WriteLine("2 - type 2"); //placeholder
                
                int type = Reader.ReadInt(M.Get_language().enterJobType.ToString());
                string source = Reader.ReadString(M.Get_language().enterJobSource.ToString(), false);
                string dest = Reader.ReadString(M.Get_language().enterJobDestination.ToString(), false);
                Console.WriteLine($"{M.Get_language().jobCreated.ToString()} {M.Get_language().name.ToString()}: {name} {M.Get_language().sourceFolder.ToString()}:{source} {M.Get_language().destinationFolder.ToString()}:{dest} {M.Get_language().type.ToString()}:{type}");

                int jobIndex = Checker.GetEmptyJobIndex(M.Get_listSaveJob());
                M.Get_listSaveJob()[jobIndex].Set_saveJobName(name);
                M.Get_listSaveJob()[jobIndex].Set_saveJobSourceDirectory(source);
                M.Get_listSaveJob()[jobIndex].Set_saveJobDestinationDirectory(dest);
                M.Get_listSaveJob()[jobIndex].Set_saveJobType(type);
                M.GetSelectedSaveJob(jobIndex).WriteJSON(M.Get_workFile());
                Reader.PressAnyKeyToContinue(M.Get_language().pressAnyToContinue.ToString());
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

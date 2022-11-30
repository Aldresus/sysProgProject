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
                Console.WriteLine("1 - Full Save"); //placeholder
                Console.WriteLine("2 - Diffenrential Save"); //placeholder
                
                int type = Reader.ReadInt(M.Get_language().enterJobType.ToString());
                string source = Reader.ReadPath(M.Get_language().enterJobSource.ToString(), false);
                string dest = Reader.ReadPath(M.Get_language().enterJobDestination.ToString(), true);
                Console.WriteLine($"\n{M.Get_language().jobCreated.ToString()}");
                Console.WriteLine($"{M.Get_language().name.ToString()}: {name} ");
                Console.WriteLine($"{M.Get_language().sourceFolder.ToString()}: {source} ");
                Console.WriteLine($"{M.Get_language().destinationFolder.ToString()}: {dest} ");
                Console.WriteLine($"{M.Get_language().type.ToString()}: {type}");

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

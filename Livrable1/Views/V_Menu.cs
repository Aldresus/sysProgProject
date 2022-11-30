using Newtonsoft.Json.Linq;
using NSModel;
using NSUtils;
using NSViewModel;
using System.Reflection;

namespace NSViews
{
    public class V_Menu
    {
        private VM_ViewModel _oViewModel;
        private V_Create _oVCreate;
        private V_Delete _oVDelete;
        private V_Edit _oVEdit;
        private V_Execute _oVExecute;
        private V_Settings _oVSettings;

        public V_Menu(VM_ViewModel VM)
        {
            this._oViewModel = VM;
            M_Model M = VM.Get_Model();
            
            U_Reader Reader = new U_Reader(M);
            U_Show Show = new U_Show(M);
            U_Checker Checker = new U_Checker();

            
            while (true)
            {
                Console.Clear();
                string ordina = M.Get_language().ordina.ToString();
                string coeur = M.Get_language().coeur.ToString();
                Console.WriteLine($"         ______________                                                                                   .----.      \r\n        |[]            |                                                                      .---------. | == |      \r\n        |  __________  |                  .                     .                     .       |.-\"\"\"\"\"-.| |----|      \r\n        |  |  Easy  |  |   .. ............;;.    .. ............;;.    .. ............;;.     ||{ordina}|| | == |      \r\n        |  |  Save! |  |    ..::::::::::::;;;;.   ..::::::::::::;;;;.   ..::::::::::::;;;;.   ||  {coeur}|| |----|      \r\n        |  |________|  |   . . ::::::::::::;;:'  . . ::::::::::::;;:'  . . ::::::::::::;;:'   |'-.....-'| |::::|      \r\n        |   ________   |                  :'                    :'                    :'      `\"\")---(\"\"` |___.|      \r\n        |   [ [ ]  ]   |                                                                     /:::::::::::\\\" _  \"      \r\n        \\___[_[_]__]___|                                                                    /:::=======:::\\`\\`\\       \r\n                                                                                            `\"\"\"\"\"\"\"\"\"\"\"\"\"`  '-'      ");

                if (Checker.CheckAnyJobs(M.Get_listSaveJob()) > 0) { 
                    Console.WriteLine(M.Get_language().availableJobs.ToString());
                    Show.ShowJobs(M.Get_listSaveJob());
                }
                
                Console.WriteLine($"\n\n1 - {M.Get_language().createJob.ToString()}");
                Console.WriteLine($"2 - {M.Get_language().executeJob.ToString()}");
                Console.WriteLine($"3 - {M.Get_language().editJob.ToString()}");
                Console.WriteLine($"4 - {M.Get_language().deleteJob.ToString()}");
                Console.WriteLine($"5 - {M.Get_language().settings.ToString()}");
                Console.WriteLine($"6 - {M.Get_language().exit.ToString()}");


                int option = Reader.ReadInt(M.Get_language().option.ToString());
                
                switch (option)
                {
                    case 1:
                        this._oVCreate = new V_Create(this._oViewModel);
                        break;
                    case 2:
                        this._oVExecute = new V_Execute(this._oViewModel);
                        break;
                    case 3:
                        this._oVEdit = new V_Edit(this._oViewModel);
                        break;
                    case 4:
                        this._oVDelete = new V_Delete(this._oViewModel);
                        break;
                    case 5:
                        this._oVSettings = new V_Settings(this._oViewModel);
                        break;
                    case 6:
                        Environment.Exit(0);
                        break;
                }
            }


        }
    }
}

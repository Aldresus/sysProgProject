using NSUtils;
using NSViewModel;
namespace NSViews
{
    public class V_Menu
    {
        private VM_ViewModel _oViewModel;
        private V_Create _oVCreate;
        private V_Delete _oVDelete;
        private V_Edit _oVEdit;
        private V_Execute _oVExecute;
        /*private V_Settings _oVSettings;*/

        public V_Menu(VM_ViewModel VM)
        {
            this._oViewModel = VM;
            U_Reader Reader = new U_Reader();
            while (true)
            {
                Console.Clear();
                Console.WriteLine("         ______________                                                                                   .----.      \r\n        |[]            |                                                                      .---------. | == |      \r\n        |  __________  |                  .                     .                     .       |.-\"\"\"\"\"-.| |----|      \r\n        |  |  Easy  |  |   .. ............;;.    .. ............;;.    .. ............;;.     ||ordina-|| | == |      \r\n        |  |  Save! |  |    ..::::::::::::;;;;.   ..::::::::::::;;;;.   ..::::::::::::;;;;.   ||  coeur|| |----|      \r\n        |  |________|  |   . . ::::::::::::;;:'  . . ::::::::::::;;:'  . . ::::::::::::;;:'   |'-.....-'| |::::|      \r\n        |   ________   |                  :'                    :'                    :'      `\"\")---(\"\"` |___.|      \r\n        |   [ [ ]  ]   |                                                                     /:::::::::::\\\" _  \"      \r\n        \\___[_[_]__]___|                                                                    /:::=======:::\\`\\`\\       \r\n                                                                                            `\"\"\"\"\"\"\"\"\"\"\"\"\"`  '-'      ");
                
                //TODO list all jobs
                Console.WriteLine("job 1"); //placeholder
                //TODO hide options if there isnt any job to edit/execute/delete
                Console.WriteLine("\n\n1 - create job");
                Console.WriteLine("2 - execute job");
                Console.WriteLine("3 - edit job");
                Console.WriteLine("4 - delete job");
                Console.WriteLine("5 - settings");
                Console.WriteLine("6 - exit");


                int option = Reader.ReadInt("\noption ");
                
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
                    /*    case 5:
                            this._oVSettings = new V_Edit(this._oViewModel);
                            break;*/
                    case 6:
                        Environment.Exit(0);
                        break;
                }
            }


        }
    }
}

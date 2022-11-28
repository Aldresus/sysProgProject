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
            
            U_Reader Reader = new U_Reader();

            //TODO move code bellow to MODEL to become new language or something
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = "Livrable1.Locales.locales.json";
            Stream stream = assembly.GetManifestResourceStream(resourceName);
            StreamReader reader = new StreamReader(stream);
            dynamic d = JObject.Parse(reader.ReadToEnd());
            
            while (true)
            {
                string locale = M.Get_language();
                Console.Clear();
                Console.WriteLine($"         ______________                                                                                   .----.      \r\n        |[]            |                                                                      .---------. | == |      \r\n        |  __________  |                  .                     .                     .       |.-\"\"\"\"\"-.| |----|      \r\n        |  |  Easy  |  |   .. ............;;.    .. ............;;.    .. ............;;.     ||{d[locale].ordina.ToString()}|| | == |      \r\n        |  |  Save! |  |    ..::::::::::::;;;;.   ..::::::::::::;;;;.   ..::::::::::::;;;;.   ||  {d[locale].coeur.ToString()}|| |----|      \r\n        |  |________|  |   . . ::::::::::::;;:'  . . ::::::::::::;;:'  . . ::::::::::::;;:'   |'-.....-'| |::::|      \r\n        |   ________   |                  :'                    :'                    :'      `\"\")---(\"\"` |___.|      \r\n        |   [ [ ]  ]   |                                                                     /:::::::::::\\\" _  \"      \r\n        \\___[_[_]__]___|                                                                    /:::=======:::\\`\\`\\       \r\n                                                                                            `\"\"\"\"\"\"\"\"\"\"\"\"\"`  '-'      ");
                
                //TODO list all jobs
                Console.WriteLine("job 1"); //placeholder
                //TODO hide options if there isnt any job to edit/execute/delete
                Console.WriteLine($"\n\n1 - {d[locale].createJob.ToString()}");
                Console.WriteLine($"2 - {d[locale].executeJob.ToString()}");
                Console.WriteLine($"3 - {d[locale].editJob.ToString()}");
                Console.WriteLine($"4 - {d[locale].deleteJob.ToString()}");
                Console.WriteLine($"5 - {d[locale].settings.ToString()}");
                Console.WriteLine($"6 - {d[locale].exit.ToString()}");


                int option = Reader.ReadInt(d[locale].option.ToString());
                
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

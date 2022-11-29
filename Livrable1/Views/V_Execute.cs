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
            this._oViewModel = VM;
            M_Model M = VM.Get_Model();

            U_Reader Reader = new U_Reader();
            U_Show Show = new U_Show();
            U_Checker Checker = new U_Checker();

            //TODO move code bellow to MODEL to become new language or something
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = "Livrable1.Locales.locales.json";
            Stream stream = assembly.GetManifestResourceStream(resourceName);
            StreamReader reader = new StreamReader(stream);
            dynamic d = JObject.Parse(reader.ReadToEnd());
            string locale = M.Get_language();

            //TODO check if there is at least 1 job
 
            if (Checker.CheckAnyJobs(M.Get_listSaveJob()) > 0)
            {
                bool validInput = false;


                while (!validInput)
                {
                    Console.Clear();
                    Console.WriteLine(d[locale].availableJobs.ToString());
                    Show.ShowJobs(M.Get_listSaveJob());
                    List<int> indexes = Reader.ReadMany(d[locale].enterJobIndexToDelete.ToString());
                    foreach (int i in indexes)
                    {
                        Console.Write($"{i} ");
                       /* M.Get_listSaveJob()[i].Execute();*/
                    }
                    Console.Write(d[locale].executed.ToString());
                    validInput = true;
                    Reader.PressAnyKeyToContinue(d[locale].pressAnyToContinue.ToString());
                }
            }
            else
            {
                Console.WriteLine(d[locale].noJob.ToString());
                Reader.PressAnyKeyToContinue(d[locale].pressAnyToContinue.ToString());
            }
        }
    }
}
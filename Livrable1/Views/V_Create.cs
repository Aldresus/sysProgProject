using Newtonsoft.Json.Linq;
using NSModel;
using NSUtils;
using NSViewModel;
using System.Reflection;

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
            
            string locale = M.Get_language();
            //TODO move code bellow to MODEL to become new language or something
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = "Livrable1.Locales.locales.json";
            Stream stream = assembly.GetManifestResourceStream(resourceName);
            StreamReader reader = new StreamReader(stream);
            dynamic d = JObject.Parse(reader.ReadToEnd());
            

            //TODO check if there is less than 5 save jobs
            if (Checker.CheckAnyJobs(M.Get_listSaveJob()) == 5)
            {
                Console.Clear();

                string name = Reader.ReadString(d[locale].enterJobName.ToString(), false);
                //print types
                Console.WriteLine("1 - type 1"); //placeholder
                Console.WriteLine("2 - type 2"); //placeholder
                int type = Reader.ReadInt(d[locale].enterJobType.ToString());
                string source = Reader.ReadString(d[locale].enterJobSource.ToString(), false);
                string dest = Reader.ReadString(d[locale].enterJobDestination.ToString(), false);
                Console.WriteLine(@$"name: {name} source:{source} dest:{dest} type:{type}");
                Reader.PressAnyKeyToContinue(d[locale].pressAnyToContinue.ToString());
            }
            else
            {
                {
                    Console.WriteLine(d[locale].maxJobReached.ToString());
                    Reader.PressAnyKeyToContinue(d[locale].pressAnyToContinue.ToString());
                }
            }
        }
    }
}

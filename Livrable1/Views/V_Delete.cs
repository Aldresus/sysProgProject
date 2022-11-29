using Newtonsoft.Json.Linq;
using NSModel;
using NSUtils;
using NSViewModel;
using System.Reflection;

namespace NSViews
{
    public class V_Delete
    {
        private VM_ViewModel _oViewModel;

        public V_Delete(VM_ViewModel VM)
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
            string locale = M.Get_language();
            //TODO check if there is at least 1 job
            if (true)
            {
                bool validInput = false;



                while (!validInput)
                {
                    
                    Console.Clear();
                    //TODO print all jobs currently available
                    Console.WriteLine("1 - job name"); //placeholder
                    List<int> indexes = Reader.ReadMany(d[locale].enterJobIndexToDelete.ToString());
                    foreach (int i in indexes)
                    {
                        Console.Write($"{i} ");
                    }

                    Console.Write(d[locale].deleted.ToString());
                    validInput = true;
                    Reader.PressAnyKeyToContinue(d[locale].pressAnyToContinue.ToString());
                    
                }
            }
        }
    }
}

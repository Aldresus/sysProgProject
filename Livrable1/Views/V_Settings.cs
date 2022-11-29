using Newtonsoft.Json.Linq;
using NSUtils;
using NSViewModel;
using NSModel;
using System;
using System.IO;
using System.Reflection;

namespace NSViews
{
    public class V_Settings
    {
        private VM_ViewModel _oViewModel;

        public V_Settings(VM_ViewModel VM)
        {
            this._oViewModel = VM;
            M_Model M = VM.Get_Model();
            U_Reader Reader = new U_Reader();
            bool validInput = false;

            //TODO move code bellow to MODEL to become new language or something
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = "Livrable1.Locales.locales.json";
            Stream stream = assembly.GetManifestResourceStream(resourceName);
            StreamReader reader = new StreamReader(stream);
            dynamic d = JObject.Parse(reader.ReadToEnd());

            while (!validInput)
            {
                int i = 1;
                List<string> languages = new List<string>();
                foreach (JProperty property in d.Properties())
                {
                    Console.WriteLine($"{i} - {property.Name}");
                    languages.Add(property.Name);
                    i++;
                }

                int option = Reader.ReadInt(M.Get_language().selectLanguage.ToString());
                if(option>=1 && option <= i-1)
                {
                    M.Set_language(d[languages[option - 1]]);
                    Console.WriteLine(d[languages[option - 1]].languageChanged.ToString());
                    validInput = true;
                    M.WriteLanguage(languages[option - 1]);
                    Reader.PressAnyKeyToContinue(d[languages[option - 1]].pressAnyToContinue.ToString());
                }
                //TODO change the param in model
            }
            


        }
    }
}
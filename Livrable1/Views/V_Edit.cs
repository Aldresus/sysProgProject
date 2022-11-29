using Newtonsoft.Json.Linq;
using NSModel;
using NSUtils;
using NSViewModel;
using System;
using System.Reflection;

namespace NSViews
{

    public class V_Edit
    {
        private VM_ViewModel _oViewModel;

        public V_Edit(VM_ViewModel VM)
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

            if (Checker.CheckAnyJobs(M.Get_listSaveJob()) > 0)//TODO check if there is jobs
            {

                bool validInput = false;

                while (!validInput)
                {
                    //TODO print all jobs currently available
                    Console.WriteLine(d[locale].availableJobs.ToString());
                    Show.ShowJobs(M.Get_listSaveJob());
                    int job = Reader.ReadInt(d[locale].whichJobToEdit.ToString());

                    //check if job exists
                    if (job >= 1 && job <= 5)//TODO change 5 by the total of jobs
                    {
                        string name = Reader.ReadString(d[locale].enterJobName.ToString(), false);
                        //TODO print types
                        Console.WriteLine("1 - type 1"); //placeholder
                        Console.WriteLine("2 - type 2"); //placeholder
                        int type = Reader.ReadInt(d[locale].enterJobType.ToString());
                        string source = Reader.ReadString(d[locale].enterJobSource.ToString(), false);
                        string dest = Reader.ReadString(d[locale].enterJobDestination.ToString(), false);
                        Console.WriteLine(@$"name: {name} source:{source} dest:{dest} type:{type}"); //placeholder
                        validInput = true;
                        Reader.PressAnyKeyToContinue(d[locale].pressAnyToContinue.ToString());
                    }
                    else
                    {
                        Console.WriteLine(d[locale].indexOutOfRange.ToString());
                    }
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

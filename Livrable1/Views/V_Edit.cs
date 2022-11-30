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
            _oViewModel = VM;
            M_Model M = VM.Get_Model();

            U_Reader Reader = new U_Reader(M);
            U_Show Show = new U_Show();
            U_Checker Checker = new U_Checker();

            if (Checker.CheckAnyJobs(M.Get_listSaveJob()) > 0)//TODO check if there is jobs
            {

                bool validInput = false;

                while (!validInput)
                {
                    Console.Clear();
                    //TODO print all jobs currently available
                    Console.WriteLine(M.Get_language().availableJobs.ToString());
                    Show.ShowJobs(M.Get_listSaveJob());
                    int i = Reader.ReadInt($"{M.Get_language().whichJobToEdit.ToString()}, {M.Get_language().enterZeroToAbort.ToString()}");

                    //check if job exists
                    if (i >= 0 && i <= 5)//TODO change 5 by the total of jobs
                    {
                        if (i == 0)
                        {
                            break;
                        }
                        else
                        {
                            string name = Reader.ReadString(M.Get_language().enterJobName.ToString(), false);
                            //print save types
                            Console.WriteLine($"1 - {M.Get_language().fullSave.ToString()}"); //placeholder
                            Console.WriteLine($"2 - {M.Get_language().differentialSave.ToString()}"); //placeholder

                            int type = Reader.ReadInt(M.Get_language().enterJobType.ToString());
                            string source = Reader.ReadPath(M.Get_language().enterJobSource.ToString(), false);
                            string dest = Reader.ReadPath(M.Get_language().enterJobDestination.ToString(), true);
                            Console.WriteLine($"{M.Get_language().jobCreated.ToString()} {M.Get_language().name.ToString()}: {name} {M.Get_language().sourceFolder.ToString()}:{source} {M.Get_language().destinationFolder.ToString()}:{dest} {M.Get_language().type.ToString()}:{type}");
                            validInput = true;

                            M.Get_listSaveJob()[i - 1].Set_saveJobName(name);
                            M.Get_listSaveJob()[i - 1].Set_saveJobSourceDirectory(source);
                            M.Get_listSaveJob()[i - 1].Set_saveJobDestinationDirectory(dest);
                            M.Get_listSaveJob()[i - 1].Set_saveJobType(type);
                            M.GetSelectedSaveJob(i - 1).WriteJSON(M.Get_workFile());
                            Reader.PressAnyKeyToContinue(M.Get_language().pressAnyToContinue.ToString());
                        }
                    }
                    else
                    {
                        Console.WriteLine(M.Get_language().indexOutOfRange.ToString());
                    }
                }
            }
            else
            {
                Console.WriteLine(M.Get_language().noJob.ToString());
                Reader.PressAnyKeyToContinue(M.Get_language().pressAnyToContinue.ToString());
            }
        }
    }
}

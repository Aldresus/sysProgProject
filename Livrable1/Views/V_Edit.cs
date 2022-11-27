using NSUtils;
using NSViewModel;
using System;

namespace NSViews
{

    public class V_Edit
    {
        private VM_ViewModel _oViewModel;

        public V_Edit(VM_ViewModel VM)
        {
            //TODO language
            this._oViewModel = VM;
            U_Reader Reader = new U_Reader();

            if (true)//TODO check if there is jobs
            {

                bool validInput = false;

                while (!validInput)
                {
                    //TODO print all jobs currently available
                    Console.WriteLine("1 - job name"); //placeholder
                    int job = Reader.ReadInt("Which job do you want to edit?");

                    //check if job exists
                    if (job >= 1 && job <= 5)//TODO change 5 by the total of jobs
                    {
                        string name = Reader.ReadString("name", false);
                        //TODO print types
                        Console.WriteLine("1 - type 1"); //placeholder
                        Console.WriteLine("2 - type 2"); //placeholder
                        int type = Reader.ReadInt("type");
                        string source = Reader.ReadString("source", false);
                        string dest = Reader.ReadString("dest", false);
                        Console.WriteLine(@$"name: {name} source:{source} dest:{dest} type:{type}"); //placeholder
                        validInput = true;
                        Reader.PressAnyKeyToContinue();
                    }
                    else
                    {
                        Console.WriteLine("index out of bound");
                    }
                }
            }
        }
    }
}

using NSUtils;
using NSViewModel;
using NSModel;
namespace NSViews
{
    public class V_Execute
    {
        private VM_ViewModel _oViewModel;

        public V_Execute(VM_ViewModel VM)
        {
            //TODO language
            this._oViewModel = VM;
            U_Reader Reader = new U_Reader();

            //TODO check if there is at least 1 job
            if (true)
            {
                bool validInput = false;


                while (!validInput)
                {
                    Console.Clear();
                    //TODO print all jobs currently available
                    Console.WriteLine("1 - job name"); //placeholder
                    List<int> indexes = Reader.ReadMany("index csv '1,2,3'");
                    foreach (int i in indexes)
                    {
                        Console.Write($"{i} ");
                        if (i == 1)
                        {
                            //TODO execute job
                            M_SaveJob a = new M_SaveJob();
                            a.Set_saveJobType("FullSave");
                            a.Execute();
                            a.Set_saveJobType("DiffentialSave");
                            a.Execute();
                        }
                        else
                        {
                             Console.Write("executed\n");
                        }
                    }
                    validInput = true;
                    Reader.PressAnyKeyToContinue();
                }
            }
        }
    }
}

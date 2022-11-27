using NSUtils;
using NSViewModel;
namespace NSViews
{
    public class V_Delete
    {
        private VM_ViewModel _oViewModel;

        public V_Delete(VM_ViewModel VM)
        {
            //TODO language
            this._oViewModel = VM;
            U_Reader Reader = new U_Reader();

            //TODO check if there is at least 1 view
            if (true)
            {
                bool validInput = false;

                //TODO print all jobs currently available
                Console.WriteLine("1 - job name"); //placeholder

                while (!validInput)
                {
                    int job = Reader.ReadInt("view to del");
                    //check if job exists
                    if (job >= 1 && job <= 5)//TODO change 5 by the total of jobs
                    {
                        Console.WriteLine($"\ndeleted {job}");
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

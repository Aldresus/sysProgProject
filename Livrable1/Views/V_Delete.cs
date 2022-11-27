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
                    Console.Clear();
                    List<int> indexes = Reader.ReadMany("index csv '1,2,3'");
                    foreach (int i in indexes)
                    {
                        Console.Write($"{i} ");
                    }

                    Console.Write("deleted\n");
                    Reader.PressAnyKeyToContinue();
                }
            }
        }
    }
}

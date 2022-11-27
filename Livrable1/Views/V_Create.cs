using NSViewModel;
using NSUtils;
namespace NSViews
{
    public class V_Create
    {
        private VM_ViewModel _oViewModel;

        public V_Create(VM_ViewModel VM)
        {
            //TODO language
            this._oViewModel = VM;
            U_Reader Reader = new U_Reader();

            //TODO check if there is less than 5 save jobs
            if (true)
            {
                Console.Clear();
                string name = Reader.ReadString("name", false);
                int type = Reader.ReadInt("type");
                string source = Reader.ReadString("source", false);
                string dest = Reader.ReadString("dest", false);
                Console.WriteLine(@$"name: {name} source:{source} dest:{dest} type:{type}");
            }
        }
    }
}

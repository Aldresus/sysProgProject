// Class ViewModel
// Interacts with the View and the Model
namespace NSViewModel
{
    public class VM_ViewModel
    {
        private string? _name;
        private string? _sourceDirectory;
        private string? _destinationDirectory;
        private string? _type;

        public string Get_Name()
        {
            return _name;
        }

        public string Get_SourceDirectory()
        {
            return _sourceDirectory;
        }

        public string Get_DestinationDirectory()
        {
            return _destinationDirectory;
        }

        public string Get_Type()
        {
            return _type;
        }

        public void Set_Name(string name)
        {
            _name = name;
        }

        public void Set_SourceDirectory(string sourceDirectory)
        {
            _sourceDirectory = sourceDirectory;
        }

        public void Set_DestinationDirectory(string destinationDirectory)
        {
            _destinationDirectory = destinationDirectory;
        }

        public void Set_Type(string type)
        {
            _type = type;
        }
        // Constructor for empty values
        public VM_ViewModel()
        {
        }
        // Constructor
        public VM_ViewModel(string name, string sourceDirectory, string destinationDirectory, string type)
        {
            _name = name;
            _sourceDirectory = sourceDirectory;
            _destinationDirectory = destinationDirectory;
            _type = type;

        }

        // Method to update data
        public void Update(string name, string sourceDirectory, string destinationDirectory, string type)
        {
            this.Set_Name(name);
            this.Set_SourceDirectory(sourceDirectory);
            this.Set_DestinationDirectory(destinationDirectory);
            this.Set_Type(type);
        }
    }
}
// Class ViewModel
// Interacts with the View and the Model

public class VM_Class
{
    private string _name;
    private string _sourceDirectory;
    private string _destinationDirectory;
    private string _type;

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

    public VM_Class(string name, string sourceDirectory, string destinationDirectory, string type)
    {
        _name = name;
        _sourceDirectory = sourceDirectory;
        _destinationDirectory = destinationDirectory;
        _type = type;
    }

    public static void Update()
    {
        // Code to update view
    }
}

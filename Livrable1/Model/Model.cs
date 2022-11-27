//Class Model
//Description : This class is used to write log file and to move files about different save.

public class Model
{
    private List<M_SaveJob> _listSaveJob;
    private FileStream _logFile;
    private FileStream _workFile;
    private string _language;

    //Getter and Setter
    
    //Getter _listSaveJob
    public List<M_SaveJob> Get_listSaveJob()
    {
        return _listSaveJob;
    }

    //Setter _listSaveJob
    public void Set_ListSaveJob(List<M_SaveJob> values)
    {
        this._listSaveJob = values;
    }

    //Getter _logFile
    public FileStream Get_logFile() {
        return _logFile;
    }

    //Setter _logFile
    public void Set_logFile(FileStream value)
    {
        _logFile = value;
    }

    //Getter _workFile
    public FileStream Get_workFile()
    {
        return _workFile;
    }

    //Setter _workFile
    public void Set_workFile(FileStream value)
    {
        _workFile = value;
    }

    //Getter _language
    public string Get_language()
    {
        return _language;
    }

    //Setter _language
    public void Set_language(string value)
    {
        _language = value;
    }

    
}
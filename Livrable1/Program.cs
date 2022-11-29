// See https://aka.ms/new-console-template for more information
using System;
using System.Text;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using NSModel;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {

            M_Model test = new M_Model();
            Console.ReadLine();
            /*M_MovingFile movingFile = new M_MovingFile();
            movingFile.Set_time(DateTime.Now);
            movingFile.Set_fileSource(@"C:/Folder/test");
            movingFile.Set_fileDestination(@"C:/Folder2/test");
            movingFile.Set_fileSize(100);

            M_SaveJob saveJob = new M_SaveJob();
            saveJob.Set_saveJobName(@"SaveJobtest");
            saveJob.Set_saveJobSourceDirectory(@"C:/Folder");
            saveJob.Set_saveJobDestinationDirectory(@"C:/Folder2");
            saveJob.Set_saveJobType(@"Complete");
            saveJob.Set_file(movingFile);
            saveJob.Set_state(@"Running");
            saveJob.Set_totalNbFile(4);
            saveJob.Set_totalSizeFile(1000);

            M_SaveJob saveJob2 = new M_SaveJob();
            saveJob2.Set_saveJobName(@"SaveJobtest2");
            saveJob2.Set_saveJobSourceDirectory(@"C:/Folder");
            saveJob2.Set_saveJobDestinationDirectory(@"C:/Folder2");
            saveJob2.Set_saveJobType(@"Complete");
            saveJob2.Set_file(movingFile);
            saveJob2.Set_state(@"Running");
            saveJob2.Set_totalNbFile(4);
            saveJob2.Set_totalSizeFile(2000);

            List<M_SaveJob> saveJobs = new List<M_SaveJob>();
            M_Model model = new M_Model();
            model.Set_ListSaveJob(saveJobs);
            model.SetSelectedSaveJob(0, saveJob);
            model.SetSelectedSaveJob(1, saveJob2);
            model.Set_logFile(@"C:\Users\tomst\OneDrive\Bureau\fileLog.json");

            model.WriteLog(0);*/


            /*JObject o1 = JObject.Parse(File.ReadAllText(@"C:\Users\tomst\OneDrive\Bureau\file.json"));
            Console.WriteLine(o1["SaveJob"][0]);
            Console.ReadLine();
            o1["SaveJob"][0]["Name"] = "test";
            Console.WriteLine(o1["SaveJob"][0]);
            Console.ReadLine();*/

            
        }
    }
}
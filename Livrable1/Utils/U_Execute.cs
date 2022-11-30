using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Text;
using System.IO;

namespace NSUtils
{
    public class U_Execute
    {
        public void Execute(string source, string destination, bool isFullSave, string FileLogPath)
        {
            
            Console.WriteLine("Source: " + source);
            Console.WriteLine("Destination: " + destination);

            string fileName;
            string destFile;

            // à redéfinir en argument
            //string sourcePath = @"C:\Users\Public\TestFolder\";
            //string targetPath = @"C:\Users\Public\TestFolder2\";

            string sourcePath = source;
            string targetPath = destination;

            // Check if the source directory exists.
            if (System.IO.Directory.Exists(sourcePath))
            {
                // Create a new target folder.
                // If the directory already exists, this method does not create a new directory.
                System.IO.Directory.CreateDirectory(targetPath);

                // Get files in source directory
                string[] files = System.IO.Directory.GetFiles(sourcePath);

                foreach (string file in files)
                {
                    // Use static Path methods to extract only the file name from the path.
                    fileName = System.IO.Path.GetFileName(file);
                    destFile = System.IO.Path.Combine(targetPath, fileName);

                    try
                    {
                        // Console.WriteLine(file);
                        // Copy the files and overwrite destination files if they already exist.
                        DateTime startCopyTime = DateTime.Now;
                        System.IO.File.Copy(file, destFile, isFullSave);
                        DateTime endCopyTime = DateTime.Now;
                        TimeSpan copyTime = endCopyTime - startCopyTime;
                        WriteLog(FileLogPath, fileName, source + fileName, destFile, source, copyTime);
                    }
                    catch (Exception e)
                    {
                        Console.Out.WriteLine(e.Message + "\n\nStackTrace : " + e.StackTrace + "\n\nInnerException :" + e.InnerException);
                    }
                }

                // get subdirectories in the source directory
                DirectoryInfo di = new DirectoryInfo(sourcePath);
                DirectoryInfo[] arrDir = di.GetDirectories();

                foreach (DirectoryInfo dir in arrDir)
                {
                    // Get all files in the sub directory iteratively
                    string[] subFiles = System.IO.Directory.GetFiles(dir.FullName);
                    foreach (string file in subFiles)
                    {
                        // Create new sub folders in target directory.
                        string targetNewFolder = System.IO.Path.Combine(targetPath, dir.Name);
                        System.IO.Directory.CreateDirectory(targetNewFolder);

                        // Use static Path methods to extract only the file name from the path.
                        fileName = System.IO.Path.GetFileName(file);
                        destFile = System.IO.Path.Combine(targetNewFolder, fileName);

                        try
                        {
                            // Console.WriteLine(destFile);
                            // Copy the files and overwrite destination files if they already exist.
                            DateTime startCopyTime = DateTime.Now;
                            System.IO.File.Copy(file, destFile, isFullSave);
                            DateTime endCopyTime = DateTime.Now;
                            TimeSpan copyTime = endCopyTime - startCopyTime;
                            WriteLog(FileLogPath, fileName, source + dir.Name, destFile, source, copyTime);
                        }
                        catch (Exception e)
                        {
                            Console.Out.WriteLine(e.Message + "\n\nStackTrace : " + e.StackTrace + "\n\nInnerException :" + e.InnerException);
                        }
                    }
                }
            }

            else
            {
                Console.WriteLine("Source path does not exist!");
            }
        }

        //TODO : d�placer la m�thode dans M_SaveJob et modifier les WriteValue n�cessaires.
        public void WriteLog(string JsonLogPath, string fileName, string fileSourcePath, string fileDestPath, string directorySource, TimeSpan copyTime)
        {
            long size;
            //Get fileinfo
            try
            {
                FileInfo fileInfo = new FileInfo(fileSourcePath);
                size = fileInfo.Length;
            }
            catch (Exception)
            {
                FileInfo fileInfo = new FileInfo(fileSourcePath+"\\"+fileName);
                size = fileInfo.Length;
            }

            //Get JSON file's content
            JObject allLog = JObject.Parse(File.ReadAllText(JsonLogPath));

            StringBuilder sb = new StringBuilder();
            StringWriter sw = new StringWriter(sb);
            JsonWriter writer = new JsonTextWriter(sw);
            writer.Formatting = Formatting.Indented;
            writer.WriteStartObject();
            writer.WritePropertyName("Name");
            writer.WriteValue(fileName);
            writer.WritePropertyName("FileSource");
            writer.WriteValue(fileSourcePath);
            writer.WritePropertyName("FileTarget");
            writer.WriteValue(fileDestPath);
            writer.WritePropertyName("destPath");
            writer.WriteValue(directorySource);
            writer.WritePropertyName("FileSize");
            writer.WriteValue(size);
            writer.WritePropertyName("FileTransferTime");
            writer.WriteValue(copyTime);
            writer.WritePropertyName("time");
            writer.WriteValue(DateTime.Now);
            writer.WriteEndObject();

            //Convert object JsonWriter to string
            string json = sb.ToString();

            //Convert string to JObject
            JObject newLog = JObject.Parse(json);

            //Get JObject "logs" of Json Lof file
            JArray arrayLogs = (JArray)allLog["logs"];

            //Add newLog to allLog
            arrayLogs.Add(newLog);

            //Convert object JObject to string
            string newLogFile = allLog.ToString();

            //Write newLogFile string to log JSON file
            File.WriteAllText(JsonLogPath, newLogFile);
        }
    }
}
namespace NSUtils
{
    public class U_Execute
    {
        public void Execute(string source, string destination, bool isFullSave)
        {
            
            Console.WriteLine("Source: " + source);
            Console.WriteLine("Destination: " + destination);

            string fileName;
            string destFile;

            // à redéfinir en argument
            string sourcePath = @"C:\Users\Public\TestFolder\";
            string targetPath = @"C:\Users\Public\TestFolder2\";

            //string sourcePath = source;
            //string targetPath = destination;

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
                        System.IO.File.Copy(file, destFile, isFullSave);
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
                            System.IO.File.Copy(file, destFile, isFullSave);
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
    }
}
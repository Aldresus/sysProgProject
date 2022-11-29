//Class Strategy
//Description : This Class is used to select the correct type of save to execute via the strategy design pattern

using System.Collections.Generic;
using System;
using NSViews;
using System.Diagnostics.Tracing;

namespace NSModel
{
    public interface IStrategy
    {
        public void Execute(string source, string destination);
    }

    class DiffentialSave : IStrategy
    {
        public void Execute(string source, string destination)
        {
            Console.WriteLine("Diff strategy selected");
            Console.WriteLine("Source: " + source);
            Console.WriteLine("Destination: " + destination);
        }
    }
    
    
    class FullSave : IStrategy
    {
        public void Execute(string source, string destination)
        {
            Console.WriteLine("Full strategy selected");
            Console.WriteLine("Source: " + source);
            Console.WriteLine("Destination: " + destination);
        }
    }
}
/* string fileName;
 string destFile;

 // à redéfinir en argument
 //string sourcePath = @"C:\Users\Public\TestFolder\";
 //string targetPath = @"C:\Users\Public\TestFolder2\";

 string sourcePath = source;
 string targetPath = destination;

 // Create a new target folder.
 // If the directory already exists, this method does not create a new directory.
 System.IO.Directory.CreateDirectory(targetPath);

 // Check if the source directory exists.
 if (System.IO.Directory.Exists(sourcePath))
 {
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
             System.IO.File.Copy(file, destFile, true);
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
                 System.IO.File.Copy(file, destFile, true);
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
*/

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NSModel;
namespace NSUtils
{
    public class U_Reader
    {

        public U_Reader()
        {
        }

        public int ReadInt(string promptText)
        {

            bool validInput = false;

            while (!validInput)
            {
                Console.WriteLine(promptText);
                string userInput = Console.ReadLine();

                if (userInput.Length > 0)
                {
                    try
                    {
                        int output = int.Parse(userInput);
                        validInput = true;
                        return output;
                    }
                    catch (System.FormatException e)
                    {
                    }
                    
                }
                else
                {
                }
            }
            return -1; // should never happen
        }
        public string ReadString(string promptText, bool canBeEmpty)
        {

            bool validInput = false;

            while (!validInput)
            {

                if (promptText.Length == 0 && canBeEmpty)
                {
                    validInput = true;
                    return promptText;
                }
                else if (promptText.Length > 0)
                {
                    validInput = true;
                    return promptText;
                }
                else
                {
                }
            }
            return null; // should never happen
        }
        public List<int> ReadMany(string promptText, M_Model model)
        {

            bool validInput = false;

            while (!validInput)
            {
                Console.WriteLine(promptText);
                string userInput = Console.ReadLine();

                if (userInput.Length > 0)
                {

                    string[] temp = userInput.Replace(" ", "").Split(",");
                    List<int> output = new List<int>();
                    try
                    {
                        foreach (string i in temp)
                        {
                            int j = int.Parse(i);
                            if (j >= 0 && j <= model.Get_listSaveJob().Count())
                            {
                                output.Add(j);
                            }
                            else
                            {
                                output.Clear();
                                break;
                            }
                        }
                        if (output.Any())
                        {
                            validInput = true;
                            return output;
                        }
                    }
                    catch (System.FormatException e)
                    {
                    }


                }
                else
                {
                }
            }
            return null; // should never happen
        }


        public void PressAnyKeyToContinue(string text)
        {
            Console.WriteLine($"\n\n{text}");
            Console.ReadKey();
        }

        public string ReadPath(string promptText, bool isDest)
        {
            bool validInput = false;

            while (!validInput)
            {
                string path = this.ReadString(promptText, false);
                if (path[^1] is not '\\' or not '/')
                {
                    path += @"\";
                }
                if (File.Exists(path) || Directory.Exists(path) || isDest)
                {
                    validInput = true;
                    return path.Replace(@"/", @"\");
                }
                else
                {
                }
            }
            return null; // should never happen
        }
    }
}
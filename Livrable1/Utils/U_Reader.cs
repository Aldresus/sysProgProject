using System.Collections.Generic;
namespace NSUtils
{
    public class U_Reader
    {

        public int ReadInt(string promptText)
        {
            //TODO translate error messages

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
                        Console.WriteLine("NaN");
                    }
                }
                else
                {
                    Console.WriteLine("cannot be empty");
                }
            }
            return -1; // should never happen
        }
        public string ReadString(string promptText, bool canBeEmpty)
        {
            //TODO translate error messages

            bool validInput = false;

            while (!validInput)
            {
                Console.WriteLine(promptText);
                string userInput = Console.ReadLine();

                if (userInput.Length == 0 && canBeEmpty)
                {
                    validInput = true;
                    return userInput;
                }
                else if (userInput.Length > 0)
                {
                    validInput = true;
                    return userInput;
                }
                else
                {
                    Console.WriteLine("cannot be empty");
                }
            }
            return null; // should never happen
        }
        public List<int> ReadMany(string promptText)
        {
            //TODO translate error messages

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
                            if (j >= 1 && j <= 5)
                            {
                                output.Add(j);
                            }
                            else
                            {
                                Console.WriteLine("index out of range");
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
                        Console.WriteLine("invalid");
                    }


                }
                else
                {
                    Console.WriteLine("cannot be empty");
                }
            }
            return null; // should never happen
        }


        public void PressAnyKeyToContinue()
        {
            Console.WriteLine("\n\nPress any key to continue");
            Console.ReadKey();
        }
    }
}
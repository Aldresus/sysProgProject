using System;
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
            return "errored"; // should never happen
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;


namespace CryptoSoft
{
    class Program
    {
        static void Main(string[] args)
        {

            // Function that takes in the source file to encrypt (or decrypt) to a destination file
            void EncryptFile(string inputFile, string outputFile)
            {
                try
                {
                    // Opens the source file, and creates or replaces the destination file if already existing
                    using (var fin = new FileStream(inputFile, FileMode.Open))
                    using (var fout = new FileStream(outputFile, FileMode.Create))
                    {
                        // Buffer is used to read the source file in chunks of 4096 bytes (to avoid memory issues)
                        byte[] buffer = new byte[4096];
                        while (true)
                        {
                            // Reads the source file using the buffer and returns the number of bytes read
                            int bytesRead = fin.Read(buffer);
                            if (bytesRead == 0)
                                // If no bytes were read, then the end of the file has been reached, so the loop is broken
                                break;
                            // Encrypts the bytes read in the buffer
                            EncryptBytes(buffer, bytesRead);
                            // Writes the encrypted bytes to the destination file
                            fout.Write(buffer, 0, bytesRead);
                        }
                    }
                }
                catch(Exception e)
                {
                    Console.WriteLine(e); //temporaire
                    Console.ReadLine();
                }
            }

            // key used to encrypt and decrypt the file
            const byte Secret = 183;

            void EncryptBytes(byte[] buffer, int count)
            {
                // Encrypts then replaces the bytes in the buffer
                for (int i = 0; i < count; i++)
                    buffer[i] = (byte)(buffer[i] ^ Secret);
            }

            EncryptFile(args[0], args[1]);
        }
    }
}

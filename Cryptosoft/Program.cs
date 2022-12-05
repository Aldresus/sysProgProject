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

            void EncryptFile(string inputFile, string outputFile)
            {
                using (var fin = new FileStream(inputFile, FileMode.Open))
                using (var fout = new FileStream(outputFile, FileMode.Create))
                {
                    byte[] buffer = new byte[4096];
                    while (true)
                    {
                        int bytesRead = fin.Read(buffer);
                        if (bytesRead == 0)
                            break;
                        EncryptBytes(buffer, bytesRead);
                        fout.Write(buffer, 0, bytesRead);
                    }
                }
            }

            const byte Secret = 183;

            void EncryptBytes(byte[] buffer, int count)
            {

                for (int i = 0; i < count; i++)
                    buffer[i] = (byte)(buffer[i] ^ Secret);
            }

            EncryptFile(@"C:\\Users\\tomst\\OneDrive\\Bureau\\cesi_CMJN_blanc.png", @"C:\\Users\\tomst\\OneDrive\\Bureau\\cesi_CMJN_blanc2.png");
            //EncryptFile(@"C:\\Users\\tomst\\OneDrive\\Bureau\\cesi_CMJN_blanc2.png", @"C:\\Users\\tomst\\OneDrive\\Bureau\\cesi_CMJN_blanc3.png");

        }
    }
}

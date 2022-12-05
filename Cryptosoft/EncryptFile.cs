using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace EncryptClassXor
{
    class EncryptFilesXor
    {
        private string _inputFile;
        private string _outputFile;
        private byte[] _encryptKey;

        //Constructor
        public EncryptFilesXor(string inputFile, string outputFile, byte[] encryptKey)
        {
            Set_inputFile(inputFile);
            Set_outputFile(outputFile);
            Set_encryptKey(encryptKey);
        }

        //Setter and Getter 

        public string Get_inputFile()
        {
            return _inputFile;
        }
        
        public void Set_inputFile(string value)
        {
            this._inputFile = value;
        }

        public string Get_outputFile()
        {
            return _outputFile;
        }

        public void Set_outputFile(string value)
        {
            this._outputFile = value;
        }

        public byte[] Get_encryptKey()
        {
            return _encryptKey;
        }
        
        public byte Get_encryptKey(int value)
        {
            return _encryptKey[value];
        }

        public void Set_encryptKey(byte[] value)
        {
            this._encryptKey = value;
        }

        // Function that takes in the source file to encrypt (or decrypt) to a destination file
        public void EncryptFile()
        {
            try
            {
                // Opens the source file, and creates or replaces the destination file if already existing
                using (var fin = new FileStream(this.Get_inputFile(), FileMode.Open))
                using (var fout = new FileStream(this.Get_outputFile(), FileMode.Create))
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
            catch (Exception e)
            {
                Console.WriteLine(e); //temporaire
                Console.ReadLine();
            }
            void EncryptBytes(byte[] buffer, int count)
            {
                // Encrypts then replaces the bytes in the buffer
                for (int i = 0; i < count; i++)
                    buffer[i] = (byte)(buffer[i] ^ this.Get_encryptKey(i % this.Get_encryptKey().Length));
            }
        }
    }
}
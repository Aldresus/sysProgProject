using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using EncryptClassXor;


namespace CryptoSoft
{
    class Program
    {
        static void Main(string[] args)
        {
            EncryptFilesXor encrypt = new EncryptFilesXor(args[0], args[1], Encoding.UTF8.GetBytes(args[2]));
            encrypt.EncryptFile();
        }
    }
}

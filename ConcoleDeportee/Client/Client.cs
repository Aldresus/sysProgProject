using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows;

namespace NSClient
{
    class Client
    {
        private Socket clientSocket;

        public static Socket SeConnecter()
        {
            EndPoint serverEndPoint = new IPEndPoint(IPAddress.Parse("192.168.1.13"), 50000);
            Socket clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            clientSocket.Bind(new IPEndPoint(IPAddress.Parse("192.168.1.13"), 50002));
            clientSocket.Connect(serverEndPoint);
            //Console.WriteLine("Client is connected to server");
            MessageBox.Show("Connecte au serveur");
            return clientSocket;
        }

        public static string EcouterReseau(Socket client)
        {
            bool nothingCame = true;
            string message = "";
            while (nothingCame)
            {
                while (client.Available == 0)
                {
                }

                byte[] buffer = new byte[1024];
                while (client.Available != 0)
                {
                    int nbOctetsRecus = client.Receive(buffer);
                    message += System.Text.Encoding.ASCII.GetString(buffer, 0, nbOctetsRecus);
                }
                nothingCame = false;
            }
            return message;
        }

        public static void EnvoyerMessage(Socket client)
        {
            while (true)
            {
                string messageEnvoye = Console.ReadLine();
                client.Send(System.Text.Encoding.ASCII.GetBytes(messageEnvoye));
            }
        }

        public static void Deconnecter(Socket socket)
        {
            socket.Close();
        }
    }
}
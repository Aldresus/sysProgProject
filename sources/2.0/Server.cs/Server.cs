using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows;

namespace NSServer
{
    class Server
    {
        public static Socket SeConnecter()
        {
            Socket serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            serverSocket.Bind(new IPEndPoint(IPAddress.Parse("192.168.1.13"), 50000));
            serverSocket.Listen(1);
            return serverSocket;
        }

        public static Socket AccepterConnexion(Socket serverSocket)
        {
            Socket socket = serverSocket.Accept();
            MessageBox.Show("Server is connected to and TcpClient");
            return socket;
        }


        public string EcouterReseau(Socket server)
        {
            while (true)
            {
                string message = "";
                while (server.Available == 0)
                {
                }
                    
                byte[] buffer = new byte[1024];
                while (server.Available != 0)
                {
                    int nbOctetsRecus = server.Receive(buffer);
                    message += System.Text.Encoding.ASCII.GetString(buffer, 0, nbOctetsRecus);
                }
                return message;
            }
        }

        public static void EnvoyerMessage(Socket server, string datacontext)
        {
            server.Send(System.Text.Encoding.ASCII.GetBytes(datacontext));
            MessageBox.Show("Message envoye");
        }

        public static void Deconnecter(Socket socket)
        {
            socket.Close();
        }
    }
}
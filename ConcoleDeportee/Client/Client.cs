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
        private string _receiveMessage;

        public string Get_receiveMessage()
        {
            return _receiveMessage;
        }

        public void Set_receiveMessage(string value)
        {
            this._receiveMessage = value;
        }

        public static Socket SeConnecter()
        {
            EndPoint serverEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 50000);
            Socket clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            //clientSocket.Bind(new IPEndPoint(IPAddress.Parse("192.168.1.13"), 50002));
            clientSocket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
            clientSocket.Connect(serverEndPoint);
            //MessageBox.Show("Connecte au serveur");
            return clientSocket;
        }

        public string EcouterReseau(Socket client)
        {
            while (true)
            {
                string message = "";
                try
                {
                    while (client.Available == 0)
                    {
                    }
                    //MessageBox.Show("Message reçu client");
                    byte[] buffer = new byte[1024];
                    while (client.Available != 0)
                    {
                        int nbOctetsRecus = client.Receive(buffer);
                        message += System.Text.Encoding.ASCII.GetString(buffer, 0, nbOctetsRecus);
                        if (message.Substring(0, 8) == "Progress")
                        {
                            string progress = message.Substring(0, message.IndexOf("____"));
                            message = message.Remove(0, message.IndexOf("____")+4);
                            return progress;
                        }
                        else
                        {
                        }
                    }
                    return message;
                }
                catch (Exception e)
                {
                    return null;
                }
            }
        }
        
        public static void EnvoyerMessage(Socket client, string message)
        {
                client.Send(System.Text.Encoding.ASCII.GetBytes(message));
        }

        public static void Deconnecter(Socket socket)
        {
            socket.Disconnect(true);
        }
    }
}
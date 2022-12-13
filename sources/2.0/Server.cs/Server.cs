using Newtonsoft.Json.Linq;
using NSModel;
using NSViewModel;
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
namespace NSServer
{
    public class Server
    {
        private M_Model _model;
        private VM_ViewModel _viewModel;
        private Socket _serverSocket;
        private Socket _socket;
        private string _receivedMessage;
        public Server(M_Model model, VM_ViewModel viewModel)
        {
            this._model = model;
            this._viewModel = viewModel;
        }
        public Socket Get_socket()
        {
            return _socket;
        }
        public static Socket SeConnecter()
        {
            Socket serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            serverSocket.Bind(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 40000));
            serverSocket.Listen(2);
            return serverSocket;
        }
        public static Socket AccepterConnexion(Socket serverSocket)
        {
            Socket socket = serverSocket.Accept();
            //MessageBox.Show("Server is connected to and TcpClient");
            return socket;
        }
        public string EcouterReseau(Socket server, Socket serverSocket)
        {
            while (true)
            {
                string message = "";
                while (server.Available == 0)
                {
                }
                //MessageBox.Show("Message reçu server");
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
            //MessageBox.Show("Message envoye");
        }
        public static void Deconnecter(Socket socket, Socket serverSocket)
        {
            while (socket.Connected == true)
            {
            }
            socket.Shutdown(SocketShutdown.Both);
            serverSocket.Shutdown(SocketShutdown.Both);
            socket.Close();
            serverSocket.Close();
        }
        public void Set_receivedMessage(string receivedMessage)
        {
            _receivedMessage = receivedMessage;
            OnMessageReceived();
        }
        public void OnMessageReceived()
        {
            string type = this._receivedMessage.Substring(0, 4);
            //System.Windows.MessageBox.Show("type : " + type);
            switch (type)
            {
                case "Exec":
                    {
                        //TODO : simplify
                        int saveJobNb = Convert.ToInt32(this._receivedMessage.Substring(4));
                        this._model.Get_listSaveJob()[saveJobNb].Execute(_viewModel, this._model.Get_listSaveJob()[saveJobNb], this._model.Get_logFile(), this._model.Get_workFile(), this._model, this);
                    }
                    break;
                case "Dele":
                    {
                        int saveJobNb = Convert.ToInt32(this._receivedMessage.Substring(4));
                        this._model.RemoveSaveJob(saveJobNb);
                        _viewModel.setupObsCollection();
                        SendToClient();
                    }
                    break;
                case "Edit":
                    {
                        string json = this._receivedMessage.Substring(4);
                        File.WriteAllText(this._model.Get_workFile(), json);
                        this._model.Get_listSaveJob().Clear();
                        JObject objJSON = JObject.Parse(json);
                        int identationIndex = 0;
                        foreach (JObject i in objJSON["State"])
                        {
                            this._model.Get_listSaveJob().Add(new M_SaveJob(i["Name"].ToString(), i["SourceFilePath"].ToString(), i["TargetFilePath"].ToString(), i["Type"].Value<int>(), i["State"].ToString(), 0, identationIndex));
                            identationIndex += 1;
                        }
                        _viewModel.setupObsCollection();
                    }
                    break;
                case "Crea":
                    {
                        string json = this._receivedMessage.Substring(4);
                        File.WriteAllText(this._model.Get_workFile(), json);
                        this._model.Get_listSaveJob().Clear();
                        JObject objJSON = JObject.Parse(json);
                        int identationIndex = 0;
                        foreach (JObject i in objJSON["State"])
                        {
                            this._model.Get_listSaveJob().Add(new M_SaveJob(i["Name"].ToString(), i["SourceFilePath"].ToString(), i["TargetFilePath"].ToString(), i["Type"].Value<int>(), i["State"].ToString(), 0, identationIndex));
                            identationIndex += 1;
                        }
                        _viewModel.setupObsCollection();
                    }
                    break;
                case "Quit":
                    {
                        this._socket.Close();
                        this._serverSocket.Close();
                    }
                    break;
                case "Pause":
                    {
                        int saveJobNb = Convert.ToInt32(this._receivedMessage.Substring(4));
                        this._model.Get_listSaveJob()[saveJobNb].pauseThread();
                    }
                    break;
                case "Stop":
                    {
                        int saveJobNb = Convert.ToInt32(this._receivedMessage.Substring(4));
                        this._model.Get_listSaveJob()[saveJobNb].stopThread();
                    }
                    break;
                default:
                    break;
            }
        }
        public void StartServer()
        {
            while (true)
            {
                Thread threadConnexion = new Thread(() => this._serverSocket = Server.SeConnecter());
                Thread threadAccepterConnexion = new Thread(() => this._socket = Server.AccepterConnexion(this._serverSocket));
                threadConnexion.Start();
                threadConnexion.Join();
                threadAccepterConnexion.Start();
                threadAccepterConnexion.Join();
                JObject objJSON = JObject.Parse(File.ReadAllText(this._model.Get_workFile()));
                string jsonState = objJSON.ToString();
                Thread threadEnvoyerMessage = new Thread(() => Server.EnvoyerMessage(this._socket, jsonState));
                threadEnvoyerMessage.Start();
                //Thread verifyConnection = new Thread(() => Server.Deconnecter(socket, serverSocket));
                Thread threadStartListening = new Thread(() => EcouterReseauEnContinue());
                threadStartListening.Start();
                threadStartListening.Join();
            }
        }
        public void SendToClient()
        {
            JObject objJSON = JObject.Parse(File.ReadAllText(this._model.Get_workFile()));
            string jsonState = objJSON.ToString();
            Thread threadEnvoyerMessage = new Thread(() => Server.EnvoyerMessage(this._socket, jsonState));
            threadEnvoyerMessage.Start();
        }

        public void SendProgressToClient(int index, int progress, string state)
        {
            string message = "Progress" + index.ToString() + "," + progress.ToString() + "," + state + "____";
            Thread threadEnvoyerMessage = new Thread(() => Server.EnvoyerMessage(this._socket, message));
            threadEnvoyerMessage.Start();
        }
        public void EcouterReseauEnContinue()
        {
            Thread threadEcouteReseau = new Thread(() => this.Set_receivedMessage(this.EcouterReseau(this._socket, this._serverSocket)));
            while (_socket.Connected)
            {
                if (!threadEcouteReseau.IsAlive)
                {
                    threadEcouteReseau = new Thread(() => this.Set_receivedMessage(this.EcouterReseau(this._socket, this._serverSocket)));
                    threadEcouteReseau.Start();
                }
            }
        }
    }
}
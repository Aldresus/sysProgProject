using Newtonsoft.Json.Linq;
using NSClient;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ConcoleDeportee
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Client client = new Client();
        private Socket socket;
        private string _receiveMessage;
        private ObservableCollection<dynamic> _data;

        public string Get_receiveMessage()
        {
            return _receiveMessage;
        }
        public void Set_receiveMessage(string value)
        {
            _receiveMessage = value;
            OnPropertyChanged(Get_receiveMessage());
        }

        private void OnPropertyChanged(string propertyName)
        {
            MessageBox.Show("Message reçu : " + propertyName);
            ConvertStringToList(propertyName);
        }
        public MainWindow()
        {
            InitializeComponent();
            this.socket = Client.SeConnecter();
            Thread threadEcouteReseau = new Thread(() => this.Set_receiveMessage(Client.EcouterReseau(this.socket)));
            threadEcouteReseau.Start();
            threadEcouteReseau.Join();
            DG_Deportee.DataContext = this._data;
        }

        public void ConvertStringToList(string message)
        {
            List<dynamic> list = new List<dynamic>();
            JObject json = JObject.Parse(message);
            JArray arrayState = (JArray)json["State"];
            foreach (var i in arrayState)
            {
                list.Add(i);
                MessageBox.Show(i.ToString());
            }
            this._data = new ObservableCollection<dynamic>(list);
        }
    }
}

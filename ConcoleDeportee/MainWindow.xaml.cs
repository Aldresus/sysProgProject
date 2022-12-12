using Newtonsoft.Json.Linq;
using NSClient;
using NSModel;
using NSViewModel;
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
using System.Windows.Markup;
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
        private string _receiveMessage = "";
        private M_Model model;
        private VM_ViewModel viewModel;


        public string Get_receiveMessage()
        {
            return _receiveMessage;
        }
        public void Set_receiveMessage(string value)
        {
            this._receiveMessage = value;
            OnPropertyChanged();
        }

        private void OnPropertyChanged()
        {
            model = new M_Model(this._receiveMessage);
            viewModel = new VM_ViewModel(model);
            MessageBox.Show("Message reçu : " + this._receiveMessage);
            model.Set_workFile(this._receiveMessage);
            viewModel.setupObsCollection();
            try
            {
                this.Dispatcher.Invoke(() =>
                {
                    DG_Deportee.DataContext = viewModel.data;
                });
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
            //DG_Deportee.DataContext = viewModel.data;
        }
        public MainWindow()
        {
            InitializeComponent();
            this.socket = Client.SeConnecter();
            Thread threadStartListening = new Thread(() => EcouterReseauEnContinue());
            threadStartListening.Start();
        }

        private void Button_OnClick(object sender, RoutedEventArgs e)
        {
            string messageToSend = "Exec" + DG_Deportee.SelectedIndex.ToString();
            Client.EnvoyerMessage(this.socket, messageToSend);
        }
        
        private void EcouterReseauEnContinue()
        {
            Thread threadEcouteReseau = new Thread(() => this.Set_receiveMessage(client.EcouterReseau(this.socket)));
            while (true)
            {
                if (!threadEcouteReseau.IsAlive)
                {
                    threadEcouteReseau = new Thread(() => this.Set_receiveMessage(client.EcouterReseau(this.socket)));
                    threadEcouteReseau.Start();
                }
            }
        }
    }
}

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
using System.Windows.Forms;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MessageBox = System.Windows.MessageBox;
using TextBox = System.Windows.Controls.TextBox;
using NSUtils;

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
        private U_Checker checker = new U_Checker();


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
            if (this._receiveMessage != null)
            {
                model = new M_Model(this._receiveMessage);
                viewModel = new VM_ViewModel(model);
               // MessageBox.Show("Message reçu : " + this._receiveMessage);
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
            }
            else
            {
                
            }
        }
        public MainWindow()
        {
            InitializeComponent();
            this.socket = Client.SeConnecter();
            Thread threadStartListening = new Thread(() => EcouterReseauEnContinue());
            threadStartListening.Start();
        }

        private void Execute_OnClick(object sender, RoutedEventArgs e)
        {
            string messageToSend = "Exec" + DG_Deportee.SelectedIndex.ToString();
            Client.EnvoyerMessage(this.socket, messageToSend);
        }

        private void Delete_OnClick(object sender, RoutedEventArgs e)
        {
            string messageToSend = "Dele" + DG_Deportee.SelectedIndex.ToString();
            Client.EnvoyerMessage(this.socket, messageToSend);
        }

        private void EcouterReseauEnContinue()
        {
            Thread threadEcouteReseau = new Thread(() => this.Set_receiveMessage(client.EcouterReseau(this.socket)));
            while (socket.Connected)
            {
                if (!threadEcouteReseau.IsAlive)
                {
                    threadEcouteReseau = new Thread(() => this.Set_receiveMessage(client.EcouterReseau(this.socket)));
                    threadEcouteReseau.Start();
                }
            }
            socket.Close();
        }

        private void DG_Deportee_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            DataGrid dataGrid = DG_Deportee;
            DataGridRow Row = (DataGridRow)dataGrid.ItemContainerGenerator.ContainerFromIndex(dataGrid.SelectedIndex);

            DataGridCell RowAndColumnName = (DataGridCell)dataGrid.Columns[0].GetCellContent(Row).Parent;
            string name;
            if (RowAndColumnName.Content is TextBox)
            {
                name = ((TextBox)RowAndColumnName.Content).Text;
            }
            else
            {
                name = ((TextBlock)RowAndColumnName.Content).Text;
            }

            DataGridCell RowAndColumnSourceDirectory = (DataGridCell)dataGrid.Columns[1].GetCellContent(Row).Parent;
            string sourceDirectory;
            if (RowAndColumnSourceDirectory.Content is TextBox)
            {
                sourceDirectory = ((TextBox)RowAndColumnSourceDirectory.Content).Text;
            }
            else
            {
                sourceDirectory = ((TextBlock)RowAndColumnSourceDirectory.Content).Text;
            }


            DataGridCell RowAndColumnDestDirectory = (DataGridCell)dataGrid.Columns[2].GetCellContent(Row).Parent;
            string destDirectory;
            if (RowAndColumnDestDirectory.Content is TextBox)
            {
                destDirectory = ((TextBox)RowAndColumnDestDirectory.Content).Text;
            }
            else
            {
                destDirectory = ((TextBlock)RowAndColumnDestDirectory.Content).Text;
            }


            DataGridCell RowAndColumnType = (DataGridCell)dataGrid.Columns[3].GetCellContent(Row).Parent;
            int type;
            if (RowAndColumnType.Content is TextBox)
            {
                type = Convert.ToInt32(((TextBox)RowAndColumnType.Content).Text);
            }
            else
            {
                type = Convert.ToInt32(((TextBlock)RowAndColumnType.Content).Text);
            }

            model.GetSelectedSaveJob(dataGrid.SelectedIndex).Update(name, sourceDirectory, destDirectory, type);
            JObject json = JObject.Parse(this._receiveMessage);
            JArray array = (JArray)json["State"];
            array[dataGrid.SelectedIndex]["Name"] = name;
            array[dataGrid.SelectedIndex]["SourceDirectory"] = sourceDirectory;
            array[dataGrid.SelectedIndex]["DestDirectory"] = destDirectory;
            array[dataGrid.SelectedIndex]["Type"] = type;
            this.Set_receiveMessage(json.ToString());
            Client.EnvoyerMessage(this.socket, "Edit" + this._receiveMessage);
        }

        private void SourceClic(object sender, RoutedEventArgs e)
        {

            txtBoxSourceDir.Text = AskForFolder();

        }
        private void DestClic(object sender, RoutedEventArgs e)
        {
            txtBoxDestDir.Text = AskForFolder();
        }
        private string AskForFolder()
        {
            bool validInput = false;
            using (var fbd = new FolderBrowserDialog())
            {
                while (!validInput)
                {
                    DialogResult result = fbd.ShowDialog();

                    if (!string.IsNullOrWhiteSpace(fbd.SelectedPath))
                    {
                        fbd.SelectedPath += @"\";
                        return fbd.SelectedPath;
                    }
                }
                return "";
            }
        }
        
        private void Ajouter_Click(object sender, RoutedEventArgs e)
        {
            string name = txtBoxName.Text;
            string sourceDirectory = txtBoxSourceDir.Text;
            string destinationDirectory = txtBoxDestDir.Text;
            string type = comboBoxType.Text;
            int SaveJobeType = 1;
            switch (type)
            {
                case "Complete":
                    SaveJobeType = 1;
                    break;
                case "Differential":
                    SaveJobeType = 2;
                    break;
                default:
                    break;
            }
            if (checker.CheckStringInput(name, false) && checker.CheckStringInput(sourceDirectory, false) && checker.CheckStringInput(destinationDirectory, false) && checker.CheckStringInput(type, false))
            {
                int indexJob = checker.GetEmptyJobIndex(model.Get_listSaveJob());
                model.InstanceNewSaveJob(name, sourceDirectory, destinationDirectory, SaveJobeType, "idle", indexJob);
                JObject json = JObject.Parse(this._receiveMessage);
                JArray array = (JArray)json["State"];
                JObject saveJob =  model.GetSelectedSaveJob(indexJob).AddSaveJobToMessage(this._receiveMessage);
                array.Add(saveJob);
                this._receiveMessage = json.ToString();
                Client.EnvoyerMessage(this.socket, "Crea" + this._receiveMessage);
                viewModel.setupObsCollection();
                DG_Deportee.DataContext = viewModel.data;
                System.Windows.Forms.MessageBox.Show($"{name} {Properties.Resources.created}");

                txtBoxName.Text = "";
                txtBoxSourceDir.Text = "";
                txtBoxDestDir.Text = "";
                comboBoxType.Text = "Complete";
            }
            else
            {
                System.Windows.Forms.MessageBox.Show(Properties.Resources.pleaseFillAll);
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            Client.EnvoyerMessage(socket, "Quit");
            Client.Deconnecter(socket);
        }
    }
}

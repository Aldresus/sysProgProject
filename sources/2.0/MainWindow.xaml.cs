using System;
using Microsoft.VisualBasic.ApplicationServices;
using NSModel;
using NSUtils;
using NSViewModel;
using System.Reflection.Metadata;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Xml.Linq;
using TextBox = System.Windows.Controls.TextBox;
using Newtonsoft.Json.Linq;
using System.Threading;
using System.IO;
using System.Net.Sockets;
using NSServer;

namespace Livrable2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Server server = new Server();
        private M_Model model;
        private VM_ViewModel viewModel;
        private U_Checker checker = new U_Checker();
        private Socket serverSocket;
        private Socket socket;
        private string _receivedMessage;

        public void Set_ReceivedMessage(string receivedMessage)
        {
            _receivedMessage = receivedMessage;
            OnMessageReceived();
        }
        
        public void OnMessageReceived()
        {
            string type = this._receivedMessage.Substring(0, 4);
            int saveJobNumber = Convert.ToInt32(this._receivedMessage.Substring(4));
            System.Windows.MessageBox.Show("type : " + type + "    saveJobNumber : " + saveJobNumber.ToString());
        }

        public MainWindow()
        {
            InitializeComponent();
            model = new M_Model();
            viewModel = new VM_ViewModel(model);
            viewModel.setupObsCollection();
            DG1.DataContext = viewModel.data;
            JObject objJSON = JObject.Parse(File.ReadAllText(model.Get_workFile()));
            string jsonState = objJSON.ToString();
            Thread threadConnexion = new Thread(() => this.serverSocket = Server.SeConnecter());
            Thread threadAccepterConnexion = new Thread(() => this.socket = Server.AccepterConnexion(this.serverSocket));
            threadConnexion.Start();
            threadConnexion.Join();
            threadAccepterConnexion.Start();
            threadAccepterConnexion.Join();
            Thread threadEnvoyerMessage = new Thread(() => Server.EnvoyerMessage(this.socket, jsonState));
            threadEnvoyerMessage.Start();
            Thread threadEcouteReseau = new Thread(() => this.Set_ReceivedMessage(server.EcouterReseau(this.socket)));
            threadEcouteReseau.Start();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Window1 settings = new Window1(model, viewModel);
            settings.Show();
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            DataGrid dataGrid = DG1;
            model.RemoveSaveJob(dataGrid.SelectedIndex);
            viewModel.setupObsCollection();
            DG1.DataContext = viewModel.data;
        }

        private void Execute_Click(object sender, RoutedEventArgs e)
        {
            DataGrid dataGrid = DG1;
            model.Get_listSaveJob()[dataGrid.SelectedIndex].Execute(model.Get_listSaveJob()[dataGrid.SelectedIndex], model.Get_logFile(), model.Get_workFile(), model);

        }
        private void Ajouter_Click(object sender, RoutedEventArgs e)
        {
            //TODO : Check entry values with readers class
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
                model.GetSelectedSaveJob(indexJob).WriteJSON(model.Get_workFile());
                viewModel.setupObsCollection();
                DG1.DataContext = viewModel.data;
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

        private void DG1_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            DataGrid dataGrid = DG1;
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
            model.GetSelectedSaveJob(dataGrid.SelectedIndex).WriteJSON(model.Get_workFile());
            viewModel.setupObsCollection();
            DG1.DataContext = viewModel.data;
        }
    }
}

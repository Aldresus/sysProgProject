using NSModel;
using NSUtils;
using NSViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Livrable2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private M_Model model;
        private VM_ViewModel viewModel;
        private U_Checker checker = new U_Checker();
        private U_Reader reader = new U_Reader();
        public MainWindow()
        {
            InitializeComponent();
            model = new M_Model();
            viewModel = new VM_ViewModel(model);
            viewModel.setupObsCollection();
            DG1.DataContext = viewModel.data;
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
        }

        private void Execute_Click(object sender, RoutedEventArgs e)
        {
            DataGrid dataGrid = DG1;
            model.Get_listSaveJob()[dataGrid.SelectedIndex].Execute(model.Get_listSaveJob()[dataGrid.SelectedIndex], model.Get_logFile(), model.Get_workFile(), model);
            System.Windows.Forms.MessageBox.Show($"{viewModel.data[dataGrid.SelectedIndex]._saveJobName} executed");

        }
        private void Ajouter_Click(object sender, RoutedEventArgs e)
        {
            //TODO : Check entry values with readers class
            string name = txtBoxName.Text;
            string sourceDirectory = reader.ReadPath(txtBoxSourceDir.Text, false);
            string destinationDirectory = reader.ReadPath(txtBoxDestDir.Text, true);
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
                System.Windows.Forms.MessageBox.Show($"{name} created");
                
                txtBoxName.Text = "";
                txtBoxSourceDir.Text = "";
                txtBoxDestDir.Text = "";
                comboBoxType.Text = "Complete";
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("Please fill all the fields");
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
                        return fbd.SelectedPath;
                    }
                }
                return "";
            }
        }
    }
}

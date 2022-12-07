﻿using NSModel;
using NSUtils;
using NSViewModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using Livrable2.Properties;

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
            System.Windows.Forms.MessageBox.Show($"{viewModel.data[dataGrid.SelectedIndex]._saveJobName} {Properties.Resources.executed}");

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
    }
}

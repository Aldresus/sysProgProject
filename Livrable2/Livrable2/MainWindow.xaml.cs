using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
using NSModel;
using NSViewModel;
using NSUtils;

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
            this.model = new M_Model();
            this.viewModel = new VM_ViewModel(model);
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
            this.model.Get_listSaveJob()[dataGrid.SelectedIndex].Execute(this.model.Get_listSaveJob()[dataGrid.SelectedIndex], this.model.Get_logFile(), this.model.Get_workFile(), this.model);
        }
        private void Ajouter_Click(object sender, RoutedEventArgs e)
        {
            //TODO : Check entry values with readers class
            string name = txtBoxName.Text;
            string sourceDirectory = txtBoxSourceDir.Text;
            sourceDirectory = reader.ReadPath(sourceDirectory, false);
            string destinationDirectory = txtBoxDestDir.Text;
            destinationDirectory = reader.ReadPath(destinationDirectory, true);
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

            int indexJob = checker.GetEmptyJobIndex(model.Get_listSaveJob());
            this.model.InstanceNewSaveJob(name, sourceDirectory, destinationDirectory, SaveJobeType, "idle", indexJob);
            this.model.GetSelectedSaveJob(indexJob).WriteJSON(this.model.Get_workFile());
            this.viewModel.setupObsCollection();
            DG1.DataContext = this.viewModel.data;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            // Create OpenFileDialog 
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();

            // Set filter for file extension and default file extension 
            dlg.DefaultExt = ".png";
            dlg.Filter = "JPEG Files (*.jpeg)|*.jpeg|PNG Files (*.png)|*.png|JPG Files (*.jpg)|*.jpg|GIF Files (*.gif)|*.gif";

            // Display OpenFileDialog by calling ShowDialog method 
            Nullable<bool> result = dlg.ShowDialog();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Newtonsoft.Json.Linq;
using NSModel;
using NSViewModel;

namespace Livrable2
{
    /// <summary>
    /// Logique d'interaction pour Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        private M_Model model;
        private VM_ViewModel viewModel;
        public Window1(M_Model model, VM_ViewModel viewModel)
        {
            InitializeComponent();
            this.model = model;
            this.viewModel = viewModel;

            viewModel.setupExtensionObsCollection();
            DG_EXT.DataContext = viewModel.extension;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string extToAdd = textBoxExtToAdd.Text;
            model.AddExtensionToCryptState(extToAdd);
            model.Add_extensionToCrypt(extToAdd);
            model.Set_extensionToCryptRegex();
            viewModel.setupExtensionObsCollection();
            textBoxExtToAdd.Text = ".";
        }

        private void Window_Closed(object sender, EventArgs e)
        {
        }
        
        private void Delete_Click(object sender, EventArgs e)
        {
            DataGrid dataGrid = DG_EXT;
            model.RemoveExtensionToCryptState(dataGrid.SelectedIndex);
            model.Remove_extensionToCrypt(dataGrid.SelectedIndex);
            model.Set_extensionToCryptRegex();
            viewModel.setupExtensionObsCollection();
        }

        private void DG_EXT_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            DataGrid dataGrid = DG_EXT;
            DataGridRow Row = (DataGridRow)dataGrid.ItemContainerGenerator.ContainerFromIndex(dataGrid.SelectedIndex);
            DataGridCell RowAndColumn = (DataGridCell)dataGrid.Columns[0].GetCellContent(Row).Parent;
            string CellValue = (RowAndColumn.Content as TextBox).Text;

            model.EditExtensionToCryptState(dataGrid.SelectedIndex, CellValue);
            model.Edit_extensionToCrypt(dataGrid.SelectedIndex, CellValue);
            model.Set_extensionToCryptRegex();
            viewModel.setupExtensionObsCollection();
        }
    }
}

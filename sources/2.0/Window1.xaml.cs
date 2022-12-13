using NSModel;
using NSViewModel;
using System;
using System.Windows;
using System.Windows.Controls;

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
            viewModel.setupExtensionObsCollectionPrio();
            /*      viewModel.setupLanguageObsCollection();*/
            DG_EXT.DataContext = viewModel.extension;
            DG_EXT_Prio.DataContext = viewModel.extensionPrio;
            /*  C_Lang.DataContext = viewModel.cbLanguageItems;*/
            var t = System.Threading.Thread.CurrentThread.CurrentUICulture;
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

        private void Delete_Click_Prio(object sender, RoutedEventArgs e)
        {
            DataGrid dataGrid = DG_EXT_Prio;
            model.RemoveExtensionPriorityState(dataGrid.SelectedIndex);
            model.Remove_extensionPriority(dataGrid.SelectedIndex);
            model.Set_extensionPriorityRegex();
            viewModel.setupExtensionObsCollectionPrio();
        }

        private void DG_EXT_Prio_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            DataGrid dataGrid = DG_EXT_Prio;
            DataGridRow Row = (DataGridRow)dataGrid.ItemContainerGenerator.ContainerFromIndex(dataGrid.SelectedIndex);
            DataGridCell RowAndColumn = (DataGridCell)dataGrid.Columns[0].GetCellContent(Row).Parent;
            string CellValue = (RowAndColumn.Content as TextBox).Text;

            model.EditExtensionPriorityState(dataGrid.SelectedIndex, CellValue);
            model.Edit_extensionPriority(dataGrid.SelectedIndex, CellValue);
            model.Set_extensionPriorityRegex();
            viewModel.setupExtensionObsCollectionPrio();
        }
        private void Button_Prio_Click(object sender, RoutedEventArgs e)
        {
            string extToAdd = textBoxExtToAddPrio.Text;
            model.AddExtensionPriorityState(extToAdd);
            model.Add_extensionPriority(extToAdd);
            model.Set_extensionPriorityRegex();
            viewModel.setupExtensionObsCollectionPrio();
            textBoxExtToAddPrio.Text = ".";
        }
    }
}

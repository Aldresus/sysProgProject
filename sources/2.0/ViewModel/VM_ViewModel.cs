// Class ViewModel
// Interacts with the View and the Model
using Livrable2;
using NSModel;
using System;
using System.Collections.ObjectModel;
using System.Windows.Controls;

namespace NSViewModel
{
    public class VM_ViewModel
    {
        private string? _name;
        private string? _sourceDirectory;
        private string? _destinationDirectory;
        private string? _type;
        private M_Model _oModel;
        public ObservableCollection<M_SaveJob> data { get; set; } = new ObservableCollection<M_SaveJob>();
        public ObservableCollection<string> extension { get; set; } = new ObservableCollection<string>();
        public ObservableCollection<ComboBoxItem> cbLanguageItems { get; set; } = new ObservableCollection<ComboBoxItem>();


        public string Get_Name()
        {
            return _name;
        }

        public string Get_SourceDirectory()
        {
            return _sourceDirectory;
        }
        public M_Model Get_Model()
        {
            return _oModel;
        }

        public string Get_DestinationDirectory()
        {
            return _destinationDirectory;
        }

        public string Get_Type()
        {
            return _type;
        }

        public void Set_Name(string name)
        {
            _name = name;
        }

        public void Set_SourceDirectory(string sourceDirectory)
        {
            _sourceDirectory = sourceDirectory;
        }

        public void Set_DestinationDirectory(string destinationDirectory)
        {
            _destinationDirectory = destinationDirectory;
        }

        public void Set_Type(string type)
        {
            _type = type;
        }
        // Constructor for empty values
        public VM_ViewModel(M_Model M)
        {
            this._oModel = M;
        }
        // Constructor
        public VM_ViewModel(string name, string sourceDirectory, string destinationDirectory, string type)
        {
            _name = name;
            _sourceDirectory = sourceDirectory;
            _destinationDirectory = destinationDirectory;
            _type = type;

        }

        // Method to update data
        public void Update(string name, string sourceDirectory, string destinationDirectory, string type)
        {
            this.Set_Name(name);
            this.Set_SourceDirectory(sourceDirectory);
            this.Set_DestinationDirectory(destinationDirectory);
            this.Set_Type(type);
        }

        public void setupObsCollection()
        {
            App.Current.Dispatcher.Invoke((Action)delegate
            {
                this.data.Clear();
                foreach (M_SaveJob saveJob in _oModel.Get_listSaveJob())
                {
                    this.data.Add(saveJob);
                }
            });
        }

        public void setupExtensionObsCollection()
        {
            this.extension.Clear();
            foreach (string extension in _oModel.Get_extensionToCrypt())
            {
                this.extension.Add(extension);
            }
        }
    }
}
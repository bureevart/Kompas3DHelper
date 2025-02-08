using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kompas3DHelper.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private string _selectedFilePath = null;
        public string SelectedFileName { get => Path.GetFileName(SelectedFilePath) ?? "Файл не выбран"; }
        public string SelectedFilePath
        {
            get => _selectedFilePath;
            set
            {
                if (_selectedFilePath != value)
                {
                    _selectedFilePath = value;
                    OnPropertyChanged(nameof(SelectedFilePath));
                    OnPropertyChanged(nameof(SelectedFileName));
                }
            }
        }
    }
}

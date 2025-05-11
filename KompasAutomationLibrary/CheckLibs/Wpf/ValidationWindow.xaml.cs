using System.Windows;
using KompasAutomationLibrary.CheckLibs.Wpf.ViewModels;

namespace KompasAutomationLibrary.CheckLibs.Wpf
{
    public partial class ValidationWindow : Window
    {
        public ValidationVm ViewModel { get; }

        public ValidationWindow(ValidationVm vm)
        {
            InitializeComponent();
            ViewModel   = vm;
            DataContext = vm;
        }

        private void BtnRun_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        public long SelectedFlags => ViewModel.GetCheckedBits();
    }
}
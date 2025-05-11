using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using KompasAutomationLibrary.CheckLibs.Wpf.ViewModels;
using KompasAutomationLibrary.CheckMeta;

namespace KompasAutomationLibrary.CheckLibs.Wpf
{
    public partial class ValidationWindow : Window
    {
        public ValidationVm ViewModel { get; }

        public ValidationWindow(ValidationVm vm)
        {
            InitializeComponent();
            ViewModel = vm;
            DataContext = vm;
        }

        private void BtnRun_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }
        
        /// <summary>Все выбранные в дереве проверки.</summary>
        public IEnumerable<CheckInfo> SelectedChecks =>
            ViewModel.Roots
                .SelectMany(r => r.Children)
                .Where(n => n.IsChecked == true)
                .Select(n => n.CheckInfo);
    
        /// <summary>Битовая маска только для выбранных</summary>
        public long SelectedBits =>
            SelectedChecks
                .Select(ci => Convert.ToInt64(ci.FlagValue))
                .Aggregate(0L, (acc, b) => acc | b);
    }
}
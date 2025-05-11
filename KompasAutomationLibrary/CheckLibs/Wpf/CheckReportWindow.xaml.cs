using System;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Input;
using Kompas3DAutomation.Results;
using KompasAutomationLibrary.CheckLibs.Wpf.ViewModels;

namespace KompasAutomationLibrary.CheckLibs.Wpf
{
    public partial class CheckReportWindow : Window
    {
        readonly Action _clearAction;
        public CheckReportWindow(CheckReport rep, Action clear)
        {
            InitializeComponent();
            _clearAction = clear;
            DataContext  = new ReportVm(rep);
        }

        void Lv_DoubleClick(object s, MouseButtonEventArgs e)
        {
            if (Lv.SelectedItem is CheckViolation v) v.Highlighter?.Invoke();
        }

        void Clear_Click(object s, RoutedEventArgs e) => _clearAction?.Invoke();

        void Save_Click(object s, RoutedEventArgs e)
        {
            var vm = (ReportVm)DataContext;
            if (vm.Items.Count == 0) { MessageBox.Show("Пусто"); return; }

            var dlg = new Microsoft.Win32.SaveFileDialog
            {
                Filter   = "CSV файл|*.csv",
                FileName = "KompasReport.csv"
            };
            if (dlg.ShowDialog(this) != true) return;

            var sb = new StringBuilder();
            sb.AppendLine("Тип проверки;Сообщение");
            foreach (var v in vm.Items)
                sb.AppendLine($"{v.CheckName};{v.Message.Replace(';',',')}");
            File.WriteAllText(dlg.FileName, sb.ToString(), Encoding.UTF8);
            MessageBox.Show("Сохранено.");
        }
    }
}
using Kompas3DAutomation;
using Kompas3DAutomation.Checks;
using Kompas3DAutomation.Checks.AssemblyChecks;
using Kompas3DAutomation.Checks.DrawingChecks;
using Kompas3DAutomation.Checks.Part3DChecks;
using Kompas3DAutomation.Results;
using Kompas3DHelper.ViewModels;
using Microsoft.Win32;
using System;
using System.IO;
using System.Windows;
using static Kompas3DAutomation.Checks.AssemblyChecks.CheckAssembly;
using static Kompas3DAutomation.Checks.DrawingChecks.CheckDrawing;
using static Kompas3DAutomation.Checks.Part3DChecks.CheckPart3D;

namespace Kompas3DHelper
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public KompasConnectionObject KompasConnectionObject { get; set; }

        public CheckTypes CurrentCheckType { get; set; } = CheckTypes.Drawing;

        public MainViewModel ViewModel { get; } = new MainViewModel();

        public MainWindow()
        {
            InitializeComponent();
            DataContext = ViewModel;

            KompasConnectionObject = new KompasConnectionObject();
            KompasConnectionObject.Connect();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog
            {
                Filter = "Файлы чертежей (*.cdw;*.dwg)|*.cdw;*.dwg|Файлы 3D детали (*.m3d)|*.m3d|Файлы сборочной единицы (*.a3d)|*.a3d|Все файлы (*.*)|*.*"
            };

            // Если пользователь выбрал файл
            if (openFileDialog.ShowDialog() == true)
            {
                ViewModel.SelectedFilePath = openFileDialog.FileName;
            }

            SelectContentControl();
        }

        private void SelectContentControl()
        {
            string ext = Path.GetExtension(ViewModel.SelectedFilePath).ToLower();
            if (ext == ".cdw" || ext == ".dwg")
            {
                // Предположим, для чертежей
                CurrentCheckType = CheckTypes.Drawing;
                ChecksContentControl.Content = new DrawingUserControl();
            }
            else if (ext == ".m3d")  // условный пример для 3D модели детали
            {
                CurrentCheckType = CheckTypes.Part3D;
                ChecksContentControl.Content = new Part3DUserControl();
            }
            else if (ext == ".a3d") // условный пример для сборочной единицы
            {
                CurrentCheckType = CheckTypes.Assembly;
                ChecksContentControl.Content = new AssemblyUserControl();
            }
            else
            {
                CurrentCheckType = CheckTypes.Other;
                ChecksContentControl.Content = null;
                MessageBox.Show("Неверный тип расширения!");
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (ViewModel.SelectedFilePath is null)
            {
                MessageBox.Show("Выберите файл");
                return;
            }

            // В зависимости от типа проверки получаем данные из нужного UserControl
            switch (CurrentCheckType)
            {
                case CheckTypes.Assembly:
                    if (ChecksContentControl.Content is AssemblyUserControl assemblyControl)
                    {
                        AssemblyChecks assemblyChecks = assemblyControl.SelectedAssemblyChecks;
                        var checkAssembly = new CheckAssembly(KompasConnectionObject);
                        var assemblyResult = checkAssembly.Check(ViewModel.SelectedFilePath, assemblyChecks);
                        MessageBox.Show(assemblyResult.ResultType.GetMessage());
                    }
                    break;
                case CheckTypes.Drawing:
                    if (ChecksContentControl.Content is DrawingUserControl drawingControl)
                    {
                        DrawingChecks drawingChecks = drawingControl.SelectedDrawingChecks;
                        var checkDrawing = new CheckDrawing(KompasConnectionObject);
                        var drawingResult = checkDrawing.Check(ViewModel.SelectedFilePath, drawingChecks);
                        MessageBox.Show(drawingResult.ResultType.GetMessage());
                    }
                    break;
                case CheckTypes.Part3D:
                    if (ChecksContentControl.Content is Part3DUserControl part3DControl)
                    {
                        Part3DChecks part3DChecks = part3DControl.SelectedPartModelChecks;
                        var checkPart3D = new CheckPart3D(KompasConnectionObject);
                        var part3DResult = checkPart3D.Check(ViewModel.SelectedFilePath, part3DChecks);
                        MessageBox.Show(part3DResult.ResultType.GetMessage());
                    }
                    break;
                case CheckTypes.Other:
                    MessageBox.Show("Неверный тип расширения!");
                    break;
            }
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}

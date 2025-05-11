using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Windows.Interop;
using Kompas3DAutomation;
using Kompas6API5;
using KompasAutomationLibrary.CheckLibs.Wpf;
using KompasAutomationLibrary.CheckLibs.Wpf.ViewModels;
using KompasAutomationLibrary.Utils;
using Microsoft.Win32;

namespace KompasAutomationLibrary.CheckLibs
{
    [ComVisible(true)]
    [Guid("a2e9bb86-8d58-4ea4-b803-b087bf4cba7b")]
    [ClassInterface(ClassInterfaceType.AutoDual)]
    public class Checker
    {
        [return: MarshalAs(UnmanagedType.BStr)]
        public string GetLibraryName()
        {
            return "Валидация ЭКД";
        }

        public void ExternalRunCommand([In] short command, [In] short mode, [In, MarshalAs(UnmanagedType.IDispatch)] object kompas_)
        {
            KompasObject kompas = (KompasObject)kompas_;

            KompasConnectionObject kompasConnectionObject = new KompasConnectionObject();
            kompasConnectionObject.Connect(kompas_);
            
            switch (command)
            {
                case 1:
                {
                    if (System.Windows.Application.Current == null)
                        new System.Windows.Application();

                    var kind = Utils.Utils.GetCurrentDocKind(kompas);
                    var vm = new ValidationVm(kind);
                    var win = new ValidationWindow(vm);

                    new WindowInteropHelper(win).Owner =
                        KompasWindowHelper.GetKompasHwnd(kompas);

                    if (win.ShowDialog() == true)
                    {
                        var bits = win.SelectedFlags;
                        if (bits == 0)
                        {
                            MessageBox.Show("Не выбрано ни одной проверки.");
                            return;
                        }
                        
                        var result = CheckRunner.Run(kompasConnectionObject, kind, bits);
                        KompasWindowHelper.Show(kompas, result.Report, result.Clear);
                    }
                    break;

                }
            }
        }

        [return: MarshalAs(UnmanagedType.BStr)]
        public string ExternalMenuItem(short number, ref short itemType, ref short command)
        {
            string result = string.Empty;
            itemType = 1;
            command = -1;

            switch (number)
            {
                case 1:
                    result = "Открыть модуль валидации ЭКД";
                    command = 1;
                    break;
                case 2:
                    itemType = 3;
                    break;
            }

            return result;
        }

        #region COM Registration
        // Эта функция выполняется при регистрации класса для COM
        // Она добавляет в ветку реестра компонента раздел Kompas_Library,
        // который сигнализирует о том, что класс является приложением Компас,
        // а также заменяет имя InprocServer32 на полное, с указанием пути.
        // Все это делается для того, чтобы иметь возможность подключить
        // библиотеку на вкладке ActiveX.
        [ComRegisterFunction]
        public static void RegisterKompasLib(Type t)
        {
            try
            {
                RegistryKey regKey = Registry.LocalMachine;
                string keyName = @"SOFTWARE\Classes\CLSID\{" + t.GUID.ToString() + "}";
                regKey = regKey.OpenSubKey(keyName, true);
                regKey.CreateSubKey("Kompas_Library");
                regKey.SetValue(null, System.Environment.GetFolderPath(Environment.SpecialFolder.System) + @"\mscoree.dll");
                regKey.Close();
            }
            catch (Exception ex)
            {
            }
        }

        // Эта функция удаляет раздел Kompas_Library из реестра
        [ComUnregisterFunction]
        public static void UnregisterKompasLib(Type t)
        {
            RegistryKey regKey = Registry.LocalMachine;
            string keyName = @"SOFTWARE\Classes\CLSID\{" + t.GUID.ToString() + "}";
            RegistryKey subKey = regKey.OpenSubKey(keyName, true);
            subKey.DeleteSubKey("Kompas_Library");
            subKey.Close();
        }
        #endregion
    }
}

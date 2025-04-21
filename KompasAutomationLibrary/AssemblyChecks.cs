using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Kompas3DAutomation.Checks.DrawingChecks;
using Kompas3DAutomation;
using Kompas6API5;
using Microsoft.Win32;
using Kompas3DAutomation.Results;
using Kompas3DAutomation.Checks.AssemblyChecks;

namespace KompasAutomationLibrary
{
    [ComVisible(true)]
    [Guid("16ca0cd7-ae8a-405e-9468-c0825d74d56b")]
    [ClassInterface(ClassInterfaceType.AutoDual)]
    public class AssemblyChecks
    {
        [return: MarshalAs(UnmanagedType.BStr)]
        public string GetLibraryName()
        {
            return "Проверки сборок";
        }

        public void ExternalRunCommand([In] short command, [In] short mode, [In, MarshalAs(UnmanagedType.IDispatch)] object kompas_)
        {
            KompasObject kompas = (KompasObject)kompas_;

            KompasConnectionObject kompasConnectionObject = new KompasConnectionObject();
            kompasConnectionObject.Connect(kompas_);

            var checkAssembly = new CheckAssembly(kompasConnectionObject);

            switch (command)
            {
                case 1:
                    var assemblyResult1 = checkAssembly.CheckForActiveDocument(CheckAssembly.AssemblyChecks.PartInterference);
                    kompas.ksMessage($"Результат проверки: {assemblyResult1.ToString()}");
                    break;
                case 2:
                    var assemblyResult2 = checkAssembly.CheckForActiveDocument(CheckAssembly.AssemblyChecks.HiddenObjectsPresent);
                    kompas.ksMessage($"Результат проверки: {assemblyResult2.ToString()}");
                    break;
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
                    result = "Проверка врезания деталей";
                    command = 1;          // ExternalRunCommand(case 1)
                    break;
                case 2:
                    result = "Проверка наличия скрытых компонентов";
                    command = 2;          // ExternalRunCommand(case 2)
                    break;
                case 3:
                    itemType = 3;
                    break;
            }

            return result;
        }




        // регистрация библиотеки
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

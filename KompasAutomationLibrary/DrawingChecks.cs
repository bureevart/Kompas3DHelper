using System;
using System.Runtime.InteropServices;
using Kompas3DAutomation;
using Kompas3DAutomation.Checks.DrawingChecks;
using Kompas3DAutomation.Checks.Part3DChecks;
using Kompas3DAutomation.Results;
using Kompas6API5;
using Microsoft.Win32;
using static Kompas3DAutomation.Checks.DrawingChecks.CheckDrawing;

namespace KompasAutomationLibrary
{
    [ComVisible(true)]
    [Guid("d009ace2-cac1-492f-9e03-56f073b2e4ab")]
    [ClassInterface(ClassInterfaceType.AutoDual)]
    public class DrawingChecks
    {
        [return: MarshalAs(UnmanagedType.BStr)]
        public string GetLibraryName()
        {
            return "Проверки чертежей";
        }

        public void ExternalRunCommand([In] short command, [In] short mode, [In, MarshalAs(UnmanagedType.IDispatch)] object kompas_)
        {
            KompasObject kompas = (KompasObject)kompas_;

            KompasConnectionObject kompasConnectionObject = new KompasConnectionObject();
            kompasConnectionObject.Connect(kompas_);

            var checkDrawing = new CheckDrawing(kompasConnectionObject);

            switch (command)
            {
                case 1:
                    var drawingResult1 = checkDrawing.CheckForActiveDocument(CheckDrawing.DrawingChecks.NoHiddenObjects);
                    kompas.ksMessage($"Результат проверки: {drawingResult1.ToString()}");
                    if (drawingResult1.HasErrors && kompas.ksYesNo("Подсветить ошибки?") == 1)
                    {
                        drawingResult1.Violations.ForEach(err => err.Highlighter.Invoke());
                    }
                    break;
                case 2:
                    var drawingResult2 = checkDrawing.CheckForActiveDocument(CheckDrawing.DrawingChecks.ManualTextDimensionChanges);
                    kompas.ksMessage($"Результат проверки: {drawingResult2.ToString()}");
                    if (drawingResult2.HasErrors && kompas.ksYesNo("Подсветить ошибки?") == 1)
                    {
                        drawingResult2.Violations.ForEach(err => err.Highlighter.Invoke());
                    }
                    break;
                case 3:
                    checkDrawing.ClearHighlightForActiveDocument();
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
                    result = "Проверка на наличие скрытых объектов в чертеже";
                    command = 1;          // ExternalRunCommand(case 1)
                    break;

                case 2:
                    result = "Проверка на ручное изменение значений";
                    command = 2;          // ExternalRunCommand(case 2)
                    break;
                case 3:
                    result = "Очистить подсветку";
                    command = 3;
                    break;
                case 4:
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

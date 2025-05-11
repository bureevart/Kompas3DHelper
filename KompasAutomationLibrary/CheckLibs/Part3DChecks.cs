﻿using System;
using System.Runtime.InteropServices;
using Kompas3DAutomation;
using Kompas3DAutomation.Checks.Part3DChecks;
using Kompas6API5;
using KompasAutomationLibrary.Utils;
using Microsoft.Win32;

namespace KompasAutomationLibrary.CheckLibs
{
    [ComVisible(true)]
    [Guid("e94f8265-2f21-4b0e-b89e-63ffe94251ff")]
    [ClassInterface(ClassInterfaceType.AutoDual)]
    public class Part3DChecks
    {
        [return: MarshalAs(UnmanagedType.BStr)]
        public string GetLibraryName()
        {
            return "Проверки моделей";
        }

        public void ExternalRunCommand([In] short command, [In] short mode, [In, MarshalAs(UnmanagedType.IDispatch)] object kompas_)
        {
            KompasObject kompas = (KompasObject)kompas_;

            KompasConnectionObject kompasConnectionObject = new KompasConnectionObject();
            kompasConnectionObject.Connect(kompas_);

            var checkPart3D = new CheckPart3D(kompasConnectionObject);
            
            switch (command)
            {
                case 1:
                    var part3DResult1 = checkPart3D.CheckForActiveDocument(CheckPart3D.Part3DChecks.HiddenObjectsPresent);
                    KompasWindowHelper.Show(kompas, part3DResult1, () => checkPart3D.ClearHighlightForActiveDocument());
                    break;
                case 2:
                    var part3DResult2 = checkPart3D.CheckForActiveDocument(CheckPart3D.Part3DChecks.SelfIntersectionOfFaces);
                    KompasWindowHelper.Show(kompas, part3DResult2, () => checkPart3D.ClearHighlightForActiveDocument());
                    break;
                case 3:
                    var part3DResult3 = checkPart3D.CheckForActiveDocument(CheckPart3D.Part3DChecks.SingleSolidBody);
                    KompasWindowHelper.Show(kompas, part3DResult3, () => checkPart3D.ClearHighlightForActiveDocument());
                    break;
                case 4:
                    checkPart3D.ClearHighlightForActiveDocument();
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
                    result = "Проверка на наличие скрытых объектов";
                    command = 1;
                    break;
                case 2:
                    result = "Проверка на самопересечения поверхностей";
                    command = 2;
                    break;
                case 3:
                    result = "Проверка на наличие более одного твердого тела";
                    command = 3;
                    break;
                case 4:
                    result = "Очистить подсветку";
                    command = 4;
                    break;
                case 5:
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

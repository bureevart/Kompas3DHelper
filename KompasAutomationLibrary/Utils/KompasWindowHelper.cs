using System;
using System.Windows.Interop;
using Kompas3DAutomation.Results;
using Kompas6API5;
using KompasAutomationLibrary.CheckLibs.Wpf;

namespace KompasAutomationLibrary.Utils
{
    /// <summary>Утилиты, связанные с главным окном КОМПАС-3D.</summary>
    public static class KompasWindowHelper
    {
        /// <returns>HWND (IntPtr) главного окна KOMPAS.</returns>
        public static IntPtr GetKompasHwnd(KompasObject kompas) =>
            new IntPtr(kompas.ksGetHWindow());
        
        public static void Show(KompasObject kompas,
            CheckReport  report,
            Action       clearHighlight)
        {
            if (System.Windows.Application.Current == null)          // инициализируем WPF
                new System.Windows.Application();

            var wnd = new CheckReportWindow(report, clearHighlight);
            new WindowInteropHelper(wnd).Owner = GetKompasHwnd(kompas);

            wnd.Show();
        }
    }
}
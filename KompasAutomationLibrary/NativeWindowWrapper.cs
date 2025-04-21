using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kompas3DAutomation.Results;
using Kompas6API5;
using System.Windows.Forms;

namespace KompasAutomationLibrary
{
    public class NativeWindowWrapper : IWin32Window
    {
        public NativeWindowWrapper(IntPtr handle) { Handle = handle; }
        public IntPtr Handle { get; }

        public static void ShowReportWinForms(KompasObject kompas, CheckReport rpt)
        {
            var form = new CheckReportForm();
            form.Bind(rpt);
            form.Show(GetKompasMainHwnd(kompas));
        }


        public static IWin32Window GetKompasMainHwnd(KompasObject kompas)
        {
            // предполагаем, что ksGetHWindow() возвращает 32‑битный дескриптор окна
            int raw = kompas.ksGetHWindow();
            IntPtr hwnd = new IntPtr(raw);
            return new NativeWindowWrapper(hwnd);
        }
    }
}

using System;
using Kompas6API5;
using KompasAPI7;
using KompasAutomationLibrary.CheckMeta;

namespace KompasAutomationLibrary.Utils
{
    public static class Utils
    {
        public static DocKind GetCurrentDocKind(KompasObject k)
        {
            var app = (IApplication)k.ksGetApplication7();
            var doc = app.ActiveDocument;
            return doc switch
            {
                IKompasDocument2D _ => DocKind.Drawing2D,
                IAssemblyDocument _ => DocKind.Assembly,
                IKompasDocument3D _ => DocKind.Part3D,
                _ => throw new InvalidOperationException("Неизвестный документ")
            };
        }

    }
}
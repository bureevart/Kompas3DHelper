using Kompas3DAutomation.Checks;
using Kompas3DAutomation.Results;
using Kompas6API5;
using Kompas6Constants;
using KompasAPI7;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kompas3DAutomation.Checks.DrawingChecks
{
    public class CheckDrawing : CheckBase
    {
        public CheckDrawing(KompasConnectionObject kompasConnectionObject) : base(kompasConnectionObject) { }

        public CheckReport Check(string path, DrawingChecks checks)
        {
            if (!_kompasObject.IsConnected)
                return CheckReport.ConnectionError();

            var app = (IApplication)_kompasObject.Kompas.ksGetApplication7();
            app.Documents.Open(path, true, true);
            var doc2D = (IKompasDocument2D)app.ActiveDocument;

            try 
            { 
                return RunChecks(doc2D, checks); 
            }
            finally 
            { 
                doc2D.Close(DocumentCloseOptions.kdDoNotSaveChanges); 
            }
        }

        public CheckReport CheckForActiveDocument(DrawingChecks checks)
        {
            if (!_kompasObject.IsConnected)
                return CheckReport.ConnectionError();

            var app = (IApplication)_kompasObject.Kompas.ksGetApplication7();
            var doc2D = (IKompasDocument2D?)app.ActiveDocument ?? throw new InvalidOperationException("Нет активного 2D‑документа");
            return RunChecks(doc2D, checks);
        }

        private CheckReport RunChecks(IKompasDocument2D doc2D, DrawingChecks checks)
        {
            var report = new CheckReport();
            void Add(IChecker ch) => report.Violations.AddRange(ch.Run());

            if (checks.HasFlag(DrawingChecks.NoHiddenObjects))
                Add(new HiddenObjectsChecker(_kompasObject.Kompas, doc2D));

            if (checks.HasFlag(DrawingChecks.ManualTextDimensionChanges))
                Add(new ManualTextDimensionChangesChecker(_kompasObject.Kompas, doc2D));

            return report;
        }

        #region CheckTypes
        /// <summary>
        /// Проверки для чертежей.
        /// </summary>
        [Flags]
        public enum DrawingChecks
        {
            None = 0,
            /// <summary>
            /// Проверка расположения объектов на заданных слоях.
            /// </summary>
            LayerObjectsPosition = 1 << 0,
            /// <summary>
            /// Отсутствие скрытых объектов в поле чертежа.
            /// </summary>
            NoHiddenObjects = 1 << 1,
            /// <summary>
            /// Отсутствие объектов за пределами чертежа.
            /// </summary>
            NoObjectsOutsideDrawing = 1 << 2,
            /// <summary>
            /// Целостность видов (к виду не применялась команда "Разрушить").
            /// </summary>
            ViewIntegrity = 1 << 3,
            /// <summary>
            /// Виды, вспомогательные линии и размеры не должны пересекать границу листа.
            /// </summary>
            NoObjectsCrossingSheetBorder = 1 << 4,
            /// <summary>
            /// Проверка размеров на ручное внесение текстовых изменений.
            /// </summary>
            ManualTextDimensionChanges = 1 << 5
        }
        #endregion
    }
}

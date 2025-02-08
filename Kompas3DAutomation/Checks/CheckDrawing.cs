using Kompas3DAutomation.Checks;
using Kompas3DAutomation.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Kompas3DAutomation.CheckAssembly;

namespace Kompas3DAutomation
{
    public class CheckDrawing : CheckBase
    {
        public CheckDrawing(KompasConnectionObject kompasConnectionObject) : base(kompasConnectionObject)
        {
        }

        public CheckResult Check(string path, DrawingChecks checks)
        {
            if (!_kompasObject.IsConnected)
            {
                return new CheckResult()
                {
                    ResultType = CheckResults.ConnectionError,
                };
            }

            throw new NotImplementedException();
        }

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
    }
}

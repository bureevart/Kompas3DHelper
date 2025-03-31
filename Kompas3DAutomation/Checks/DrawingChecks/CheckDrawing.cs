using Kompas3DAutomation.Checks;
using Kompas3DAutomation.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kompas3DAutomation.Checks.DrawingChecks
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

            try
            {
                var errors = new List<string>();
                if (checks.HasFlag(DrawingChecks.NoHiddenObjects))
                {
                    if (!HiddenObjectsChecker.CheckHiddenObjectsPresent(_kompasObject.Kompas, path))
                        errors.Add("Ошибка наличия скрытых объектов");
                }

                if (checks.HasFlag(DrawingChecks.ViewIntegrity))
                {
                    if (!ViewIntegrityChecker.CheckViewIntegrity(_kompasObject.Kompas, path))
                        errors.Add("Ошибка целостности видов");
                }

                if (checks.HasFlag(DrawingChecks.NoObjectsOutsideDrawing))
                {
                    if (!NoObjectsCrossingSheetBorderChecker.CheckNoObjectsCrossingSheetBorder(_kompasObject.Kompas, path))
                        errors.Add("Ошибка наличия обьектов за пределами чертежа");
                }

                if (checks.HasFlag(DrawingChecks.ManualTextDimensionChanges))
                {
                    if (!ManualTextDimensionChangesChecker.CheckManualTextDimensionChanges(_kompasObject.Kompas, path))
                        errors.Add("Ошибка ручного изменения размера");
                }

                if (errors.Count > 0)
                {
                    var error = string.Empty;
                    foreach (var err in errors)
                        error += err + " ";

                    return new CheckResult()
                    {
                        ResultType = CheckResults.Error,
                        InnerResult = error
                    };
                }

                return CheckResult.GetNoErrorsResult();
            }
            catch (Exception ex)
            {
                return new CheckResult()
                {
                    ResultType = CheckResults.Error,
                    InnerResult = $"Ошибка: {ex}"
                };
            }
        }

        public CheckResult CheckForActiveDocument(DrawingChecks checks)
        {
            if (!_kompasObject.IsConnected)
            {
                return new CheckResult()
                {
                    ResultType = CheckResults.ConnectionError,
                };
            }

            try
            {
                var errors = new List<string>();
                if (checks.HasFlag(DrawingChecks.NoHiddenObjects))
                {
                    if (!HiddenObjectsChecker.CheckHiddenObjectsPresentForActiveDocument(_kompasObject.Kompas))
                        errors.Add("Ошибка наличия скрытых объектов");
                }

                if (checks.HasFlag(DrawingChecks.ViewIntegrity))
                {
                    if (!ViewIntegrityChecker.CheckViewIntegrityForActiveDocument(_kompasObject.Kompas))
                        errors.Add("Ошибка целостности видов");
                }

                if (checks.HasFlag(DrawingChecks.NoObjectsOutsideDrawing))
                {
                    if (!NoObjectsCrossingSheetBorderChecker.CheckNoObjectsCrossingSheetBorderForActiveDocument(_kompasObject.Kompas))
                        errors.Add("Ошибка наличия обьектов за пределами чертежа");
                }

                if (checks.HasFlag(DrawingChecks.ManualTextDimensionChanges))
                {
                    if (!ManualTextDimensionChangesChecker.CheckManualTextDimensionChangesForActiveDocument(_kompasObject.Kompas))
                        errors.Add("Ошибка ручного изменения размера");
                }

                if (errors.Count > 0)
                {
                    var error = string.Empty;
                    foreach (var err in errors)
                        error += err + " ";

                    return new CheckResult()
                    {
                        ResultType = CheckResults.Error,
                        InnerResult = error
                    };
                }

                return CheckResult.GetNoErrorsResult();
            }
            catch (Exception ex)
            {
                return new CheckResult()
                {
                    ResultType = CheckResults.Error,
                    InnerResult = $"Ошибка: {ex}"
                };
            }
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

using Kompas3DAutomation.Checks;
using Kompas3DAutomation.Checks.Part3DChecks;
using Kompas3DAutomation.Results;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kompas3DAutomation.Checks.AssemblyChecks
{
    public class CheckAssembly : CheckBase
    {
        public CheckAssembly(KompasConnectionObject kompasConnectionObject) : base(kompasConnectionObject)
        {
        }

        public CheckResult Check(string path, AssemblyChecks checks)
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

                if (checks.HasFlag(AssemblyChecks.PartInterference))
                {
                    if (!PartInterferenceChecker.CheckPartInterference(_kompasObject.Kompas, path))
                        errors.Add("Ошибка самопересечения граней.");
                }

                if (checks.HasFlag(AssemblyChecks.HiddenObjectsPresent))
                {
                    if (!HiddenObjectsChecker.CheckHiddenObjects(_kompasObject.Kompas, path))
                        errors.Add("Ошибка наличия скрытых компонентов.");
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

        public CheckResult CheckForActiveDocument(AssemblyChecks checks)
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

                if (checks.HasFlag(AssemblyChecks.PartInterference))
                {
                    if (!PartInterferenceChecker.CheckPartInterferenceForActiveDocument(_kompasObject.Kompas))
                        errors.Add("Ошибка самопересечения граней.");
                }

                if (checks.HasFlag(AssemblyChecks.HiddenObjectsPresent))
                {
                    if (!HiddenObjectsChecker.CheckHiddenObjectsForActiveDocument(_kompasObject.Kompas))
                        errors.Add("Ошибка наличия скрытых компонентов.");
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
        /// Проверки для моделей СЕ (сборочной единицы).
        /// </summary>
        [Flags]
        public enum AssemblyChecks
        {
            None = 0,
            /// <summary>
            /// Врезание деталей не допускается (анализ зазоров по сопряженным поверхностям).
            /// </summary>
            PartInterference = 1 << 0,
            /// <summary>
            /// Наличие скрытых объектов (компонентов).
            /// </summary>
            HiddenObjectsPresent = 1 << 1
        }
    }
}

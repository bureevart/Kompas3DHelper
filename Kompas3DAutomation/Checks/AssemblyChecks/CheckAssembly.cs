using Kompas3DAutomation.Checks;
using Kompas3DAutomation.Results;
using System;
using System.Collections.Generic;
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

            throw new NotImplementedException();
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
            PartInterference = 1 << 0
        }
    }
}

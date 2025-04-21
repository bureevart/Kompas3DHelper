using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kompas3DAutomation.Results;

namespace Kompas3DAutomation.Checks.DrawingChecks
{
    internal interface IChecker
    {
        IEnumerable<CheckViolation> Run();
    }
}

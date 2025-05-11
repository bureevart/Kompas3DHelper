using System.Collections.Generic;
using Kompas3DAutomation.Results;

namespace Kompas3DAutomation.Checks
{
    internal interface IChecker
    {
        IEnumerable<CheckViolation> Run();
    }
}

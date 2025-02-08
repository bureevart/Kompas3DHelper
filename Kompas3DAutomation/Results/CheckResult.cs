using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kompas3DAutomation.Results
{
    //TODO: add support to get more than 1 error
    public class CheckResult
    {
        public CheckResults ResultType { get; set; }
        public string InnerResult { get; set; } = string.Empty;
    }
}

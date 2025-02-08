using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kompas3DAutomation.Checks
{
    public abstract class CheckBase
    {
        public CheckBase(KompasConnectionObject kompasConnectionObject)
        {
            _kompasObject = kompasConnectionObject;
        }

        protected KompasConnectionObject _kompasObject;
    }
}

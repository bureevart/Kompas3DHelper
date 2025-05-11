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

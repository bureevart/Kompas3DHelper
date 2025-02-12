using Kompas6API5;
using KompasAPI7;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kompas3DAutomation.Checks.Part3DChecks
{
    public class SingleSolidBodyChecker
    {
        public static bool CheckSingleSolidBody(KompasObject kompasObject, string path)
        {
            IApplication app = (IApplication)kompasObject.ksGetApplication7();

            if (app == null)
            {
                throw new Exception("Не удалось получить экземпляр IApplication через API7.");
            }

            app.Documents.Open(path, true, true);
            IKompasDocument3D doc3D = (IKompasDocument3D)app.ActiveDocument;

            try
            {
                if (doc3D is null)
                {
                    throw new Exception("Документ не является 3D документом");
                }


                Part7 part7 = doc3D.TopPart;
                if (part7 == null)
                {
                    throw new Exception("Не удалось получить интерфейс IPart7 из открытого документа.");
                }

                if (!CheckSingleBody((IFeature7)part7))
                    return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw ex;
            }
            finally
            {
                doc3D.Close(Kompas6Constants.DocumentCloseOptions.kdDoNotSaveChanges);
            }

            return true;
        }

        private static bool CheckSingleBody(IFeature7 f7)
        {
            if (f7.ResultBodies is object[] bodies)
            {
                if (bodies.Length > 1) return false;
            }

            return true;
        }
    }
}

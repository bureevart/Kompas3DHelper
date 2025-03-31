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

        public static bool CheckSingleSolidBodyForActiveDocument(KompasObject kompasObject)
        {
            IApplication app = (IApplication)kompasObject.ksGetApplication7();

            if (app == null)
            {
                throw new Exception("Не удалось получить экземпляр IApplication через API7.");
            }

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
                //doc3D.Close(Kompas6Constants.DocumentCloseOptions.kdDoNotSaveChanges);
            }

            return true;
        }

        private static bool CheckSingleBody(IFeature7 f7)
        {
            if (f7.ResultBodies is object[] bodies)
            {
                var cntSolid = 0;
                foreach (Body7 body in bodies)
                {
                    cntSolid = body.IsSolid ? cntSolid + 1 : cntSolid;
                    Console.WriteLine(body.Name + " IsSolid " + body.IsSolid);
                }
                if (cntSolid > 1) return false;

            }
            else if (f7.ResultBodies is IBody7 body)
            {
                Console.WriteLine(body.Name + "IsSolid" + body.IsSolid);
            }

            return true;
        }
    }
}

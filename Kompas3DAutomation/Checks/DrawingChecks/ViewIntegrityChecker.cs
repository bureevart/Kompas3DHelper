using Kompas6API5;
using KompasAPI7;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kompas3DAutomation.Checks.DrawingChecks
{
    public class ViewIntegrityChecker
    {
        public static bool CheckViewIntegrity(KompasObject kompasObject, string path)
        {
            IApplication app = (IApplication)kompasObject.ksGetApplication7();

            if (app == null)
            {
                throw new Exception("Не удалось получить экземпляр IApplication через API7.");
            }

            app.Documents.Open(path, true, true);
            IKompasDocument2D doc2D = (IKompasDocument2D)app.ActiveDocument;

            try
            {
                if (doc2D is null)
                {
                    throw new Exception("Документ не является 3D документом");
                }

                if (!CheckViewsNotBroken(doc2D))
                    return false;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw ex;
            }
            finally
            {
                doc2D.Close(Kompas6Constants.DocumentCloseOptions.kdDoNotSaveChanges);
            }


            return true;
        }

        public static bool CheckViewIntegrityForActiveDocument(KompasObject kompasObject)
        {
            IApplication app = (IApplication)kompasObject.ksGetApplication7();

            if (app == null)
            {
                throw new Exception("Не удалось получить экземпляр IApplication через API7.");
            }

            IKompasDocument2D doc2D = (IKompasDocument2D)app.ActiveDocument;

            try
            {
                if (doc2D is null)
                {
                    throw new Exception("Документ не является 2D документом");
                }

                if (!CheckViewsNotBroken(doc2D))
                    return false;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw ex;
            }
            finally
            {
                //doc2D.Close(Kompas6Constants.DocumentCloseOptions.kdDoNotSaveChanges);
            }


            return true;
        }

        //TODO fix
        private static bool CheckViewsNotBroken(IKompasDocument2D doc2D)
        {
            /*var manager = doc2D.ViewsAndLayersManager;
            IKompasDocument2D1 doc2D1 = (IKompasDocument2D1)doc2D;

            var views = manager.Views;

            foreach (View view in views)
            {
                Console.WriteLine(view.Name);
                var result = doc2D1.DestroyObjects(view);
                if (!result)
                    return false;
            }*/

            return true;
        }
    }
}

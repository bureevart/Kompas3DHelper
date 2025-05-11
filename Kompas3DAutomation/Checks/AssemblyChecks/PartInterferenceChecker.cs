using System;
using Kompas6API5;
using KompasAPI7;

namespace Kompas3DAutomation.Checks.AssemblyChecks
{
    public class PartInterferenceChecker
    {
        public static bool CheckPartInterference(KompasObject kompasObject, string path, bool checkSketches = false, bool checkCoordinates = false)
        {
            IApplication app = (IApplication)kompasObject.ksGetApplication7();

            if (app == null)
            {
                throw new Exception("Не удалось получить экземпляр IApplication через API7.");
            }

            app.Documents.Open(path, true, true);
            IAssemblyDocument assemblyDocument = (IAssemblyDocument)app.ActiveDocument;

            try
            {
                if (assemblyDocument is null)
                {
                    throw new Exception("Документ не является 3D документом");
                }


                Part7 part7 = assemblyDocument.TopPart;
                if (part7 == null)
                {
                    throw new Exception("Не удалось получить интерфейс IPart7 из открытого документа.");
                }


                if (!CheckPartIntersect(kompasObject, (IFeature7)part7))
                    return false;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw ex;
            }
            finally
            {
                assemblyDocument.Close(Kompas6Constants.DocumentCloseOptions.kdDoNotSaveChanges);
            }


            return true;
        }

        public static bool CheckPartInterferenceForActiveDocument(KompasObject kompasObject, bool checkSketches = false, bool checkCoordinates = false)
        {
            IApplication app = (IApplication)kompasObject.ksGetApplication7();

            if (app == null)
            {
                throw new Exception("Не удалось получить экземпляр IApplication через API7.");
            }

            IAssemblyDocument assemblyDocument = (IAssemblyDocument)app.ActiveDocument;

            try
            {
                if (assemblyDocument is null)
                {
                    throw new Exception("Документ не является сборкой");
                }


                Part7 part7 = assemblyDocument.TopPart;
                if (part7 == null)
                {
                    throw new Exception("Не удалось получить интерфейс IPart7 из открытого документа.");
                }

                if (!CheckPartIntersect(kompasObject, (IFeature7)part7))
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

        private static bool CheckPartIntersect(KompasObject kompasObject, IFeature7 f7)
        {
            if (f7.ResultBodies is object[] bodies)
            {
                foreach (var item in bodies)
                {
                    if (item is IBody7 body)
                    {
                        kompasObject.ksMessage(body.Name);

                        //if (body.Hidden) return false;
                    }
                }
            }
            else if (f7.ResultBodies is IBody7 body)
            {
                kompasObject.ksMessage(body.Name);
                if (body.Hidden) return false;
            }
            else 
            {
                kompasObject.ksMessage("f7.ResultBodies");
                kompasObject.ksMessage(f7.ResultBodies);
            }

            return true;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kompas6API5;
using KompasAPI7;

namespace Kompas3DAutomation.Checks.AssemblyChecks
{
    public class HiddenObjectsChecker
    {
        public static bool CheckHiddenObjects(KompasObject kompasObject, string path, bool checkSketches = false, bool checkCoordinates = false)
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


                if (!CheckHiddenObjects(kompasObject, part7))
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

        public static bool CheckHiddenObjectsForActiveDocument(KompasObject kompasObject, bool checkSketches = false, bool checkCoordinates = false)
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

                if (!CheckHiddenObjects(kompasObject, part7))
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

        private static bool CheckHiddenObjects(KompasObject kompasObject, IPart7 part)
        {
            var details = GetDetailsAndAssembliesRecursive(part);

            foreach (var detail in details)
            {
                Console.WriteLine($"{detail.Name}");
                if(detail.Hidden)
                    return false;
            }

            return true;
        }

        private static List<IPart7> GetDetailsAndAssembliesRecursive(IPart7 part)
        {
            var parts = new List<IPart7>
            {
                part
            };

            foreach(IPart7 item in part.Parts)
            {
                if (item.Detail == true) parts.Add(item);
                if (item.Detail == false)
                    parts.AddRange(GetDetailsAndAssembliesRecursive(item));
            }

            return parts;

        }
    }
}

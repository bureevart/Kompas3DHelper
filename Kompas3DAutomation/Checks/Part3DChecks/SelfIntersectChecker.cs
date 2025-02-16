using Kompas6API5;
using Kompas6Constants3D;
using KompasAPI7;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace Kompas3DAutomation.Checks.Part3DChecks
{
    public class SelfIntersectChecker
    {
        public static bool CheckSelfIntersectionOfFaces(KompasObject kompasObject, string path)
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

                if (!CheckSelfIntersect((ISurfaceContainer)part7))
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

        private static bool CheckSelfIntersect(ISurfaceContainer part)
        {
            foreach (IConicSurface surface in part.ConicSurfaces)
            {
                if (surface.IsSelfIntersect)
                    return false;
            }

            // Проверка CloudPointsSurfaces
            foreach (ICloudPointsSurface surface in part.CloudPointsSurfaces)
            {
                if (surface.CheckSelfIntersection)
                    return false;
            }

            // Проверка JointSurfaces
            foreach (IJointSurface surface in part.JointSurfaces)
            {
                if (surface.CheckSelfIntersection)
                    return false;
            }

            // Проверка MeshPointsSurfaces
            foreach (IMeshPointsSurface surface in part.MeshPointsSurfaces)
            {
                if (surface.CheckSelfIntersection)
                    return false;
            }

            // Проверка NurbsSurfacesByCurvesMeshs
            foreach (INurbsSurfaceByCurvesMesh surface in part.NurbsSurfacesByCurvesMeshs)
            {
                if (surface.CheckSelfIntersection)
                    return false;
            }

            // Проверка RuledSurfaces
            foreach (IRuledSurface surface in part.RuledSurfaces)
            {
                if (surface.CheckSelfIntersection)
                    return false;
            }

            return true;
        }
    }
}

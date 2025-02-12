using Kompas6API5;
using KompasAPI7;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kompas3DAutomation.Checks.Part3DChecks
{
    public class HiddenObjectsChecker
    {
        public static bool CheckHiddenObjectsPresent(KompasObject kompasObject, string path, bool checkSketches = false, bool checkCoordinates = false)
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


                if (checkSketches && !CheckSketchHidden((IModelContainer)part7))
                    return false;

                if (checkCoordinates && !CheckCoordinates((IAuxiliaryGeomContainer)part7))
                    return false;

                if (!CheckSurfaces((ISurfaceContainer)part7))
                    return false;

                if (!CheckBodiesHidden((IFeature7)part7))
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

        private static bool CheckBodiesHidden(IFeature7 f7)
        {
            if (f7.ResultBodies is object[] bodies)
            {
                foreach (var item in bodies)
                {
                    if (item is IBody7 body)
                    {
                        if (body.Hidden) return false;
                    }
                }
            }
            else if (f7.ResultBodies is IBody7 body)
            {
                if (body.Hidden) return false;
            }

            return true;
        }

        public static bool CheckSurfaces(ISurfaceContainer part)
        {
            // Проверка ConicSurfaces
            foreach (IConicSurface surface in part.ConicSurfaces)
            {
                if (surface.Hidden)
                    return false;
            }

            // Проверка CloudPointsSurfaces
            foreach (ICloudPointsSurface surface in part.CloudPointsSurfaces)
            {
                if (surface.Hidden)
                    return false;
            }

            // Проверка EquidistantSurfaces
            foreach (IEquidistantSurface surface in part.EquidistantSurfaces)
            {
                if (surface.Hidden)
                    return false;
            }

            // Проверка EvolutionSurfaces
            foreach (IEvolution surface in part.EvolutionSurfaces) // Если тип известен, замените var на конкретный интерфейс
            {
                // Предполагается, что у surface есть свойство Hidden
                if (surface.Hidden)
                    return false;
            }

            // Проверка ExtensionSurfaces
            foreach (IExtensionSurface surface in part.ExtensionSurfaces)
            {
                if (surface.Hidden)
                    return false;
            }

            // Проверка ExtrusionSurfaces
            foreach (IExtrusionSurface surface in part.ExtrusionSurfaces) // Замените var на конкретный интерфейс, если он известен
            {
                if (surface.Hidden)
                    return false;
            }

            // Проверка ImportedSurfaces
            foreach (IImportedSurface surface in part.ImportedSurfaces)
            {
                if (surface.Hidden)
                    return false;
            }

            // Проверка JointSurfaces
            foreach (IJointSurface surface in part.JointSurfaces)
            {
                if (surface.Hidden)
                    return false;
            }

            // Проверка LoftSurfaces
            foreach (Loft surface in part.LoftSurfaces)
            {
                if (surface.Hidden)
                    return false;
            }

            // Проверка MeshPointsSurfaces
            foreach (IMeshPointsSurface surface in part.MeshPointsSurfaces)
            {
                if (surface.Hidden)
                    return false;
            }

            // Проверка NurbsSurfaces
            foreach (INurbsSurface surface in part.NurbsSurfaces)
            {
                if (surface.Hidden)
                    return false;
            }

            // Проверка NurbsSurfacesByCurvesMeshs
            foreach (INurbsSurfaceByCurvesMesh surface in part.NurbsSurfacesByCurvesMeshs)
            {
                if (surface.Hidden)
                    return false;
            }

            // Проверка RestoredSurfaces
            foreach (IRestoredSurface surface in part.RestoredSurfaces)
            {
                if (surface.Hidden)
                    return false;
            }

            // Проверка RotatedSurfaces
            foreach (IRotatedSurface surface in part.RotatedSurfaces)
            {
                if (surface.Hidden)
                    return false;
            }

            // Проверка RuledSurfaces
            foreach (IRuledSurface surface in part.RuledSurfaces)
            {
                if (surface.Hidden)
                    return false;
            }

            // Проверка SurfacePatches
            foreach (ISurfacePatch surface in part.SurfacePatches)
            {
                if (surface.Hidden)
                    return false;
            }

            // Проверка SurfaceSewers
            foreach (ISurfaceSewer surface in part.SurfaceSewers)
            {
                if (surface.Hidden)
                    return false;
            }

            // Проверка TrimmedSurfaces
            foreach (ITrimmedSurface surface in part.TrimmedSurfaces)
            {
                if (surface.Hidden)
                    return false;
            }

            return true;
        }

        public static bool CheckCoordinates(IAuxiliaryGeomContainer part)
        {
            var coordSystems = part.LocalCoordinateSystems;

            foreach (ILocalCoordinateSystem coordSystem in coordSystems)
            {
                if (coordSystem.Hidden)
                    return false;
            }
            //TODO не проверяется ось "Начало координат"

            return true;
        }


        public static bool CheckSketchHidden(IModelContainer part)
        {
            var sketchs = part.Sketchs;

            foreach (ISketch sketch in sketchs)
            {
                Console.WriteLine("Проверка Sketch: " + sketch.Name);

                //есть серые эскизы, их нельзя вручную включить видимость
                if (sketch.Hidden && sketch.Valid)
                    return false;
            }

            return true;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using Kompas6API5;
using KompasAPI7;
using Kompas3DAutomation.Checks;
using Kompas3DAutomation.Results;
using Kompas3DAutomation.Checks.DrawingChecks;

namespace Kompas3DAutomation.Checks.Part3DChecks
{
    internal sealed class HiddenObjectsChecker : IChecker
    {
        private readonly KompasObject _kompas;
        private readonly IKompasDocument3D _doc3D;
        private readonly bool _checkSketches;
        private readonly bool _checkCoordinates;

        public HiddenObjectsChecker(
            KompasObject kompas,
            IKompasDocument3D doc3D,
            bool checkSketches,
            bool checkCoordinates)
        {
            _kompas = kompas;
            _doc3D = doc3D;
            _checkSketches = checkSketches;
            _checkCoordinates = checkCoordinates;
        }

        public IEnumerable<CheckViolation> Run()
        {
            var chooser = _doc3D.ChooseManager;
            var part7 = _doc3D.TopPart;

            // Sketches
            if (_checkSketches)
            {
                var mc = (IModelContainer)part7;
                foreach (ISketch sk in mc.Sketchs)
                    if (sk.Hidden && sk.Valid)
                        yield return Violation(sk, $"Эскиз «{sk.Name}» скрыт");
            }

            // Coordinate Systems
            if (_checkCoordinates)
            {
                var ac = (IAuxiliaryGeomContainer)part7;
                foreach (ILocalCoordinateSystem cs in ac.LocalCoordinateSystems)
                    if (cs.Hidden)
                        yield return Violation(cs, "Локальная система координат скрыта");
            }

            // Surfaces
            var sc = (ISurfaceContainer)part7;
            foreach (var surf in IterateAllSurfaces(sc))
                if (surf.Hidden)
                    yield return Violation(surf, $"Поверхность «{surf.GetType().Name}» скрыта");

            // Bodies
            var f7 = (IFeature7)part7;
            if (f7.ResultBodies is IEnumerable bodies)
            {
                foreach (var o in bodies)
                {
                    if (o is IBody7 b && b.Hidden)
                        yield return Violation(b, $"Тело «{b.Name}» скрыто");
                }
            }
        }

        private static IEnumerable<dynamic> IterateAllSurfaces(ISurfaceContainer c)
        {
            // сводим все виды поверхностей в один поток
            foreach (IConicSurface s in c.ConicSurfaces) yield return s;
            foreach (ICloudPointsSurface s in c.CloudPointsSurfaces) yield return s;
            foreach (IEquidistantSurface s in c.EquidistantSurfaces) yield return s;
            foreach (IEvolution s in c.EvolutionSurfaces) yield return s;
            foreach (IExtensionSurface s in c.ExtensionSurfaces) yield return s;
            foreach (IExtrusionSurface s in c.ExtrusionSurfaces) yield return s;
            foreach (IImportedSurface s in c.ImportedSurfaces) yield return s;
            foreach (IJointSurface s in c.JointSurfaces) yield return s;
            foreach (Loft s in c.LoftSurfaces) yield return s;
            foreach (IMeshPointsSurface s in c.MeshPointsSurfaces) yield return s;
            foreach (INurbsSurface s in c.NurbsSurfaces) yield return s;
            foreach (INurbsSurfaceByCurvesMesh s in c.NurbsSurfacesByCurvesMeshs) yield return s;
            foreach (IRestoredSurface s in c.RestoredSurfaces) yield return s;
            foreach (IRotatedSurface s in c.RotatedSurfaces) yield return s;
            foreach (IRuledSurface s in c.RuledSurfaces) yield return s;
            foreach (ISurfacePatch s in c.SurfacePatches) yield return s;
            foreach (ISurfaceSewer s in c.SurfaceSewers) yield return s;
            foreach (ITrimmedSurface s in c.TrimmedSurfaces) yield return s;
        }

        private CheckViolation Violation(object target, string msg) =>
            new CheckViolation(
                CheckName: nameof(CheckPart3D.Part3DChecks.HiddenObjectsPresent),
                Message: msg,
                TargetObject: target,
                Highlighter: () => _doc3D.ChooseManager.Choose(target)
            );
    }
}

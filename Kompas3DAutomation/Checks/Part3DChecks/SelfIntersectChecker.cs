using System.Collections.Generic;
using Kompas6API5;
using KompasAPI7;
using Kompas3DAutomation.Results;

namespace Kompas3DAutomation.Checks.Part3DChecks
{
    internal sealed class SelfIntersectChecker : IChecker
    {
        private readonly KompasObject _kompas;
        private readonly IKompasDocument3D _doc3D;

        public SelfIntersectChecker(KompasObject kompas, IKompasDocument3D doc3D)
        {
            _kompas = kompas;
            _doc3D = doc3D;
        }

        public IEnumerable<CheckViolation> Run()
        {
            var chooser = _doc3D.ChooseManager;
            var part7 = _doc3D.TopPart;
            var surfC = (ISurfaceContainer)part7;

            // ConicSurfaces
            foreach (IConicSurface s in surfC.ConicSurfaces)
                if (s.IsSelfIntersect)
                    yield return Violation(s, "Коническая поверхность самопересекается");

            // CloudPointsSurfaces
            foreach (ICloudPointsSurface s in surfC.CloudPointsSurfaces)
                if (s.CheckSelfIntersection)
                    yield return Violation(s, "Поверхность точек облака самопересекается");

            // JointSurfaces
            foreach (IJointSurface s in surfC.JointSurfaces)
                if (s.CheckSelfIntersection)
                    yield return Violation(s, "Сопрягающая поверхность самопересекается");

            // MeshPointsSurfaces
            foreach (IMeshPointsSurface s in surfC.MeshPointsSurfaces)
                if (s.CheckSelfIntersection)
                    yield return Violation(s, "Сеть поверхностей самопересекается");

            // NurbsSurfacesByCurvesMeshs
            foreach (INurbsSurfaceByCurvesMesh s in surfC.NurbsSurfacesByCurvesMeshs)
                if (s.CheckSelfIntersection)
                    yield return Violation(s, "Nurbs‑поверхность самопересекается");

            // RuledSurfaces
            foreach (IRuledSurface s in surfC.RuledSurfaces)
                if (s.CheckSelfIntersection)
                    yield return Violation(s, "Поверхность по правилам самопересекается");
        }

        private CheckViolation Violation(object target, string msg) =>
            new CheckViolation(
                CheckName: nameof(CheckPart3D.Part3DChecks.SelfIntersectionOfFaces),
                Message: msg,
                TargetObject: target,
                Highlighter: () => _doc3D.ChooseManager.Choose(target)
            );
    }
}

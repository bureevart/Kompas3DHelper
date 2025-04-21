using System.Collections;
using System.Collections.Generic;
using Kompas6API5;
using Kompas6Constants3D;
using KompasAPI7;
using Kompas3DAutomation.Checks;
using Kompas3DAutomation.Results;
using Kompas3DAutomation.Checks.DrawingChecks;

namespace Kompas3DAutomation.Checks.Part3DChecks
{
    internal sealed class SingleSolidBodyChecker : IChecker
    {
        private readonly KompasObject _kompas;
        private readonly IKompasDocument3D _doc3D;

        public SingleSolidBodyChecker(KompasObject kompas, IKompasDocument3D doc3D)
        {
            _kompas = kompas;
            _doc3D = doc3D;
        }

        public IEnumerable<CheckViolation> Run()
        {
            var chooser = _doc3D.ChooseManager;
            var part7 = _doc3D.TopPart;
            var f7 = (IFeature7)part7;

            int count = 0;
            object first = null;

            if (f7.ResultBodies is IEnumerable bodies)
            {
                foreach (var o in bodies)
                    if (o is IBody7 b && b.IsSolid)
                    {
                        if (count == 0) first = b;
                        count++;
                    }
            }

            if (count > 1 && first != null)
            {
                yield return new CheckViolation(
                    CheckName: nameof(CheckPart3D.Part3DChecks.SingleSolidBody),
                    Message: $"Найдено {count} тел вместо одного",
                    TargetObject: first,
                    Highlighter: () => chooser.Choose(first)
                );
            }
        }
    }
}

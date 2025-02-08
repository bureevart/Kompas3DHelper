using Kompas3DAutomation.Checks;
using Kompas3DAutomation.Results;
using Kompas6API5;
using Kompas6Constants3D;
using System;

namespace Kompas3DAutomation
{
    public class CheckPart3D : CheckBase
    {
        public CheckPart3D(KompasConnectionObject kompasConnectionObject) : base(kompasConnectionObject)
        {
        }

        public CheckResult Check(string path, Part3DChecks checks)
        {
            if (!_kompasObject.IsConnected)
            {
                return new CheckResult()
                {
                    ResultType = CheckResults.ConnectionError,
                };
            }

            try
            {
                var doc3D = (ksDocument3D)_kompasObject.Kompas.Document3D();
                doc3D.Open(path, false);

                var part = (ksPart)doc3D.GetPart((short)Part_Type.pTop_Part);
                if (part == null)
                {
                    return new CheckResult()
                    {
                        ResultType = CheckResults.Error,
                        InnerResult = "Деталь не найдена!"
                    };
                }

                return CheckMassProperties(part);
            }
            catch (Exception ex)
            {
                return new CheckResult()
                {
                    ResultType = CheckResults.NoErrors,
                    InnerResult = $"Ошибка: {ex}"
                };
            }
        }

        private CheckResult CheckMassProperties(ksPart part)
        {
            var massParams = part.GetMass();

            return new CheckResult()
            {
                ResultType = massParams > 0 ? CheckResults.NoErrors : CheckResults.Error,
                InnerResult = massParams > 0 ? $"Масса: {massParams} кг" : "Масса не рассчитана"
            };
        }

        /// <summary>
        /// Проверки для модели деталей.
        /// </summary>
        [Flags]
        public enum Part3DChecks
        {
            None = 0,
            /// <summary>
            /// Самопересечение граней.
            /// </summary>
            SelfIntersectionOfFaces = 1 << 0,
            /// <summary>
            /// Ограничения в эскизе.
            /// </summary>
            SketchConstraints = 1 << 1,
            /// <summary>
            /// Цвет модели соответствует заданному.
            /// </summary>
            ColorMatchesSpecification = 1 << 2,
            /// <summary>
            /// Не допускается наличие более одного твердого тела в файле модели.
            /// </summary>
            SingleSolidBody = 1 << 3,
            /// <summary>
            /// Проверка расположения объектов по слоям.
            /// </summary>
            LayeredObjectPosition = 1 << 4,
            /// <summary>
            /// Наличие скрытых объектов.
            /// </summary>
            HiddenObjectsPresent = 1 << 5
        }
    }
}

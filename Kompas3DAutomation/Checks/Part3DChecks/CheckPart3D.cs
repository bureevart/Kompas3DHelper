using Kompas3DAutomation.Checks;
using Kompas3DAutomation.Results;
using Kompas6API5;
using Kompas6Constants;
using Kompas6Constants3D;
using KompasAPI7;
using System;
using System.Collections.Generic;

namespace Kompas3DAutomation.Checks.Part3DChecks
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
                    ResultType = CheckResults.ConnectionError
                };
            }

            try
            {
                var errors = new List<string>();

                if (checks.HasFlag(Part3DChecks.SelfIntersectionOfFaces))
                {
                    if (!CheckSelfIntersectionOfFaces())
                        errors.Add("Ошибка самопересечения граней.");
                }

                if (checks.HasFlag(Part3DChecks.SketchConstraints))
                {
                    if (!CheckSketchConstraints())
                        errors.Add("Ошибка ограничений в эскизе.");
                }

                if (checks.HasFlag(Part3DChecks.LayeredObjectPosition))
                {
                    if (!CheckLayeredObjectPosition())
                        errors.Add("Ошибка расположения объектов по слоям.");
                }

                if (checks.HasFlag(Part3DChecks.HiddenObjectsPresent))
                {
                    var checkSketches = !checks.HasFlag(Part3DChecks.DontCheckSketches);
                    var checkCoordinates = !checks.HasFlag(Part3DChecks.DontCheckCoordinates);

                    
                    if (!HiddenObjectsChecker.CheckHiddenObjectsPresent(_kompasObject.Kompas, path, 
                        checkSketches: checkSketches, 
                        checkCoordinates: checkCoordinates))
                        errors.Add("Ошибка наличия скрытых объектов");
                }

                if (checks.HasFlag(Part3DChecks.SingleSolidBody))
                {
                    if (!SingleSolidBodyChecker.CheckSingleSolidBody(_kompasObject.Kompas, path))
                        errors.Add("Ошибка допуска наличия более одного твердого тела в файле модели.");
                }

                if (checks.HasFlag(Part3DChecks.ColorMatchesSpecification))
                {
                    if (!CheckColorMatchesSpecification())
                        errors.Add("Ошибка несоответствия цвета модели.");
                }

                if (errors.Count > 0)
                {
                    var error = string.Empty;
                    foreach (var err in errors)
                        error += err + " ";

                    return new CheckResult()
                    {
                        ResultType = CheckResults.Error,
                        InnerResult = error
                    };
                }

                return CheckResult.GetNoErrorsResult();
            }
            catch (Exception ex)
            {
                return new CheckResult()
                {
                    ResultType = CheckResults.Error,
                    InnerResult = $"Ошибка: {ex}"
                };
            }
        }

        public bool CheckSelfIntersectionOfFaces()
        {
            

            return true;
        }

        public bool CheckSketchConstraints()
        {


            return true;
        }

        public bool CheckLayeredObjectPosition()
        {


            return true;
        }

        public bool CheckColorMatchesSpecification( )
        {


            return true;
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
            HiddenObjectsPresent = 1 << 5,
            /// <summary>
            /// Не проверять эскизы (при скрытых проверках)
            /// </summary>
            DontCheckSketches = 1 << 6,
            /// <summary>
            /// Не проверять координаты (при скрытых проверках)
            /// </summary>
            DontCheckCoordinates = 1 << 7,
        }
    }
}

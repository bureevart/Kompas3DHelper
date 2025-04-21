using Kompas3DAutomation.Checks;
using Kompas3DAutomation.Checks.DrawingChecks;
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
        public CheckPart3D(KompasConnectionObject kompasConnectionObject)
            : base(kompasConnectionObject) { }

        public CheckReport Check(string path, Part3DChecks checks)
        {
            if (!_kompasObject.IsConnected)
                return CheckReport.ConnectionError();

            var app = (IApplication)_kompasObject.Kompas.ksGetApplication7();
            app.Documents.Open(path, true, true);
            var doc3D = (IKompasDocument3D)app.ActiveDocument;

            try
            {
                return RunChecks(doc3D, checks);
            }
            finally
            {
                doc3D.Close(DocumentCloseOptions.kdDoNotSaveChanges);
            }
        }

        public CheckReport CheckForActiveDocument(Part3DChecks checks)
        {
            if (!_kompasObject.IsConnected)
                return CheckReport.ConnectionError();

            var app = (IApplication)_kompasObject.Kompas.ksGetApplication7();
            var doc3D = app.ActiveDocument as IKompasDocument3D
                      ?? throw new InvalidOperationException("Нет активного 3D‑документа.");
            return RunChecks(doc3D, checks);
        }

        private CheckReport RunChecks(IKompasDocument3D doc3D, Part3DChecks checks)
        {
            var report = new CheckReport();
            void Add(IChecker c) => report.Violations.AddRange(c.Run());

            if (checks.HasFlag(Part3DChecks.SelfIntersectionOfFaces))
                Add(new SelfIntersectChecker(_kompasObject.Kompas, doc3D));

            if (checks.HasFlag(Part3DChecks.HiddenObjectsPresent))
            {
                var checkSketches = !checks.HasFlag(Part3DChecks.DontCheckSketches);
                var checkCoordinates = !checks.HasFlag(Part3DChecks.DontCheckCoordinates);
                Add(new HiddenObjectsChecker(
                    _kompasObject.Kompas,
                    doc3D,
                    checkSketches,
                    checkCoordinates));
            }

            if (checks.HasFlag(Part3DChecks.SingleSolidBody))
                Add(new SingleSolidBodyChecker(_kompasObject.Kompas, doc3D));

            // TODO: при необходимости добавить SketchConstraints, ColorMatchesSpecification, LayeredObjectPosition

            return report;
        }

        #region CheckTypes

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
        #endregion
    }
}

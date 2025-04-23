using System;
using System.Collections;
using System.Collections.Generic;
using Kompas6API5;
using KompasAPI7;
using Kompas3DAutomation.Results;

namespace Kompas3DAutomation.Checks.DrawingChecks
{
    /// <summary>
    /// Проверка ручного изменения текста размеров.
    /// Логика обхода сохранена, заменены только ранние return → yield return.
    /// </summary>
    internal sealed class ManualTextDimensionChangesChecker : IChecker
    {
        private readonly KompasObject _kompas;
        private readonly IKompasDocument2D _doc2D;

        public ManualTextDimensionChangesChecker(KompasObject kompas, IKompasDocument2D doc2D)
        {
            _kompas = kompas;
            _doc2D = doc2D;
        }

        public IEnumerable<CheckViolation> Run()
        {
            foreach (var v in CheckManualTextDimensionChanges(_doc2D))
                yield return v;
        }

        private IEnumerable<CheckViolation> CheckManualTextDimensionChanges(IKompasDocument2D doc2D)
        {
            var chooser = ((IKompasDocument2D1)doc2D).ChooseManager;
            var views = doc2D.ViewsAndLayersManager.Views;

            foreach (IView view in views)
            {
                ISymbols2DContainer c = (ISymbols2DContainer)view;

                // Пробег по всем коллекциям размеров
                foreach (var v in EmitIfManual(c.AngleDimensions, "Угловой размер", chooser)) yield return v;
                foreach (var v in EmitIfManual(c.ArcDimensions, "Дуговой размер", chooser)) yield return v;
                foreach (var v in EmitIfManual(c.BreakLineDimensions, "Ломаный размер", chooser)) yield return v;
                foreach (var v in EmitIfManual(c.DiametralDimensions, "Диаметральный размер", chooser)) yield return v;
                foreach (var v in EmitIfManual(c.HeightDimensions, "Высотный размер", chooser)) yield return v;
                foreach (var v in EmitIfManual(c.LineDimensions, "Линейный размер", chooser)) yield return v;
                foreach (var v in EmitIfManual(c.RadialDimensions, "Радиальный размер", chooser)) yield return v;
            }
        }

        /// <summary>
        /// Перебирает любую COM‑коллекцию размеров (не только IEnumerable<IDimensionText>)
        /// </summary>
        private IEnumerable<CheckViolation> EmitIfManual(
            object dimsCollection,
            string kind,
            dynamic chooser)
        {
            if (dimsCollection is IEnumerable dims)
            {
                foreach (var item in dims)
                {
                    var d = (IDimensionText)item;
                    if (!d.AutoNominalValue)
                    {
                        yield return new CheckViolation(
                            CheckName: $"{nameof(CheckDrawing.DrawingChecks.ManualTextDimensionChanges)}",
                            Message: $"{kind}: ручной текст «{d.NominalText.Str}» Номинальное значение: {d.NominalValue}",
                            TargetObject: d,
                            Highlighter: () => chooser.Choose(d)
                        );
                    }
                }
            }
        }
    }
}

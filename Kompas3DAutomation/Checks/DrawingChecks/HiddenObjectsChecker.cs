using System.Collections.Generic;
using Kompas6API5;
using KompasAPI7;
using Kompas3DAutomation.Results;

namespace Kompas3DAutomation.Checks.DrawingChecks
{
    /// <summary>
    /// Проверка отсутствия скрытых объектов. Алгоритм обхода сохранён в точности, изменены только return‑ы на yield‑return.
    /// </summary>
    internal sealed class HiddenObjectsChecker : IChecker
    {
        private readonly IKompasDocument2D _doc2D;
        private readonly KompasObject _kompas;

        public HiddenObjectsChecker(KompasObject kompas, IKompasDocument2D doc2D)
        {
            _kompas = kompas;
            _doc2D = doc2D;
        }

        public IEnumerable<CheckViolation> Run()
        {
            foreach (var v in CheckViewsAndLayersHidden(_doc2D))
                yield return v;

            foreach (var v in CheckLayersInLayerGroupsHidden(_doc2D))
                yield return v;
        }

        // ---- Оригинальные методы пользователя (bool → IEnumerable<CheckViolation>) ----

        private IEnumerable<CheckViolation> CheckViewsAndLayersHidden(IKompasDocument2D doc2D)
        {
            var manager = doc2D.ViewsAndLayersManager;
            var views = manager.Views;
            var chooser = ((IKompasDocument2D1)doc2D).ChooseManager;

            foreach (View view in views)
            {
                if (!view.Visible)
                    yield return new CheckViolation($"{nameof(CheckDrawing.DrawingChecks.NoHiddenObjects)}", $"Вид \"{view.Name}\" скрыт", view, () => chooser.Choose(view));

                foreach (Layer layer in view.Layers)
                {
                    if (!layer.Visible)
                        yield return new CheckViolation($"{nameof(CheckDrawing.DrawingChecks.NoHiddenObjects)}", $"Слой \"{layer.Name}\" во виде \"{view.Name}\" скрыт", layer, () => chooser.Choose(layer));
                }
            }
        }

            private IEnumerable<CheckViolation> CheckLayersInLayerGroupsHidden(IKompasDocument2D doc2D)
            {
                var manager = doc2D.ViewsAndLayersManager;
                var layerGroups = manager.LayerGroups;

                foreach (LayerGroup layerGroup in layerGroups)
                {
                    foreach (var v in CheckForHiddenInLayerGroup(layerGroup, doc2D))
                        yield return v;

                    /*var chooser = ((IKompasDocument2D1)doc2D).ChooseManager;
                    foreach (Layer layer in layerGroup.Layers)
                    {
                        if (!layer.Visible)
                            yield return new CheckViolation($"{nameof(CheckDrawing.DrawingChecks.NoHiddenObjects)}", $"Слой \"{layer.Name}\" в группе \"{layerGroup.Name}\" скрыт", layer, () => chooser.Choose(layer));
                    }*/
                }
            }

            private IEnumerable<CheckViolation> CheckForHiddenInLayerGroup(LayerGroup layerGroup, IKompasDocument2D doc2D)
            {
                var chooser = ((IKompasDocument2D1)doc2D).ChooseManager;

                foreach (LayerGroup innerLayerGroup in layerGroup.LayerGroups)
                {
                    foreach (var v in CheckForHiddenInLayerGroup(innerLayerGroup, doc2D))
                        yield return v;
                }

                foreach (Layer layer in layerGroup.Layers)
                {
                    if (!layer.Visible)
                        yield return new CheckViolation($"{nameof(CheckDrawing.DrawingChecks.NoHiddenObjects)}", $"Слой \"{layer.Name}\" в группе \"{layerGroup.Name}\" скрыт", layer, () => chooser.Choose(layer));
                }
            }
    }
}
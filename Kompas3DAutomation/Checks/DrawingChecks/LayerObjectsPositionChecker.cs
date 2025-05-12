using System;
using System.Collections.Generic;
using Kompas3DAutomation.Results;
using Kompas6API5;
using KompasAPI7;

namespace Kompas3DAutomation.Checks.DrawingChecks
{
    public class LayerObjectsPositionChecker : IChecker
    {
        private readonly IKompasDocument2D _doc2D;
        private readonly KompasObject _kompas;
        
        public LayerObjectsPositionChecker(KompasObject kompas, IKompasDocument2D doc2D)
        {
            _kompas = kompas;
            _doc2D = doc2D;
        }
        
        public IEnumerable<CheckViolation> Run()
        {
            foreach (var v in CheckLayerObjectsPosition(_doc2D))
                yield return v;
        }

        private IEnumerable<CheckViolation> CheckLayerObjectsPosition(IKompasDocument2D doc2D)
        {
            var chooser = ((IKompasDocument2D1)doc2D).ChooseManager;
            IKompasDocument2D1 doc2D1 = ((IKompasDocument2D1)doc2D);
            doc2D1.RebuildDocument();
            
            
            var viewManager = doc2D.ViewsAndLayersManager;

            var views = viewManager.Views;
            var view = views.ActiveView;
            view.Update();

            var layers = view.Layers;
            
            foreach (ILayer layer in layers)
            {
                layer.Update();
                yield return new CheckViolation(
                    CheckName: $"{nameof(CheckDrawing.DrawingChecks.LayerObjectsPosition)}",
                    Message: layer.Name + " " + layer.LayerNumber + " " + layer.Color,
                    TargetObject: layer,
                    Highlighter: () => chooser.Choose(layer)
                );
            }
        }
    }
}
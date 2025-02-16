using Kompas6API5;
using KompasAPI7;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace Kompas3DAutomation.Checks.DrawingChecks
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
            IKompasDocument2D doc2D = (IKompasDocument2D)app.ActiveDocument;
            
            try
            {
                if (doc2D is null)
                {
                    throw new Exception("Документ не является 3D документом");
                }

                if (!CheckViewsAndLayersHidden(doc2D))
                    return false;

                if (!CheckLayersInLayerGroupsHidden(doc2D))
                    return false;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw ex;
            }
            finally
            {
                doc2D.Close(Kompas6Constants.DocumentCloseOptions.kdDoNotSaveChanges);
            }


            return true;
        }

        private static bool CheckViewsAndLayersHidden(IKompasDocument2D doc2D)
        {
            var manager = doc2D.ViewsAndLayersManager;

            var views = manager.Views;

            foreach (View view in views)
            {
                Console.WriteLine(view.Name);
                if(!view.Visible)
                    return false;

                foreach (Layer layer in view.Layers)
                {
                    Console.WriteLine("Слой " + layer.Name);
                    if (!layer.Visible)
                        return false;
                }
            }

            return true;
        }

        private static bool CheckLayersInLayerGroupsHidden(IKompasDocument2D doc2D)
        {
            var manager = doc2D.ViewsAndLayersManager;

            var layerGroups = manager.LayerGroups;

            foreach (LayerGroup layerGroup in layerGroups)
            {
                Console.WriteLine("Группа слоев " + layerGroup.Name);
                if (!CheckForHiddenInLayerGroup(layerGroup))
                    return false;

                foreach (Layer layer in layerGroup.Layers)
                {
                    Console.WriteLine("Слой " + layer.Name);
                    if (!layer.Visible)
                        return false;
                }
            }

            return true;
        }

        public static bool CheckForHiddenInLayerGroup(LayerGroup layerGroup)
        {
            foreach (LayerGroup innerLayerGroup in layerGroup.LayerGroups)
            {
                Console.WriteLine("Группа слоев " + layerGroup.Name);
                if (!CheckForHiddenInLayerGroup(innerLayerGroup))
                    return false;
            }

            foreach (Layer layer in layerGroup.Layers)
            {
                Console.WriteLine("Слой " + layerGroup.Name);
                if (!layer.Visible)
                    return false;
            }

            return true;
        }
    }
}

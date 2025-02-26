using Kompas6API5;
using KompasAPI7;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kompas3DAutomation.Checks.DrawingChecks
{
    public class ManualTextDimensionChangesChecker
    {
        public static bool CheckManualTextDimensionChanges(KompasObject kompasObject, string path)
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

                if (!CheckManualTextDimensionChanges(doc2D))
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

        private static bool CheckManualTextDimensionChanges(IKompasDocument2D doc2D)
        {
            var manager = doc2D.ViewsAndLayersManager;

            var views = manager.Views;
            var layoutSheets = doc2D.LayoutSheets;


            foreach (IView view in views)
            {
                Console.WriteLine(view.Name);

                var x = view.X;
                var y = view.Y;

                ISymbols2DContainer symbols2DContainer = (ISymbols2DContainer)view;

                foreach (IDimensionText item in symbols2DContainer.AngleDimensions)
                {
                    Console.WriteLine("Действительное значение: " + item.NominalValue);
                    Console.WriteLine("Действительный текст: " + item.NominalText.Str + "\n");
                    if (!item.AutoNominalValue) return false;
                }

                foreach (IDimensionText item in symbols2DContainer.ArcDimensions)
                {
                    Console.WriteLine("Действительное значение: " + item.NominalValue);
                    Console.WriteLine("Действительный текст: " + item.NominalText.Str + "\n");
                    if (!item.AutoNominalValue) return false;
                }

                foreach (IDimensionText item in symbols2DContainer.BreakLineDimensions)
                {
                    Console.WriteLine("Действительное значение: " + item.NominalValue);
                    Console.WriteLine("Действительный текст: " + item.NominalText.Str + "\n");
                    if (!item.AutoNominalValue) return false;
                }

                foreach (IDimensionText item in symbols2DContainer.DiametralDimensions)
                {
                    Console.WriteLine("Действительное значение: " + item.NominalValue);
                    Console.WriteLine("Действительный текст: " + item.NominalText.Str + "\n");
                    if (!item.AutoNominalValue) return false;
                }

                foreach (IDimensionText item in symbols2DContainer.HeightDimensions)
                {
                    Console.WriteLine("Действительное значение: " + item.NominalValue);
                    Console.WriteLine("Действительный текст: " + item.NominalText.Str + "\n");
                    if (!item.AutoNominalValue) return false;
                }

                foreach (IDimensionText item in symbols2DContainer.LineDimensions)
                {
                    Console.WriteLine("Действительное значение: " + item.NominalValue);
                    Console.WriteLine("Действительный текст: " + item.NominalText.Str + "\n");
                    if (!item.AutoNominalValue) return false;
                }

                foreach (IDimensionText item in symbols2DContainer.RadialDimensions)
                {
                    Console.WriteLine("Действительное значение: " + item.NominalValue);
                    Console.WriteLine("Действительный текст: " + item.NominalText.Str + "\n");
                    if (!item.AutoNominalValue) return false;
                }

                foreach (IDimensionText item in symbols2DContainer.BreakLineDimensions)
                {
                    Console.WriteLine("Действительное значение: " + item.NominalValue);
                    Console.WriteLine("Действительный текст: " + item.NominalText.Str + "\n");
                    if (!item.AutoNominalValue) return false;
                }
            }

            return true;
        }
    }
}

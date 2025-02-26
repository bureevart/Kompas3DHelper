using Kompas6API5;
using KompasAPI7;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Kompas3DAutomation.Checks.DrawingChecks
{
    public class NoObjectsCrossingSheetBorderChecker
    {
        public static bool CheckNoObjectsCrossingSheetBorder(KompasObject kompasObject, string path)
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

                if (!CheckNoObjectsCrossingSheetBorder(doc2D))
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

        //TODO fix
        private static bool CheckNoObjectsCrossingSheetBorder(IKompasDocument2D doc2D)
        {
            var manager = doc2D.ViewsAndLayersManager;
            IKompasDocument2D1 doc2D1 = (IKompasDocument2D1)doc2D;

            var views = manager.Views;
            var layoutSheets = doc2D.LayoutSheets;
            

            foreach (IView view in views)
            {
                Console.WriteLine(view.Name);
                
                var x = view.X;
                var y = view.Y;

                IDrawingContainer drawingContainer = (IDrawingContainer)view;
                foreach (ICircle item in drawingContainer.Circles)
                {
                    var cx1 = item.X;
                    var cy1 = item.Y;
                    var cx2 = item.X + item.Radius * 2;
                    var cy2 = item.Y + item.Radius * 2;

                    if (!GetPlace(layoutSheets, cx1, cy1, cx2, cy2)) return false;
                }

                foreach (IRectangle item in drawingContainer.Rectangles)
                {
                    var rx1 = item.X;
                    var ry1 = item.Y;
                    var rx2 = item.X + item.Width;
                    var ry2 = item.Y + item.Height;

                    if (!GetPlace(layoutSheets, rx1, ry1, rx2, ry2)) return false;
                }
            }

            return true;
        }

        public static bool GetPlace(LayoutSheets layoutSheets, double x1, double y1, double x2, double y2)
        {
            foreach (LayoutSheet layoutSheet in layoutSheets)
            {
                var sheetFormat = layoutSheet.Format;

                /*var result = layoutSheet.GetPlaceInsideFrames(out x1, out y1, out x2, out y2);
                if (!result) return false;*/
            }

            return true;
        }
    }
}

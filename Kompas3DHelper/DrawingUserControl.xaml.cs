using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static Kompas3DAutomation.CheckDrawing;

namespace Kompas3DHelper
{
    /// <summary>
    /// Interaction logic for DrawingUserControl.xaml
    /// </summary>
    public partial class DrawingUserControl : UserControl
    {
        public DrawingUserControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Возвращает выбранные проверки для чертежа, основанные на состоянии CheckBox’ов.
        /// </summary>
        public DrawingChecks SelectedDrawingChecks
        {
            get
            {
                DrawingChecks checks = DrawingChecks.None;
                if (DrawingCheck1.IsChecked == true)
                    checks |= DrawingChecks.LayerObjectsPosition;
                if (DrawingCheck2.IsChecked == true)
                    checks |= DrawingChecks.NoHiddenObjects;
                if (DrawingCheck3.IsChecked == true)
                    checks |= DrawingChecks.NoObjectsOutsideDrawing;
                if (DrawingCheck4.IsChecked == true)
                    checks |= DrawingChecks.ViewIntegrity;
                if (DrawingCheck5.IsChecked == true)
                    checks |= DrawingChecks.NoObjectsCrossingSheetBorder;
                if (DrawingCheck6.IsChecked == true)
                    checks |= DrawingChecks.ManualTextDimensionChanges;
                return checks;
            }
        }
    }
}

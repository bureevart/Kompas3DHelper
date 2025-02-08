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
using static Kompas3DAutomation.CheckPart3D;

namespace Kompas3DHelper
{
    /// <summary>
    /// Interaction logic for Part3DUserControl.xaml
    /// </summary>
    public partial class Part3DUserControl : UserControl
    {
        public Part3DUserControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Возвращает выбранные проверки для модели деталей, основанные на состоянии CheckBox’ов.
        /// </summary>
        public Part3DChecks SelectedPartModelChecks
        {
            get
            {
                Part3DChecks checks = Part3DChecks.None;
                if (Part3DCheck1.IsChecked == true)
                    checks |= Part3DChecks.SelfIntersectionOfFaces;
                if (Part3DCheck2.IsChecked == true)
                    checks |= Part3DChecks.SketchConstraints;
                if (Part3DCheck3.IsChecked == true)
                    checks |= Part3DChecks.ColorMatchesSpecification;
                if (Part3DCheck4.IsChecked == true)
                    checks |= Part3DChecks.SingleSolidBody;
                if (Part3DCheck5.IsChecked == true)
                    checks |= Part3DChecks.LayeredObjectPosition;
                if (Part3DCheck6.IsChecked == true)
                    checks |= Part3DChecks.HiddenObjectsPresent;
                return checks;
            }
        }
    }
}

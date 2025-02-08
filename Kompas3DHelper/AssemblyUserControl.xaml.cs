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
using static Kompas3DAutomation.CheckAssembly;

namespace Kompas3DHelper
{
    /// <summary>
    /// Interaction logic for AssemblyUserControl.xaml
    /// </summary>
    public partial class AssemblyUserControl : UserControl
    {
        public AssemblyUserControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Возвращает выбранные проверки для сборочной единицы.
        /// В данном примере проверка одна, но если будет больше – можно объединять их через флаги.
        /// </summary>
        public AssemblyChecks SelectedAssemblyChecks
        {
            get
            {
                AssemblyChecks checks = AssemblyChecks.None;
                if (AssemblyCheck1.IsChecked == true)
                    checks |= AssemblyChecks.PartInterference;
                return checks;
            }
        }
    }
}

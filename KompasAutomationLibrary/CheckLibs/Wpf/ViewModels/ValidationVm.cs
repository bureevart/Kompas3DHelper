using System;
using System.Collections.ObjectModel;
using System.Linq;
using KompasAutomationLibrary.CheckMeta;

namespace KompasAutomationLibrary.CheckLibs.Wpf.ViewModels
{
    public sealed class ValidationVm
    {
        public DocKind CurrentKind { get; }
        public ObservableCollection<CheckNode> Roots { get; } = new();

        public ValidationVm(DocKind currentKind)
        {
            CurrentKind = currentKind;

            foreach (var grp in ImplementedChecks.AllChecks
                         .Where(c => c.Implemented)
                         .GroupBy(c => c.Kind))
            {
                var root = new CheckNode(ImplementedChecks.KindDisplay[grp.Key], isRoot: true);
                Roots.Add(root);

                foreach (var ci in grp)
                {
                    var leaf = new CheckNode(ci) { IsChecked = false };

                    root.Children.Add(leaf);
                }
            }
        }

        /// <summary>Собирает битовую маску из отмеченных проверок.</summary>
        public long GetCheckedBits() =>
            Roots.SelectMany(r => r.Children)
                .Where(n => n.IsChecked == true)
                .Aggregate(0L, (sum, n) => sum | Convert.ToInt64(n.CheckInfo.FlagValue));
    }
}
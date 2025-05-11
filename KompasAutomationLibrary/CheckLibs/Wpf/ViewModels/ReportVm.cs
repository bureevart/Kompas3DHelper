using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Data;
using Kompas3DAutomation.Results;

namespace KompasAutomationLibrary.CheckLibs.Wpf.ViewModels
{
    public sealed class ReportVm : INotifyPropertyChanged
    {
        public ObservableCollection<CheckViolation> Items { get; }
        public ReportVm(CheckReport rep)
        {
            Items = new ObservableCollection<CheckViolation>(rep.Violations);
        }

        /* grouping по CheckName (для XAML) */
        public ICollectionView Grouped =>
            CollectionViewSource.GetDefaultView(Items);
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
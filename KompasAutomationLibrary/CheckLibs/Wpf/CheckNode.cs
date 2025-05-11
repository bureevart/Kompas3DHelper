using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using KompasAutomationLibrary.CheckMeta;

namespace KompasAutomationLibrary.CheckLibs.Wpf
{
    public sealed class CheckNode : INotifyPropertyChanged
    {
        public string Title { get; }
        public bool IsRoot { get; }
        public DocKind Kind { get; }
        public CheckInfo CheckInfo{ get; }

        bool? _isChecked;
        public bool? IsChecked
        {
            get => _isChecked;
            set { _isChecked = value; OnPropertyChanged(); }
        }

        public ObservableCollection<CheckNode> Children { get; } = new();

        public CheckNode(string title, bool isRoot = false)
        {
            Title = title;
            IsRoot = isRoot;
        }

        public CheckNode(CheckInfo ci)
        {
            Title = ci.Display;
            CheckInfo = ci;
            Kind = ci.Kind;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        void OnPropertyChanged([CallerMemberName] string p = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(p));
    }
}
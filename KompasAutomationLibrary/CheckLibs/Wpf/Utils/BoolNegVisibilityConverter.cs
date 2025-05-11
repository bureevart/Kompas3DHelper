using System;
using System.Globalization;
using System.Windows.Controls;
using System.Windows.Data;

namespace KompasAutomationLibrary.CheckLibs.Wpf.Utils
{
    public class BoolNegVisibilityConverter : IValueConverter
    {
        private readonly BooleanToVisibilityConverter _base = new();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool b = System.Convert.ToBoolean(value);
            // если передали параметр "neg", инвертируем
            if (parameter?.ToString() == "neg") b = !b;
            return _base.Convert(b, targetType, parameter, culture);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }
}
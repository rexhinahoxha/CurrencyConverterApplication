using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace CurrencyConverterControl
{
    internal class ComboboxValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string selectedString = value as string;
            if (selectedString != null)
            {
                
                return selectedString;
            }
            else
            {
                
                return Binding.DoNothing; 
            }
        }
    }
}

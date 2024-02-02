using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;


namespace CurrencyConverterApplication
{
    internal class CurrencyValueConverter : IMultiValueConverter
    {
        CurrencyConverterControl.CurrencyConverterControl CurrencyConverterControl { get; set; }
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            // Ensure values array is not null and has expected length
            if (values != null && values.Length == 2)
            {
                // Extract values from the array
                double price = (double)values[0];
                string currencyCode = (string)values[1];

                // Implement your logic to convert the values here
                // For example, concatenate price and currencyCode

                return  CurrencyConverterControl.ConvertValues("USD", currencyCode, price);
            }

            // Handle invalid values array if needed
            return Binding.DoNothing;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

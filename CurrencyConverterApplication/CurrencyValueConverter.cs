using System.Globalization;
using System.Windows.Data;


namespace CurrencyConverterApplication
{
    //Using this converter to hande the conversion of the data grid cells 
    internal class CurrencyValueConverter : IMultiValueConverter
    {
        CurrencyConverterControl.CurrencyConverterControl CurrencyConverterControl = new();
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
         
           
            if (values != null && values.Length >0 && values[0].ToString().Length==3)
            {
                // Extract values from the array
                double price = (double)values[1];
                string currencyCode = (string)values[0];
                
                double priceConverted= CurrencyConverterControl.ConvertValues("USD", currencyCode, price);
                //convert to string as the data grid cells accept string values 
                return string.Format(culture, "{0:0.00}", priceConverted); 
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

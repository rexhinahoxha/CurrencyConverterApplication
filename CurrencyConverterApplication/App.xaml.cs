using System.Configuration;
using System.Data;
using System.Windows;

namespace CurrencyConverterApplication
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static string BaseUrl { get; } = "http://api.currencylayer.com/";
        public static string Access_key { get; } = "6ac174a15508ce09e2e89ad74ae79c45";
    }

}

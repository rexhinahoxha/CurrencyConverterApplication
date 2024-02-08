using CurrencyConverterControl.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CurrencyConverterControl.Data
{
    public interface ICurrencyDataProvider
    {
        List<Currency> GetCurrenciesData();
        Task<double> ConvertAsync(string currencyfrom, string currencyto, double amount);
        double GetConversionRate(string currencyfrom, string currencyto);
        void GetConvertionRates();
    }
}

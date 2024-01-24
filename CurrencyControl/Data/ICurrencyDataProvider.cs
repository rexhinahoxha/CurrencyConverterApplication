using CurrencyControl.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CurrencyControl.Data
{
    public  interface ICurrencyDataProvider
    {
        Task<List<string>> GetCurrencies();
        Task<List<Currency>> GetCurrenciesData();
        Task<double> ConvertAsync(string currencyfrom, string currencyto, double amount);
    }
}

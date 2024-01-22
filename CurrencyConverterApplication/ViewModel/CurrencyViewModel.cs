using CurrencyConverterApplication.Data;
using CurrencyConverterApplication.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CurrencyConverterApplication.ViewModel
{
    public class CurrencyViewModel:ViewModelBase
    {
        private readonly ICurrencyDataProvider _currencyDataProvider;
        private string outputValue;

        public CurrencyViewModel(ICurrencyDataProvider currencyDataProvider)
        {
            this._currencyDataProvider = currencyDataProvider;

        }

        public ObservableCollection<string> Currencylist { get; } = new();
       
        public string OutputValue
        {
            get => outputValue;
            set
            {
                outputValue = value;
                RaisePropertychanged();

            }
        }

        public async override Task LoadAsync()
        {
            try
            {
                if (Currencylist.Any())
                {
                    return;
                }
                var currencies = await _currencyDataProvider.GetCurrencies();

                if (currencies is not null)
                {
                    foreach (var currency in currencies)
                    {

                        Currencylist.Add(currency);
                    }
                }
            }
            catch(Exception ex) { throw ex; }
            
        }

        public void Calculate(string ccyFrom, string ccyTo, double InputValue)
        {
            //code to be done 
            MessageBox.Show("vlera " + InputValue);
            OutputValue = _currencyDataProvider.ConvertAsync(ccyFrom, ccyTo, InputValue).Result.ToString();
        }

        internal void Calculate(string sourceCurrency, string destinationCurrency, bool v)
        {
            throw new NotImplementedException();
        }
    }
}

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
    public class CurrencyViewModel : ViewModelBase
    {
        private readonly ICurrencyDataProvider _currencyDataProvider;
        private readonly IProductDataProvider _productDataProvider;
        private double outputValue;

        public CurrencyViewModel(ICurrencyDataProvider currencyDataProvider, IProductDataProvider productDataProvider)
        {
            this._currencyDataProvider = currencyDataProvider;
            _productDataProvider = productDataProvider;
        }

        public ObservableCollection<string> Currencylist { get; } = new();
        public ObservableCollection<ProductViewItem> ProductsList { get; } = new();

        private string? col3;

        public string? Col3
        {
            get { return col3; }
            set 
            { 
                col3 = value;
                RaisePropertychanged();
            }
        }
        public double OutputValue
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
                if (ProductsList.Any())
                {
                    return;
                }

                var products = await _productDataProvider.GetAllAsync();
                if (products is not null)
                {
                    foreach (var product in products)
                    {
                        ProductsList.Add(new ProductViewItem(product));
                    }
                }
            }
            catch (Exception ex) { throw ex; }

        }

        public void Calculate(string ccyFrom, string ccyTo, double InputValue)
        {
            //code to be done 
            MessageBox.Show("vlera " + InputValue);
            OutputValue = _currencyDataProvider.ConvertAsync(ccyFrom, ccyTo, InputValue).Result;

        }


      

        public  void GetProductPricesConverted(string currencyto)
        {

            try
            {
                string currencyFrom = "USD";
                Col3=currencyto.ToUpper();

                if (this.ProductsList.Any())
                {


                    foreach (var product in ProductsList)
                    {
                        double priceConverted =  _currencyDataProvider.ConvertAsync(currencyFrom, currencyto, product.Price).Result;
                        product.PriceConverted = priceConverted;

                    }
                }

            }
            catch (Exception ex) { throw ex; }

        }
    }
}

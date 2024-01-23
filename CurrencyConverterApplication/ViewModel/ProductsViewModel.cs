using CurrencyConverterApplication.Data;
using CurrencyConverterApplication.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CurrencyConverterApplication.ViewModel
{
    public class ProductsViewModel : ViewModelBase
    {
       
        private readonly IProductDataProvider _productDataProvider;
        private readonly ICurrencyDataProvider _currencyDataProvider;


        public ProductsViewModel(IProductDataProvider productDataProvider, ICurrencyDataProvider currencyDataProvider)
        {
            _productDataProvider = productDataProvider;
            _currencyDataProvider = currencyDataProvider;

                    
        }       

        public ObservableCollection<ProductViewItem> Products { get; } = new();
       
        private string? col3;

        public string? Col3
        {
            get { return col3; }
            set { col3 = value; }
        }

        public override async Task LoadAsync()
        {
            if (Products.Any())
            {
                return;
            }

            var products = await _productDataProvider.GetAllAsync();
            if (products is not null)
            {
                foreach (var product in products)
                {
                    Products.Add(new ProductViewItem(product));
                }
            }
        }

        public  void GetProductPricesConverted(string currencyto)
        {

            try
            {
                ObservableCollection<Product> ProductsList = new();
                string currencyFrom = "USD";
                
                if (!this.Products.Any())
                {
                    return;
                }
                foreach (var product in Products)
                {
                    double priceConverted = _currencyDataProvider.ConvertAsync(currencyFrom, currencyto, product.Price).Result;
                    product.PriceConverted = priceConverted;

                }


            }
            catch (Exception ex) { throw ex; }

        }




    }
}

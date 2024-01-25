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
       


        public ProductsViewModel(IProductDataProvider productDataProvider)
        {
            _productDataProvider = productDataProvider;
           

                    
        }
        private ObservableCollection<ProductViewItem> products=new ObservableCollection<ProductViewItem>();
        public ObservableCollection<ProductViewItem> Products { get=>products; set { products = value; } }
       
        private string? col3;

        public string? Col3
        {
            get => col3;
            set
            {
                col3 = value;                
                RaisePropertychanged();
            }
        }       

        public override async Task LoadAsync()
        {
            try
            {
                if (products.Any())
                {
                    return;
                }

                var _products = await _productDataProvider.GetAllAsync();
                if (_products is not null)
                {
                    foreach (var product in _products)
                    {
                        products.Add(new ProductViewItem(product));
                    }
                }
            }
            catch(Exception ex) { Console.WriteLine($"Error: {ex.Message}"); }
        }

        public async void GetProductPricesConverted(string currencyto)
        {

            try
            {              
                string currencyFrom = "USD";              
                if (!this.products.Any())
                {
                    return;
                }
                double pricetoetRate = 10;
                double conversionRate = await _productDataProvider.GetConversionRate(currencyFrom, currencyto, pricetoetRate);
                foreach (var product in products)
                {
                    
                    product.PriceConverted= conversionRate*product.Price;

                }
     

            }
            catch (Exception ex) { Console.WriteLine($"Error: {ex.Message}"); }

        }




    }
}

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
        /// <summary>
        /// Gets or sets the ObservableCollection of Products objects.
        /// </summary>
        /// <remarks>
        /// The ProductsList property represents a collection of Products objects,
        /// typically used as a data context to the Products Grid
        /// </remarks>
        public ObservableCollection<ProductViewItem> ProductsList { get; private set; } = new ObservableCollection<ProductViewItem>();

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

        public void LoadProductsList()
        {
            try
            {
                if (ProductsList.Any())
                {
                    return;
                }

                var _products =  _productDataProvider.GetAllAsync();
                if (_products is not null)
                {
                    foreach (var product in _products)
                    {
                        ProductsList.Add(new ProductViewItem(product));
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
                if (!this.ProductsList.Any())
                {
                    return;
                }
                double pricetoetRate = 10;
                double conversionRate = 0.0;// await _productDataProvider.GetConversionRate(currencyFrom, currencyto, pricetoetRate);
                foreach (var product in ProductsList)
                {
                    
                    product.PriceConverted= conversionRate*product.Price;

                }
     

            }
            catch (Exception ex) { Console.WriteLine($"Error: {ex.Message}"); }

        }




    }
}

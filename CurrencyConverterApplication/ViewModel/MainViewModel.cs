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
    public class MainViewModel : ViewModelBase
    {
       
        private readonly IProductDataProvider _productDataProvider;
       


        public MainViewModel(IProductDataProvider productDataProvider)
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

        public ObservableCollection<ProductViewItem> LoadProductsList()
        {
            try
            {
                if (ProductsList.Any())
                {
                    return new();
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
            return ProductsList;
        }


    }
}

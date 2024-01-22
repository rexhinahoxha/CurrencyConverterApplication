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

        

        public ObservableCollection<Product> Products { get; } = new();
        public ObservableCollection<double> Converted { get; } = new();
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
                    Products.Add(product);
                }
            }
        }
    }
}

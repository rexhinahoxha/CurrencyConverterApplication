using CurrencyConverterApplication.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CurrencyConverterApplication.Data
{
    public interface IProductDataProvider
    {
        IEnumerable<Product> GetAllAsync();

        
    }
    public class ProductDataProvider : IProductDataProvider
    {
        public IEnumerable<Product> GetAllAsync()
        {
           return new List<Product>
            {
                 new Product{ProductName="Cappuccino", Price=1.7},
                 new Product{ProductName="Doppio", Price=1.2},
                 new Product{ProductName="Espresso", Price=1.2},
                 new Product{ProductName="Frappuccino", Price=2.5},
                 new Product{ProductName="Mochaccino", Price=3.5}

            };
        }
               
        }
}

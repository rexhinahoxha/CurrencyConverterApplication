using CurrencyConverterApplication.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CurrencyConverterApplication.Data
{
    public interface IProductDataProvider
    {
        Task<IEnumerable<Product>?> GetAllAsync();
    }
    public class ProductDataProvider : IProductDataProvider
    {
        public async Task<IEnumerable<Product>?> GetAllAsync()
        {
            await Task.Delay(100); // Simulate a bit of server work

            return new List<Product>
            {
                 new Product{ProductName="Cappuccino", Price=1.7},
                 new Product{ProductName="Doppio", Price=1.7},
                 new Product{ProductName="Espresso", Price=1}

            };
        }
    }
}

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
        Task<IEnumerable<Product>?> GetAllAsync();
        Task<double> ConvertAsync(string currencyfrom, string currencyto, double amount);
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


        string baseURl = "http://api.currencylayer.com/";
        string access_key = "2833017b9126749777a5ee7ddb74862f";


        //Perform exchange API
        public async Task<ConvertModel?> Convert(string currencyfrom, string currencyto, double amount)
        {
            try
            {
                using (HttpClient httpClient = new HttpClient())
                {
                    httpClient.DefaultRequestHeaders.Accept.Clear();
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    string endpoint = String.Format("{0}convert?access_key={1}&from={2}&to={3}&amount={4}", baseURl, access_key, currencyfrom, currencyto, amount);
                    HttpResponseMessage response = await httpClient.GetAsync(endpoint).ConfigureAwait(false);

                    if (response.IsSuccessStatusCode)
                    {
                        string content = await response.Content.ReadAsStringAsync();
                        var myDeserializedClass = JsonConvert.DeserializeObject<ConvertModel>(content);
                        return myDeserializedClass;
                    }
                    else
                    {
                        Console.WriteLine("Request failed with status code: " + response.StatusCode);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
            return new ConvertModel();
        }
        public async Task<double> ConvertAsync(string currencyfrom, string currencyto, double amount)
        {
            MessageBox.Show("hyri ketu");
            try
            {
                ConvertModel? result = await Convert(currencyfrom, currencyto, amount).ConfigureAwait(false);
                if (result != null)
                {
                    return result.result;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
            return -1;
        }
    }
}

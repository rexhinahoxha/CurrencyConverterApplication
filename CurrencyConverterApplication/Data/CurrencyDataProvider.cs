using CurrencyConverterApplication.Model;
using System.Net.Http.Headers;
using System.Net.Http;
using Newtonsoft.Json;

namespace CurrencyConverterApplication.Data
{
    public class CurrencyDataProvider : ICurrencyDataProvider
    {
        string baseURl = "http://api.currencylayer.com/";
        string access_key = "e6446dd83fd16c672f50407fe9cb795e";

        // Get currencies from API
        public async Task<CurrencyModel?> GetAllCurrenciesAsync()
        {

            try
            {
                using (HttpClient httpClient = new HttpClient())
                {
                    httpClient.DefaultRequestHeaders.Accept.Clear();
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    HttpResponseMessage response = await httpClient.GetAsync(baseURl + "list?access_key=" + access_key);
                    if (response.IsSuccessStatusCode)
                    {
                        string content = await response.Content.ReadAsStringAsync();
                        var myDeserializedClass = JsonConvert.DeserializeObject<CurrencyModel>(content);

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
            return new CurrencyModel();
        }
        public async Task<List<Currency>?> GetCurrenciesData()
        {
            List<Currency> currencyList = new List<Currency>();
            try
            {
                CurrencyModel? currencyModel = await GetAllCurrenciesAsync();

                if (currencyModel != null)
                {
                    var _currencies = typeof(Currencies).GetProperties().ToList();
                    //if(myDeserializedClass.currencies.ToString() )
                    foreach (var _currency in _currencies)
                    {
                        Currency ccy = new Currency();
                        ccy.currencyCode = _currency.Name;
                        ccy.currencyFlag = String.Concat("/Images/", _currency.Name.ToLower(), ".png");
                        currencyList.Add(ccy);
                    }

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }

            return currencyList;
        }

        public async Task<List<string>?> GetCurrencies()
        {
            List<string> currencyList = new List<string>();
            try
            {
                CurrencyModel? currencyModel = await GetAllCurrenciesAsync();

                if (currencyModel != null)
                {
                    var _currencies = typeof(Currencies).GetProperties().ToList();
                    //if(myDeserializedClass.currencies.ToString() )
                    foreach (var _currency in _currencies)
                    {
                        currencyList.Add(_currency.Name);
                    }

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }

            return currencyList;
        }

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

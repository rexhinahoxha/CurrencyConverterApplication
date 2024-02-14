using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml.Serialization;
using CurrencyConverterControl.Model;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Configuration;

namespace CurrencyConverterControl.Data
{
    internal class CurrencyDataProvider : ICurrencyDataProvider
    {
        private readonly IHttpClientFactory _httpClientFactory = null!;
        string baseURl = "http://api.currencylayer.com/";
        string access_key = "63da635cc4fff2e738aeb69cd8de0e91";
        Dictionary<string, double> exchangeRates = new Dictionary<string, double>();
        public CurrencyDataProvider(
        IHttpClientFactory httpClientFactory) =>
        (_httpClientFactory) = (httpClientFactory);

        /// <summary>
        /// This method is used to perfom the currency convertion
        /// </summary>
        /// <param name="currencyfrom">This parameter gets the value of the source currency</param>
        /// <param name="currencyto">This parameter gets the value of the destination currency</param>
        /// <param name="amount">This parameter gets the value which needs to be converted</param>
        /// <returns></returns>
        /// <exception cref="HttpRequestException"></exception>
        public double Convert(string currencyfrom, string currencyto, double amount)
        {
            try
            {
                if (currencyfrom == currencyto) { return amount; }
                using (HttpClient httpClient = _httpClientFactory.CreateClient(""))
                {
                    httpClient.DefaultRequestHeaders.Accept.Clear();
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    string endpoint = String.Format("{0}convert?access_key={1}&from={2}&to={3}&amount={4}", baseURl, access_key, currencyfrom, currencyto, amount);
                    HttpResponseMessage response =  httpClient.GetAsync(endpoint).Result;

                    if (response.IsSuccessStatusCode)
                    {
                        string content =  response.Content.ReadAsStringAsync().Result;
                        JObject json = JObject.Parse(content);
                        double value = (double)json["result"];                        
                        return value;
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
            return -1;
        }
        /// <summary>
        /// This method is used to get List of currencies which holds Currency Code and Flag 
        /// </summary>
        /// <returns>Returns a List of Currency Objects </returns>
        /// <exception cref="DirectoryNotFoundException">When Path of the file is not found</exception>
        public List<Currency> GetCurrenciesData()
        {
            string filePath = @"Resources\CurrencyListXML.xml";

            // Call the method to read data
            return DeserializeFromXml(filePath);
        }
        private List<Currency> DeserializeFromXml(string inputPath)
            {
               CurrencyList currencies = new CurrencyList();

                XmlSerializer serializer = new XmlSerializer(typeof(CurrencyList));

                try
                {
                    using (TextReader reader = new StreamReader(inputPath))
                    {
                        currencies = (CurrencyList)serializer.Deserialize(reader);
                    }

                    Console.WriteLine($"XML file successfully deserialized from: {inputPath}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error deserializing XML file: {ex.Message}");
                }

                return currencies.CurrenciesList;
            }

        /// <summary>
        /// Gets the list of convertion Rates for two currencies
        /// </summary>
        /// <param name="currencyfrom"></param>
        /// <param name="currencyto"></param>        
        /// <returns>Returns the value of the convertion at that moment</returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="System.NullReferenceException">Happens when your monthly usage limit has been reached.  </exception>
        public double GetConversionRate(string currencyfrom, string currencyto)
        {
            try
            {
                if (exchangeRates.Count > 0)
                {
                    string parameterValue = String.Concat(currencyfrom, currencyto);
                    return exchangeRates.GetValueOrDefault(parameterValue);
                }               
              
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
            return -1;
        }
        /// <summary>
        /// This method calls USD Rates API and stores them into a dictionary variable type. 
        /// </summary>
        /// <exception cref="HttpResponseMessage"></exception>
        public void GetConvertionRates()
        {
            try
            {
                using (HttpClient httpClient = _httpClientFactory.CreateClient(""))
                {
                    httpClient.DefaultRequestHeaders.Accept.Clear();
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    string endpoint = String.Format("{0}live?access_key={1}", baseURl, access_key);
                    HttpResponseMessage response =  httpClient.GetAsync(endpoint).Result;

                    if (response.IsSuccessStatusCode)
                    {
                         string content = response.Content.ReadAsStringAsync().Result;                       
                        JObject contentToJson = JObject.Parse(content);
                        string quotesObject = contentToJson["quotes"].ToString();
                        if (quotesObject != null)
                        {
                            exchangeRates = JsonConvert.DeserializeObject<Dictionary<string, double>>(quotesObject);                            
                        }
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
        }
    
    }
}

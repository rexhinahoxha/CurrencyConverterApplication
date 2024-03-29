﻿using System;
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
using System.Threading;

namespace CurrencyConverterControl.Data
{
    internal class CurrencyDataProvider : ICurrencyDataProvider
    {
        private static readonly HttpClient httpClient = new HttpClient();
        string baseURl = "http://api.currencylayer.com/";
        string access_key = "6ac174a15508ce09e2e89ad74ae79c45";
        Dictionary<string, double> exchangeRates = new Dictionary<string, double>();
        public CurrencyDataProvider()
        {
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

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

                if (exchangeRates.Count > 0)
                {
                    double exchangeRate1 = 1;
                    double exchangeRate2 = 1;
                    string rate1 = String.Concat("USD",currencyfrom);
                    string rate2 = String.Concat("USD", currencyto);
                    //Get rates
                    if (!rate1.Equals("USDUSD")) exchangeRate1 = exchangeRates.GetValueOrDefault(rate1);
                    if (!rate2.Equals("USDUSD")) exchangeRate2 = exchangeRates.GetValueOrDefault(rate2);

                    // Convert source to base currency USD
                    double amountInUsd = amount / exchangeRate1;

                    // Convert amount in USD to destination currency
                    double amountConverted = amountInUsd * exchangeRate2;


                    return amountConverted;
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
                
                    //httpClient.DefaultRequestHeaders.Accept.Clear();
                    //httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

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
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }          
        }
    
    }
}

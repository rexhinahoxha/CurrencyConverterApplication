using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using CurrencyConverterControl.Model;
using OfficeOpenXml;

namespace CurrencyConverterControl.Data
{
    public class CurrencyDataProvider : ICurrencyDataProvider
    {
        string baseURl = "http://api.currencylayer.com/";
        string access_key = "6ac174a15508ce09e2e89ad74ae79c45";

        /// <summary>
        /// 
        /// </summary>
        /// <param name="currencyfrom"></param>
        /// <param name="currencyto"></param>
        /// <param name="amount"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public Task<double> ConvertAsync(string currencyfrom, string currencyto, double amount)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// This method is used to get List of currencies which holds Currency Code and Flag 
        /// </summary>
        /// <returns>Returns a List of Currency Objects </returns>
        public List<Currency> GetCurrenciesData()
        {
            string filePath = @"C:\Users\User\Desktop\CurrencyConverterApplication\CurrencyConverterControl\Resources\CurrencyListXML.xml";

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

       
    }
}

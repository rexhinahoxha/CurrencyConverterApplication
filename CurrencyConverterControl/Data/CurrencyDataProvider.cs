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

namespace CurrencyConverterControl.Data
{
    internal class CurrencyDataProvider : ICurrencyDataProvider
    {
        string baseURl = "http://api.currencylayer.com/";
        string access_key = "63da635cc4fff2e738aeb69cd8de0e91";
        Dictionary<string, double> exchangeRates = new Dictionary<string, double>();

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
                using (HttpClient httpClient = new HttpClient())
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
                using (HttpClient httpClient = new HttpClient())
                {
                    httpClient.DefaultRequestHeaders.Accept.Clear();
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    string endpoint = String.Format("{0}live?access_key={1}", baseURl, access_key);
                    HttpResponseMessage response =  httpClient.GetAsync(endpoint).Result;

                    if (response.IsSuccessStatusCode)
                    {
                         string content = response.Content.ReadAsStringAsync().Result;
                        //string content = "{\"success\":true,\"terms\":\"https://currencylayer.com/terms\",\"privacy\":\"https://currencylayer.com/privacy\",\"timestamp\":1706794983,\"source\":\"USD\",\"quotes\":{\"USDAED\":3.672945,\"USDAFN\":74.705414,\"USDALL\":96.145397,\"USDAMD\":405.854741,\"USDANG\":1.801743,\"USDAOA\":833.000137,\"USDARS\":826.880605,\"USDAUD\":1.53201,\"USDAWG\":1.8025,\"USDAZN\":1.700471,\"USDBAM\":1.808949,\"USDBBD\":2.018443,\"USDBDT\":109.713461,\"USDBGN\":1.809098,\"USDBHD\":0.376898,\"USDBIF\":2852.531493,\"USDBMD\":1,\"USDBND\":1.34085,\"USDBOB\":6.908225,\"USDBRL\":4.934903,\"USDBSD\":0.999649,\"USDBTC\":2.3732802e-5,\"USDBTN\":82.943927,\"USDBWP\":13.600881,\"USDBYN\":3.271611,\"USDBYR\":19600,\"USDBZD\":2.015113,\"USDCAD\":1.34405,\"USDCDF\":2740.000113,\"USDCHF\":0.86141,\"USDCLF\":0.033901,\"USDCLP\":935.429933,\"USDCNY\":7.1061,\"USDCOP\":3903.7,\"USDCRC\":512.682961,\"USDCUC\":1,\"USDCUP\":26.5,\"USDCVE\":101.985793,\"USDCZK\":23.011967,\"USDDJF\":177.999963,\"USDDKK\":6.887502,\"USDDOP\":58.630656,\"USDDZD\":134.641998,\"USDEGP\":30.902197,\"USDERN\":15,\"USDETB\":56.583624,\"USDEUR\":0.924009,\"USDFJD\":2.2442,\"USDFKP\":0.785474,\"USDGBP\":0.78885,\"USDGEL\":2.675039,\"USDGGP\":0.785474,\"USDGHS\":12.311605,\"USDGIP\":0.785474,\"USDGMD\":67.42494,\"USDGNF\":8595.238536,\"USDGTQ\":7.815571,\"USDGYD\":209.28469,\"USDHKD\":7.819845,\"USDHNL\":24.659169,\"USDHRK\":6.88032,\"USDHTG\":131.513716,\"USDHUF\":354.009499,\"USDIDR\":15756.4,\"USDILS\":3.65259,\"USDIMP\":0.785474,\"USDINR\":82.97075,\"USDIQD\":1309.587673,\"USDIRR\":42044.99977,\"USDISK\":137.039992,\"USDJEP\":0.785474,\"USDJMD\":155.533769,\"USDJOD\":0.7092,\"USDJPY\":146.520496,\"USDKES\":160.495873,\"USDKGS\":89.319584,\"USDKHR\":4083.50136,\"USDKMF\":454.049513,\"USDKPW\":899.985178,\"USDKRW\":1332.339343,\"USDKWD\":0.30753,\"USDKYD\":0.833071,\"USDKZT\":449.999075,\"USDLAK\":20758.896571,\"USDLBP\":15025.504659,\"USDLKR\":313.231839,\"USDLRD\":190.000267,\"USDLSL\":18.695997,\"USDLTL\":2.95274,\"USDLVL\":0.60489,\"USDLYD\":4.827133,\"USDMAD\":10.034916,\"USDMDL\":17.829676,\"USDMGA\":4512.682961,\"USDMKD\":56.984036,\"USDMMK\":2099.372907,\"USDMNT\":3423.072472,\"USDMOP\":8.052535,\"USDMRU\":39.67505,\"USDMUR\":44.939783,\"USDMVR\":15.409924,\"USDMWK\":1682.618949,\"USDMXN\":17.17337,\"USDMYR\":4.729497,\"USDMZN\":63.492693,\"USDNAD\":18.580054,\"USDNGN\":1194.50654,\"USDNIO\":36.6174,\"USDNOK\":10.4867,\"USDNPR\":132.710246,\"USDNZD\":1.63883,\"USDOMR\":0.384935,\"USDPAB\":0.999741,\"USDPEN\":3.804195,\"USDPGK\":3.745925,\"USDPHP\":56.055503,\"USDPKR\":278.466391,\"USDPLN\":4.004842,\"USDPYG\":7277.267787,\"USDQAR\":3.6405,\"USDRON\":4.595804,\"USDRSD\":108.278975,\"USDRUB\":90.004985,\"USDRWF\":1270.371261,\"USDSAR\":3.750023,\"USDSBD\":8.425945,\"USDSCR\":13.157591,\"USDSDG\":601.000095,\"USDSEK\":10.453145,\"USDSGD\":1.339775,\"USDSHP\":1.26905,\"USDSLE\":22.549157,\"USDSLL\":19749.999936,\"USDSOS\":570.999496,\"USDSRD\":36.729498,\"USDSTD\":20697.981008,\"USDSYP\":13001.804733,\"USDSZL\":18.736936,\"USDTHB\":35.462502,\"USDTJS\":10.921729,\"USDTMT\":3.51,\"USDTND\":3.118495,\"USDTOP\":2.363704,\"USDTRY\":30.360305,\"USDTTD\":6.772262,\"USDTWD\":31.3025,\"USDTZS\":2544.999549,\"USDUAH\":37.598354,\"USDUGX\":3818.050648,\"USDUYU\":39.105422,\"USDUZS\":12341.145785,\"USDVEF\":3612745.32141,\"USDVES\":36.096651,\"USDVND\":24410,\"USDVUV\":119.856108,\"USDWST\":2.751098,\"USDXAF\":606.704712,\"USDXAG\":0.044184,\"USDXAU\":0.00049,\"USDXCD\":2.70255,\"USDXDR\":0.750661,\"USDXOF\":606.704712,\"USDXPF\":110.549721" +
                        //     ",\"USDYER\":250.349774,\"USDZAR\":18.65935,\"USDZMK\":9001.202876,\"USDZMW\":27.16674,\"USDZWL\":321.999592}}";
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

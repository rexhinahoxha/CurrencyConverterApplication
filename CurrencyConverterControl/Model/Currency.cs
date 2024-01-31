using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace CurrencyConverterControl.Model
{
   
    public class Currency
    {
        [XmlElement("currencyCode")]
        public string CurrencyCode { get; set; }

        [XmlElement("currencyFlag")]
        public string CurrencyFlag { get; set; }
    }

    [XmlRoot("currencylist")]
    public class CurrencyList
    {
        public CurrencyList() { CurrenciesList = new List<Currency>(); }
        [XmlElement("currency")]
        public List<Currency> CurrenciesList { get; set; }
    }
}

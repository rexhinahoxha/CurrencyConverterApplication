using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CurrencyControl.Model
{
    public class Info
    {
        public int timestamp { get; set; }
        public double quote { get; set; }
    }

    public class Query
    {
        public string from { get; set; }
        public string to { get; set; }
        public double amount { get; set; }
    }

    public class ConvertModel
    {
        public bool success { get; set; }
        public string terms { get; set; }
        public string privacy { get; set; }
        public Query query { get; set; }
        public Info info { get; set; }
        public double result { get; set; }
    }
}

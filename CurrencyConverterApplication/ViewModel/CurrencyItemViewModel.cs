using CurrencyConverterApplication.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CurrencyConverterApplication.ViewModel
{
    public class CurrencyItemViewModel : ValidationViewModelBase
    {
        private readonly Currency _customer;

        public CurrencyItemViewModel(Currency currency)
        {
            this._customer = currency;
        }



        public string? CurrencyCode
        {
            get => _customer.currencyCode;

            set
            {
                _customer.currencyCode = value;
                RaisePropertychanged();

            }
        }

        public string? CurrencyFlag
        {
            get => _customer.currencyFlag;

            set
            {
                _customer.currencyFlag = value;
                RaisePropertychanged();

            }
        }

    }
}

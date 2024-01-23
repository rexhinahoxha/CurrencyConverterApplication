using CurrencyConverterApplication.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CurrencyConverterApplication.ViewModel
{
    public  class ProductViewItem:ViewModelBase
    {
        private readonly Product _model;
        public ProductViewItem(Product model)
        {
            _model = model;
        }

       

        public string? ProductName
        {
            get => _model.ProductName;
            set
            {
                _model.ProductName = value;
                RaisePropertychanged();                
            }
        }

        public double Price
        {
            get => _model.Price;
            set
            {
                _model.Price = value;
                RaisePropertychanged();
            }
        }
        public double PriceConverted
        {
            get => _model.PriceConverted;
            set
            {
                _model.PriceConverted = value;
                RaisePropertychanged();
            }
        }

   
    }
}

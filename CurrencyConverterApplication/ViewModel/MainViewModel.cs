using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CurrencyConverterApplication.ViewModel
{
    public class MainViewModel: ViewModelBase
    {

        public MainViewModel(ProductsViewModel productsView)
        {
            ProductsViewModel = productsView;
        }

        public ProductsViewModel ProductsViewModel { get; }

        public async override Task LoadAsync()
        {
            try
            {
                await ProductsViewModel.LoadAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
           
        }
    }
}

using CurrencyConverterApplication.Data;
using CurrencyConverterApplication.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace CurrencyConverterApplication
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        private ProductsViewModel _viewModel;
        public Window1()
        {
            InitializeComponent();
            _viewModel = new ProductsViewModel(new ProductDataProvider());
            DataContext = _viewModel;
            LoadProductList();
            //_viewModel.Col3 = "Converted Price in ()";
        }

        private void LoadProductList()
        {
             _viewModel.LoadProductsList();
        }
    }
}

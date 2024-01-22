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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CurrencyConverterApplication
{
    /// <summary>
    /// Interaction logic for ProductsView.xaml
    /// </summary>
    public partial class ProductsView : UserControl
    {
        private ProductsViewModel _viewModel;
        public ProductsView()
        {
            InitializeComponent();
            _viewModel = new ProductsViewModel(new ProductDataProvider());
            DataContext = _viewModel;
            Loaded += ProductsControl_Loaded;
        }

        private async void ProductsControl_Loaded(object sender, RoutedEventArgs e)
        {
            await _viewModel.LoadAsync();
        }
    }
}

using CurrencyConverterApplication.Data;
using CurrencyConverterApplication.ViewModel;
using System.Windows;
using System.Windows.Controls;

namespace CurrencyConverterApplication
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ProductsViewModel _viewModel;
        public MainWindow()
        {
            InitializeComponent();
            _viewModel = new ProductsViewModel(new ProductDataProvider());
            DataContext = _viewModel;
            Loaded += CurrencyConverterControl_Loaded;
            _viewModel.Col3 = "Product Price in ___";


        }
      

        private async void CurrencyConverterControl_Loaded(object sender, RoutedEventArgs e)
        {
            await _viewModel.LoadAsync();
        }

        private  void LoadGrid_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.GetProductPricesConverted(pnlConverter.DestinationCurrency);
            _viewModel.Col3= String.Format("Product Price in {0}", pnlConverter.DestinationCurrency);
            //dtProducts.Columns[2].Header = String.Format("Product Price in {0}", pnlConverter.DestinationCurrency);


        }
    }
}
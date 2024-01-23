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
        private CurrencyViewModel _viewModel;
        public MainWindow()
        {
            InitializeComponent();
            _viewModel = new CurrencyViewModel(new CurrencyDataProvider(), new ProductDataProvider());
            DataContext = _viewModel;
            Loaded += CurrencyConverterControl_Loaded;
            pnlConverter.ConvertButtonClicked += btnConvert_Click;
          

           


        }
        private async void CurrencyConverterControl_Loaded(object sender, RoutedEventArgs e)
        {
            await _viewModel.LoadAsync();
        }
        private void btnConvert_Click(object sender, RoutedEventArgs e)
        {
           // _viewModel.Calculate(pnlConverter.SourceCurrency, pnlConverter.DestinationCurrency, (double)pnlConverter.InputValue);
            _viewModel.GetProductPricesConverted(pnlConverter.DestinationCurrency);
        }

       

    }
}
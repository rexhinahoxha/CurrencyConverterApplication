using CurrencyConverterApplication.Data;
using CurrencyConverterApplication.ViewModel;
using System.Windows;

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
            _viewModel = new CurrencyViewModel(new CurrencyDataProvider());
            DataContext = _viewModel;
            Loaded += CurrencyConverterControl_Loaded;
            pnlConverter.ConvertButtonClicked += HandleConvertButtonClicked;

        }
        private async void CurrencyConverterControl_Loaded(object sender, RoutedEventArgs e)
        {
            await _viewModel.LoadAsync();
        }
        private void HandleConvertButtonClicked(object sender, RoutedEventArgs e)
        {
            _viewModel.Calculate(pnlConverter.SourceCurrency, pnlConverter.DestinationCurrency, (double)pnlConverter.InputValue);
        }

    }
}
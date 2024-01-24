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
          
        }
        private async void CurrencyConverterControl_Loaded(object sender, RoutedEventArgs e)
        {
            await _viewModel.LoadAsync();
        }
        

       

    }
}
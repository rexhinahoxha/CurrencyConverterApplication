using CurrencyConverterApplication.Data;
using CurrencyConverterApplication.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
        private MainViewModel _viewModel;
        public Window1()
        {
            InitializeComponent();
            _viewModel = new MainViewModel(new ProductDataProvider());
            DataContext = _viewModel;
            _viewModel.LoadProductsList();
            //Loaded += LoadProductsList;
           
        }

    }
}

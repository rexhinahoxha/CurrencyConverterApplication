using CurrencyConverterControl.Data;
using CurrencyConverterControl.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
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

namespace CurrencyConverterControl
{
    /// <summary>
    /// Represents a custom WPF control for currency conversion.
    /// This control extends the base Control class and provides functionality
    /// for converting values between different currencies, along with a customizable
    /// user interface.
    /// </summary>
    [TemplatePart(Name = "CmbDestination", Type = typeof(ComboBox))]
    [TemplatePart(Name = "CmbSource", Type= typeof(ComboBox))]   
    public class CurrencyConverterControl : Control, INotifyPropertyChanged
    {
        private static ICurrencyDataProvider CurrencyDataProvider;
        private ComboBox _currencyDestination;
        private ComboBox _currencySource;
        
        static CurrencyConverterControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(CurrencyConverterControl), new FrameworkPropertyMetadata(typeof(CurrencyConverterControl)));
            CurrencyDataProvider = new CurrencyDataProvider();
        }

        #region Dependency Properties
        /// <summary>
        /// Gets or sets the input value for currency conversion within the CurrencyConverterControl.
        /// </summary>
        /// <remarks>
        /// The InputValue property represents the value to be converted between different currencies.
        /// It is bound to the user interface elements where users can input the amount for conversion.
        /// </remarks>
        [Description("Value to be converted")]
        public double InputValue
        {
            get { return (double)GetValue(InputValueProperty); }
            set { SetValue(InputValueProperty, value); }
        }
        public static readonly DependencyProperty InputValueProperty =
            DependencyProperty.Register("InputValue", typeof(double), typeof(CurrencyConverterControl));

        /// <summary>
        /// Gets or sets the output value for the result of currency conversion within the CurrencyConverterControl.
        /// </summary>
        /// <remarks>
        /// The OutputValue property represents the result of the currency conversion based on the provided input value.
        /// It is bound to the user interface elements where the converted amount is displayed to the user.
        /// </remarks>
         [Description("Converted value")]
        public double OutputValue
        {
            get { return (double)GetValue(OutputValueProperty); }
            set { SetValue(OutputValueProperty, value); }
        }

        public static readonly DependencyProperty OutputValueProperty =
           DependencyProperty.Register("OutputValue", typeof(double), typeof(CurrencyConverterControl));
        
        public static readonly DependencyProperty DestinationCurrencyProperty =
           DependencyProperty.Register("DestinationCurrency", typeof(Currency), typeof(CurrencyConverterControl));
        /// <summary>
        /// Gets the Destination Currency to perform the currency conversion  
        /// </summary>
        [Description("Value of the destination currency")]
        public Currency DestinationCurrency
        {
            get { return (Currency)GetValue(DestinationCurrencyProperty); }
            set { SetValue(DestinationCurrencyProperty, value); }
        }

        public static readonly DependencyProperty SourceCurrencyProperty =
            DependencyProperty.Register("SourceCurrency", typeof(Currency), typeof(CurrencyConverterControl));

        /// <summary>
        /// Gets the Source Currency to perform the currency conversion  
        /// </summary>
         [Description("Value of the source currency")]
        public Currency SourceCurrency
        {
            get { return (Currency)GetValue(SourceCurrencyProperty); }
            set { SetValue(SourceCurrencyProperty, value); }
        }

        /// <summary>
        /// Gets or sets the ObservableCollection of Currency objects.
        /// </summary>
        /// <remarks>
        /// The CurrencyList property represents a collection of Currency objects,
        /// typically used for storing a list of currencies available in the application
        /// to fill in the ComboBox element.
        /// It is initialized with a new instance of ObservableCollection<Currency>.
        /// </remarks>
        [Description("Collection of currencies list")]
        public ObservableCollection<Currency> CurrencyList { get; private set; } = new ObservableCollection<Currency>();

        /// <summary>
        /// Property to allow replacement of the currency converter
        /// </summary>
        public ICurrencyDataProvider CurrencyConverter
        {
            get { return CurrencyDataProvider; }
            set { CurrencyDataProvider = value ?? throw new ArgumentNullException(nameof(value)); }
        }
        #endregion
        public override void OnApplyTemplate()
        {
           
            LoadCurrencies();
            _currencyDestination = Template.FindName("CmbDestination", this) as ComboBox;
             _currencySource = Template.FindName("CmbSource", this) as ComboBox;
            _currencyDestination.Loaded += ComboBox_Loaded;         
            _currencySource.Loaded += ComboBox_Loaded;
            _currencyDestination.SelectionChanged += CmbDestination_SelectionChanged;
            _currencySource.SelectionChanged += CmbSource_SelectionChanged;            
            InputValue = 233.3; //providing a default value           
            base.OnApplyTemplate();
        }

        
       
        //Method to load the currencies list
        private void LoadCurrencies()
        {
            try
            {
                if (CurrencyList.Any())
                {
                    return;
                }
                var currencies = CurrencyDataProvider.GetCurrenciesData();

                if (currencies.Any())
                {
                    foreach (var currency in currencies)
                    {

                        CurrencyList.Add(currency);
                    }
                }

            }
            catch (Exception ex) { throw ex; }

        }

        #region Events
        //Method to set the default value after the ComboBox is loaded
        private void ComboBox_Loaded(object sender, RoutedEventArgs e)
        {
            
            var comboBox = sender as ComboBox;
            if (comboBox != null && comboBox.Items.Count > 0)
            {
                comboBox.SelectedIndex = 0; 
            }
        }

        // Perform the convertion 
        private void CmbSource_SelectionChanged(object sender, SelectionChangedEventArgs args)
        {
            try
            {
                //check if all have values before going to API calculate
                if (SourceCurrency is null|| String.IsNullOrEmpty(SourceCurrency.CurrencyCode) 
                   || DestinationCurrency is null || String.IsNullOrEmpty(DestinationCurrency.CurrencyCode))
                {
                    return;
                }
                OutputValue = CurrencyDataProvider.ConvertAsync(SourceCurrency.CurrencyCode, DestinationCurrency.CurrencyCode, InputValue).Result;
                _currencyDestination.SelectedValue = DestinationCurrency;
            }
            catch (Exception ex) { Console.WriteLine($"Error: {ex.Message}"); }
        }
        private void CmbDestination_SelectionChanged(object sender, RoutedEventArgs args)
        {
            try
            {
                //check if all have values before going to API calculate
                if (SourceCurrency is null || String.IsNullOrEmpty(SourceCurrency.CurrencyCode)
                   || DestinationCurrency is null || String.IsNullOrEmpty(DestinationCurrency.CurrencyCode))
                {
                    MessageBox.Show("Please fill in all fields!", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                OutputValue = CurrencyDataProvider.ConvertAsync(SourceCurrency.CurrencyCode, DestinationCurrency.CurrencyCode, InputValue).Result;
                _currencySource.SelectedValue = SourceCurrency;
                
            }
            catch (Exception ex) { Console.WriteLine($"Error: {ex.Message}"); }
        }


        #endregion

        #region Notify Property Change 
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void RaisePropertychanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public virtual Task LoadAsync() 
        {
            // Call the protected LoadCurrencies method
            LoadCurrencies();
            return Task.CompletedTask;
        }
        #endregion
        #region public functions
        /// <summary>
        /// This function is used to execute the convertion
        /// of an amount from USD to the chosen currency 
        /// </summary>
        /// <param name="currencyFrom"></param>
        /// <param name="currencyTo"></param>
        /// <param name="amount"></param>
        /// <returns>Returns the convertion value</returns>
        public double ConvertValues(string currencyFrom, string currencyTo, double amount)
        {
            try
            {

                double conversionRate = CurrencyDataProvider.GetConversionRate(currencyFrom, currencyTo).Result;
                var convertedValue= conversionRate * amount;
                return convertedValue;
            }
            catch (Exception ex) { Console.WriteLine($"Error: {ex.Message}"); }
            return -1; 
        }
        #endregion
    }
}

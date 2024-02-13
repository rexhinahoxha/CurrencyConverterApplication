using CurrencyConverterControl.Data;
using CurrencyConverterControl.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
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
    [TemplatePart(Name = "CmbSource", Type = typeof(ComboBox))]
    [StyleTypedProperty(Property = "TextBoxStyle",  StyleTargetType = typeof(TextBox))]
    public class CurrencyConverterControl : Control
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
            set 
            {
                SetValue(InputValueProperty, value);               
            }
        }
        public static readonly DependencyProperty InputValueProperty =
            DependencyProperty.Register(nameof(InputValue), typeof(double), typeof(CurrencyConverterControl), 
                                        new FrameworkPropertyMetadata(new PropertyChangedCallback(InputValue_TextChanged)),
                                        validateValueCallback: new ValidateValueCallback(IsValidValue));

        private static bool IsValidValue(object value)
        {
            double val = (double)value;
            return !val.Equals(double.NegativeInfinity) &&
                !val.Equals(double.PositiveInfinity);
        }

        // Adding a Text Change call back
        private static void InputValue_TextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            try
            {
                CurrencyConverterControl control = (CurrencyConverterControl)d;
                //check if all have values before going to API calculate
                if (String.IsNullOrEmpty(control.SourceCurrency) || String.IsNullOrEmpty(control.DestinationCurrency))
                {
                    return;
                }
                control.OutputValue = CurrencyDataProvider.Convert(control.SourceCurrency,
                                                            control.DestinationCurrency, control.InputValue);             
              
            }
            catch (Exception ex) { Console.WriteLine($"Error: {ex.Message}"); }
        }

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
            set 
            { 
                SetValue(OutputValueProperty, value);                
            }
        }

        public static readonly DependencyProperty OutputValueProperty =
                            DependencyProperty.Register(nameof(OutputValue), typeof(double), typeof(CurrencyConverterControl),
                            new FrameworkPropertyMetadata(new PropertyChangedCallback(OutputValue_Textchanged)));

        private static void OutputValue_Textchanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            CurrencyConverterControl control = d as CurrencyConverterControl;
            if (control != null)
            {
                control.OutputValue = (double)e.NewValue;
            }
        }

        public static readonly DependencyProperty DestinationCurrencyProperty =
           DependencyProperty.Register(nameof(DestinationCurrency), typeof(string), typeof(CurrencyConverterControl),
                                new FrameworkPropertyMetadata(new PropertyChangedCallback(DestinationCurrencyChanged)));
        /// <summary>
        /// Gets the Destination Currency to perform the currency conversion  
        /// </summary>
        [Description("Value of the destination currency")]
        public string DestinationCurrency
        {
            get { return (string)GetValue(DestinationCurrencyProperty); }
            set
            {
                SetValue(DestinationCurrencyProperty, value);
                
            }
        }
        private static void DestinationCurrencyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            try
            {
                CurrencyConverterControl control = (CurrencyConverterControl)d;
                //check if all have values before going to API calculate
                if (String.IsNullOrEmpty(control.SourceCurrency) || String.IsNullOrEmpty(control.DestinationCurrency))
                {
                    return;
                }
                control.OutputValue = CurrencyDataProvider.Convert(control.SourceCurrency,
                                                            control.DestinationCurrency, control.InputValue);
                
            }
            catch (Exception ex) { Console.WriteLine($"Error: {ex.Message}"); }
        }

      

        public static readonly DependencyProperty SourceCurrencyProperty =
            DependencyProperty.Register(nameof(SourceCurrency), typeof(string), typeof(CurrencyConverterControl),
                                new FrameworkPropertyMetadata(new PropertyChangedCallback(SourceCurrencyChanged)));

        private static void SourceCurrencyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            try
            {
                CurrencyConverterControl control = (CurrencyConverterControl)d;
                //check if all have values before going to API calculate
                if (String.IsNullOrEmpty(control.SourceCurrency) || String.IsNullOrEmpty(control.DestinationCurrency))
                {
                    return;
                }
                control.OutputValue = CurrencyDataProvider.Convert(control.SourceCurrency,
                                                            control.DestinationCurrency, control.InputValue);               
                
            }
            catch (Exception ex) { Console.WriteLine($"Error: {ex.Message}"); }
        }

        /// <summary>
        /// Gets the Source Currency to perform the currency conversion  
        /// </summary>
        [Description("Value of the source currency")]
        public string SourceCurrency
        {
            get { return (string)GetValue(SourceCurrencyProperty); }
            set 
            { 
                SetValue(SourceCurrencyProperty, value);
                
            }
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

        public static readonly DependencyProperty TextBoxStyleProperty =
            DependencyProperty.Register(nameof(TextBoxStyle), typeof(Style), typeof(CurrencyConverterControl));
        /// <summary>
        /// Define custom styles on the control
        /// </summary>
        public Style TextBoxStyle
        {
            get { return (Style)GetValue(TextBoxStyleProperty); }
            set { SetValue(TextBoxStyleProperty, value); }
        }
        #endregion
        public override void OnApplyTemplate()
        {

            LoadCurrencies();
            CurrencyDataProvider.GetConvertionRates();
            // Check if existing controls are not null and unsubscribe from events
            if (_currencyDestination != null)
            {
                _currencyDestination.Loaded -= ComboBox_Loaded;
                _currencyDestination.SelectionChanged -= CmbDestination_SelectionChanged;
            }

            if (_currencySource != null)
            {
                _currencySource.Loaded -= ComboBox_Loaded;
                _currencySource.SelectionChanged -= CmbSource_SelectionChanged;
            }
            _currencyDestination = Template.FindName("CmbDestination", this) as ComboBox;
            _currencySource = Template.FindName("CmbSource", this) as ComboBox;
            //To Be done
            if (_currencyDestination is null || _currencySource is null)
            { throw new InvalidOperationException("The Combobox in the Template is not found! Please double check!"); }
            else 
            {
                _currencyDestination.Loaded += ComboBox_Loaded;         
                _currencySource.Loaded += ComboBox_Loaded;
                _currencyDestination.SelectionChanged += CmbDestination_SelectionChanged;
                _currencySource.SelectionChanged += CmbSource_SelectionChanged;   
            }
                     
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
            if (comboBox != null && comboBox.Items.Count > 0 && comboBox.SelectedIndex == -1)
            {
                comboBox.SelectedIndex = 0; 
            }
        }

        // Perform the convertion 
        private void CmbSource_SelectionChanged(object sender, SelectionChangedEventArgs args)
        {
            try
            {
               _currencySource.SelectedValue = SourceCurrency;
            }
            catch (Exception ex) { Console.WriteLine($"Error: {ex.Message}"); }
        }
        private void CmbDestination_SelectionChanged(object sender, RoutedEventArgs args)
        {
            try
            {
                _currencyDestination.SelectedValue = DestinationCurrency;
            }
            catch (Exception ex) { Console.WriteLine($"Error: {ex.Message}"); }
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
                double conversionRate = CurrencyDataProvider.GetConversionRate(currencyFrom, currencyTo);
                var convertedValue= conversionRate * amount;
                return convertedValue;
            }
            catch (Exception ex) { Console.WriteLine($"Error: {ex.Message}"); }
            return -1; 
        }
        #endregion
    }
}

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

        private static ICurrencyDataProvider _currencyDataProvider;
        /// <summary>
        /// Property to allow replacement of the currency converter
        /// </summary>
        public static  ICurrencyDataProvider CurrencyConverter
        {
            get { return _currencyDataProvider; }
            set
            {
                if (_currencyDataProvider != value)
                {
                    ICurrencyDataProvider previousProvider = _currencyDataProvider;
                    _currencyDataProvider = value ?? throw new ArgumentNullException(nameof(value));
                    // Create an instance of CurrencyConverterControl to call the method
                    CurrencyConverterControl instance = new CurrencyConverterControl();
                    instance.OnCurrencyConverterChanged(previousProvider, _currencyDataProvider);
                }
            }
        }
        public event EventHandler<CurrencyConverterChangedEventArgs> CurrencyConverterChanged;

        protected virtual void OnCurrencyConverterChanged(ICurrencyDataProvider previousProvider, ICurrencyDataProvider newProvider)
        {
            CurrencyConverterChanged?.Invoke(null, new CurrencyConverterChangedEventArgs(previousProvider, newProvider));
        }

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
                OnInputValueChanged(this, new ValueChangedEventArgs(value));
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
                control.PerformCalculation(control);
            }
            catch (Exception ex) { Console.WriteLine($"Error: {ex.Message}"); }
        }

        // Define CLR events for input and output value changes
        public event EventHandler<ValueChangedEventArgs> InputValueChanged;

        public void OnInputValueChanged(object sender, ValueChangedEventArgs e)
        {
            InputValueChanged?.Invoke(sender, e);
            PerformCalculation(this);
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
           private set 
            { 
                SetValue(OutputValueProperty, value);                
            }
        }

        public static readonly DependencyProperty OutputValueProperty =
                            DependencyProperty.Register(nameof(OutputValue), typeof(double), typeof(CurrencyConverterControl),
                            new FrameworkPropertyMetadata(new PropertyChangedCallback(OutputValue_Textchanged)),
                                                validateValueCallback: new ValidateValueCallback(IsValidValue));

        private static void OutputValue_Textchanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            CurrencyConverterControl control = d as CurrencyConverterControl;
            if (control != null && CurrencyConverter is  null)
            {                
                //if Currency Converter is null, OnApplyTemplate() not entered yet
                control.InputValue = CurrencyDataProvider.Convert(control.DestinationCurrency, control.SourceCurrency, (double)e.NewValue);
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
                control.PerformCalculation(control);

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
                control.PerformCalculation(control);
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
            try
            {
                // Set currency converter and load currencies
                CurrencyConverter = CurrencyDataProvider;
                LoadCurrencies();
                CurrencyConverter.GetConvertionRates();

                // Unsubscribe from events if controls are not null
                UnsubscribeFromEvents(_currencyDestination, ComboBox_Loaded, CmbDestination_SelectionChanged);
                UnsubscribeFromEvents(_currencySource, ComboBox_Loaded, CmbSource_SelectionChanged);

                // Find currency destination and source controls in the template
                _currencyDestination = Template.FindName("CmbDestination", this) as ComboBox;
                _currencySource = Template.FindName("CmbSource", this) as ComboBox;

                // Check if controls were found
                if (_currencyDestination is null || _currencySource is null)
                {
                    throw new InvalidOperationException("The Combobox in the Template is not found! Please double check!");
                }
                else
                {
                    // Subscribe to events
                    SubscribeToEvents(_currencyDestination, ComboBox_Loaded, CmbDestination_SelectionChanged);
                    SubscribeToEvents(_currencySource, ComboBox_Loaded, CmbSource_SelectionChanged);
                }

               if( InputValue==0) InputValue=233.3 ;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
            finally
            {              
                base.OnApplyTemplate();
            }

        }

        // Method to unsubscribe from events if control is not null
        void UnsubscribeFromEvents(ComboBox control, RoutedEventHandler loadedHandler, SelectionChangedEventHandler selectionChangedHandler)
        {
            if (control != null)
            {
                control.Loaded -= loadedHandler;
                control.SelectionChanged -= selectionChangedHandler;
            }
        }

        // Method to subscribe to events if control is not null
        void SubscribeToEvents(ComboBox control, RoutedEventHandler loadedHandler, SelectionChangedEventHandler selectionChangedHandler)
        {
            if (control != null)
            {
                control.Loaded += loadedHandler;
                control.SelectionChanged += selectionChangedHandler;
            }
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
                var currencies = CurrencyConverter.GetCurrenciesData();

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

        //
        private void PerformCalculation(CurrencyConverterControl control)
        {
            try
            {
                //check if all have values before going to API calculate
                if (String.IsNullOrEmpty(control.SourceCurrency) || String.IsNullOrEmpty(control.DestinationCurrency)
                    || control.InputValue==0 || CurrencyConverter is null)
                {
                    return;
                }
                
                control.OutputValue = CurrencyConverter.Convert(control.SourceCurrency,
                                                            control.DestinationCurrency, control.InputValue);
            }
            catch (Exception ex) { Console.WriteLine($"Error: {ex.Message}"); }
            
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
                double conversionRate = CurrencyConverter.GetConversionRate(currencyFrom, currencyTo);
                var convertedValue= conversionRate * amount;
                return convertedValue;
            }
            catch (Exception ex) { Console.WriteLine($"Error: {ex.Message}"); }
            return -1; 
        }
        #endregion
    }

    public class CurrencyConverterChangedEventArgs : EventArgs
    {
        public ICurrencyDataProvider PreviousProvider { get; }
        public ICurrencyDataProvider NewProvider { get; }

        public CurrencyConverterChangedEventArgs(ICurrencyDataProvider previousProvider, ICurrencyDataProvider newProvider)
        {
            PreviousProvider = previousProvider;
            NewProvider = newProvider;
        }
    }

    // using for input and output value changes
    public class ValueChangedEventArgs : EventArgs
    {
        public double NewValue { get; }
        
        public ValueChangedEventArgs(double newValue)
        {
            NewValue = newValue;
        }
    }

}

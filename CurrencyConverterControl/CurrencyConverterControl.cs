using CurrencyConverterControl.Data;
using CurrencyConverterControl.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace CurrencyConverterControl
{
    /// <summary>
    /// Represents a custom WPF control for currency conversion.
    /// This control extends the base Control class and provides functionality
    /// for converting values between different currencies, along with a customizable
    /// user interface.
    /// </summary>
    public class CurrencyConverterControl : Control
    {
        private TextBox _inputvalue;
        private TextBox _outputvalue;
        private static CurrencyDataProvider _currencyDataProvider;
         static CurrencyConverterControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(CurrencyConverterControl), new FrameworkPropertyMetadata(typeof(CurrencyConverterControl)));
            _currencyDataProvider = new CurrencyDataProvider();
        }

        /// <summary>
        /// Gets or sets the input value for currency conversion within the CurrencyConverterControl.
        /// </summary>
        /// <remarks>
        /// The InputValue property represents the value to be converted between different currencies.
        /// It is bound to the user interface elements where users can input the amount for conversion.
        /// </remarks>
        public double InputValue
        {
            get { return (double)GetValue(InputValueProperty); }
            set { SetValue(InputValueProperty, value); }
        }
        private static readonly DependencyProperty InputValueProperty =
            DependencyProperty.Register("InputValue", typeof(double), typeof(CurrencyConverterControl));
                
        /// <summary>
        /// Gets or sets the output value for the result of currency conversion within the CurrencyConverterControl.
        /// </summary>
        /// <remarks>
        /// The OutputValue property represents the result of the currency conversion based on the provided input value.
        /// It is bound to the user interface elements where the converted amount is displayed to the user.
        /// </remarks>
        public double OutputValue
        {
            get { return (double)GetValue(OutputValueProperty); }
            set { SetValue(OutputValueProperty, value); }
        }

        public static readonly DependencyProperty OutputValueProperty =
           DependencyProperty.Register("OutputValue", typeof(double), typeof(CurrencyConverterControl));
        public override void OnApplyTemplate()
        {
            _inputvalue =Template.FindName("txtinput",this) as TextBox;
            _outputvalue = Template.FindName("txtOutput", this) as TextBox;
            LoadAsync();
            base.OnApplyTemplate();
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
        public ObservableCollection<Currency> CurrencyList { get; private set; } = new ObservableCollection<Currency>();

        private void LoadAsync()
        {
            try
            {
                if (CurrencyList.Any())
                {
                    return;
                }
                var currencies =  _currencyDataProvider.GetCurrenciesData();

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

        public static readonly DependencyProperty DestinationCurrencyProperty =
           DependencyProperty.Register("DestinationCurrency", typeof(string), typeof(CurrencyConverterControl));
        public string DestinationCurrency
        {
            get { return (string)GetValue(DestinationCurrencyProperty); }
            set { SetValue(DestinationCurrencyProperty, value); }
        }

        public static readonly DependencyProperty SourceCurrencyProperty =
            DependencyProperty.Register("SourceCurrency", typeof(string), typeof(CurrencyConverterControl));
        public string SourceCurrency
        {
            get { return (string)GetValue(SourceCurrencyProperty); }
            set { SetValue(SourceCurrencyProperty, value); }
        }

    }
}

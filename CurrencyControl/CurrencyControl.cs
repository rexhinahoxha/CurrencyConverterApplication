using CurrencyControl.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CurrencyControl
{
    [TemplatePart(Name = "BtnConvert", Type = typeof(Button))]
    public class CurrencyControl : Control
    {
        private static readonly CurrencyDataProvider _currencyDataProvider;
        

        static CurrencyControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(CurrencyControl), new FrameworkPropertyMetadata(typeof(CurrencyControl)));
            _currencyDataProvider = new CurrencyDataProvider();
           
        }

        public static readonly DependencyProperty InputValueProperty =
            DependencyProperty.Register("InputValue", typeof(double), typeof(CurrencyControl));

        public double InputValue
        {
            get { return (double)GetValue(InputValueProperty); }
            set { SetValue(InputValueProperty, value); }
        }

        public static readonly DependencyProperty OutputtValueProperty =
            DependencyProperty.Register("OutputtValue", typeof(double), typeof(CurrencyControl));

        public double OutputtValue
        {
            get { return (double)GetValue(OutputtValueProperty); }
            set { SetValue(OutputtValueProperty, value); }
        }

        public static readonly DependencyProperty CurrencylistProperty =
           DependencyProperty.Register("Currencylist", typeof(ObservableCollection<string>), typeof(CurrencyControl), new PropertyMetadata(new ObservableCollection<string>()));

        public ObservableCollection<string> Currencylist
        {
            get { return (ObservableCollection<string>)GetValue(CurrencylistProperty); }
            set { SetValue(CurrencylistProperty, value); }
        }

        public static readonly DependencyProperty SourceCurrencyProperty =
            DependencyProperty.Register("SourceCurrency", typeof(string), typeof(CurrencyControl));
        public string SourceCurrency
        {
            get { return (string)GetValue(SourceCurrencyProperty); }
            set { SetValue(SourceCurrencyProperty, value); }
        }

        public static readonly DependencyProperty DestinationCurrencyProperty =
           DependencyProperty.Register("DestinationCurrency", typeof(string), typeof(CurrencyControl));
        public string DestinationCurrency
        {
            get { return (string)GetValue(DestinationCurrencyProperty); }
            set { SetValue(DestinationCurrencyProperty, value); }
        }

        public static readonly RoutedEvent ConvertButtonClickedEvent =
        EventManager.RegisterRoutedEvent("ConvertButtonClicked", RoutingStrategy.Bubble,
            typeof(RoutedEventHandler), typeof(CurrencyControl));

        public event RoutedEventHandler ConvertButtonClicked
        {
            add { AddHandler(ConvertButtonClickedEvent, value); }
            remove { RemoveHandler(ConvertButtonClickedEvent, value); }
        }

        private void BtnConvert_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //check if all have values before going to API calculate
                if (String.IsNullOrEmpty(SourceCurrency) || String.IsNullOrEmpty(DestinationCurrency) || InputValue == 0.00)
                {
                    MessageBox.Show("Please fill in all the marked * as required fields!", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                OutputtValue = _currencyDataProvider.ConvertAsync(SourceCurrency, DestinationCurrency, InputValue).Result;
                RaiseEvent(new RoutedEventArgs(ConvertButtonClickedEvent, this));
            }
            catch (Exception ex) { Console.WriteLine($"Error: {ex.Message}"); }
        }
        public override void OnApplyTemplate()
        {
            BtnConvert = GetTemplateChild("BtnConvert") as Button;
            LoadAsync();
        }
        

        private Button btnConvert;

        private Button BtnConvert
        {
            get
            {
                return btnConvert;
            }

            set
            {
                if (btnConvert != null)
                {
                    btnConvert.Click -=
                        new RoutedEventHandler(BtnConvert_Click);
                }
                btnConvert = value;

                if (btnConvert != null)
                {
                    btnConvert.Click +=
                        new RoutedEventHandler(BtnConvert_Click);
                }
            }
        }

        async void LoadAsync()
        {
            try
            {
                if (Currencylist.Any())
                {
                    return;
                }
                var currencies = await _currencyDataProvider.GetCurrencies();

                if (currencies.Any())
                {
                    foreach (var currency in currencies)
                    {

                        Currencylist.Add(currency);
                    }
                }

            }
            catch (Exception ex) { throw ex; }

        }

        #region Properties 
        public static readonly DependencyProperty CustomFontProperty =
            DependencyProperty.Register(nameof(CustomFont), typeof(FontFamily), typeof(CurrencyControl));

        public FontFamily CustomFont
        {
            get { return (FontFamily)GetValue(CustomFontProperty); }
            set { SetValue(CustomFontProperty, value); }
        }

        public static readonly DependencyProperty CustomBackgroundProperty =
            DependencyProperty.Register(nameof(CustomBackground), typeof(Brush), typeof(CurrencyControl));

        public Brush CustomBackground
        {
            get { return (Brush)GetValue(CustomBackgroundProperty); }
            set { SetValue(CustomBackgroundProperty, value); }
        }

        public static readonly DependencyProperty CustomForegroundProperty =
            DependencyProperty.Register(nameof(CustomForeground), typeof(Brush), typeof(CurrencyControl));

        public Brush CustomForeground
        {
            get { return (Brush)GetValue(CustomForegroundProperty); }
            set { SetValue(CustomForegroundProperty, value); }
        }

        public static readonly DependencyProperty CustomStyleProperty =
            DependencyProperty.Register("CustomStyle", typeof(Style), typeof(CurrencyControl));
        public Style CustomStyle
        {
            get { return (Style)GetValue(CustomStyleProperty); }
            set { SetValue(CustomStyleProperty, value); }
        }
        #endregion


    }
}

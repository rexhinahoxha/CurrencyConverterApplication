using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    [TemplatePart(Name = "cmbToCurrency", Type = typeof(ComboBox))]
    public class CurrencyControl : Control
    {

       
        static CurrencyControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(CurrencyControl), new FrameworkPropertyMetadata(typeof(CurrencyControl)));
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
            // Raise the routed event
            RaiseEvent(new RoutedEventArgs(ConvertButtonClickedEvent, this));
        }
        public override void OnApplyTemplate()
        {
            BtnConvert = GetTemplateChild("BtnConvert") as Button;
            cmbToCurrency=GetTemplateChild("cmbToCurrency") as ComboBox;


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

        public static readonly RoutedEvent SourceComboBoxSelectionChangedEvent =
          EventManager.RegisterRoutedEvent("SourceComboBoxSelectionChangedEvent", RoutingStrategy.Bubble,
          typeof(RoutedEventHandler), typeof(CurrencyControl));

        public event RoutedEventHandler SourceComboBoxSelection
        {
            add { AddHandler(SourceComboBoxSelectionChangedEvent, value); }
            remove { RemoveHandler(SourceComboBoxSelectionChangedEvent, value); }
        }

        private ComboBox _cmbToCurrency;

        private ComboBox cmbToCurrency
        {
            get
            {
                return _cmbToCurrency;
            }

            set
            {
                if (_cmbToCurrency != null)
                {
                    _cmbToCurrency.SelectionChanged -=
                        new SelectionChangedEventHandler(cmbToCurrency_SelectionChanged);
                }
                _cmbToCurrency = value;

                if (_cmbToCurrency != null)
                {
                    _cmbToCurrency.SelectionChanged +=
                        new SelectionChangedEventHandler(cmbToCurrency_SelectionChanged);
                }
            }
        }
        private void cmbToCurrency_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Raise the routed event
            RaiseEvent(new RoutedEventArgs(SourceComboBoxSelectionChangedEvent, this));
            
        }


    }
}

using CurrencyConverterControl;
using CurrencyConverterControl.Model;
using NUnit.Framework;
using System;
using System.Windows.Controls;


namespace CurrencyConverterApplicationTests.UnitTests
{
    [TestFixture]
    public class CurrencyConverterTests
    {
      

        [Test]
        [Apartment(ApartmentState.STA)]
        public void ConvertValues_SuccessfulRequest_ShouldReturnPositiveConvertedAmount() 
        {
            // Arrange
            var converterControl = new CurrencyConverterControl.CurrencyConverterControl();

            // Act
            converterControl.InputValue = 100;
            converterControl.SourceCurrency = "USD";
            converterControl.DestinationCurrency =  "EUR";

            converterControl.OutputValue= converterControl.ConvertValues(converterControl.SourceCurrency,
                converterControl.DestinationCurrency,converterControl.InputValue);

            // Assert
            //If the result is Negative means that Getting Rate has failed
            Assert.That(converterControl.OutputValue, Is.Positive);
        }

        [Test]
        [Apartment(ApartmentState.STA)]
        public void ConvertValues_FailedRequest_ShouldReturnNegativeConvertedAmount()
        {
            // Arrange
            var converterControl = new CurrencyConverterControl.CurrencyConverterControl();

            // Act
            converterControl.InputValue = 100;
            converterControl.SourceCurrency = "ALL";
            converterControl.DestinationCurrency = "EUR" ;

            converterControl.OutputValue = converterControl.ConvertValues(converterControl.SourceCurrency, 
                                            converterControl.DestinationCurrency, converterControl.InputValue);

            // Assert
            //If the result is Negative means that Getting Rate has failed
            Assert.That(converterControl.OutputValue, Is.Negative);
        }


        [Test]
        [Apartment(System.Threading.ApartmentState.STA)]
        public void TestLoadCurrencies_ListResultNotEmpty()
        {
            // Arrange
            var converterControl = new CurrencyConverterControl.CurrencyConverterControl();

            // Act
            converterControl.LoadAsync();

            // Assert
            Assert.That(converterControl.CurrencyList, Is.Not.Empty);
        }
    }
}

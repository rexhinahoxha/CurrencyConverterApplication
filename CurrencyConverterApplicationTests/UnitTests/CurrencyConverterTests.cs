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

            // Assert
            //If the result is Negative means that Getting Rate has failed
            Assert.That(converterControl.OutputValue, Is.Positive);
        }

        [Test]
        [Apartment(ApartmentState.STA)]
        public void ConvertValues_SuccessfulRequest_ShouldReturnSameAmountConvertedAmount()
        {
            // Arrange
            var converterControl = new CurrencyConverterControl.CurrencyConverterControl();

            // Act
            converterControl.InputValue = 100;
            converterControl.SourceCurrency = "EUR";
            converterControl.DestinationCurrency = "EUR";

            // Assert
            //If the result is Negative means that Getting Rate has failed
            Assert.That(converterControl.OutputValue,Is.EqualTo(converterControl.InputValue));
        }

        [Test]
        [Apartment(ApartmentState.STA)]
        public void ConvertValues_FailedRequest_ShouldReturnValueGreaterThanConvertedAmount()
        {
            // Arrange
            var converterControl = new CurrencyConverterControl.CurrencyConverterControl();

            // Act
            converterControl.InputValue = 100;
            converterControl.SourceCurrency = "ALL";
            converterControl.DestinationCurrency = "EUR" ;

            // Assert
            //If the result is Negative means that Getting Rate has failed
            Assert.That(converterControl.OutputValue, Is.GreaterThan(0.9));
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

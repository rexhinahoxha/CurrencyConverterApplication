using CurrencyControl.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;



namespace CurrencyConverterApplicationTests.UnitTests
{
    [TestClass]
    public class CurrencyUnitTestTests
    {
        private CurrencyDataProvider _currencyService=new CurrencyDataProvider();


        [TestMethod]
        public async Task GetAllCurrenciesAsync_SuccessfulRequest_ShouldReturnCurrencyModel()
        {
           
            var result = await _currencyService.GetAllCurrenciesAsync();

            // Assert
            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsNotNull(result);
       
        }

        [TestMethod]
        public async Task ConvertAsync_ValidConversion_ShouldReturnConvertedAmount()
        {
            // Arrange
            string currencyFrom = "USD";
            string currencyTo = "EUR";
            double amount = 10.0;

            // Act
            var result = await _currencyService.ConvertAsync(currencyFrom, currencyTo, amount);

            // Assert
            Assert.AreNotEqual(10.0, result, "Expected converted amount is not the same value as the currencies are different");
        }


        [TestMethod]
        public async Task ConvertAsync_ValidConversion_ShouldReturnZeroConvertedAmount()
        {
            // Arrange
            string currencyFrom = "5454";
            string currencyTo = "EUR";
            double amount = 10.0;

            // Act
            var result = await _currencyService.ConvertAsync(currencyFrom, currencyTo, amount);

            // Assert
            Assert.AreEqual(0, result, "Expected converted amount is not returned");
        }

    }
}

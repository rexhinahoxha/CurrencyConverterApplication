using CurrencyConverterApplication.Data;
using CurrencyConverterApplication.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;



namespace CurrencyConverterApplication.UnitTests.Tests
{

    [TestClass]
    public class ProductUnitTestTests
    {

        [TestMethod]
        public async Task GetAllAsync_ShouldNotReturnAnEmptyList()
        {
            // Arrange
            var productProvider = new ProductDataProvider();

            // Act
            var result =  productProvider.GetAllAsync();

            // Assert
            Assert.IsNotNull(result, "Result should not be null");
        }

        [TestMethod]
        public async Task GetAllAsync_ShouldReturnListOfProducts()
        {
            // Arrange
            var productProvider = new ProductDataProvider();

            // Act
            var result =  productProvider.GetAllAsync();
            var productList = result.ToList();

            // Assert
            Assert.AreEqual(2, productList.Count, "Expected 2 products");
        }

        //[TestMethod]
        //public async Task GetConversionRate_SuccessfulApiResponse_ShouldReturnConversionRate()
        //{
        //    // Arrange
        //    var productProvider = new ProductDataProvider();
        //    string currencyFrom = "USD";
        //    string currencyTo = "EUR";
        //    double amount = 10.0;

        //    // Act
        //    var result =  productProvider.GetConversionRate(currencyFrom, currencyTo, amount);

        //    // Assert
        //    Assert.AreNotEqual(result, 0, "Expected a positive conversion rate for a successful API response");
        //}

        //[TestMethod]
        //public async Task GetConversionRate_UnsuccessfulApiResponse_ShouldLogErrorMessageAndReturnNegativeValue()
        //{
        //    // Arrange
        //    var productProvider = new ProductDataProvider(); 
        //    string currencyFrom = "InvalidCurrency";
        //    string currencyTo = "EUR";
        //    double amount = 10.0;

        //    // Act
        //    var result = await productProvider.GetConversionRate(currencyFrom, currencyTo, amount);

        //    // Assert
        //    Assert.AreEqual(-1, result, "Expected conversion rate to be zero for an unsuccessful API response");            
        //}
               

    }

}

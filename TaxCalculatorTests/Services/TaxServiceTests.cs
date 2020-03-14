using Microsoft.VisualStudio.TestTools.UnitTesting;
using TaxCalculator.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

using TaxCalculator.Models;

namespace TaxCalculator.Services.Tests
{
    [TestClass()]
    public class TaxServiceTests
    {
        private TaxService taxService;

        public TaxServiceTests()
        {
            taxService = new TaxService(new TaxJarCalculator());
        }

        [TestMethod()]
        public async Task GetTaxRate_Valid_Zip_Country_City_US()
        {
            // arrange
            var taxRateRequest = new TaxRatesRequest()
            {
                Zip = "90404",
                Country = "US",
                City = "Santa Monica"
            };

            var expectedCombinedRate = "0.1025";
            var expectedStateRate = "0.0625";
            var expectedZip = "90404";

            // act
            var taxRateResponse = await taxService.GetTaxRatesForLocation(taxRateRequest);

            // assert
            Assert.AreEqual(expectedCombinedRate, taxRateResponse.CombinedRate);
            Assert.AreEqual(expectedStateRate, taxRateResponse.StateRate);
            Assert.AreEqual(expectedZip, taxRateResponse.Zip);
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentException), "Param Zip is required.")]
        public async Task GetTaxRate_Valid_EmptyZip_Country_City_US()
        {
            // arrange
            var taxRateRequest = new TaxRatesRequest()
            {
                Zip = "",
                Country = "US",
                City = "Santa Monica"
            };

            // act
            await taxService.GetTaxRatesForLocation(taxRateRequest);

            // assert
        }

        [TestMethod()]
        public async Task GetTaxes_Valid_Data_US()
        {
            // arrage
            var taxesRequest = new TaxesRequest()
            {
                FromCountry = "US",
                FromZip = "07001",
                FromState = "NJ",
                ToCountry = "US",
                ToZip = "07446",
                ToState = "NJ",
                Amount = 16.50,
                Shipping = 1.5,
                LineItems = new List<GetTaxesRequestLineItem>()
                {
                    new GetTaxesRequestLineItem
                    {
                        Quantity = 1,
                        UnitPrice = 15.0,
                        ProductTaxCode = "31000"
                    }
                }
            };

            var expectedAmountToCollect = 1.09;
            var expectedTaxableAmount = 16.5;

            // act
            var taxesResponse = await taxService.GetTaxesForOrder(taxesRequest);

            // assert
            Assert.AreEqual(expectedAmountToCollect, taxesResponse.AmountToCollect);
            Assert.AreEqual(expectedTaxableAmount, taxesResponse.TaxableAmount);
        }
    }
}
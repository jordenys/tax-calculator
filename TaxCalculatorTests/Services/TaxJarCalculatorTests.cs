using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using TaxCalculator.Services;
using TaxCalculator.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace TaxCalculator.Services.Tests
{
    [TestClass()]
    public class TaxJarCalculatorTests
    {
        private ITaxCalculator taxCalculator  = new TaxJarCalculator();

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
            var taxRateResponse = await taxCalculator.GetTaxRatesForLocation(taxRateRequest);

            // assert
            Assert.AreEqual(expectedCombinedRate, taxRateResponse.CombinedRate);
            Assert.AreEqual(expectedStateRate, taxRateResponse.StateRate);
            Assert.AreEqual(expectedZip, taxRateResponse.Zip);
        }

        [TestMethod()]
        public async Task GetTaxRate_Valid_Zip_Country_City_Street_US()
        {
            // arrange
            var taxRateRequest = new TaxRatesRequest()
            {
                Zip = "98109",
                Country = "US",
                City = "Seattle",
                Street = "400 Broad St"
            };

            var expectedCountryRate = "0.0";

            // act
            var taxRateResponse = await taxCalculator.GetTaxRatesForLocation(taxRateRequest);

            // assert
            Assert.AreEqual(expectedCountryRate, taxRateResponse.CountryRate);
        }

        [TestMethod()]
        public async Task GetTaxRate_Valid_Zip_Country_City_CA()
        {
            // arrange
            var taxRateRequest = new TaxRatesRequest()
            {
                Zip = "V5K0A1",
                Country = "CA",
                City = "Vancouver"
            };

            var expectedCity = "Vancouver";
            var expectedCombinedRate = "0.12";

            // act
            var taxRateResponse = await taxCalculator.GetTaxRatesForLocation(taxRateRequest);

            // assert
            Assert.AreEqual(expectedCity, taxRateResponse.City);
            Assert.AreEqual(expectedCombinedRate, taxRateResponse.CombinedRate);
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentException),
            "Param Zip is required.")]
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
            await taxCalculator.GetTaxRatesForLocation(taxRateRequest);
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
            var taxesResponse = await taxCalculator.GetTaxesForOrder(taxesRequest);

            // assert
            Assert.AreEqual(expectedAmountToCollect, taxesResponse.AmountToCollect);
            Assert.AreEqual(expectedTaxableAmount, taxesResponse.TaxableAmount);
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentException),
            "Param ToCountry is required.")]
        public async Task GetTaxes_NoToCountry()
        {
            // arrage
            var taxesRequest = new TaxesRequest()
            {
                FromCountry = "US",
                FromZip = "07001",
                FromState = "NJ",
                ToCountry = "",
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
            var taxesResponse = await taxCalculator.GetTaxesForOrder(taxesRequest);

            // assert
            Assert.AreEqual(expectedAmountToCollect, taxesResponse.AmountToCollect);
            Assert.AreEqual(expectedTaxableAmount, taxesResponse.TaxableAmount);
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentException),
            "Param ToCountry is required.")]
        public async Task GetTaxes_NoToZip_US()
        {
            // arrage
            var taxesRequest = new TaxesRequest()
            {
                FromCountry = "US",
                FromZip = "07001",
                FromState = "NJ",
                ToCountry = "US",
                ToZip = "",
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
            var taxesResponse = await taxCalculator.GetTaxesForOrder(taxesRequest);

            // assert
            Assert.AreEqual(expectedAmountToCollect, taxesResponse.AmountToCollect);
            Assert.AreEqual(expectedTaxableAmount, taxesResponse.TaxableAmount);
        }

        [TestMethod()]
        public async Task GetTaxes_Valid_CA()
        {
            // arrage
            var taxesRequest = new TaxesRequest()
            {
                FromCountry = "CA",
                FromZip = "V6G 3E",
                FromState = "BC",
                ToCountry = "CA",
                ToZip = "M5V 2T6",
                ToState = "ON",
                Amount = 16.95,
                Shipping = 10,
                LineItems = new List<GetTaxesRequestLineItem>()
                {
                    new GetTaxesRequestLineItem
                    {
                        Quantity = 1,
                        UnitPrice = 16.95
                    }
                }
            };

            var expectedAmountToCollect = 3.5;
            var expectedTaxableAmount = 26.95;

            // act
            var taxesResponse = await taxCalculator.GetTaxesForOrder(taxesRequest);

            // assert
            Assert.AreEqual(expectedAmountToCollect, taxesResponse.AmountToCollect);
            Assert.AreEqual(expectedTaxableAmount, taxesResponse.TaxableAmount);
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentException),
            "Param ToState is required for US and CA.")]
        public async Task GetTaxes_NoToState_CA()
        {
            // arrage
            var taxesRequest = new TaxesRequest()
            {
                FromCountry = "CA",
                FromZip = "V6G 3E",
                FromState = "BC",
                ToCountry = "CA",
                ToZip = "M5V 2T6",
                Amount = 16.95,
                Shipping = 10,
                LineItems = new List<GetTaxesRequestLineItem>()
                {
                    new GetTaxesRequestLineItem
                    {
                        Quantity = 1,
                        UnitPrice = 16.95
                    }
                }
            };

            var expectedAmountToCollect = 3.5;
            var expectedTaxableAmount = 26.95;

            // act
            var taxesResponse = await taxCalculator.GetTaxesForOrder(taxesRequest);

            // assert
            Assert.AreEqual(expectedAmountToCollect, taxesResponse.AmountToCollect);
            Assert.AreEqual(expectedTaxableAmount, taxesResponse.TaxableAmount);
        }

    }
}
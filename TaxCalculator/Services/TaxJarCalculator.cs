using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using TaxCalculator.Models;
using TaxCalculator.Helpers;
using System.Threading.Tasks;
using System.Net.Http;
using TaxCalculator.Infrastructure;

using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace TaxCalculator.Services
{
    public class TaxJarCalculator: ITaxCalculator
    {
        async public Task<TaxesResponse> GetTaxesForOrder(TaxesRequest taxesRequest)
        {
            if (taxesRequest == null)
            {
                throw new ArgumentNullException("taxesRequest");
            }

            if (string.IsNullOrWhiteSpace(taxesRequest.ToCountry))
            {
                throw new ArgumentException("Param ToCountry is required.", nameof(taxesRequest));
            }

            if (string.IsNullOrWhiteSpace(taxesRequest.ToZip)) {
                if (taxesRequest.ToCountry == Constants.COUNTRY_CODE_US)
                {
                    throw new ArgumentException("Param ToZip is required for US.", nameof(taxesRequest));
                }
            }

            if (string.IsNullOrWhiteSpace(taxesRequest.ToState))
            {
                if (taxesRequest.ToCountry == Constants.COUNTRY_CODE_US
                    || taxesRequest.ToCountry == Constants.COUNTRY_CODE_CANADA) {
                    throw new ArgumentException("Param ToState is required for US and CA.", nameof(taxesRequest));
                }
            }

            // TODO: Finish all other possible validations as per api doc: https://developers.taxjar.com/api/reference/#post-calculate-sales-tax-for-an-order

            try
            {
                var taxesResponse = await TaxJarHttpClientProvider.PostAsync<TaxesJarResponse, TaxesRequest>("taxes", taxesRequest, Constants.API_KEY);
                return taxesResponse.Tax;
            }
            catch (Exception ex)
            {
                throw new TaxCalculatorException(ex.Message);
            }
        }

        async public Task<TaxRatesResponse> GetTaxRatesForLocation(TaxRatesRequest taxRateRequest)
        {
            if (taxRateRequest == null)
            {
                throw new ArgumentNullException("taxRateRequest");
            }

            if (string.IsNullOrWhiteSpace(taxRateRequest.Zip))
            {
                throw new ArgumentException("Param Zip is required for US.", nameof(taxRateRequest));
            }

            try
            {
                var taxRateRequestUri = BuildTaxRateRequestUri(taxRateRequest);

                var taxRateJarResponse = await TaxJarHttpClientProvider.GetAsync<TaxRatesJarResponse>(taxRateRequestUri, Constants.API_KEY);
                return taxRateJarResponse.Rate;
            } 
            catch (Exception ex)
            {
                throw new TaxCalculatorException(ex.Message);
            }
        }

        private string BuildTaxRateRequestUri(TaxRatesRequest taxRateRequest)
        {
            string taxRateQueryString = BuildTaxRateQueryString(taxRateRequest);

            return $"rates/{taxRateRequest.Zip}?{taxRateQueryString}";
        }

        private string BuildTaxRateQueryString(TaxRatesRequest taxRateRequest)
        {
            var queryString = System.Web.HttpUtility.ParseQueryString(string.Empty);
           
            if (!string.IsNullOrWhiteSpace(taxRateRequest.Country))
            {
                queryString.Add("country", taxRateRequest.Country);
            } 

            if (!string.IsNullOrWhiteSpace(taxRateRequest.State))
            {
                queryString.Add("state", taxRateRequest.State);
            }

            if (!string.IsNullOrWhiteSpace(taxRateRequest.City)) 
            {
                queryString.Add("city", taxRateRequest.City);
            }

            if (!string.IsNullOrWhiteSpace(taxRateRequest.Street))
            {
                queryString.Add("street", taxRateRequest.Street);
            }

            // Since the actual object is an HttpValueCollection, calling ToString will format the collection as a URL-encoded query string.
            return queryString.ToString();
        }



        public class TaxRatesJarResponse
        {
            public TaxRatesResponse Rate { get; set; }
        }

        public class TaxesJarResponse
        {
            public TaxesResponse Tax { get; set; }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

using TaxCalculator.Models;

namespace TaxCalculator.Services
{
    public class TaxService
    {
        private ITaxCalculator _taxCalculator;

        public TaxService(ITaxCalculator taxCalculator)
        {
            this._taxCalculator = taxCalculator;
        }

        async public Task<TaxRatesResponse> GetTaxRatesForLocation(TaxRatesRequest taxRateRequest)
        {
            // Do some businees logic here
            return await this._taxCalculator.GetTaxRatesForLocation(taxRateRequest);
        }

        async public Task<TaxesResponse> GetTaxesForOrder(TaxesRequest taxesRequest)
        {
            // Do some businees logic here
            return await this._taxCalculator.GetTaxesForOrder(taxesRequest);
        }
    }
}

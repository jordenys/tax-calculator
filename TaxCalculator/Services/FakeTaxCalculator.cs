using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

using TaxCalculator.Models;

namespace TaxCalculator.Services
{
    public class FakeTaxCalculator: ITaxCalculator
    {
        async public Task<TaxRatesResponse> GetTaxRatesForLocation(TaxRatesRequest taxRateRequest)
        {
            throw new NotImplementedException();
        }

        async public Task<TaxesResponse> GetTaxesForOrder(TaxesRequest taxesRequest)
        {
            throw new NotImplementedException();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TaxCalculator.Models
{
    public interface ITaxCalculator
    {
        public Task<TaxRatesResponse> GetTaxRatesForLocation(TaxRatesRequest taxRateRequest);
        public Task<TaxesResponse> GetTaxesForOrder(TaxesRequest taxesRequest);
    }
}

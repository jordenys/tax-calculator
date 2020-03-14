using System;
using System.Collections.Generic;
using System.Text;

namespace TaxCalculator.Models
{
    public class TaxRatesResponse
    {
        public string City { get; set; }
        public string CityRate { get; set; }
        public string CombinedDistrictRate { get; set; }
        public string CombinedRate { get; set; }
        public string Country { get; set; }
        public string CountryRate { get; set; }
        public string County { get; set; }
        public string CountyRate { get; set; }
        public string FreightTaxable { get; set; }
        public string State { get; set; }
        public string StateRate { get; set; }
        public string Zip { get; set; }
    }
}

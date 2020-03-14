using System;
using System.Collections.Generic;
using System.Text;

namespace TaxCalculator.Models
{
    public class TaxRatesRequest
    {
        public string Zip { get; set; }
        public string Country { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
    }
}

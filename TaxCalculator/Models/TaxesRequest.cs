using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace TaxCalculator.Models
{
    public class TaxesRequest
    {
        [JsonProperty("from_country")]
        public string FromCountry { get; set; }
        public string FromZip { get; set; }
        public string FromState { get; set; }

        public string ToCountry { get; set; }
        public string ToZip { get; set; }
        public string ToState { get; set; }
        public double Amount { get; set; }
        public double Shipping { get; set; }

        public IEnumerable<GetTaxesRequestLineItem> LineItems { get; set; }
    }

    public class GetTaxesRequestLineItem
    {
        public int Quantity { get; set; }
        public double UnitPrice { get; set; }
        public string ProductTaxCode { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace TaxCalculator.Models
{
    public class TaxesResponse
    {
        public double AmountToCollect { get; set; }
        public bool FreightTaxable { get; set; }
        public double OrderTotalAmount { get; set; }
        public double Rate { get; set; }
        public double Shipping { get; set; }
        public string TaxSource { get; set; }
        public double TaxableAmount { get; set; }
    }
}

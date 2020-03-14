using System;
using System.Collections.Generic;
using System.Net;
using System.Text;


using TaxCalculator.Models;

namespace TaxCalculator.Infrastructure
{
    [Serializable]
    public class TaxCalculatorException : ApplicationException
	{

		public TaxCalculatorException(string message) : base(message)
		{
		}
	}
}

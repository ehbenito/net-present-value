using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NPV.Requests
{
    public class NpvRequest
    {
        public decimal InitialValue { get; set; }
        public decimal LowerBoundDiscountRate { get; set; }
        public decimal UpperBoundDiscountRate { get; set; }
        public decimal DiscountRateIncrement { get; set; }
        public List<decimal> CashFlows { get; set; }
    }

}
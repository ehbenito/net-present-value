using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NPV.Model
{
    public class NPVRecord
    {
        public int Id { get; set; }
        public string CashFlows { get; set; }
        public decimal InitialValue { get; set; }
        public decimal DiscountRate { get; set; }
        public decimal NetPresentValue { get; set; }
    }
}

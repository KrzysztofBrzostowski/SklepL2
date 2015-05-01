using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SklepL2.ViewModel
{
    public class FindResultViewModel
    {
        public int Invoice_ID { get; set; }
        public String InvoiceNumber { get; set; }
        public String CustomerName { get; set; }
        public Decimal TotalAmount { get; set; }
    }
}
using System;

namespace Models
{
    public class CheckoutItem
    {
        public string Name { get; set; }
        public decimal Cost { get; set; }
        public SaleUnit Unit { get; set; }
    }
}

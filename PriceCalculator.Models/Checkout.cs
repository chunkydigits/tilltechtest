using System;
using System.Collections.Generic;
using System.Linq;

namespace Models
{
    public class Checkout
    {
        public Checkout()
        {
            Errors = new List<KeyValuePair<string, string>>();
        }

        public List<CheckoutItem> CheckoutItems { get; set; }
        public List<Offer> Offers { get; set; }

        public DateTime Timestamp { get; set; }

        public decimal Total
        {
            get { return CheckoutItems.Sum(o => o.Cost); }
        }

        public decimal TotalDiscount
        {
            get { return Offers.Sum(o => o.Discount); }
        }

        public List<KeyValuePair<string, string>> Errors { get; set; }

        // NOTE: Could add manual amendments here to allow for operator override and potentially an operator object so that they entire receipt can be generated from this object.
    }
}
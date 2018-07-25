using System;
using System.Collections.Generic;

namespace Models
{
    public class Offer
    {
        public string Name { get; set; }
        public CheckoutItem TriggerCheckoutItem { get; set; }
        public OfferType Type { get; set; }
        public List<string> MultibuyTriggerItems { get; set; }
        public decimal Amount { get; set; }
        public Delegate Calculation { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }
        public bool Applied { get; set; }
        public decimal Discount { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using Models;

namespace Persistence
{
    public class Database : IDatabase
    {
        // NOTE: This is simulating a database at present. 
        //       This would be the database entry point, to which all db transactions flow through. 

        public Database()
        {
            CheckoutItems = new List<CheckoutItem>();
            CheckoutItems.Add(new CheckoutItem { Name = "apples", Cost = 1m, Unit = SaleUnit.PerBag });
            CheckoutItems.Add(new CheckoutItem { Name = "beans", Cost = 0.65m, Unit = SaleUnit.PerItem });
            CheckoutItems.Add(new CheckoutItem { Name = "milk", Cost = 1.3m, Unit = SaleUnit.PerItem });
            CheckoutItems.Add(new CheckoutItem { Name = "bread", Cost = 0.8m, Unit = SaleUnit.PerItem });

            Offers = new List<Offer>();
            Offers.Add(new Offer { TriggerCheckoutItem = CheckoutItems.First(o => o.Name == "apples"), Type = OfferType.PercentageAmountReduction, Name = "10% off a bag of apples", Amount = 10m, ValidFrom = new DateTime(2018, 07, 25, 08, 00, 00), ValidTo = new DateTime(2018, 07, 30, 08, 00, 00) });
            Offers.Add(new Offer { TriggerCheckoutItem = CheckoutItems.First(o => o.Name == "beans"), Type = OfferType.PercentageAmountReduction, Name = "Buy 2 cans of beans and get a loaf of bread for half price", Amount = 10m, ValidFrom = new DateTime(2018, 07, 25, 08, 00, 00), ValidTo = new DateTime(2018, 07, 30, 08, 00, 00) });
        }

        public List<Offer> Offers { get; set; }

        public List<CheckoutItem> CheckoutItems { get; set; }

    }
}

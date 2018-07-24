using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Models;

namespace Persistence
{
    public delegate decimal SimpleDelegate(List<CheckoutItem> items);

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
            Offers.Add(new Offer
            {
                TriggerCheckoutItem = CheckoutItems.First(o => o.Name == "apples"),
                Type = OfferType.PercentageAmountReduction,
                Calculation = new SimpleDelegate(TenPercentOffApples),
                Name = "10% off a bag of apples",
                Amount = 10m,
                ValidFrom = new DateTime(2018, 07, 24, 08, 00, 00),
                ValidTo = new DateTime(2018, 07, 30, 08, 00, 00)
            });
            Offers.Add(new Offer
            {
                TriggerCheckoutItem = CheckoutItems.First(o => o.Name == "beans"),
                Type = OfferType.MultibuyDeal,
                Name = "Buy 2 cans of beans and get a loaf of bread for half price",
                Calculation = new SimpleDelegate(BeansOnToastSpecialDeal),
                Amount = 10m,
                ValidFrom = new DateTime(2018, 07, 24, 08, 00, 00),
                ValidTo = new DateTime(2018, 07, 30, 08, 00, 00)
            });
        }

        public List<Offer> Offers { get; set; }

        public List<CheckoutItem> CheckoutItems { get; set; }


        #region Delegate Methods 

        // This could be extracted into a different project so that it could be updated with new offers, 
        // built and deployed with an Event Handler mechanism that would dynamically be able to load the 
        // latest DLL without having to restart the service.

        // Alternatively with a little thought you could store the offers in the DB, but if new offer 
        // types were added, more complex logic would be added and downtime would be required. 

        // Possibly a combination of the two approaches would suit best for long term high availability.  
        public static decimal TenPercentOffApples(List<CheckoutItem> items)
        {
            return items.Where(o => o.Name == "apples").Sum(o => o.Cost) / 10;
        }

        public static decimal BeansOnToastSpecialDeal(List<CheckoutItem> items)
        {
            var numberOfBeans = items.Count(o => o.Name == "beans");
            var numberOfBread = items.Count(o => o.Name == "bread");

            int qualifyingBeansAndToastOffers = numberOfBeans / 2;
            if (numberOfBeans % 2 != 0)
            {
                qualifyingBeansAndToastOffers = (int)((numberOfBeans - 1) / 2);
            }
            
            var fullOffers = qualifyingBeansAndToastOffers > numberOfBread
                ? numberOfBread
                : qualifyingBeansAndToastOffers;
            var discountPerOffer = items.First(o => o.Name == "bread").Cost / 2;

            // Possible round issue here - would research further if production code
            return fullOffers * discountPerOffer;
        }

        #endregion Delegate Methods
    }
}

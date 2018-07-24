using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Models;
using Persistence;
using Till.Repositories;

namespace Till
{
    class Program
    {
        private static ITillRepository _tillRepository;
        private static IOfferRepository _offerRepository;

        private const string NO_OFFERS_TEXT = "No offers available";

        static void Main(string[] args)
        {
            WelcomeMessage();
            
            if (args.Length == 0)
            {
                Console.WriteLine("No items found");
                return;
            }

            Setup();

            var checkout = _tillRepository.ProcessCheckoutItems(args);
            var offers = _offerRepository.GetOffersToApply(checkout);

            checkout.Offers = offers;

            if (offers != null)
                _offerRepository.ApplyOffers(checkout);
            OutputBasketValues(checkout);
            OutputTotals(checkout);

            Console.Write("Done");
        }

        private static void OutputBasketValues(Checkout checkout)
        {
            Console.WriteLine("");
            Console.WriteLine("Item\t\tCost");
            Console.WriteLine("--------------------------");
            foreach (var item in checkout.CheckoutItems)
            {
                Console.WriteLine(item.Name + "\t\t" + item.Cost.ToString("c2"));
            }

            Console.WriteLine("--------------------------");
            Console.WriteLine("");
        }

        #region Supporting Methods 

        private static void OutputTotals(Checkout checkout)
        {
            Console.WriteLine("Subtotal: " + checkout.Total.ToString("c2"));
            if (!checkout.Offers.Any(o => o.Applied))
                Console.WriteLine(NO_OFFERS_TEXT);
            else
                _offerRepository.OutputOfferText(checkout);
            Console.WriteLine("Subtotal: " + checkout.Total.ToString("c2"));
        }

        private static void WelcomeMessage()
        {
            Console.WriteLine("**********************************************");
            Console.WriteLine("******************** Till ********************");
            Console.WriteLine("**********************************************");
        }

        private static void Setup()
        {
            var serviceProvider = new ServiceCollection()
                .AddSingleton<ITillRepository, TillRepository>()
                .AddSingleton<IOfferRepository, OfferRepository>()
                .AddSingleton<IDatabase, Database>()
                .AddSingleton<IDateRepository, DateRepository>()
                .BuildServiceProvider();

            _tillRepository = serviceProvider.GetService<ITillRepository>();
            _offerRepository = serviceProvider.GetService<IOfferRepository>();
        }

        #endregion Supporting Methods
    }
}

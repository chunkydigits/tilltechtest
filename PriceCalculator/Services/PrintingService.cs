using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Models;
using Persistence;
using Till.Repositories;

namespace Till.Services
{
    public class PrintingService : IPrintingService
    {
        private readonly IOfferRepository _offerRepository;
        private const string NO_OFFERS_TEXT = "No offers available";

        public PrintingService(IOfferRepository offerRepository)
        {
            _offerRepository = offerRepository ?? throw new ArgumentException(nameof(offerRepository));
        }

        public void WelcomeMessage()
        {
            Console.WriteLine("**********************************************\r\n******************** Till ********************\r\n**********************************************");
        }


        public void OutputTotals(Checkout checkout)
        {
            Console.WriteLine("Subtotal: " + checkout.Total.ToString("c2"));
            if (!checkout.Offers.Any(o => o.Applied))
                Console.WriteLine(NO_OFFERS_TEXT);
            else
                Console.WriteLine(_offerRepository.OutputOfferText(checkout));

            var finalTotal = checkout.Total - checkout.TotalDiscount;
            Console.WriteLine("Grand Total: " + finalTotal.ToString("c2"));
        }


        public void OutputBasketValues(Checkout checkout)
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
    }
}

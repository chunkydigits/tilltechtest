using System;
using System.Linq;
using Models;
using PriceCalculator.Repositories;

namespace PriceCalculator.Services
{
    public class PrintingService : IPrintingService
    {
        private const string NO_OFFERS_TEXT = "No offers available";
        private readonly IOfferRepository _offerRepository;

        public PrintingService(IOfferRepository offerRepository)
        {
            _offerRepository = offerRepository ?? throw new ArgumentException(nameof(offerRepository));
        }

        public void WelcomeMessage()
        {
            Console.WriteLine(
                "**********************************************\r\n******************** Till ********************\r\n**********************************************");
        }


        public void OutputTotals(Checkout checkout)
        {
            Console.WriteLine($"Subtotal: {checkout.Total.ToString("c2")}");
            if (!checkout.Offers.Any(o => o.Applied))
                Console.WriteLine(NO_OFFERS_TEXT);
            else
                Console.WriteLine(_offerRepository.OutputOfferText(checkout));

            var finalTotal = checkout.Total - checkout.TotalDiscount;
            Console.WriteLine($"Grand Total: {finalTotal.ToString("c2")}");
        }


        public void OutputBasketValues(Checkout checkout)
        {
            Console.WriteLine("");
            Console.WriteLine("\tItem\t\tCost");
            Console.WriteLine("----------------------------------------------");
            foreach (var item in checkout.CheckoutItems)
                Console.WriteLine($"\t{item.Name}\t\t{item.Cost.ToString("c2")}");

            Console.WriteLine("----------------------------------------------");
            Console.WriteLine("");
        }
    }
}
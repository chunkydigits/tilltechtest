using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Models;
using Persistence;
using Till.Repositories;
using Till.Services;

namespace Till
{
    class Program
    {
        private static ITillRepository _tillRepository;
        private static IOfferRepository _offerRepository;

        private static IPrintingService _printingService;
        

        static void Main(string[] args)
        {
            Setup();

            _printingService.WelcomeMessage();

            if (args.Length == 0)
            {
                Console.WriteLine("No items found");
                return;
            }

            var checkout = _tillRepository.ProcessCheckoutItems(args);
            var offers = _offerRepository.GetOffersToApply(checkout);

            checkout.Offers = offers;

            if (offers != null)
                _offerRepository.ApplyOffers(checkout);
            _printingService.OutputBasketValues(checkout);
            _printingService.OutputTotals(checkout);

            Console.Write("Done");
        }

        #region Supporting Methods 

        private static void Setup()
        {
            var serviceProvider = new ServiceCollection()
                .AddSingleton<IPrintingService, PrintingService>()
                .AddSingleton<ITillRepository, TillRepository>()
                .AddSingleton<IOfferRepository, OfferRepository>()
                .AddSingleton<IDatabase, Database>()
                .AddSingleton<IDateRepository, DateRepository>()
                .BuildServiceProvider();

            _printingService = serviceProvider.GetService<IPrintingService>();
            _tillRepository = serviceProvider.GetService<ITillRepository>();
            _offerRepository = serviceProvider.GetService<IOfferRepository>();
        }

        #endregion Supporting Methods
    }
}

using System;
using Microsoft.Extensions.DependencyInjection;
using Persistence;
using PriceCalculator.Repositories;
using PriceCalculator.Services;

namespace PriceCalculator
{
    internal class Program
    {
        private static ITillRepository _tillRepository;
        private static IOfferRepository _offerRepository;

        private static IPrintingService _printingService;

        private static void Main(string[] args)
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
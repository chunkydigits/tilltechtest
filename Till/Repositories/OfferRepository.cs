using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Models;
using Persistence;

namespace Till.Repositories
{
    public class OfferRepository : IOfferRepository
    {
        private readonly IDatabase _database;
        private readonly IDateRepository _dateRepository;

        public OfferRepository(IDatabase database, IDateRepository dateRepository)
        {
            _database = database ?? throw new ArgumentNullException(nameof(database));
            _dateRepository = dateRepository ?? throw new ArgumentException(nameof(database));
        }

        public List<Offer> GetOffersToApply(Checkout checkout)
        {
            return _database.Offers.Where(o => checkout.CheckoutItems.Select(p => p.Name).Contains(o.TriggerCheckoutItem.Name) 
                                               && o.ValidTo >= _dateRepository.UtcNow()
                                               && o.ValidFrom <= _dateRepository.UtcNow()).ToList();
        }

        public string OutputOfferText(Checkout checkout)
        {
            var returnMessage = new StringBuilder();
            foreach (var offer in checkout.Offers.Where(o => o.Applied))
            {
                returnMessage.AppendLine(GetOfferSummaryTextWithSubTotal(offer));
            }

            return returnMessage.ToString();
        }

        public Checkout ApplyOffers(Checkout checkout)
        {
            foreach (var offer in checkout.Offers)
            {
                // Want to execute a function here 
                // does it need to be a delegate or function or what? 
                // checkoutOffer.Calculation(checkout.CheckoutItems);
            }
            return checkout;
        }

        #region Supporting Methods 

        private string GetOfferSummaryTextWithSubTotal(Offer offer)
        {
            var text = offer.Name + ": ";

            // Considerations around ordering would be required when applying these offers as a fixed as totals could differ depending the order that they are applied
            switch (offer.Type)
            {
                case OfferType.FixedAmountReduction:
                    text += ("£" + offer.Amount);
                    break;
                case OfferType.PercentageAmountReduction:
                    //text += ("" + offer.Calculation);
                    // Calculate this 
                    break;
                case OfferType.MultibuyDeal:
                    text += "Multibuy Offer";
                    break;
            }

            return text;
        }

        #endregion Supporting Methods 
    }
}

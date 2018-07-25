using System.Collections.Generic;
using Models;

namespace Till.Repositories
{
    public interface IOfferRepository
    {
        List<Offer> GetOffersToApply(Checkout checkout);
        string OutputOfferText(Checkout checkout);
        Checkout ApplyOffers(Checkout checkout);
    }
}
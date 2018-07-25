using System.Collections.Generic;
using Models;

namespace Persistence
{
    public interface IDatabase
    {
        List<Offer> Offers { get; set; }

        List<CheckoutItem> CheckoutItems { get; set; }
    }
}
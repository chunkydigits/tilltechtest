using System;
using System.Collections.Generic;
using System.Linq;
using Models;

namespace Persistence
{
    public interface IDatabase
    {
        List<Offer> Offers { get; set; }

        List<CheckoutItem> CheckoutItems { get; set; }   
    }
}

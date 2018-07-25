using System.Collections.Generic;
using Models;

namespace Till.Repositories
{
    public interface ITillRepository
    {
        Checkout ProcessCheckoutItems(string[] checkoutItems);
    }
}
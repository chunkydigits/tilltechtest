using Models;

namespace PriceCalculator.Repositories
{
    public interface ITillRepository
    {
        Checkout ProcessCheckoutItems(string[] checkoutItems);
    }
}
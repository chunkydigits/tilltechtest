using Models;

namespace PriceCalculator.Services
{
    public interface IPrintingService
    {
        void WelcomeMessage();
        void OutputTotals(Checkout checkout);
        void OutputBasketValues(Checkout checkout);
    }
}
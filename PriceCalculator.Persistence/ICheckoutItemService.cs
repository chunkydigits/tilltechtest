using Models;

namespace Persistence
{
    public interface ICheckoutItemService
    {
        CheckoutItem GetItemByName(string name);
    }
}
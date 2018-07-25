using System.Linq;
using Models;

namespace Persistence
{
    public class CheckoutItemService : ICheckoutItemService
    {
        public Database _database = new Database();

        public CheckoutItem GetItemByName(string name)
        {
            return _database.CheckoutItems.FirstOrDefault(o => o.Name == name);
        }
    }
}
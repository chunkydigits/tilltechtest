using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Models;
using Persistence;

namespace Till.Repositories
{
    public class TillRepository : ITillRepository
    {
        private readonly IDatabase _database;
        private readonly IDateRepository _dateRepository;

        public TillRepository(IDatabase database, IDateRepository dateRepository)
        {
            _database = database ?? throw new ArgumentNullException(nameof(database));
            _dateRepository = dateRepository ?? throw new ArgumentException(nameof(database));
        }

        public Checkout ProcessCheckoutItems(string[] checkoutItems)
        {
            var checkout = new Checkout();
            checkout.Timestamp = _dateRepository.UtcNow();
            checkout.CheckoutItems = new List<CheckoutItem>();


            if (checkoutItems == null)
                return checkout;

            foreach (var checkoutItem in checkoutItems)
            {
                var itemFromDB = _database.CheckoutItems.FirstOrDefault(o => o.Name == checkoutItem);

                if(itemFromDB != null)
                    checkout.CheckoutItems.Add(itemFromDB);
                else 
                    checkout.Errors.Add(new KeyValuePair<string, string>("Item not found in DB", checkoutItem));
            }

            return checkout;
        }
    }
}

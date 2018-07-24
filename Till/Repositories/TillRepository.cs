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

            foreach (var checkoutItem in checkoutItems)
            {
                checkout.CheckoutItems.Add(_database.CheckoutItems.FirstOrDefault(o => o.Name == checkoutItem));
            }

            return checkout;
        }
    }
}

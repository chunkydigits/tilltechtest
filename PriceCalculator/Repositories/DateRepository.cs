using System;

namespace PriceCalculator.Repositories
{
    public class DateRepository : IDateRepository
    {
        public DateTime UtcNow()
        {
            return DateTime.UtcNow;
        }
    }
}
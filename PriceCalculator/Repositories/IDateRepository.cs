using System;

namespace PriceCalculator.Repositories
{
    public interface IDateRepository
    {
        DateTime UtcNow();
    }
}
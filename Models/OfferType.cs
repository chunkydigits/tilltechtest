using System;

namespace Models
{
    public enum OfferType
    {
        FixedAmountReduction = 1, 
        PercentageAmountReduction = 2, 
        MultibuyDeal = 4
        // NOTE: Add more deal types here to accomodate different ways to offer money off
    }
}

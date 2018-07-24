using System;
using System.Collections.Generic;
using Models;

namespace Till.Services
{
    public interface IPrintingService
    {
        void WelcomeMessage();
        void OutputTotals(Checkout checkout);
        void OutputBasketValues(Checkout checkout);
    }
}
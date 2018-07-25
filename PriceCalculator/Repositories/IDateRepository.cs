using System;
using System.Collections.Generic;
using Models;

namespace Till.Repositories
{
    public interface IDateRepository
    {
        DateTime UtcNow();
    }
}
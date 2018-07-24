using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Models;
using Persistence;

namespace Till.Repositories
{
    public class DateRepository : IDateRepository
    {
        public DateTime UtcNow()
        {
            return DateTime.UtcNow;
        }
    }
}

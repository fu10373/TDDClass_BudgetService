using System;
using System.Collections.Generic;
using System.Text;

namespace BudgetServiceCore.Service
{
    public interface IBudgetService
    {
        public decimal Query(DateTime start, DateTime end);
    }
}

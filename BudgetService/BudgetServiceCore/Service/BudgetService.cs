using BudgetServiceConsoleApp.Repo;
using BudgetServiceCore.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace BudgetServiceCore.Service
{
    public class BudgetService : IBudgetService
    {
        private readonly IBudgetRepo _budgetRepo;

        public BudgetService(IBudgetRepo budgetRepo)
        {
            _budgetRepo = budgetRepo;
        }

        private class MonthToGetBudget
        {
            //public string YearMonth { get { return Year.ToString("####") + Month.ToString("##");  } }
            public string YearMonthKey { get; set; }
            public int Year { get; set; }
            public int Month { get; set; }
            public int DaysToCalculate { get; set; }
        }        

        private List<MonthToGetBudget> breakDateRangeToMonthToGetBudgets(DateTime start, DateTime end)
        {
            var result = new List<MonthToGetBudget>();

            // The purpose of this function is to break a range into each month to get budget for
            // For example, 2020/6/29 ~ 2020/8/1
            // Result:
            //   2020/6/29~2020/6/30 (2 days)
            //   2020/7/1~2020/7/31 (31 days)
            //   2020/8/1~2020/8/1 (1 day)

            // Start with endIndex=DateTime.MinValue to make sure that loop is entered
            var startIndex = start;
            var endIndex = DateTime.MinValue;
            while (endIndex < end)
            {
                endIndex = new DateTime(startIndex.Year, startIndex.Month, DateTime.DaysInMonth(startIndex.Year, startIndex.Month));
                if (endIndex > end)
                {
                    endIndex = end;
                }

                MonthToGetBudget oneRange = new MonthToGetBudget()
                {
                    YearMonthKey = startIndex.ToString("yyyyMM"),
                    Year = startIndex.Year,
                    Month = startIndex.Month,
                    DaysToCalculate = (endIndex - startIndex).Days + 1
                };
                result.Add(oneRange);

                startIndex = endIndex.AddDays(1);
            }

            return result;
        }

        private decimal GetBudgetForMonth(MonthToGetBudget forMonth, Dictionary<string, Budget> useLookup)
        {
            var budget = 0;
            if (useLookup.ContainsKey(forMonth.YearMonthKey))
            {
                budget = useLookup[forMonth.YearMonthKey].Amount;
            }

            return Convert.ToDecimal(forMonth.DaysToCalculate) * Convert.ToDecimal(budget) / DateTime.DaysInMonth(forMonth.Year, forMonth.Month);

        }

        public decimal Query(DateTime start, DateTime end)
        {
            if (end < start)
            {
                return 0;
            }

            // Get all budgets and sort to lookup
            var allBudgets = _budgetRepo.GetAll();
            var budgetLookup = allBudgets.ToDictionary(item => item.YearMonth);

            // Break query into ranges
            var ranges = breakDateRangeToMonthToGetBudgets(start, end);

            // For each range, get budget
            decimal result = 0;
            foreach (var range in ranges)
            {
                decimal tempResult = GetBudgetForMonth(range, budgetLookup);
                result += tempResult;
            }

            return result;
        }
    }
}

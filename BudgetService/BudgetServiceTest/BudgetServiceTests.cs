using BudgetServiceConsoleApp.Repo;
using BudgetServiceCore.Model;
using BudgetServiceCore.Service;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace BudgetServiceTest
{
    [TestFixture]
    public class BudgetServiceTests
    {
        [Test]
        public void Test_FindResult_QueryOneDayInSameMonth()
        {
            var budgetRepo = NSubstitute.Substitute.For<IBudgetRepo>();
            budgetRepo.GetAll().Returns(new List<Budget>
            {
                new Budget(){YearMonth="202004", Amount=30},
            });

            var budgetService = new BudgetService(budgetRepo);
            var result = budgetService.Query(
                new DateTime(2020, 4, 1),
                new DateTime(2020, 4, 1)
            );

            Assert.AreEqual(1, result);
        }


        [Test]
        public void Test_FindNoResult_QueryMoreDaysInSameMonth()
        {
            var budgetRepo = NSubstitute.Substitute.For<IBudgetRepo>();
            budgetRepo.GetAll().Returns(new List<Budget>
            {
                new Budget(){YearMonth="202003", Amount=30},
            });

            var budgetService = new BudgetService(budgetRepo);
            var result = budgetService.Query(
                new DateTime(2020, 4, 1),
                new DateTime(2020, 4, 5)
            );

            Assert.AreEqual(0, result);
        }


        [Test]
        public void Test_FindResult_TestQueryMoreDaysInSameMonth()
        {
            var budgetRepo = NSubstitute.Substitute.For<IBudgetRepo>();
            budgetRepo.GetAll().Returns(new List<Budget>
            {
                new Budget(){YearMonth="202004", Amount=30},
            });

            var budgetService = new BudgetService(budgetRepo);
            var result = budgetService.Query(
                new DateTime(2020, 4, 1),
                new DateTime(2020, 4, 5)
            );

            Assert.AreEqual(5, result);
        }

        [Test]
        public void Test_FindNoResult_TestQueryMoreDaysInSameMonth()
        {
            var budgetRepo = NSubstitute.Substitute.For<IBudgetRepo>();
            budgetRepo.GetAll().Returns(new List<Budget>
            {
                new Budget(){YearMonth="202003", Amount=30},
            });

            var budgetService = new BudgetService(budgetRepo);
            var result = budgetService.Query(
                new DateTime(2020, 4, 1),
                new DateTime(2020, 4, 5)
            );

            Assert.AreEqual(0, result);
        }


        [Test]
        public void Test_FindResult_TestQueryMoreDaysInDifferentMonths()
        {
            var budgetRepo = NSubstitute.Substitute.For<IBudgetRepo>();
            budgetRepo.GetAll().Returns(new List<Budget>
            {
                new Budget(){YearMonth="202004", Amount=30},
                new Budget(){YearMonth="202005", Amount=62},
            });

            var budgetService = new BudgetService(budgetRepo);
            var result = budgetService.Query(
                new DateTime(2020, 4, 29),
                new DateTime(2020, 5, 2)
            );

            Assert.AreEqual(6, result);
        }

        [Test]
        public void Test_FindNoResult_TestQueryMoreDaysInDifferentMonths()
        {
            var budgetRepo = NSubstitute.Substitute.For<IBudgetRepo>();
            budgetRepo.GetAll().Returns(new List<Budget>
            {
                new Budget(){YearMonth="202005", Amount=62},
                new Budget(){YearMonth="202006", Amount=30}
            });

            var budgetService = new BudgetService(budgetRepo);
            var result = budgetService.Query(
                new DateTime(2020, 4, 29),
                new DateTime(2020, 5, 2)
            );

            Assert.AreEqual(4, result);
        }


        [Test]
        public void Test_FindResult_TestQueryMoreDaysInDifferentYears()
        {
            var budgetRepo = NSubstitute.Substitute.For<IBudgetRepo>();
            budgetRepo.GetAll().Returns(new List<Budget>
            {
                new Budget(){YearMonth="202012", Amount=31},
                new Budget(){YearMonth="202101", Amount=62}
            });

            var budgetService = new BudgetService(budgetRepo);
            var result = budgetService.Query(
                new DateTime(2020, 12, 31),
                new DateTime(2021, 1, 31)
            );

            Assert.AreEqual(63, result);
        }

        [Test]
        public void Test_FindNoResult_TestQueryMoreDaysInDifferentYears()
        {
            var budgetRepo = NSubstitute.Substitute.For<IBudgetRepo>();
            budgetRepo.GetAll().Returns(new List<Budget>
            {
                new Budget(){YearMonth="202011", Amount=30},
                new Budget(){YearMonth="202101", Amount=62}
            });

            var budgetService = new BudgetService(budgetRepo);
            var result = budgetService.Query(
                new DateTime(2020, 11, 1),
                new DateTime(2021, 1, 31)
            );

            Assert.AreEqual(92, result);
        }


        [Test]
        public void Test_FindResult_TestQueryMoreDaysInLeapYear()
        {
            // 閏年
            var budgetRepo = NSubstitute.Substitute.For<IBudgetRepo>();
            budgetRepo.GetAll().Returns(new List<Budget>
            {
                new Budget(){YearMonth="202001", Amount=31},
                new Budget(){YearMonth="202002", Amount=29},
                new Budget(){YearMonth="202003", Amount=31}
            });

            var budgetService = new BudgetService(budgetRepo);
            var result = budgetService.Query(
                new DateTime(2020, 1, 1),
                new DateTime(2020, 3, 1)
            );

            Assert.AreEqual(61, result);
        }

        [Test]
        public void Test_FindNoResult_TestQueryMoreDaysInLeapYear()
        {
            // 閏年
            var budgetRepo = NSubstitute.Substitute.For<IBudgetRepo>();
            budgetRepo.GetAll().Returns(new List<Budget>
            {
                new Budget(){YearMonth="202002", Amount=29},
                new Budget(){YearMonth="202003", Amount=31}
            });

            var budgetService = new BudgetService(budgetRepo);
            var result = budgetService.Query(
                new DateTime(2020, 1, 1),
                new DateTime(2020, 3, 1)
            );

            Assert.AreEqual(30, result);
        }


        [Test]
        public void Test_FindResult_TestQueryMoreDaysInDifferentYearsIncludeLeapYear()
        {
            // 閏年
            var budgetRepo = NSubstitute.Substitute.For<IBudgetRepo>();
            budgetRepo.GetAll().Returns(new List<Budget>
            {
                new Budget(){YearMonth="201912", Amount=31},
                new Budget(){YearMonth="202001", Amount=31},
                new Budget(){YearMonth="202002", Amount=29},
                new Budget(){YearMonth="202003", Amount=31}
            });

            var budgetService = new BudgetService(budgetRepo);
            var result = budgetService.Query(
                new DateTime(2019, 12, 1),
                new DateTime(2020, 3, 1)
            );

            Assert.AreEqual(92, result);
        }

        public void Test_FindNoResult_TestQueryMoreDaysInDifferentYearsIncludeLeapYear()
        {
            // 閏年
            var budgetRepo = NSubstitute.Substitute.For<IBudgetRepo>();
            budgetRepo.GetAll().Returns(new List<Budget>
            {
                new Budget(){YearMonth="202001", Amount=31},
                new Budget(){YearMonth="202002", Amount=29},
                new Budget(){YearMonth="202003", Amount=31}
            });

            var budgetService = new BudgetService(budgetRepo);
            var result = budgetService.Query(
                new DateTime(2019, 12, 1),
                new DateTime(2020, 3, 1)
            );

            Assert.AreEqual(61, result);
        }

        public void TestQueryDay()
        {
            var budgetRepo = NSubstitute.Substitute.For<IBudgetRepo>();
            budgetRepo.GetAll().Returns(new List<Budget>
            {
                new Budget(){YearMonth="202004", Amount=30},
            });

            var budgetService = new BudgetService(budgetRepo);
            var result = budgetService.Query(
                new DateTime(2020, 4, 1),
                new DateTime(2020, 4, 1)
            );

            Assert.AreEqual(0, result);
        }

    }
}

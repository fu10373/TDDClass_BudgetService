using BudgetServiceCore.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace BudgetServiceConsoleApp.Repo
{
    public interface IBudgetRepo
    {
        List<Budget> GetAll();
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.GeneralLedger
{
    public class GLBudgetVwBO
    {
        public GLBudgetBO budget = new GLBudgetBO();
        public List<GLBudgetDetailsBO> budgetDetails = new List<GLBudgetDetailsBO>();
        public string budgetTable { get; set; }
    }
}

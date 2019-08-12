using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LeshLoanMngt.Entities
{
    public class Expense : Entity
    {
        public string ExpenseID;

        //public string ExpenseName;

        public string Amount;

        public string ExpenseDate;

        public string Description;

        public string Type;

        public string ReceiptNumber;
    }
}

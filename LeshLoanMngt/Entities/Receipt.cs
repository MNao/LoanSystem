using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LeshLoanMngt.Entities
{
   public class Receipt:Entity
    {
        public string ReceiptNumber;

        public string LoanNumber;

        public string ClientID;

        public string PaymentType;

        //private string chequeNumberField;

        //private string bankNameField;

        //private string bankAccountNameField;

        //private string bankAccountNumberField;

        public string PaymentDate;

        //private string recieptCategoryField;

        //private string imageOfRecieptField;

        public string CurrencyCode;

        public string ReceiptAmount;

        public string Remarks;
    }
}

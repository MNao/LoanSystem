using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LeshLoanMngt.Entities
{
    public class LoanDetails:Entity
    {
        public string ClientID = "";
        public string LoanNo = "";
        public string LoanAmount = "";
        public string LoanDesc = "";
        public string LoanDate = "";
        public string InterestRate = "";
        public string ApprovedAmount = "";
        public string LastLoanAmount = "";
        public string CurrentDebt = "";
        public string Organization = "";
        public string MonthsToPayIn = "";
        public string EasyPaidAmountPerMonth = "";
        public string Approved = "";
        public string AgreeToLoanAggreement = "";

        //collateral Details
        public string Name = "";
        public string Type = "";
        public string Model = "";
        public string Make = "";
        public string SerialNumber = "";
        public string EstimatedPrice = "";
        public string ImageProof = "";
        public string Observations = "";

        ////Repayment Details
        //public string PaidAmount = "";
        //public string 
    }
}

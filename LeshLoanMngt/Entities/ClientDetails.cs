using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LeshLoanMngt.Entities
{
    public class ClientDetails : Entity
    {

        //public string BranchCode = "";
        public string ClientNo = "";
        public string ClientName = "";
        public string ClientAddress = "";
        public string ClientPhoneNumber = "";
        public string Referee = "";
        public string RefrereePhoneNo = "";
        public string Gender = "";
        public string IDType = "";
        public string IDNumber = "";
        public string IDPhoto = "";
        public string ClientPhoto = "";
        public string ClientEmail = "";
        public string ClientPassword = "";

        //Additional client details
        public string DOB = "";
        public string BusinessLoc = "";
        public string Occupation = "";
        public string NoOfBeneficiaries = "";
        public string EducLevel = "";
        public string MonthlyIncome = "";

        public override bool IsValid()
        {
            string propertiesThatCanBeNull = "ClientAddress|ClientEmail|Gender|BusinessLoc|Occupation|NoOfBeneficiaries|EducLevel|MonthlyIncome";
            string nullCheckResult = SharedCommons.CheckForNulls(this, propertiesThatCanBeNull);
            if (nullCheckResult != Globals.SUCCESS_STATUS_TEXT)
            {
                StatusCode = Globals.FAILURE_STATUS_CODE;
                StatusDesc = nullCheckResult;
                return false;
            }


            //if (!SharedCommons.IsValidBoolean(IsPaid))
            //{
            //    StatusCode = Globals.FAILURE_STATUS_CODE;
            //    StatusDesc = "IS PAID SHOULD BE A BOOLEAN";
            //    return false;
            //}
            //if (!SharedCommons.IsNumeric(DiscountAmount))
            //{
            //    StatusCode = Globals.FAILURE_STATUS_CODE;
            //    StatusDesc = "DISCOUNT AMOUNT SHOULD BE NUMERIC";
            //    return false;
            //}
            //if (!SharedCommons.IsNumericAndAboveZero(TotalInvoiceAmount))
            //{
            //    StatusCode = Globals.FAILURE_STATUS_CODE;
            //    StatusDesc = "TOTAL INVOICE AMOUNT SHOULD BE NUMERIC AND ABOVE ZERO";
            //    return false;
            //}
            return base.IsValid();
        }
    }
}

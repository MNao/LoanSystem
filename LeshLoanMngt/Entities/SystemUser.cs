using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LeshLoanMngt.Entities
{
    public class SystemUser : Entity
    {
        public string UserId = "";
        public string Password = "";
        public string Name = "";
        //public string UserType = "";
        public string RoleCode = "";
        public string IsActive = "";
        public string Email = "";
        public bool ResetPassword;
        public override bool IsValid()
        {
            string propertiesThatCanBeNull = "ResetPassword|Email";
            string nullCheckResult = SharedCommons.CheckForNulls(this, propertiesThatCanBeNull);
            if (nullCheckResult != Globals.SUCCESS_STATUS_TEXT)
            {
                StatusCode = Globals.FAILURE_STATUS_CODE;
                StatusDesc = nullCheckResult;
                return false;
            }

            if (string.IsNullOrEmpty(IsActive))
            {
                IsActive = "False";
            }

            return base.IsValid();
        }
    }
}

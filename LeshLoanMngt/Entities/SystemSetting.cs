using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeshLoanMngt.Entities
{
    public class SystemSetting : Entity
    {
        public string SettingName = "";
        public string SettingCode = "";
        public string SettingValue = "";

        public override bool IsValid()
        {
            string propertiesThatCanBeNull = "SettingName";
            string nullCheckResult = SharedCommons.CheckForNulls(this, propertiesThatCanBeNull);
            if (nullCheckResult != Globals.SUCCESS_STATUS_TEXT)
            {
                StatusCode = Globals.FAILURE_STATUS_CODE;
                StatusDesc = nullCheckResult;
                return false;
            }
            return base.IsValid();
        }
    }
}


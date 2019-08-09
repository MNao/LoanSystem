using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LeshLoanMngt.Entities
{
    public class Entity : Status
    {
        public string RecordId = "";
        public string CompanyCode = "";
        public string CreatedBy = "";
        public string ModifiedBy = "";
        public string CreatedOn;
        public string ModifiedOn;

        public virtual bool IsValid()
        {
            Entity anEntity = GetEntity();
            string propertiesThatCanBeNull = "CompanyCode|RecordId|CreatedBy|CreatedOn|ModifiedOn";
            string nullCheckResult = SharedCommons.CheckForNulls(anEntity, propertiesThatCanBeNull);
            if (nullCheckResult != Globals.SUCCESS_STATUS_TEXT)
            {
                StatusCode = Globals.FAILURE_STATUS_CODE;
                StatusDesc = nullCheckResult;
                return false;
            }

            StatusCode = Globals.SUCCESS_STATUS_CODE;
            StatusDesc = Globals.SUCCESS_STATUS_TEXT;
            return true;
        }

        private Entity GetEntity()
        {
            Entity entity = new Entity();
            entity.CompanyCode = this.CompanyCode;
            entity.CreatedBy = this.CreatedBy;
            entity.CreatedOn = this.CreatedOn;
            entity.ModifiedBy = this.ModifiedBy;
            entity.ModifiedOn = this.ModifiedOn;
            entity.RecordId = this.RecordId;
            entity.StatusCode = this.StatusCode;
            entity.StatusDesc = this.StatusDesc;
            return entity;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LeshLoanMngt.ControlClasses;
using System.Data;
using LeshLoanMngt.Entities;

namespace LeshLoanMngt
{
    public class LeshLoanInterface
    {
        BusinessLogic bll = new BusinessLogic();
        public DataSet ExecuteDataSet(string storedProcName, string[] parameters)
        {
            return bll.ExecuteDataSet(storedProcName, parameters);
        }

        public int ExecuteNonQuery(string storedProcName, string[] parameters)
        {
            return bll.ExecuteNonQuery(storedProcName, parameters);
        }

        public SystemUser Login(string UserId, string Password, string Identifier)
        {
            return bll.Login(UserId, Password, Identifier);
        }

        public Result LogOut(string UserId)
        {
            return bll.LogOut(UserId);
        }
        public Result LogError(string ErrorIdentifier, string StackTrace, string BankCode, string Message, string ErrorType)
        {
            return bll.LogError(ErrorIdentifier, StackTrace, BankCode, Message, ErrorType);
        }

        public Result SaveClientDetails(ClientDetails req)
        {
            Result Res = new Result();
            Res = bll.SaveClientDetails(req);
            return Res;
        }

        public Result SaveAdditionalClientDetails(ClientDetails req)
        {
            Result Res = new Result();
            Res = bll.SaveAdditionalClientDetails(req);
            return Res;
        }

        public Result SaveLoanDetails(LoanDetails req)
        {
            Result Res = new Result();
            Res = bll.SaveLoanDetails(req);
            return Res;
        }
        
        public Result SaveSystemUser(SystemUser req)
        {
            Result Res = new Result();
            Res = bll.SaveSystemUser(req);
            return Res;
        }

        public Result SaveSystemSetting(SystemSetting setting)
        {
            Result Res = new Result();
            Res = bll.SaveSystemSetting(setting);
            return Res;
        }

        public Result SaveReceipt(Receipt rcpt)
        {
            Result Res = new Result();
            Res = bll.SaveReceipt(rcpt);
            return Res;
        }

        public Result SaveInjection(Injection Inj)
        {
            Result Res = new Result();
            Res = bll.SaveInjection(Inj);
            return Res;
        }

        public Result SaveIncome(Income Inco)
        {
            Result Res = new Result();
            Res = bll.SaveIncome(Inco);
            return Res;
        }

        public Result SaveExpense(Expense Exp)
        {
            Result Res = new Result();
            Res = bll.SaveExpense(Exp);
            return Res;
        }



        public ClientDetails[] GetClientDetails(string BankCode, string UserId, string KYCID)
        {
            ClientDetails[] Res = new ClientDetails[0];
            Res = bll.GetClientDetails(BankCode, UserId, KYCID);
            return Res;
        }

        public Result ResetUserCreds(SystemUser req)
        {
            Result Res = new Result();
            Res = bll.ResetUserCreds(req);
            return Res;
        }
}
}

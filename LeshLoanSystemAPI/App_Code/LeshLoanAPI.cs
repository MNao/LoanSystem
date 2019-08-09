using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using LeshLoanMngt;
using LeshLoanMngt.Entities;
using System.Data;
using System.Web.Services.Protocols;

/// <summary>
/// Summary description for LeshLoanAPI
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
// [System.Web.Script.Services.ScriptService]
public class LeshLoanAPI : System.Web.Services.WebService
{

    LeshLoanInterface LeshLoanSystem = new LeshLoanInterface();
    public LeshLoanAPI()
    {
    }

    [WebMethod]
    public DataSet ExecuteDataSet(string storedProcName, string[] parameters)
    {
        DataSet ds = new DataSet();
        try
        {
            ds = LeshLoanSystem.ExecuteDataSet(storedProcName, parameters);
            return ds;
        }
        catch (Exception ex)
        {
            LeshLoanSystem.LogError(storedProcName, ex.StackTrace, storedProcName, ex.Message, "EXCEPTION");
            throw new SoapException(ex.Message, new System.Xml.XmlQualifiedName(ex.Message), ex);
        }
    }

    [WebMethod]
    public int ExecuteNonQuery(string storedProcName, string[] parameters)
    {
        int rowsAffected = 0;
        try
        {
            rowsAffected = LeshLoanSystem.ExecuteNonQuery(storedProcName, parameters);
            return rowsAffected;
        }
        catch (Exception ex)
        {
            LeshLoanSystem.LogError(storedProcName, ex.StackTrace, storedProcName, ex.Message, "EXCEPTION");
            throw new SoapException(ex.Message, new System.Xml.XmlQualifiedName(ex.Message), ex);
        }
    }


    [WebMethod]
    public SystemUser Login(string UserId, string Password, string Identifier)
    {
        SystemUser result = new SystemUser();
        try
        {
            result = LeshLoanSystem.Login(UserId, Password, Identifier);
        }
        catch (Exception ex)
        {
            LeshLoanSystem.LogError(UserId + "-" + Password, ex.StackTrace, UserId, ex.Message, "EXCEPTION");
            result.StatusCode = Globals.HIDE_FAILURE_STATUS_CODE;
            result.StatusDesc = "EXCEPTION: " + ex.Message;
        }
        return result;
    }

    [WebMethod]
    public Result LogOut(string UserId)
    {
        Result result = new Result();
        try
        {
            result = LeshLoanSystem.LogOut(UserId);
        }
        catch (Exception ex)
        {
            LeshLoanSystem.LogError(UserId, ex.StackTrace, UserId, ex.Message, "EXCEPTION");
            result.StatusCode = Globals.HIDE_FAILURE_STATUS_CODE;
            result.StatusDesc = "EXCEPTION: " + ex.Message;
        }
        return result;
    }

    [WebMethod]
    public Result SaveClientDetails(ClientDetails req)
    {
        Result result = new Result();
        try
        {
            result = LeshLoanSystem.SaveClientDetails(req);
        }
        catch (Exception ex)
        {
            LeshLoanSystem.LogError(req.ClientName + "-" + req.ClientPhoneNumber, ex.StackTrace, req.CompanyCode, ex.Message, "EXCEPTION");
            result.StatusCode = Globals.HIDE_FAILURE_STATUS_CODE;
            result.StatusDesc = "EXCEPTION: " + ex.Message;
        }
        return result;
    }

    [WebMethod]
    public Result SaveAdditionalClientDetails(ClientDetails req)
    {
        Result result = new Result();
        try
        {
            result = LeshLoanSystem.SaveAdditionalClientDetails(req);
        }
        catch (Exception ex)
        {
            LeshLoanSystem.LogError(req.ClientName + "-" + req.ClientPhoneNumber, ex.StackTrace, req.CompanyCode, ex.Message, "EXCEPTION");
            result.StatusCode = Globals.HIDE_FAILURE_STATUS_CODE;
            result.StatusDesc = "EXCEPTION: " + ex.Message;
        }
        return result;
    }
    

    [WebMethod]
    public Result SaveSystemUser(SystemUser user)
    {
        Result result = new Result();
        try
        {
            result = LeshLoanSystem.SaveSystemUser(user);
        }
        catch (Exception ex)
        {
            LeshLoanSystem.LogError(user.UserId + "-" + user.Password, ex.StackTrace, user.UserId, ex.Message, "EXCEPTION");
            result.StatusCode = Globals.HIDE_FAILURE_STATUS_CODE;
            result.StatusDesc = "EXCEPTION: " + ex.Message;
        }
        return result;
    }

    [WebMethod]
    public Result SaveSystemSetting(SystemSetting req)
    {
        Result result = new Result();
        try
        {
            result = LeshLoanSystem.SaveSystemSetting(req);
        }
        catch (Exception ex)
        {
            LeshLoanSystem.LogError(req.SettingCode, ex.StackTrace, req.CompanyCode, ex.Message, "EXCEPTION");
            result.StatusCode = Globals.HIDE_FAILURE_STATUS_CODE;
            result.StatusDesc = "EXCEPTION: " + ex.Message;
        }
        return result;
    }

    [WebMethod]
    public Result SaveLoanDetails(LoanDetails req)
    {
        Result result = new Result();
        try
        {
            result = LeshLoanSystem.SaveLoanDetails(req);
        }
        catch (Exception ex)
        {
            LeshLoanSystem.LogError(req.LoanNo + "-" + req.LoanDesc, ex.StackTrace, req.CompanyCode, ex.Message, "EXCEPTION");
            result.StatusCode = Globals.HIDE_FAILURE_STATUS_CODE;
            result.StatusDesc = "EXCEPTION: " + ex.Message;
        }
        return result;
    }

    [WebMethod]
    public ClientDetails[] GetClientDetails(string BankCode, string UserId, string ClientID)
    {
        ClientDetails[] result = new ClientDetails[0];
        ClientDetails Res = new ClientDetails();
        try
        {
            result = LeshLoanSystem.GetClientDetails(BankCode, UserId, ClientID);
        }
        catch (Exception ex)
        {
            LeshLoanSystem.LogError(BankCode, ex.StackTrace, UserId, ex.Message, "EXCEPTION");
            Res.StatusCode = Globals.HIDE_FAILURE_STATUS_CODE;
            Res.StatusDesc = "EXCEPTION: " + ex.Message;
            return result;
        }
        return result;
    }

    [WebMethod]
    public Result ResetPassword(SystemUser Reset)
    {
        Result Res = new Result();
        try
        {
            Res = LeshLoanSystem.ResetUserCreds(Reset);
        }
        catch (Exception ex)
        {
            LeshLoanSystem.LogError(Reset.CompanyCode, ex.StackTrace, Reset.UserId, ex.Message, "EXCEPTION");
            Res.StatusCode = Globals.HIDE_FAILURE_STATUS_CODE;
            Res.StatusDesc = "EXCEPTION: " + ex.Message;
            return Res;
        }
        return Res;
    }
    //[WebMethod]
    //public List<KYCDetails> GetKYCDetails(string BankCode, string UserId, string KYCID)
    //{
    //    List<KYCDetails> result = new List<KYCDetails>();
    //    KYCDetails Res = new KYCDetails();
    //    try
    //    {
    //        result = UBASystem.GetKYCDetails(BankCode, UserId, KYCID);
    //    }
    //    catch (Exception ex)
    //    {
    //        UBASystem.LogError(BankCode, ex.StackTrace, UserId, ex.Message, "EXCEPTION");
    //        Res.StatusCode = Globals.HIDE_FAILURE_STATUS_CODE;
    //        Res.StatusDesc = "EXCEPTION: " + ex.Message;

    //        result.Add(Res);
    //        return result;
    //    }
    //    return result;
    //}

    [WebMethod]
    public Result SaveReceipt(Receipt recp)
    {
        Result result = new Result();
        try
        {
            result = LeshLoanSystem.SaveReceipt(recp);
        }
        catch (Exception ex)
        {
            LeshLoanSystem.LogError(recp.ReceiptNumber + "-" + recp.CompanyCode, ex.StackTrace, recp.LoanNumber, ex.Message, "EXCEPTION");
            result.StatusCode = Globals.HIDE_FAILURE_STATUS_CODE;
            result.StatusDesc = "EXCEPTION: " + ex.Message;
        }
        return result;
    }
}

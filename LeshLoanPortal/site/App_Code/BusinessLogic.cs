using System;
using System.Web.UI;
using System.Data;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using System.Drawing;
using System.IO;
using System.Web;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using System.Collections;
using System.Net.Mail;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
using System.Text.RegularExpressions;
using InterConnect.LeshLaonApi;
using System.Text;
using System.Data.SqlClient;
using InterConnect;

/// <summary>
/// Summary description for BusinessLogic
/// </summary>
public class BusinessLogic
{
	public BusinessLogic()
	{
	}

    //ServicePointManager.ServerCertificateValidationCallback = RemoteCertValidation;
    LeshLoanAPI Client = new LeshLoanAPI();

    public void ShowMessage(Label lblmsg, string msg, bool IsError, System.Web.SessionState.HttpSessionState Session)
    {
        lblmsg.Text = msg;
        if (IsError)
        {
            Session["IsError"] = "True";
            lblmsg.ForeColor = Color.Red;
            lblmsg.Font.Bold = true;
        }
        else
        {
            Session["IsError"] = "False";
            lblmsg.ForeColor = Color.Green;
            lblmsg.Font.Bold = true;
        }
    }

    public string[] GetStatsToDisplay(SystemUser user)
    {
        List<string> Details = new List<string>();
        try
        {
            DataTable datatable = Client.ExecuteDataSet("GetStatsToDisplay",
                                                           new string[]
                                                           {
                                                             user.CompanyCode,
                                                           }).Tables[0];
            string Pending = datatable.Rows[0][0].ToString();
            string Verified = datatable.Rows[0][1].ToString();
            string Approved = datatable.Rows[0][2].ToString();
            string Rejected = datatable.Rows[0][3].ToString();
            Details.Add(Pending);
            Details.Add(Verified);
            Details.Add(Approved);
            Details.Add(Rejected);
            return Details.ToArray();
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public Result SaveInterestSetting(SystemUser user,SystemSetting setting)
    {
        Result res = new Result();
        string[] Params = { user.CompanyCode, setting.SettingName, setting.SettingCode, setting.SettingValue, user.UserId };
        DataTable dt= Client.ExecuteDataSet("SaveInterestSetting", Params).Tables[0];
        if (dt.Rows.Count < 0)
        {
            res.StatusCode = Globals.FAILURE_STATUS_CODE;
            res.StatusDesc = "SAVE INTEREST SETTING FAILED";
            return res;
        }
        res.StatusCode = Globals.SUCCESS_STATUS_CODE;
        res.StatusDesc = Globals.SUCCESS_STATUS_CODE;
        return res;
    }

    public void ShowExternalMessage(Label lblmsg, System.Web.SessionState.HttpSessionState Session)
    {
        //no external Msg
        if (Session["ExternalMsg"] == null)
        {
            return;
        }

        lblmsg.Text = Session["ExternalMsg"] as string;
        bool IsError = false;

        if (IsError)
        {
            Session["IsError"] = "True";
            lblmsg.ForeColor = Color.Red;
        }
        else
        {
            Session["IsError"] = "False";
            lblmsg.ForeColor = Color.Green;
        }

        //make this null since it has been read / consumed / acted upon
        Session["ExternalMsg"] = null;
    }

    public void ShowMessage(Label lblmsg, string msg, bool IsError)
    {
        lblmsg.Text = msg;
        if (IsError)
        {
            lblmsg.ForeColor = Color.Red;
        }
        else
        {
            lblmsg.ForeColor = Color.Green;
        }
    }

    public string InsertIntoAuditLog(string ActionType, string TableName, string CompanyCode, string ModifiedBy, string Action)
    {
        try
        {
            DataTable datatable = Client.ExecuteDataSet("InsertIntoAuditTrail",
                                                           new string[]
                                                           {
                                                             ActionType,
                                                             TableName,
                                                             CompanyCode,
                                                             ModifiedBy,
                                                             Action
                                                           }).Tables[0];
            return datatable.Rows[0][0].ToString();
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }


    public Result SendCredentialsToUser(SystemUser user, string Password)
    {       
        string UserType = "";
        if (user.RoleCode=="001")
        {
            UserType = "Company Administrator";
        }
        else if (user.RoleCode == "002")
        {
            UserType = "Approver";
        }
        else if (user.RoleCode == "003")
        {
            UserType = "Initiator";
        }
        else if (user.RoleCode == "004")
        {
            UserType = "Client";
        }
        else
        {
            UserType = "System Role UnKnown";// "General System Administrator";
        }

        Result result = new Result();
        try
        {
            //http://192.168.23.15:5099/MailApi/Messenger.asmx?WSDL
            InterConnect.MailApi.Messenger mailApi = new InterConnect.MailApi.Messenger();
            InterConnect.MailApi.Email email = new InterConnect.MailApi.Email();
            email.From = "lensh.finance@gmail.com";
            email.Subject = "LENSH LOAN SYSTEM USER CREDENTIALS";
            email.Message = "Hi " + user.Name + "<br/>" +
                            "Your Credentials for The LENSH LOAN System are Below<br/>" +
                            "UserId: " + user.UserId + "<br/>" +
                            "Password: " + Password + "<br/>" +
                            "Role: " + UserType + "<br/>" +
                            "Thank you. <br/>";
            InterConnect.MailApi.EmailAddress address = new InterConnect.MailApi.EmailAddress();
            address.Address = user.Email;
            address.AddressType = InterConnect.MailApi.EmailAddressType.To;
            address.Name = user.Name;

            email.MailAddresses = new InterConnect.MailApi.EmailAddress[] { address };
            InterConnect.MailApi.Result resp = mailApi.PostEmail(email);
            result.StatusCode = resp.StatusCode;
            result.StatusDesc = resp.StatusDesc;
        }
        catch (Exception ex)
        {
            result.StatusCode = Globals.FAILURE_STATUS_CODE;
            result.StatusDesc = "EXCEPTION: " + ex.Message;
        }
        return result;
    }

    public Result SendCredentialsToClientUser(ClientDetails user, string Password)
    {
        string UserType = "Client";

        Result result = new Result();
        try
        {   
            //smtp creds
            int smtpPort = 587;
            string smtpServer = "smtp.gmail.com";
            const string smtpPassword = "0701081899";
            const string smtpUsername = "timothykasaga@gmail.com";
            //email.From = "lensh.finance@gmail.com";

            //BUILD EMAIL
            MailMessage message = new MailMessage();
            message.To.Clear();
            message.To.Add(user.ClientEmail);
            message.Subject = "LENSH LOAN SYSTEM USER CREDENTIALS";
            message.Body = "Hi " + user.ClientName + "<br/>" +
                            "Your Credentials for The LENSH LOAN System are Below<br/>" +
                            "UserId: " + user.ClientNo + "<br/>" +
                            "Password: " + Password + "<br/>" +
                            "Role: " + UserType + "<br/>" +
                            "Thank you. <br/>";
            message.IsBodyHtml = true;
            message.From = new MailAddress(smtpUsername);

            NetworkCredential cred = new NetworkCredential(smtpUsername, smtpPassword);
            SmtpClient mailClient = new SmtpClient(smtpServer, smtpPort);
            mailClient.EnableSsl = true;
            mailClient.DeliveryMethod = SmtpDeliveryMethod.Network;
            mailClient.UseDefaultCredentials = false;
            mailClient.Timeout = 450000;
            mailClient.Credentials = cred;

            //SEND EMAIL
            mailClient.Send(message);


            result.StatusCode = Globals.SUCCESS_STATUS_CODE;
            result.StatusDesc = "Email Sent Successfully";
            return result;
            
        }
        catch (Exception ex)
        {
            result.StatusCode = Globals.FAILURE_STATUS_CODE;
            result.StatusDesc = "EXCEPTION: " + ex.Message;
        }
        return result;
    }

    public Result SendNotification(string[] user, string Status, string Reason)
    {
        Result result = new Result();
        try
        {
            //http://192.168.23.15:5099/MailApi/Messenger.asmx?WSDL
            InterConnect.MailApi.Messenger mailApi = new InterConnect.MailApi.Messenger();
            InterConnect.MailApi.Email email = new InterConnect.MailApi.Email();
            email.From = "notifications@pegasustechnologies.co.ug";
            email.Subject = "UBA KYC Approval";

            if (Status == "Approved")
            {
                email.Message = "Hi "+user[1]+ "<br/>" +
                            "You have a KYC Customer to be " + Status + "<br/>" +
                            "Kindly Login and Approve it"+ "<br/>" +
                            "URL: https://test.pegasus.co.ug:8019/TestUBAKYCPortal/" + "<br/>" +
                            "Thank you." + "<br/>";
            }
            else
            {
                email.Message = "Hi "+user[1]+ " < br/>" +
                            "Your KYC Customer has been " + Status + "<br/>" +
                            "With Reason: " + Reason + "<br/>" +
                            "Kindly Login again and Rectify it <br/>" +
                            "Thank you. <br/>";
            }
            InterConnect.MailApi.EmailAddress address = new InterConnect.MailApi.EmailAddress();
            address.Address = user[0];
            address.AddressType = InterConnect.MailApi.EmailAddressType.To;
            address.Name = user[1];

            email.MailAddresses = new InterConnect.MailApi.EmailAddress[] { address };
            InterConnect.MailApi.Result resp = mailApi.PostEmail(email);
            result.StatusCode = resp.StatusCode;
            result.StatusDesc = resp.StatusDesc;
        }
        catch (Exception ex)
        {
            result.StatusCode = Globals.FAILURE_STATUS_CODE;
            result.StatusDesc = "EXCEPTION: " + ex.Message;
        }
        return result;
    }

    public string URL = "http://pegasus.co.ug:8021/SMSGATEWAY/";
    public void SendSMSNotificationtoUser(string PhoneNumber, string Message, string VendorTranID)
    {

        //The SMS Gateway API expects a post request,
        //So below we generate the post request data

        string request = "Phone=" + PhoneNumber + "&" +
                     "Message=" + Message + "&" +
                     "Sender=PEGASUS&" +
                     "Password=Tingate710&" +
                     "DigitalSignature=NO DIGITAL SIGNATURE NEEDED FOR THIS ACCOUNT&" +
                     "VendorTranId=" + VendorTranID + "&" +
                     "Mask=PEGPAY";

        //Send the post request
        string result = SendHttpPost(URL, request, "application/x-www-form-urlencoded");
        //"StatusCode=100&Reference=&StatusDescription=Unknown Sender id supplied"
        string[] response = result.Split('=');
        string StatusDescription = response[3];

        if (StatusDescription.Equals("SUCCESS"))
        {
        }
    }

public string SendHttpPost(string url, string data, string contentType)
{
    string method = "POST";
    byte[] dataBytes = System.Text.Encoding.UTF8.GetBytes(data);

    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
    request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
    request.ContentLength = dataBytes.Length;
    request.ContentType = contentType;
    request.Method = method;

    using (Stream requestBody = request.GetRequestStream())
    {
        requestBody.Write(dataBytes, 0, dataBytes.Length);
    }

    using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
    using (Stream stream = response.GetResponseStream())
    using (StreamReader reader = new StreamReader(stream))
    {
        return reader.ReadToEnd();
    }
}
public string SendMailMessageWithAttachment(SystemUser user)
    {

        const string smtpServer = "192.185.83.129";//"64.233.167.108";
        const int smtpPort = 587;
        const string smtpPassword = "T3rr1613";
        const string smtpUsername = "notifications@pegasustechnologies.co.ug";

        //int smtpport = 587;
        //string smtpserver = "smtp.gmail.com";
        //const string smtppassword = "0701081899";
        //const string smtpusername = "timothykasaga@gmail.com";

        //BUILD EMAIL
        MailMessage message = new MailMessage();
        message.To.Clear();
        //foreach (string email in toEmails)
        //{
        //    message.To.Add(new MailAddress(email));
        //}
        //foreach (string email in ccEmails)
        //{
        //    message.CC.Add(new MailAddress(email));
        //}
        message.To.Add(user.Email);
        message.Subject = "PEGASUS FINANCE MANAGEMENT SYSTEM USER CREDENTIALS";
        message.Body = "Hi " + user.UserId + "<br/>" +
                            "Your Credentials for The Pegasus Bussiness Management Portal are Below<br/>" +
                            "UserId: " + user.UserId + "<br/>" +
                            "Password: " + user.Password + "<br/>" +
                            "Role: " + user.RoleCode + "<br/>" +
                            "Thank you. <br/>";
        message.IsBodyHtml = true;
        message.From = new MailAddress(smtpUsername);

        NetworkCredential cred = new NetworkCredential(smtpUsername, smtpPassword);
        SmtpClient mailClient = new SmtpClient(smtpServer, smtpPort);
        mailClient.EnableSsl = true;
        mailClient.DeliveryMethod = SmtpDeliveryMethod.Network;
        mailClient.UseDefaultCredentials = false;
        mailClient.Timeout = 450000;
        mailClient.Credentials = cred;

        //SEND EMAIL
        mailClient.Send(message);

        return "Email Sent Successfully";
    }


    public string[] GetUserEmail(SystemUser user, string RoleCode)
    {
        List<string> Details = new List<string>();
        try
        {
            DataTable datatable = Client.ExecuteDataSet("GetUserEmail",
                                                           new string[]
                                                           {
                                                             user.CompanyCode,
                                                             RoleCode
                                                           }).Tables[0];
          string  Email = datatable.Rows[0][0].ToString();
          string Username = datatable.Rows[0][1].ToString();
            Details.Add(Email);
            Details.Add(Username); 
            return Details.ToArray();
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    private static bool RemoteCertificateValidation(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
    {
        return true;
    }
    public bool ObeysPasswordPolicy(string newPassword, string companyCode)
    {
        string policy = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,15}$";
        return Regex.IsMatch(newPassword, policy);
    }

    public string getIp()
    {
        string _custIP = null;
        try
        {

            _custIP = HttpContext.Current.Request.ServerVariables["HTTP_Client_IP"];

            if (String.IsNullOrEmpty(_custIP))
            {
                _custIP = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

            }
            if (String.IsNullOrEmpty(_custIP))
            {
                _custIP = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            }
        }
        catch (Exception ex)
        {
            _custIP = "";
        }
        return _custIP;

    }

    public Result SaveLoanApproval(string LoanNo, string approved, string approvedAmount, string modifiedBy, string monthsToPayIn, string easyPaidAmountPerMonth, string GuarantorImageApprov)
    {
        Result result = new Result();
        try
        {
            //InsertIntoAuditLog("PASSWORD CHANGE", "SYSTEMUSERS", CompanyCode, Id, "Changed Password of " + Id);
            DataTable datatable = Client.ExecuteDataSet("SaveLoanApproval",
                                                           new string[]
                                                           {    
                                                            LoanNo,
                                                             approved,
                                                             approvedAmount,
                                                             monthsToPayIn,
                                                             easyPaidAmountPerMonth,
                                                             GuarantorImageApprov,
                                                             modifiedBy
                                                           }).Tables[0];
            //return datatable.Rows[0].ToString();

            if (datatable.Rows.Count == 0)
            {
                result.StatusCode = Globals.FAILURE_STATUS_CODE;
                result.StatusDesc = "LOAN APPROVAL FOR [" + LoanNo + "] FAILED";
                return result;
            }

            DataRow dr = datatable.Rows[0];
            result.LoanID = dr["InsertedID"].ToString();
            result.StatusCode = Globals.SUCCESS_STATUS_CODE;
            result.StatusDesc = "SUCCESS";
           
        }
        catch (Exception ex)
        {

        }
        return result;
    }

    public Result SaveCollateralDetails(string LoanNo, string Name, string Type, string Model, string Make, string SerialNo, string EstPrice, string ImgProof, string Obsvs, string AddedBy)
    {
        Result result = new Result();
        try
        {
            //InsertIntoAuditLog("PASSWORD CHANGE", "SYSTEMUSERS", CompanyCode, Id, "Changed Password of " + Id);
            DataTable datatable = Client.ExecuteDataSet("SaveCollateralDetails",
                                                           new string[]
                                                           {
                                                            LoanNo,
                                                             Name,
                                                             Type,
                                                             Model,
                                                             Make,
                                                             SerialNo,
                                                             EstPrice,
                                                             ImgProof,
                                                             Obsvs,
                                                             AddedBy
                                                           }).Tables[0];
            //return datatable.Rows[0].ToString();

            if (datatable.Rows.Count == 0)
            {
                result.StatusCode = Globals.FAILURE_STATUS_CODE;
                result.StatusDesc = "COLLATERAL DETAILS FOR [" + LoanNo + "] FAILED TO SAVE";
                return result;
            }

            DataRow dr = datatable.Rows[0];
            result.LoanID = dr["InsertedID"].ToString();
            result.StatusCode = Globals.SUCCESS_STATUS_CODE;
            result.StatusDesc = "SUCCESS";

        }
        catch (Exception ex)
        {

        }
        return result;
    }
    
    public Result SavePaymentDetails(LoanDetails Payment)
    {
        Result result = new Result();
        try
        {
            DataTable datatable = Client.ExecuteDataSet("SaveLoanPaymentDetails",
                                                           new string[]
                                                           {
                                                            Payment.ClientID,
                                                            Payment.LoanNo,
                                                             Payment.LoanAmount,
                                                             Payment.LoanDate,
                                                             Payment.Observations,
                                                             Payment.CreatedBy
                                                           }).Tables[0];

            if (datatable.Rows.Count == 0)
            {
                result.StatusCode = Globals.FAILURE_STATUS_CODE;
                result.StatusDesc = "Loan Payment DETAILS FOR [" + Payment.LoanNo + "] FAILED TO SAVE";
                return result;
            }

            DataRow dr = datatable.Rows[0];
            result.LoanID = dr["InsertedID"].ToString();
            result.StatusCode = Globals.SUCCESS_STATUS_CODE;
            result.StatusDesc = "SUCCESS";

        }
        catch(SqlException ex)
        {
            //to seach about
            if (ex.ErrorCode==16)
            {

            }
        }
        catch (Exception ex)
        {
            //throw;
            result.StatusCode = Globals.FAILURE_STATUS_CODE;
            result.StatusDesc = ex.Message;
        }
        return result;
    }

    public string GetImageUploadedInBase64String(FileUpload fuInvoiceImage)
    {
        if (fuInvoiceImage.HasFile)
        {
            string fileName = fuInvoiceImage.FileName.ToUpper();
            string extension = Path.GetExtension(fileName);
            if (InterConnect.SharedCommons.AllowedImageExtensions.Contains(extension.ToUpper()))
            {
                System.IO.Stream fs = fuInvoiceImage.PostedFile.InputStream;
                System.IO.BinaryReader br = new System.IO.BinaryReader(fs);
                Byte[] bytes = br.ReadBytes((Int32)fs.Length);
                //return bytes;
                string base64String = Convert.ToBase64String(bytes, 0, bytes.Length);
                base64String = "data:image/" + extension.ToLower().Replace(".", string.Empty) + ";base64," + base64String;
                return base64String;
            }
            else
            {
                return null;
            }
        }
        return null;
    }

    //public InterConnect.PegasusManagementApi.Result ChangeUsersPassword(string id, string companyCode, string password, string usertype, bool v1, string v2)
    //{
    //    throw new NotImplementedException();
    //}

    public Result ChangeUsersPassword(string Id, string CompanyCode, String Password, string RoleCode)
    {
        Result result = new Result();
        try
        {
            InsertIntoAuditLog("PASSWORD CHANGE", "SYSTEMUSERS", CompanyCode, Id, "Changed Password of " + Id);
            DataTable datatable = Client.ExecuteDataSet("ChangePassword",
                                                           new string[]
                                                           {
                                                             Id,
                                                             CompanyCode,
                                                             Password,
                                                             RoleCode
                                                           }).Tables[0];
            //return datatable.Rows[0].ToString();

            if (datatable.Rows.Count == 0)
            {
                result.StatusCode = Globals.FAILURE_STATUS_CODE;
                result.StatusDesc = "USER WITH ID [" + Id + "] NOT FOUND";
                return result;
            }

            DataRow dr = datatable.Rows[0];
            result.LoanID = dr["UserTypeCode"].ToString();

            if (!(result.StatusCode != Globals.FAILURE_STATUS_CODE))
            {
                result.StatusDesc = "USER WITH ID [" + Id + "] IS DEACTIVATED";
                return result;
            }

            result.StatusCode = Globals.SUCCESS_STATUS_CODE;
            result.StatusDesc = Globals.SUCCESS_STATUS_TEXT;
            return result;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    //public UserType GetUserTypeById(string CompanyCode, string userType)
    //{
    //    UserType user = new UserType();
    //    UBAKYCAPI mSystem = new UBAKYCAPI();
    //    DataTable dt = mSystem.ExecuteDataSet("GetUserTypeByUserTypeCode", new string[] { CompanyCode, userType }).Tables[0];


    //    if (dt.Rows.Count == 0)
    //    {
    //        user.StatusCode = Globals.FAILURE_STATUS_CODE;
    //        user.StatusDesc = "USER TYPE WITH ID [" + userType + "] NOT FOUND";
    //        return user;
    //    }

    //    DataRow dr = dt.Rows[0];
    //    user.UserTypeCode = dr["UserTypeCode"].ToString();
    //    user.UserTypeName = dr["UserTypeName"].ToString();
    //    user.IsActive = dr["IsActive"].ToString();
    //    user.ModifiedBy = dr["ModifiedBy"].ToString();
    //    user.CreatedBy = dr["CreatedBy"].ToString();
    //    user.CompanyCode = dr["CompanyCode"].ToString();

    //    if (user.IsActive.ToUpper() != "TRUE")
    //    {
    //        user.StatusCode = Globals.FAILURE_STATUS_CODE;
    //        user.StatusDesc = "USERTYPES WITH ID [" + userType + "] ARE DEACTIVATED. ie Role Has Been Deactived. PLEASE CONTACT SYSTEM ADMINISTRATORS";
    //        return user;
    //    }

    //    user.StatusCode = Globals.SUCCESS_STATUS_CODE;
    //    user.StatusDesc = Globals.SUCCESS_STATUS_TEXT;
    //    return user;
    //}

    public void LoadCompanysIntoDropDownALL(SystemUser user, DropDownList ddlst)
    {
        string[] parameters = { };
        DataTable dt = Client.ExecuteDataSet("GetCompaniesForDropDown", parameters).Tables[0];

        ddlst.Items.Clear();
        ddlst.Items.Add(new ListItem("-- ALL --", ""));
        foreach (DataRow dr in dt.Rows)
        {
            string Text = dr["CompanyCode"].ToString();
            string Value = dr["CompanyName"].ToString();
            ddlst.Items.Add(new ListItem(Value, Text));
        }

        if (user.RoleCode != "005")
        {
            ddlst.SelectedValue = user.CompanyCode;
            ddlst.Enabled = false;
        }
    }

    public void LoadAgentsIntopDropDown(SystemUser user, DropDownList ddlst)
    {
        string[] parameters = { user.CompanyCode};
        DataTable dt = Client.ExecuteDataSet("GetAgentsForDropDown", parameters).Tables[0];

        ddlst.Items.Clear();
        ddlst.Items.Add(new ListItem("-- ALL --", ""));
        foreach (DataRow dr in dt.Rows)
        {
            string Text = dr["UserId"].ToString();
            string Value = dr["Name"].ToString();
            ddlst.Items.Add(new ListItem(Value, Text));
        }
    }
    

    public void loadCompanyColorsInToDropDown(SystemUser user, DropDownList ddlst)
    {
        string[] parameters = { };
        DataTable dt = Client.ExecuteDataSet("GetColorsForDropDown", parameters).Tables[0];

        ddlst.Items.Clear();
        ddlst.Items.Add(new ListItem("-- Select Color --", ""));
        foreach (DataRow dr in dt.Rows)
        {
            string Text = dr["ColorName"].ToString();
            string Value = dr["ColorCode"].ToString();
            ddlst.Items.Add(new ListItem(Value, Text));
        }
    }
    public void GetCEOSforApproval(string CompanyCode, SystemUser user, DropDownList ddlst)
    {
        string[] parameters = { CompanyCode };
        DataTable dt = Client.ExecuteDataSet("GetCEOSforApproval", parameters).Tables[0];

        ddlst.Items.Clear();
        ddlst.Items.Add(new ListItem("--Select CEO/Director--", ""));
        foreach (DataRow dr in dt.Rows)
        {
            string Text = dr["Name"].ToString();
            string Value = dr["UserId"].ToString();
            ddlst.Items.Add(new ListItem(Text, Value));
        }
    }

    public void LoadCompanysInToDropDown(string CompanyCode, SystemUser user, DropDownList ddlst)
    {
        string[] parameters = { CompanyCode };
        DataTable dt = Client.ExecuteDataSet("GetCompanysForDropDown", parameters).Tables[0];

        ddlst.Items.Clear();
        ddlst.Items.Add(new ListItem("--Select Company--", ""));
        foreach (DataRow dr in dt.Rows)
        {
            string Text = dr["CompanyName"].ToString();
            string Value = dr["CompanyName"].ToString();
            ddlst.Items.Add(new ListItem(Text, Value));
        }
    }


    public void LoadCompanysAttachedToCompanyInToDropDown(string CompanyCode, SystemUser user, DropDownList ddlst)
    {
        string[] parameters = { CompanyCode };
        DataTable dt = Client.ExecuteDataSet("GetCompanysAttachedToCompanyForDropDown", parameters).Tables[0];

        ddlst.Items.Clear();
        ddlst.Items.Add(new ListItem("--Select Company--", ""));
        foreach (DataRow dr in dt.Rows)
        {
            string Text = dr["CompanyName"].ToString();
            string Value = dr["CompanyName"].ToString();
            ddlst.Items.Add(new ListItem(Text, Value));
        }
    }
    public void LoadCompanyAccountsIntoDropDown(string CompanyCode, SystemUser user, string CompanyName, string Currency, DropDownList ddlst)
    {
        string[] parameters = { CompanyCode, CompanyName, Currency };
        DataTable dt = Client.ExecuteDataSet("GetCompanyAccountsForDropDown", parameters).Tables[0];

        ddlst.Items.Clear();
        ddlst.Items.Add(new ListItem("--Select Company Account--", ""));
        foreach (DataRow dr in dt.Rows)
        {
            string Text = dr["AccountNumber"].ToString() + "-" + dr["AccountType"].ToString();
            string Value = dr["AccountNumber"].ToString();
            ddlst.Items.Add(new ListItem(Text, Value));
        }
    }
    public void LoadEmplooyesIntoDropDown(SystemUser user, string CompanyCode, DropDownList ddlst)
    {
        string[] parameters = { CompanyCode };
        DataTable dt = Client.ExecuteDataSet("GetEmployeesForDropDown", parameters).Tables[0];

        ddlst.Items.Clear();
        foreach (DataRow dr in dt.Rows)
        {
            string Text = dr["SupplierName"].ToString();
            string Value = dr["SupplierCode"].ToString();
            ddlst.Items.Add(new ListItem(Text, Value));
        }

        if (user.RoleCode != "001" && user.RoleCode != "ACCOUNTANT")
        {
            ddlst.SelectedValue = user.CompanyCode;
            ddlst.Enabled = false;
        }
    }

    public void LoadFloatCategoriesIntoDropDown(string CompanyCode, SystemUser user, DropDownList ddlst)
    {
        string[] parameters = { CompanyCode };
        DataTable dt = Client.ExecuteDataSet("GetFloatCategoriesForDropDown", parameters).Tables[0];

        ddlst.Items.Clear();
        ddlst.Items.Add(new ListItem("--Select Float Category--", ""));
        foreach (DataRow dr in dt.Rows)
        {
            string Text = dr["FloatCategoryName"].ToString();
            string Value = dr["FloatCategoryCode"].ToString();
            ddlst.Items.Add(new ListItem(Text, Value));
        }
    }

    public void LoadInvoiceFrequencies(DropDownList ddlst)
    {
        ddlst.Items.Clear();
        ddlst.Items.Add(new ListItem("--Select Invoice Frequency--", ""));
        ddlst.Items.Add(new ListItem("MONTHLY", "MONTHLY"));
        ddlst.Items.Add(new ListItem("QUARTERLY", "QUARTER"));
        ddlst.Items.Add(new ListItem("BI-ANNUAL", "HALF"));
        ddlst.Items.Add(new ListItem("ANNUALLY", "YEAR"));
        ddlst.Items.Add(new ListItem("ONE-OFF", "ONE0FF"));
    }

    public void LoadContractTypesIntoDropDown(string CompanyCode, SystemUser user, DropDownList ddlst)
    {
        string[] parameters = { CompanyCode };
        //DataSet ds = Client.ExecuteDataSet("GetContractTypesForDropDown", parameters);
        //DataTable dt = ds.Tables[0];
        DataTable dt = Client.ExecuteDataSet("GetContractTypesForDropDown", parameters).Tables[0];

        ddlst.Items.Clear();
        foreach (DataRow dr in dt.Rows)
        {
            string Text = dr["ContractTypeName"].ToString();
            string Value = dr["ContractTypeCode"].ToString();
            ddlst.Items.Add(new ListItem(Text, Value));
        }
    }

    public void LoadUnpaidInvoicesIntoDropDown(string CompanyCode, SystemUser user, DropDownList ddlst)
    {
        string[] parameters = { CompanyCode };
        //DataSet ds = Client.ExecuteDataSet("GetUnpaidInvoicesForDropDown", parameters);
        //DataTable dt = ds.Tables[0];
        DataTable dt = Client.ExecuteDataSet("GetUnpaidInvoicesForDropDown", parameters).Tables[0];

        ddlst.Items.Clear();
        foreach (DataRow dr in dt.Rows)
        {
            string Value = dr["InvoiceNumber"].ToString();
            string Text = Value + " for " + dr["ClientCode"].ToString();
            ddlst.Items.Add(new ListItem(Text, Value));
        }
    }

    public void LoadUnpaidSupplierPurchasesIntoDropDown(string CompanyCode, SystemUser user, DropDownList ddlst)
    {
        string[] parameters = { CompanyCode };
        //DataSet ds = Client.ExecuteDataSet("GetUnpaidPurchasesForDropDown", parameters);
        //DataTable dt = ds.Tables[0];
        DataTable dt = Client.ExecuteDataSet("GetUnpaidPurchasesForDropDown", parameters).Tables[0];

        ddlst.Items.Clear();
        foreach (DataRow dr in dt.Rows)
        {
            string Value = dr["InvoiceNumber"].ToString();
            string Text = Value + " for " + dr["SupplierCode"].ToString();
            ddlst.Items.Add(new ListItem(Text, Value));
        }
    }

    public void LoadUnpaidPaymentVouchersIntoDropDown(string CompanyCode, SystemUser user, DropDownList ddlst)
    {
        string[] parameters = { CompanyCode };
        //DataSet ds = Client.ExecuteDataSet("GetUnpaidPurchasesForDropDown", parameters);
        //DataTable dt = ds.Tables[0];
        DataTable dt = Client.ExecuteDataSet("GetUnpaidPaymentVouchersForDropDown", parameters).Tables[0];

        ddlst.Items.Clear();
        foreach (DataRow dr in dt.Rows)
        {
            string Value = dr["InvoiceNumber"].ToString();
            string Text = Value + " for " + dr["VoucherCode"].ToString();
            ddlst.Items.Add(new ListItem(Text, Value));
        }
    }

    public void LoadUnpaidInvoicesForSupplierIntoDropDown(string CompanyCode, SystemUser user, DropDownList ddlst, string SupplierCode)
    {
        string[] parameters = { CompanyCode, SupplierCode };
        //DataSet ds = Client.ExecuteDataSet("GetUnpaidInvoicesOfSupplierForDropDown", parameters);
        //DataTable dt = ds.Tables[0];
        DataTable dt = Client.ExecuteDataSet("GetUnpaidInvoicesOfSupplierForDropDown", parameters).Tables[0];

        ddlst.Items.Clear();
        foreach (DataRow dr in dt.Rows)
        {
            //if(dr["IsEmployee"].Equals(1))
            //{
            //    string Value = dr["EmployeeID"].ToString();
            //    string Text = Value + " for " + dr["ClientCode"].ToString();
            //    ddlst.Items.Add(new ListItem(Text, Value));
            //}
            //else
            {
                string Value = dr["InvoiceNumber"].ToString();
                string Text = Value + " for " + dr["ClientCode"].ToString();
                ddlst.Items.Add(new ListItem(Text, Value));
            }

        }
    }

    public string LoadDataSpecificForSupplierDetailsIntoDropDown(string CompanyCode, SystemUser user, string supplierCode)
    {
        string[] parameters = { CompanyCode, supplierCode };
        //DataSet ds = Client.ExecuteDataSet("GetSupplierPurchaseDetailsForDropDown", parameters);
        //DataTable dt = ds.Tables[0];
        DataTable dt = Client.ExecuteDataSet("GetSupplierPurchaseDetailsForDropDown", parameters).Tables[0];

        string Value;
        DataRow dr = dt.Rows[0];
        if (Convert.ToString(dr["SupplierCode"]) == "PEGEMP")
        {
            string Text = dr["SupplierCode"].ToString() + dr["EmployeeDepartment"].ToString() + dr["EmployeeID"].ToString();
            Value = "INV-" + dr["SupplierCode"].ToString() + dr["EmployeeID"].ToString() + dr["EmployeeDepartment"].ToString();
        }
        else
        {
            Value = "Enter Supplier's Invoice Number";
        }
        return Value;

    }
    public void LoadSuppliersIntoDropDown(string CompanyCode, SystemUser user, DropDownList ddlst)
    {
        string[] parameters = { CompanyCode };
        //DataSet ds = Client.ExecuteDataSet("GetSuppliersForDropDown", parameters);
        //DataTable dt = ds.Tables[0];
        DataTable dt = Client.ExecuteDataSet("GetSuppliersForDropDown", parameters).Tables[0];

        ddlst.Items.Clear();
        ddlst.Items.Add(new ListItem("--Select Supplier--", ""));
        foreach (DataRow dr in dt.Rows)
        {
            string Text = dr["SupplierName"].ToString();
            string Value = dr["SupplierCode"].ToString();
            ddlst.Items.Add(new ListItem(Text, Value));
        }
        ddlst.Items.Add(new ListItem("Other", "Other"));
    }

    public void LoadCurrenciesIntoDropDown(string CompanyCode, SystemUser user, DropDownList ddlst)
    {
        string[] parameters = { CompanyCode };
        //DataSet ds = Client.ExecuteDataSet("GetCurrenciesForDropDown", parameters);
        //DataTable dt = ds.Tables[0];
        DataTable dt = Client.ExecuteDataSet("GetCurrenciesForDropDown", parameters).Tables[0];

        ddlst.Items.Clear();
        ddlst.Items.Add(new ListItem("--Select Currency--", ""));
        foreach (DataRow dr in dt.Rows)
        {
            string Text = dr["CurrencyName"].ToString();
            string Value = dr["CurrencyCode"].ToString();
            ddlst.Items.Add(new ListItem(Text, Value));
        }
    }

    public void LoadClientsIntoDropDown(string CompanyCode, string user, DropDownList ddlst)
    {
        string[] parameters = { CompanyCode, user };
        //DataSet ds = Client.ExecuteDataSet("GetClientsForDropDown", parameters);
        //DataTable dt = ds.Tables[0];
        DataTable dt = Client.ExecuteDataSet("GetClientsForDropDown", parameters).Tables[0];
        
        ddlst.Items.Clear();
        ddlst.Items.Add(new ListItem("--Select Client--", ""));
        foreach (DataRow dr in dt.Rows)
        {
            string Text = dr["ClientName"].ToString() +"--"+ dr["ClientNo"].ToString();
            string Value = dr["ClientNo"].ToString();
            ddlst.Items.Add(new ListItem(Text, Value));
        }
        ddlst.Items.Add(new ListItem("Other", "Other"));
    }

    public void LoadClientsAndSuppliersIntoDropDown(string CompanyCode, SystemUser user, DropDownList ddlst)
    {
        string[] parameters = { CompanyCode };
        //DataSet ds = Client.ExecuteDataSet("GetClientsAndSuppliersForDropDown", parameters);
        //DataTable dt = ds.Tables[0];
        DataTable dt = Client.ExecuteDataSet("GetClientsAndSuppliersForDropDown", parameters).Tables[0];

        ddlst.Items.Clear();
        foreach (DataRow dr in dt.Rows)
        {
            string Text = dr["Name"].ToString();
            string Value = dr["Code"].ToString();
            ddlst.Items.Add(new ListItem(Text, Value));
        }
    }

    public void LoadCompanyAccountsIntoDropDown(string CompanyCode, SystemUser user, DropDownList ddlst)
    {
        string[] parameters = { CompanyCode };
        DataTable dt = Client.ExecuteDataSet("GetClientsAndSuppliersAccountsForDropDown", parameters).Tables[0];

        ddlst.Items.Clear();
        ddlst.Items.Add(new ListItem("ALL", "ALL"));
        foreach (DataRow dr in dt.Rows)
        {
            string Text = dr["Name"].ToString();
            string Value = dr["AccountNumber"].ToString();
            ddlst.Items.Add(new ListItem(Text, Value));
        }
    }

    public void LoadContractCatgoriesIntoDropDown(string CompanyCode, SystemUser user, DropDownList ddlst)
    {
        string[] parameters = { CompanyCode };
        //DataSet ds = Client.ExecuteDataSet("GetContractCatgoriesForDropDown", parameters);
        //DataTable dt = ds.Tables[0];
        DataTable dt = Client.ExecuteDataSet("GetContractCatgoriesForDropDown", parameters).Tables[0];

        ddlst.Items.Clear();
        foreach (DataRow dr in dt.Rows)
        {
            string Text = dr["CategoryName"].ToString();
            string Value = dr["CategoryCode"].ToString();
            ddlst.Items.Add(new ListItem(Text, Value));
        }
    }

    public void LoadInvoiceCatgoriesIntoDropDown(string CompanyCode, SystemUser user, DropDownList ddlst)
    {
        string[] parameters = { CompanyCode };
        //DataSet ds = Client.ExecuteDataSet("GetInvoiceCategoriesForDropDown", parameters);
        //DataTable dt = ds.Tables[0];
        DataTable dt = Client.ExecuteDataSet("GetInvoiceCategoriesForDropDown", parameters).Tables[0];

        ddlst.Items.Clear();
        ddlst.Items.Add(new ListItem("-- ALL --", ""));
        foreach (DataRow dr in dt.Rows)
        {
            string Text = dr["CategoryName"].ToString();
            string Value = dr["CategoryCode"].ToString();
            ddlst.Items.Add(new ListItem(Text, Value));
        }
    }

    public void LoadPaymentTypesIntoDropDown(string CompanyCode, SystemUser user, DropDownList ddlst)
    {
        string[] parameters = { CompanyCode };
        //DataSet ds = Client.ExecuteDataSet("GetPaymentTypesForDropDown", parameters);
        //DataTable dt = ds.Tables[0];
        DataTable dt = Client.ExecuteDataSet("GetPaymentTypesForDropDown", parameters).Tables[0];

        ddlst.Items.Clear();
        ddlst.Items.Add(new ListItem("--Select Payment Type--", ""));
        foreach (DataRow dr in dt.Rows)
        {
            string Text = dr["PaymentTypeName"].ToString();
            string Value = dr["PaymentTypeCode"].ToString();
            ddlst.Items.Add(new ListItem(Text, Value));
        }
    }

    public void LoadRecieptCatgoriesIntoDropDown(string CompanyCode, SystemUser user, DropDownList ddlst)
    {
        string[] parameters = { CompanyCode };
        //DataSet ds = Client.ExecuteDataSet("GetRecieptCategoriesForDropDown", parameters);
        //DataTable dt = ds.Tables[0];
        DataTable dt = Client.ExecuteDataSet("GetRecieptCategoriesForDropDown", parameters).Tables[0];

        ddlst.Items.Clear();
        foreach (DataRow dr in dt.Rows)
        {
            string Text = dr["CategoryName"].ToString();
            string Value = dr["CategoryCode"].ToString();
            ddlst.Items.Add(new ListItem(Text, Value));
        }
    }

    public void LoadRolesIntoDropDown(string CompanyCode, SystemUser user, DropDownList ddlst)
    {
        string[] parameters = { };
        //DataSet ds = Client.ExecuteDataSet("GetUserTypesForDropDown", parameters);
        //DataTable dt = ds.Tables[0];
        DataTable dt = Client.ExecuteDataSet("GetUserTypesForDropDown", parameters).Tables[0];

        ddlst.Items.Clear();
        ddlst.Items.Add(new ListItem("--Select User Type--", ""));
        foreach (DataRow dr in dt.Rows)
        {
            string Text = dr["RoleName"].ToString();
            string Value = dr["RoleCode"].ToString();
            ddlst.Items.Add(new ListItem(Text, Value));
        }
    }

    public void LoadBranchesIntoDropDown(string CompanyCode, SystemUser user, DropDownList ddlst)
    {
        string[] parameters = { CompanyCode, ""};
        DataTable dt = Client.ExecuteDataSet("GetCompanyBranchesForDropDown", parameters).Tables[0];

        ddlst.Items.Clear();
        ddlst.Items.Add(new ListItem("--Select Branch--", ""));
        foreach (DataRow dr in dt.Rows)
        {
            string Text = dr["BranchName"].ToString();
            string Value = dr["BranchCode"].ToString();
            ddlst.Items.Add(new ListItem(Text, Value));
        }
    }

    public void LoadBranchesForSearchIntoDropDown(string CompanyCode, SystemUser user, DropDownList ddlst)
    {
        string[] parameters = { CompanyCode };
        DataTable dt = Client.ExecuteDataSet("GetCompanyBranchesForDropDown", parameters).Tables[0];

        ddlst.Items.Clear();
        ddlst.Items.Add(new ListItem("--Select Branch--", ""));
        foreach (DataRow dr in dt.Rows)
        {
            string Text = dr["BranchName"].ToString();
            string Value = dr["BranchCode"].ToString();
            ddlst.Items.Add(new ListItem(Text, Value));

            if (user.RoleCode != "001" && user.RoleCode != "002")
            {
                //ddlst.SelectedItem.Value = dr["BranchCode"].ToString();
                ddlst.SelectedItem.Text = dr["BranchName"].ToString();
                ddlst.SelectedValue = dr["BranchCode"].ToString();
                ddlst.Enabled = false;
            }
        }
    }

    public void LoadInvoicesForClient(string CompanyCode, SystemUser user, DropDownList ddlst, string ClientCode)
    {
        string[] parameters = { CompanyCode, ClientCode };
        //DataSet ds = Client.ExecuteDataSet("GetInvoiceByClientCodeForDropDown", parameters);
        //DataTable dt = ds.Tables[0];
        DataTable dt = Client.ExecuteDataSet("GetInvoiceByClientCodeForDropDown", parameters).Tables[0];

        ddlst.Items.Clear();
        foreach (DataRow dr in dt.Rows)
        {
            string Text = dr["InvoiceNumber"].ToString();
            string Value = dr["InvoiceNumber"].ToString();
            ddlst.Items.Add(new ListItem(Text, Value));
        }
    }

    public void LoadContractBillRatesInToDropDown(DropDownList ddBillRate)
    {
        ddBillRate.Items.Clear();
        ddBillRate.Items.Add(new ListItem("WEEKLY", "PERWEEEK"));
        ddBillRate.Items.Add(new ListItem("MONTHLY", "PERMONTH"));
        ddBillRate.Items.Add(new ListItem("YEARLY", "PERANNUM"));
    }

    public void LoadSupplierTypesIntoDropDown(string CompanyCode, SystemUser user, DropDownList ddlst)
    {
        string[] parameters = { CompanyCode };
        //DataSet ds = Client.ExecuteDataSet("GetSuppliersForDropDown", parameters);
        //DataTable dt = ds.Tables[0];
        DataTable dt = Client.ExecuteDataSet("GetSupplierTypesForDropDown", parameters).Tables[0];

        ddlst.Items.Clear();
        ddlst.Items.Add(new ListItem("--Select Supplier Type--", ""));
        foreach (DataRow dr in dt.Rows)
        {
            string Text = dr["TypeName"].ToString();
            string Value = dr["TypeCode"].ToString();
            ddlst.Items.Add(new ListItem(Text, Value));
        }
    }


    public void LoadSupplierCategoriesIntoDropDown(string CompanyCode, SystemUser user, DropDownList ddlst)
    {
        string[] parameters = { CompanyCode };
        //DataSet ds = Client.ExecuteDataSet("GetSuppliersForDropDown", parameters);
        //DataTable dt = ds.Tables[0];
        DataTable dt = Client.ExecuteDataSet("GetSupplierCategoriesForDropDown", parameters).Tables[0];

        ddlst.Items.Clear();
        ddlst.Items.Add(new ListItem("--Select Category--", ""));
        foreach (DataRow dr in dt.Rows)
        {
            string Text = dr["CategoryName"].ToString();
            string Value = dr["CategoryCode"].ToString();
            ddlst.Items.Add(new ListItem(Text, Value));
        }
    }

    public void LoadSupplierSubCategoriesIntoDropDown(string CompanyCode, SystemUser user, DropDownList ddlst)
    {
        string[] parameters = { CompanyCode };
        //DataSet ds = Client.ExecuteDataSet("GetSuppliersForDropDown", parameters);
        //DataTable dt = ds.Tables[0];
        DataTable dt = Client.ExecuteDataSet("GetSupplierSubCategoriesForDropDown", parameters).Tables[0];

        ddlst.Items.Clear();
        ddlst.Items.Add(new ListItem("--Select Sub Category--", ""));
        foreach (DataRow dr in dt.Rows)
        {
            string Text = dr["CategoryName"].ToString();
            string Value = dr["CategoryCode"].ToString();
            ddlst.Items.Add(new ListItem(Text, Value));
        }
    }


    public Result LogError(string CompanyCode, string BranchCode, string Message, string ErrorType, string Identifier, string StackTrace)
    {
        Result result = new Result();
        try
        {
            //result = Client.LogError(Identifier, StackTrace, CompanyCode, Message, ErrorType);
            DataTable datatable = Client.ExecuteDataSet("InsertIntoErrorLogs",
                                                           new string[]
                                                           {
                                                             CompanyCode,
                                                             BranchCode,
                                                             ErrorType,
                                                             Message
                                                             
                                                           }).Tables[0];
          result.LoanID = datatable.Rows[0][0].ToString();
            return result;
        }
        catch (Exception ex)
        {
            result.StatusCode = Globals.FAILURE_STATUS_CODE;
            result.StatusDesc = "EXCEPTION: " + ex.Message;
        }
        return result;
    }

    public SystemUser GetSystemUserByUserId(string UserId)
    {
        SystemUser user = new SystemUser();

        DataTable dt = Client.ExecuteDataSet("GetSystemUserByUserId", new string[] { UserId }).Tables[0];


        if (dt.Rows.Count == 0)
        {
            user.StatusCode = Globals.FAILURE_STATUS_CODE;
            user.StatusDesc = "USER WITH ID [" + UserId + "] NOT FOUND";
            return user;
        }

        DataRow dr = dt.Rows[0];
        user.Name = dr["Name"].ToString();
        user.Password = dr["Password"].ToString();
        user.IsActive = dr["IsActive"].ToString();
        user.ModifiedBy = dr["ModifiedBy"].ToString();
        user.CreatedBy = dr["CreatedBy"].ToString();
        //user.UserType = dr["UserType"].ToString();
        user.CompanyCode = dr["CompanyCode"].ToString();
        user.UserId = dr["UserId"].ToString();
        user.Email = dr["Email"].ToString();
        user.RoleCode = dr["RoleCode"].ToString();
        string ResetPassword = dr["ResetPassword"].ToString();

        //if (ResetPassword == "0")
        //{
        //    ResetPassword = "FALSE";
        //}
        //else
        //{
        //    ResetPassword = "TRUE";
        //}
        user.ResetPassword = Convert.ToBoolean(ResetPassword);

        if (user.IsActive.ToUpper() != "TRUE")
        {
            user.StatusCode = Globals.FAILURE_STATUS_CODE;
            user.StatusDesc = "USER WITH ID [" + UserId + "] IS DEACTIVATED. PLEASE CONTACT SYSTEM ADMINISTRATORS";
            return user;
        }

        user.StatusCode = Globals.SUCCESS_STATUS_CODE;
        user.StatusDesc = Globals.SUCCESS_STATUS_TEXT;
        return user;

    }

    public SystemSetting GetSystemSettingById(string settingCode, string companyCode)
    {
        SystemSetting setting = new SystemSetting();
        string[] parameters = { companyCode, settingCode };
        DataTable dt = Client.ExecuteDataSet("SystemSettings_SelectRow", parameters).Tables[0];
        if (dt.Rows.Count > 0)
        {
            setting.SettingValue = dt.Rows[0]["SettingValue"].ToString();
            setting.SettingCode = dt.Rows[0]["SettingCode"].ToString();
            setting.ModifiedBy = dt.Rows[0]["ModifiedBy"].ToString();
            setting.CompanyCode = dt.Rows[0]["CompanyCode"].ToString();
            setting.StatusCode = "0";
            setting.StatusDesc = "SUCCESS";
        }
        else
        {
            setting.StatusCode = "100";
            setting.StatusDesc = "FAILED: NO SETTING FOUND WITH ID [" + settingCode + "] UNDER BANK " + companyCode;
        }
        return setting;
    }

    public SystemSetting GetInterestSetting(string companyCode, string InterestCode)
    {
        SystemSetting setting = new SystemSetting();
        string[] parameters = { companyCode, InterestCode };
        DataTable dt = Client.ExecuteDataSet("SearchInterestSettingTable", parameters).Tables[0];
        if (dt.Rows.Count > 0)
        {
            setting.SettingValue = dt.Rows[0]["InterestValue"].ToString();
            setting.SettingName = dt.Rows[0]["InterestName"].ToString();
            setting.SettingCode = dt.Rows[0]["InterestCode"].ToString();
            setting.ModifiedBy = dt.Rows[0]["CreatedBy"].ToString();
            setting.CompanyCode = dt.Rows[0]["CompanyCode"].ToString();
            setting.StatusCode = "0";
            setting.StatusDesc = "SUCCESS";
        }
        else
        {
            setting.StatusCode = "100";
            setting.StatusDesc = "FAILED: NO SETTING FOUND WITH ID [" + InterestCode + "] UNDER COMPANY " + companyCode;
        }
        return setting;
    }

    //public Company GetCompanyByCompanyCode(string CompanyCode)
    //{
    //    Company company = new Company();
    //    UBAKYCAPI mSystem = new UBAKYCAPI();
    //    DataTable dt = mSystem.ExecuteDataSet("GetCompanyByCompanyCode", new string[] { CompanyCode }).Tables[0];


    //    if (dt.Rows.Count == 0)
    //    {
    //        company.StatusCode = Globals.FAILURE_STATUS_CODE;
    //        company.StatusDesc = "COMPANY WITH ID [" + CompanyCode + "] NOT FOUND";
    //        return company;
    //    }

    //    DataRow dr = dt.Rows[0];
    //    company.CompanyName = dr["CompanyName"].ToString();
    //    company.CompanyContactEmail = dr["CompanyContactEmail"].ToString();
    //    company.IsActive = dr["IsActive"].ToString();
    //    company.ModifiedBy = dr["ModifiedBy"].ToString();
    //    company.CreatedBy = dr["CreatedBy"].ToString();
    //    company.NavBarTextColor = dr["NavBarTextColor"].ToString();
    //    company.PathToLogoImage = dr["PathToLogoImage"].ToString();
    //    company.ThemeColor = dr["ThemeColor"].ToString();
    //    company.CompanyCode = dr["CompanyCode"].ToString();

    //    if (company.IsActive.ToUpper() != "TRUE")
    //    {
    //        company.StatusCode = Globals.FAILURE_STATUS_CODE;
    //        company.StatusDesc = "COMPANY WITH ID [" + CompanyCode + "] IS DEACTIVATED. PLEASE CONTACT SYSTEM ADMINISTRATORS";
    //        return company;
    //    }

    //    company.StatusCode = Globals.SUCCESS_STATUS_CODE;
    //    company.StatusDesc = Globals.SUCCESS_STATUS_TEXT;
    //    return company;

    //}

    //public string GetSaleItemsForNarration(string SaleID)
    //{
    //    Company company = new Company();
    //    UBAKYCAPI mSystem = new UBAKYCAPI();
    //    DataTable dt = mSystem.ExecuteDataSet("GetSaleItemsBySaleId", new string[] { SaleID }).Tables[0];
    //    string result = "";
    //    foreach (DataRow dr in dt.Rows)
    //    {
    //        result += dr["ItemDesc"].ToString() + " - " + dr["SubTotal"].ToString() + "\n";
    //    }
    //    return result;

    //}

    public DataTable GetSaleItemById(string CompanyCode, string objectId)
    {
        LeshLoanAPI mSystem = new LeshLoanAPI();

        string[] param = { CompanyCode, objectId };
        DataTable dt = mSystem.ExecuteDataSet("GetSaleItemById", param).Tables[0];
        return dt;
    }
    public DataTable GetSaleItemById_1(string CompanyCode, string objectId, string invoiceNumber)
    {
        LeshLoanAPI mSystem = new LeshLoanAPI();

        string[] param = { CompanyCode, objectId, invoiceNumber };
        DataTable dt = mSystem.ExecuteDataSet("GetSaleItemById1", param).Tables[0];
        return dt;
    }
    //public string[] GetSaleItemsDetails(string SaleID, string ItemDesc)
    //{
    //    Company company = new Company();
    //    UBAKYCAPI mSystem = new UBAKYCAPI();
    //    DataTable dt = mSystem.ExecuteDataSet("GetSaleItemsBySaleId", new string[] { SaleID, ItemDesc }).Tables[0];
    //    List<string> result = new List<string>();
    //    for (int i = 0; i < dt.Rows.Count; i++)
    //        foreach (DataRow dr in dt.Rows)
    //        {
    //            result.Add(dr["UnitPrice"].ToString());
    //            result.Add(dr["ItemQuantity"].ToString());
    //            result.Add(dr["ItemDesc"].ToString());
    //            result.Add(dr["SubTotal"].ToString());
    //            //result += dr["ItemDesc"].ToString() + " - " + dr["SubTotal"].ToString() + "\n";
    //        }
    //    return result.ToArray();

    //}
    public DataTable SearchSuppliersTable(string[] searchParams)
    {
        DataTable dt = Client.ExecuteDataSet("SearchSuppliersTable", searchParams).Tables[0];

        return dt;
    }

    public DataTable SearchSupplierSubCategoriesTable(string[] searchParams)
    {
        DataTable dt = Client.ExecuteDataSet("SearchSupplierSubCategoriesTable", searchParams).Tables[0];

        return dt;
    }

    public DataTable SearchSystemUsers(string[] searchParams)
    {
        DataTable dt = Client.ExecuteDataSet("SearchSystemUsers", searchParams).Tables[0];

        return dt;
    }

    public DataTable SearchSystemUsersToApprove(string[] searchParams)
    {
        DataTable dt = Client.ExecuteDataSet("SearchSystemUsersToApprove", searchParams).Tables[0];

        return dt;
    }

    public DataTable SearchEditedSystemUsersToApprove(string[] searchParams)
    {
        DataTable dt = Client.ExecuteDataSet("SearchEditedSystemUsersToApprove", searchParams).Tables[0];

        return dt;
    }

    public DataTable SearchInterestSettingTable(string[] searchParams)
    {
        DataTable dt = Client.ExecuteDataSet("SearchInterestSettingTable", searchParams).Tables[0];

        return dt;
    }

    public DataTable SearchClientDetailsTable(string[] searchParams)
    {
        DataTable dt = Client.ExecuteDataSet("SearchClientDetailsTable", searchParams).Tables[0];

        return dt;
    }

    public DataTable SearchLoanDetailsTable(string[] searchParams)
    {
        DataTable dt = Client.ExecuteDataSet("SearchLoanDetailsTable", searchParams).Tables[0];
        return dt;
    }

    public DataTable SearchLoanDetailsTableForApproval(string[] searchParams)
    {
        DataTable dt = Client.ExecuteDataSet("SearchLoanForApproval", searchParams).Tables[0];
        return dt;
    }

    public DataTable SearchLoanDetailsTableForRepayment(string[] searchParams)
    {
        DataTable dt = Client.ExecuteDataSet("SearchLoanForRepay", searchParams).Tables[0];
        return dt;
    }

    public DataTable SearchLoanPayments(string[] searchParams)
    {
        DataTable dt = Client.ExecuteDataSet("SearchLoanPayments", searchParams).Tables[0];
        return dt;
    }


    public DataTable SearchKYCDetailsToApprove(string[] searchParams)
    {
        DataTable dt = Client.ExecuteDataSet("SearchKYCDetailsToApprove", searchParams).Tables[0];

        return dt;
    }

    public DataTable SearchIncomeExpenseDetailsForreport(string[] searchParams)
    {
        DataTable dt = Client.ExecuteDataSet("SearchIncomeExpenseDetailsForreport", searchParams).Tables[0];

        return dt;
    }

    public DataTable SearchAuditTrail(string[] searchParams)
    {
        DataTable dt = Client.ExecuteDataSet("SearchAuditTrail", searchParams).Tables[0];

        return dt;
    }
    

    public void CancelInvoice(string[] searchParams)
    {
        Client.ExecuteNonQuery("CancelInvoice", searchParams);

    }

    public DataTable SearchClientsTable(string[] searchParams)
    {
        //DataSet ds = Client.ExecuteDataSet("SearchClientsTable", searchParams);
        //DataTable dt = ds.Tables[0];
        DataTable dt = Client.ExecuteDataSet("SearchClientsTable", searchParams).Tables[0];
        return dt;
    }

    public DataTable SearchClientsorSuppliersTable(string[] searchParams)
    {
        DataTable dt = Client.ExecuteDataSet("SearchClientsorSuppliersTable", searchParams).Tables[0];
        return dt;
    }

    public DataTable SearchInjectionDetailsTable(string[] searchParams)
    {
        DataTable dt = Client.ExecuteDataSet("SearchInjectionsTable", searchParams).Tables[0];
        return dt;
    }

    public DataTable SearchPaymentVouchersTable(string[] searchParams)
    {
        DataTable dt = Client.ExecuteDataSet("SearchPaymentVouchersTable", searchParams).Tables[0];
        return dt;
    }

    public DataTable SearchInvoicesTable(string[] searchParams)
    {
        DataTable dt = Client.ExecuteDataSet("SearchInvoicesTable", searchParams).Tables[0];
        return dt;
    }

    public DataTable SearchInvoices(string[] searchParams)
    {
        DataTable dt = Client.ExecuteDataSet("SearchInvoices", searchParams).Tables[0];
        return dt;
    }
    public DataTable SearchReceipts(string[] searchParams)
    {
        DataTable dt = Client.ExecuteDataSet("SearchForReceipts", searchParams).Tables[0];
        return dt;
    }

    public DataTable SearchCancelledInvoices(string[] searchParams)
    {
        DataTable dt = Client.ExecuteDataSet("SearchForCancelledInvoices", searchParams).Tables[0];
        return dt;
    }
    public DataTable SearchSalesTable(string[] searchParams)
    {
        //DataSet ds = Client.ExecuteDataSet("SearchSalesTable", searchParams);
        //DataTable dt = ds.Tables[0];
        DataTable dt = Client.ExecuteDataSet("SearchInvoicesTable", searchParams).Tables[0];
        return dt;
    }


    public DataTable SearchGeneralLedgerTable(string[] searchParams)
    {
        //DataSet ds = Client.ExecuteDataSet("SearchGeneralLedgerTable", searchParams);
        //DataTable dt = ds.Tables[0];
        DataTable dt = Client.ExecuteDataSet("SearchGeneralLedgerTable", searchParams).Tables[0];
        return dt;
    }

    public DataTable GetCountID(string[] searchParams)
    {
        //string[] searchParams = { systemCode };
        DataTable dt = Client.ExecuteDataSet("GetCountID", searchParams).Tables[0];
        return dt;
    }

    public string GenerateSystemCode(string SystemCode)
    {
        string code = "";
        string[] searchParams = { SystemCode };
        DataTable dt = GetCountID(searchParams);
        foreach (DataRow dr in dt.Rows)
        {
            code = dr["CountID"].ToString();
        }
        return code;
    }

    public string GetPurchaseRequistionImage(SystemUser user, string InvoiceNumber)
    {
        string code = "";
        string[] searchParams = { user.CompanyCode, InvoiceNumber };
        DataTable dt = Client.ExecuteDataSet("GetPurchaseById", searchParams).Tables[0];
        foreach (DataRow dr in dt.Rows)
        {
            code = dr["ImageOfPurchaseRequistion"].ToString();
        }
        return code;
    }

    public DataTable BalanceAccounts(string[] searchParams)
    {
        DataTable dt = Client.ExecuteDataSet("GetTotalBusinessAmount", searchParams).Tables[0];
        return dt;
    }

    public DataTable SearchPurchases(string[] searchParams)
    {
        //DataSet ds = Client.ExecuteDataSet("SearchPurchases", searchParams);
        //DataTable dt = ds.Tables[0];
        DataTable dt = Client.ExecuteDataSet("SearchPurchases", searchParams).Tables[0];
        return dt;
    }

    public DataTable SearchPurchasesReq(string[] searchParams)
    {
        //DataSet ds = Client.ExecuteDataSet("SearchPurchases", searchParams);
        //DataTable dt = ds.Tables[0];
        DataTable dt = Client.ExecuteDataSet("SearchPurchasesReq", searchParams).Tables[0];
        return dt;
    }

    public DataTable SearchSales(string[] searchParams)
    {
        //DataSet ds = Client.ExecuteDataSet("SearchSales", searchParams);
        //DataTable dt = ds.Tables[0];
        DataTable dt = Client.ExecuteDataSet("SearchSales", searchParams).Tables[0];
        return dt;
    }

    public DataTable SearchSalesReq(string[] searchParams)
    {
        //DataSet ds = Client.ExecuteDataSet("SearchPurchases", searchParams);
        //DataTable dt = ds.Tables[0];
        DataTable dt = Client.ExecuteDataSet("SearchSalesReq", searchParams).Tables[0];
        return dt;
    }

    public DataTable SearchSaleItemsTable(string[] searchParams)
    {
        //DataSet ds = Client.ExecuteDataSet("SearchSaleItemsTable", searchParams);
        //DataTable dt = ds.Tables[0];
        DataTable dt = Client.ExecuteDataSet("SearchSaleItemsTable", searchParams).Tables[0];
        return dt;
    }

    public DataTable SearchSaleItemsTableForEdit(string[] searchParams)
    {
        //DataSet ds = Client.ExecuteDataSet("SearchSaleItemsTable", searchParams);
        //DataTable dt = ds.Tables[0];
        DataTable dt = Client.ExecuteDataSet("SearchSaleItemsTableForEdit", searchParams).Tables[0];
        return dt;
    }

    public DataTable SearchAuditlogsTable(string[] parameters)
    {
        DataTable dt = Client.ExecuteDataSet("SearchAuditlogsTable", parameters).Tables[0];
        return dt;
    }

    public DataTable SearchErrorlogsTable(string[] parameters)
    {
        DataTable dt = Client.ExecuteDataSet("SearchErrorlogsTable", parameters).Tables[0];
        return dt;
    }

    public DataTable ViewInvoice(string[] searchParams)
    {
        DataTable dt = Client.ExecuteDataSet("ViewInvoice", searchParams).Tables[0];
        return dt;
    }

    public DataTable ViewPurchase(string[] searchParams)
    {
        DataTable dt = Client.ExecuteDataSet("ViewPurchase", searchParams).Tables[0];
        return dt;
    }

    public DataTable ViewReceipt(string[] searchParams)
    {
        DataTable dt = Client.ExecuteDataSet("ViewReciept", searchParams).Tables[0];
        return dt;
    }

    public string UpdateClientStatus(string ClientID, string Reason, string UserId)
    {
        string[] parameters = { ClientID, Reason, UserId };
        int dt = Client.ExecuteNonQuery("UpdateClientStatus", parameters);
        return dt.ToString();
    }

    public string UpdateLoanStatus(string LoanNumber, string ClientID, string UserType, string UserId)
    {
        string[] parameters = { LoanNumber, ClientID, UserType, UserId };
        int dt = Client.ExecuteNonQuery("UpdateLoanStatus", parameters);
        return dt.ToString();
    }

    //public string GeneratePassword(int length)
    //{

    //    var random = new Random((int)DateTime.Now.Ticks);
    //    try
    //    {
    //        var result = new byte[length];
    //        for (var index = 0; index < length; index++)
    //        {
    //            result[index] = (byte)random.Next(33, 126);
    //        }
    //        return System.Text.Encoding.ASCII.GetString(result);
    //    }
    //    catch (Exception ex)
    //    {
    //        throw new Exception(ex.Message, ex);
    //    }
    //}

    public string GeneratePassword()
    {
        int intMin = 0;
        int intMax = 9;
        int charMin = 26;
        int charMax = 38;
        int strMin = 0;
        int strMax = 25;
        string[] alphabet = new string[39];
        alphabet[0] = "A";
        alphabet[1] = "B";
        alphabet[2] = "C";
        alphabet[3] = "D";
        alphabet[4] = "E";
        alphabet[5] = "F";
        alphabet[6] = "G";
        alphabet[7] = "H";
        alphabet[8] = "I";
        alphabet[9] = "J";
        alphabet[10] = "K";
        alphabet[11] = "L";
        alphabet[12] = "M";
        alphabet[13] = "N";
        alphabet[14] = "O";
        alphabet[15] = "P";
        alphabet[16] = "Q";
        alphabet[17] = "R";
        alphabet[18] = "S";
        alphabet[19] = "T";
        alphabet[12] = "U";
        alphabet[21] = "V";
        alphabet[22] = "W";
        alphabet[23] = "X";
        alphabet[24] = "Y";
        alphabet[25] = "Z";
        alphabet[26] = "*";
        alphabet[27] = "$";
        alphabet[28] = "-";
        alphabet[29] = "+";
        alphabet[30] = "?";
        alphabet[31] = "_";
        alphabet[32] = "&";
        alphabet[33] = "=";
        alphabet[34] = "!";
        alphabet[35] = "%";
        alphabet[36] = "{";
        alphabet[37] = "}";
        alphabet[38] = "/";
        string pass = "";
        Random random1 = new Random();
        Random random = new Random();
        while (pass.Length < 10)
        {
            if (pass.Length == 1 || pass.Length == 5 || pass.Length == 6)
            {
                int rand = random1.Next(strMin, strMax);
                string letter = alphabet[rand];
                pass = pass + letter;
            }
            else if (pass.Length == 2 || pass.Length == 7 || pass.Length == 9)
            {
                int rand = random1.Next(charMin, charMax);
                string letter = alphabet[rand];
                pass = pass + letter;
            }
            else
            {
                int randomno = random.Next(intMin, intMax);
                pass = pass + randomno.ToString();
            }
        }
        return pass;
    }

    public void DeactivateUser(string userid, string channel, string ip, string AccountCode)
    {
        string[] parameters = { userid, channel, ip, AccountCode };
        int rowsAffected = Client.ExecuteNonQuery("DeactivateUser", parameters);
    }

    //public SystemSetting GetSystemSettingById(string settingCode, string companyCode)
    //{
    //    SystemSetting setting = new SystemSetting();
    //    string[] parameters = { companyCode, settingCode };
    //    DataTable dt = Client.ExecuteDataSet("SystemSettings_SelectRow", parameters).Tables[0];
    //    if (dt.Rows.Count > 0)
    //    {
    //        setting.SettingValue = dt.Rows[0]["SettingValue"].ToString();
    //        setting.SettingCode = dt.Rows[0]["SettingCode"].ToString();
    //        setting.ModifiedBy = dt.Rows[0]["ModifiedBy"].ToString();
    //        setting.CompanyCode = dt.Rows[0]["CompanyCode"].ToString();
    //        setting.StatusCode = "0";
    //        setting.StatusDesc = "SUCCESS";
    //    }
    //    else
    //    {
    //        setting.StatusCode = "100";
    //        setting.StatusDesc = "FAILED: NO SETTING FOUND WITH ID [" + settingCode + "] UNDER BANK " + companyCode;
    //    }
    //    return setting;
    //}

    public void LogUserLogin(string Channel, string IP, string Id, string SessionKey, string status, string description, string category)
    {
        string[] data = { Channel, IP, Id, SessionKey, status, description, category };
        Client.ExecuteNonQuery("LogUserLogin", data);
    }

    public bool PasswordExpired(string userId, string companyCode, string ip)
    {
        DataTable dt = Client.ExecuteDataSet("PasswordTrack_Select", new string[] { userId }).Tables[0];
        if (dt.Rows.Count > 0)
        {
            DateTime date = DateTime.Parse(dt.Rows[0]["RecordDate"].ToString());
            int Duration = 90;
            string duration = "";// GetSystemSettingById("PASSWORD_PERIOD", companyCode).SettingValue;
            //Duration = Convert.ToInt16(duration);
            DateTime Today = DateTime.Now;
            TimeSpan t = Today.Subtract(date);
            double dateDiff = t.TotalDays;
            if (dateDiff > Duration)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }

    public Result UpdateUserPassword(SystemUser User)
    {
        Result result = new Result();
        string[] parameters = { User.CompanyCode, User.UserId, User.Password };
        int rowsAffected = Client.ExecuteNonQuery("UpdateUserPassword", parameters);
        if (rowsAffected > 0)
        {
            result.StatusCode = "0";
            result.StatusDesc = "SUCCESS";
        }
        else
        {
            result.StatusCode = "100";
            result.StatusDesc = "NO ROWS AFFECTED";
        }
        return result;
    }

    public Result ReActivateUser(SystemUser User, string ResetBy, string Type)
    {
        Result result = new Result();
        string[] parameters = { User.CompanyCode, User.UserId, User.Password, User.RoleCode, User.IsActive, Type, ResetBy, User.Name };
        int rowsAffected = Client.ExecuteNonQuery("UpdateUserType", parameters);
        if (rowsAffected > 0)
        {
            result.StatusCode = "0";
            result.StatusDesc = "SUCCESS";
        }
        else
        {
            result.StatusCode = "100";
            result.StatusDesc = "NO ROWS AFFECTED";
        }
        return result;
    }

    public Result ApproveEditedUsers(string CompanyCode, string BranchCode, string UserId, string Approver)
    {
        Result result = new Result();
        string[] parameters = { CompanyCode, BranchCode, UserId, Approver};
        int rowsAffected = Client.ExecuteNonQuery("UpdateEditedUsers", parameters);
        if (rowsAffected > 0)
        {
            result.StatusCode = "0";
            result.StatusDesc = "SUCCESS";
        }
        else
        {
            result.StatusCode = "100";
            result.StatusDesc = "NO ROWS AFFECTED";
        }
        return result;
    }

    public Result ResendCredentials(SystemUser user, string credentialType, string credential)
    {
        Result result = new Result();
        try
        {
            //Company company = HttpContext.Current.Session["UsersCompany"] as Company;
            string email = user.Email;
            if (!string.IsNullOrEmpty(email))
            {
                string Subject = "UBA KYC COLLECTION SYSTEM WEB PORTAL CREDENTIALS";
                string Message = "Dear " + user.Name + ", <br/>" +
                                 "Your UBA KYC Collection System Account credentials have been reset. Below are the details to access the UBA KYC Web Portal.<br/>" +
                                 "URL: https://test.pegasus.co.ug:8019/TestUBAKYCPortal/ <br/>" +
                                 "Username: " + user.UserId + "<br/>" +
                                 "Password: " + credential + "<br/>" +
                                 "Thank you.<br/><br/>";

                InterConnect.MailApi.Email mail = new InterConnect.MailApi.Email();
                InterConnect.MailApi.EmailAddress addr = new InterConnect.MailApi.EmailAddress();
                addr.Address = email;
                addr.Name = user.Name;
                addr.AddressType = InterConnect.MailApi.EmailAddressType.To;

                InterConnect.MailApi.EmailAddress[] addresses = { addr };
                mail.MailAddresses = addresses;
                mail.From = "notifications@pegasustechnologies.co.ug";
                mail.Message = Message;
                mail.Subject = Subject;


                InterConnect.MailApi.Messenger mapi = new InterConnect.MailApi.Messenger();

                System.Net.ServicePointManager.ServerCertificateValidationCallback = RemoteCertificateValidation;
                InterConnect.MailApi.Result resp = mapi.PostEmail(mail);

                if (resp.StatusCode != "0")
                {
                    result.StatusCode = "100";
                    result.StatusDesc = resp.StatusDesc;
                    return result;
                }
            }
            else
            {
                result.StatusCode = "100";
                result.StatusDesc = "NO EMAIL FOUND";
                return result;
            }
            result.StatusCode = "0";
            result.StatusDesc = "SUCCESS";
        }
        catch (Exception ex)
        {
            result.StatusCode = "100";
            result.StatusDesc = "FAILED: INTERNAL ERROR WHEN TRYING TO SEND EMAIL WITH CREDENTIALS";
        }
        return result;
    }

    public void Log(string procedure, string[] parameters)
    {
        try
        {
            Client.ExecuteNonQuery(procedure, parameters);
        }
        catch { }
    }

    public bool PasswordHasBeenUsed(string user, string password)
    {
        DataTable dt = Client.ExecuteDataSet("PasswordTrack_SelectRow", new string[] { user, password }).Tables[0];
        if (dt.Rows.Count > 0)
        {
            DateTime passwordDate = DateTime.Parse(dt.Rows[0]["RecordDate"].ToString());
            TimeSpan time = DateTime.Now - passwordDate;
            if (time.Days > 365)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        else
        {
            return false;
        }
    }

    public void ExportToExcel(DataTable table, string filename, HttpResponse Response)
    {
        if (table == null)
        {
            return;
        }
        if (table.Rows.Count > 0)
        {
            Response.Clear();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment;filename=Report.xls");
            Response.Charset = "";
            Response.ContentType = "application/vnd.ms-excel";
            using (StringWriter sw = new StringWriter())
            {
                HtmlTextWriter hw = new HtmlTextWriter(sw);
                GridView grid = new GridView();
                //To Export all pages
                grid.AllowPaging = false;
                grid.DataSource = table;
                grid.DataBind();

                grid.HeaderRow.BackColor = Color.White;
                foreach (TableCell cell in grid.HeaderRow.Cells)
                {
                    cell.BackColor = grid.HeaderStyle.BackColor;
                }
                foreach (GridViewRow row in grid.Rows)
                {
                    row.BackColor = Color.White;
                    foreach (TableCell cell in row.Cells)
                    {
                        if (row.RowIndex % 2 == 0)
                        {
                            cell.BackColor = grid.AlternatingRowStyle.BackColor;
                        }
                        else
                        {
                            cell.BackColor = grid.RowStyle.BackColor;
                        }
                        cell.CssClass = "textmode";
                    }
                }

                grid.RenderControl(hw);

                //style to format numbers to string
                string style = @"<style> .textmode { } </style>";
                Response.Write(style);
                Response.Output.Write(sw.ToString());
                Response.Flush();
                Response.End();
            }
        }
    }

    public void ExportToPdf(DataTable table, string filename, HttpResponse Response)
    {
        using (StringWriter sw = new StringWriter())
        {
            using (HtmlTextWriter hw = new HtmlTextWriter(sw))
            {
                //To Export all pages

                GridView gridView = new GridView();
                gridView.DataSource = table;
                gridView.DataBind();
                gridView.AllowPaging = false;

                gridView.RenderControl(hw);
                StringReader sr = new StringReader(sw.ToString());
                iTextSharp.text.Document pdfDoc = new iTextSharp.text.Document(iTextSharp.text.PageSize.A2, 10f, 10f, 10f, 0f);
                HTMLWorker htmlparser = new HTMLWorker(pdfDoc);
                PdfWriter.GetInstance(pdfDoc, Response.OutputStream);
                pdfDoc.Open();
                htmlparser.Parse(sr);
                pdfDoc.Close();

                Response.ContentType = "application/pdf";
                Response.AddHeader("content-disposition", "attachment;filename=Report.pdf");
                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                Response.Write(pdfDoc);
                Response.End();
            }
        }
    }

    public ClientDetails GetClientDetails(SystemUser user, string ClientID)
    {
        ClientDetails Det = new ClientDetails();
        string[] Params = { user.CompanyCode, ClientID };
        DataTable dt = Client.ExecuteDataSet("GetClientDetails", Params).Tables[0];
        if (dt.Rows.Count > 0)
        {
            //Det.CompanyCode = dt.Rows[0]["CompanyCode"].ToString();
            Det.ClientNo = dt.Rows[0]["ClientNo"].ToString();
            Det.ClientName = dt.Rows[0]["ClientName"].ToString();
            Det.ClientAddress = dt.Rows[0]["ClientAddress"].ToString();
            Det.ClientPhoneNumber = dt.Rows[0]["TelephoneNumber"].ToString();
            Det.Gender = dt.Rows[0]["Gender"].ToString();
            Det.IDNumber = dt.Rows[0]["IDNumber"].ToString();
            Det.ClientEmail = dt.Rows[0]["Email"].ToString();
            Det.IDType = dt.Rows[0]["IDType"].ToString();

            Det.Referee = dt.Rows[0]["RefereeName"].ToString();
            Det.RefrereePhoneNo = dt.Rows[0]["RefereePhoneNo"].ToString();
            Det.ClientAddress = dt.Rows[0]["ClientAddress"].ToString();
            Det.ModifiedBy = dt.Rows[0]["CreatedBy"].ToString();
            Det.ModifiedOn = dt.Rows[0]["CreatedOn"].ToString();
            Det.ClientPhoto = dt.Rows[0]["ClientImage"].ToString();
            Det.IDPhoto = dt.Rows[0]["IDImage"].ToString();
            //return Det;
        }
        else
        {
            Det.StatusCode = "100";
            Det.StatusDesc = "NO RECORDS FOUND";
        }
        return Det;
    }

    public Receipt GetReceiptDetails(SystemUser user, string ReceiptNo, string ClientID)
    {
        Receipt Rcpt = new Receipt();
        string[] Params = { user.CompanyCode, ReceiptNo, ClientID };
        DataTable dt = Client.ExecuteDataSet("GetReceiptDetails", Params).Tables[0];
        if (dt.Rows.Count > 0)
        {
            //Det.CompanyCode = dt.Rows[0]["CompanyCode"].ToString();
            Rcpt.ClientID = dt.Rows[0]["ClientCode"].ToString();
            Rcpt.LoanNumber = dt.Rows[0]["LoanNumber"].ToString();
            Rcpt.ReceiptNumber = dt.Rows[0]["ReceiptNumber"].ToString();
            Rcpt.ReceiptAmount = dt.Rows[0]["ReceiptAmount"].ToString();
            Rcpt.PaymentDate = dt.Rows[0]["PaymentDate"].ToString();
            Rcpt.PaymentType = dt.Rows[0]["PaymentType"].ToString();
            Rcpt.ModifiedBy = dt.Rows[0]["AddedBy"].ToString();
            Rcpt.ModifiedOn = dt.Rows[0]["AddedOn"].ToString();
            Rcpt.CurrencyCode = dt.Rows[0]["Currency"].ToString();
            Rcpt.StatusCode = Globals.SUCCESS_STATUS_CODE;
            Rcpt.StatusDesc = Globals.SUCCESS_STATUS_TEXT;
            //return Det;
        }
        else
        {
            Rcpt.StatusCode = "100";
            Rcpt.StatusDesc = "NO RECORDS FOUND";
        }
        return Rcpt;
    }

    public ListItemCollection GetClientSearchDetails(string ClientDet)
    {
        ListItemCollection Cli = new ListItemCollection();
        //List<string> ClientSearch = new List<string>();

        string[] Params = { ClientDet };
        DataTable dt = Client.ExecuteDataSet("GetClientSearchDetails", Params).Tables[0];
        //DataSet ds = Client.ExecuteDataSet("GetClientSearchDetails", Params);
        foreach (DataRow dr in dt.Rows)
           // while (dt.Rows.Count > 0)
        {
            Cli.Add(new ListItem(dt.Rows[0]["ClientName"].ToString() + "--" + dt.Rows[0]["ClientNo"].ToString()));
                //ClientSearch.Add(dt.Rows[0]["ClientName"].ToString() + "--" + dt.Rows[0]["ClientNo"].ToString());
        }
        ListItem[] rankArray = new ListItem[100];

        //var result = Cli.Cast<ListItem>().ToArray();
        //Cli.Value = ClientSearch.ToArray().ToString(); //ClientSearch.ToString();
        //Cli.Text = ClientSearch.ToString();
        return Cli;
    }

    public LoanDetails GetLoanDetails(SystemUser user, string ClientID, string LoanNo)
    {
        LoanDetails Det = new LoanDetails();
        string[] Params = { ClientID, LoanNo };
        DataTable dt = Client.ExecuteDataSet("GetLoanDetails", Params).Tables[0];
        if (dt.Rows.Count > 0)
        {
            //Det.CompanyCode = dt.Rows[0]["CompanyCode"].ToString();
            Det.ClientID = dt.Rows[0]["ClientID"].ToString();
            Det.LoanNo = dt.Rows[0]["LoanNo"].ToString();
            Det.LoanAmount = dt.Rows[0]["LoanAmount"].ToString();
            Det.LoanDate = dt.Rows[0]["LoanDate"].ToString();
            Det.LoanDesc = dt.Rows[0]["LoanDesc"].ToString();
            Det.Organization = dt.Rows[0]["organization"].ToString();
            Det.ApprovedAmount = dt.Rows[0]["ApprovedAmount"].ToString();

            Det.InterestRate = dt.Rows[0]["InterestRate"].ToString();
            Det.CreatedBy = dt.Rows[0]["AddedBy"].ToString();
            Det.ModifiedOn = dt.Rows[0]["AddedOn"].ToString();
            //return Det;
        }
        else
        {
            Det.StatusCode = "100";
            Det.StatusDesc = "NO RECORDS FOUND";
        }
        return Det;
    }
    public string GetBase64String(string Base64String)
    {
        byte[] byteCustPhoto = Encoding.UTF8.GetBytes(Base64String);
        //byte[] byteArray = Encoding.ASCII.GetBytes(contents);
        //MemoryStream stream = new MemoryStream(byteArray);

        //System.IO.Stream fs = ;
        //StreamReader reader = new StreamReader(Base64String);//System.IO.BinaryReader br = new System.IO.BinaryReader(fs);
        //Byte[] bytes = reader.ReadBytes((Int32)fs.Length);

        //Byte[] byteCustPhoto = Encoding.ASCII.GetBytes(Base64String);
        string base64String = Convert.ToBase64String(byteCustPhoto, 0, byteCustPhoto.Length);
        base64String = "data:image/jpg;base64," + base64String;
        return base64String;
    }
    public string GetUserRoleName(string UserType)
    {
        string RoleCode = "";
        string []Params = { UserType };
        DataTable dt = Client.ExecuteDataSet("GetUserRoleCode", Params).Tables[0];
        RoleCode = dt.Rows[0]["RoleCode"].ToString();
        return RoleCode;
    }

    public Result AddEditedUserToTable(SystemUser Details, string AddedBy)
    {
        Result Res = new Result();
        try
        {
            string[] Params = { Details.CompanyCode, Details.Name, Details.RoleCode, Details.Email, Details.IsActive, Details.ModifiedBy};
            DataTable dt = Client.ExecuteDataSet("SaveSystemUsersToEdit", Params).Tables[0];
            Res.LoanID = dt.Rows[0]["InsertedID"].ToString();
            Res.StatusCode = Globals.SUCCESS_STATUS_CODE;
            Res.StatusDesc = Globals.SUCCESS_STATUS_TEXT;
        }
        catch (Exception ex)
        {
            Res.StatusCode = "100";
            Res.StatusDesc = "FAILED: INTERNAL ERROR WHEN TRYING TO EDIT USER DETAILS";
            LogError(Details.CompanyCode, "", "ADD-EDITEDUSER" + ex.Message, "EXCEPTION", "", "");
        }
        
        return Res;
    }

}
using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using InterConnect;
using InterConnect.LeshLaonApi;

public partial class _Default : System.Web.UI.Page 
{
    //Processfile process_file = new Processfile();
    DataTable data_table = new DataTable();
    BusinessLogic bll = new BusinessLogic();
    string ip = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                //DisableBtnsOnClick();
                MultiView1.ActiveViewIndex = 0;
            }
        }
        catch (Exception ex)
        {
            ShowMessage(ex.Message, true);
        }
    }
    private void ShowMessage(string Message, bool Error)
    {
        if (Error) { lblmsg.ForeColor = System.Drawing.Color.Red; lblmsg.Font.Bold = true; }
        else { lblmsg.ForeColor = System.Drawing.Color.Green; lblmsg.Font.Bold = true; }
        if (Message == ".")
        {
            lblmsg.Text = ".";
        }
        else
        {
            lblmsg.Text = "MESSAGE: " + Message.ToUpper();
        }
    }
    private void DisableBtnsOnClick()
    {
        string strProcessScript = "this.value='Working...';this.disabled=true;";
        btnlogin.Attributes.Add("onclick", strProcessScript + ClientScript.GetPostBackEventReference(btnlogin, "").ToString());  
        btnchange.Attributes.Add("onclick", strProcessScript + ClientScript.GetPostBackEventReference(btnchange, "").ToString());         
    }
    protected void Btnlogin_Click(object sender, EventArgs e)
    {
        try
        {
            string userId = txtUsername.Text.Trim();
            string passwd = txtpassword.Text.Trim();
            if (userId.Equals(""))
            {
                ShowMessage("UserName Required", true);
                txtUsername.Focus();
            }
            else if (passwd.Equals(""))
            {
                ShowMessage("Password Required", true);
                txtpassword.Focus();
            }
            else
            {
                System_login(userId, passwd);
            }
        }
        catch (Exception ex)
        {
            ShowMessage(ex.Message, true);
        }
    }

    private void System_login(string UserId, string passwd)
    {
        string msg = "";
        SystemUser user = bll.GetSystemUserByUserId(UserId);//process_file.LoginDetails(userId, passwd);
        
        if (user.StatusCode != Globals.SUCCESS_STATUS_CODE)
        {
            msg = "FAILED: " + user.StatusDesc;
            bll.InsertIntoAuditLog("LOGIN", "", user.CompanyCode, UserId, "Unsuccessfull login of User with ID :" + UserId + " Error: " + msg);
            ShowMessage(msg, true);
            return;
        }


        string md5HashOfPassword = SharedCommons.GenerateMD5Hash(passwd);

        if (user.Password.ToUpper() != md5HashOfPassword.ToUpper())
        {
            msg = "FAILED: INVALID USERNAME OR PASSWORD SUPPLIED";
            bll.InsertIntoAuditLog("LOGIN", "", user.CompanyCode, UserId, "Unsuccessfull login of User with ID :" + UserId + " Error: " + msg);

            if (MaxInvalidLoginsIsExceeded())
            {
                bll.InsertIntoAuditLog("DE-ACTIVATION", "", user.CompanyCode, user.UserId, "Deactivated: Maximum number of Invalid Logins Reached by User[" + user.UserId + "]");
                bll.DeactivateUser(user.UserId, "PORTAL", ip, user.CompanyCode);// user.PhoneNumber
                msg = "User Credentials Deactivated: Maximum number of Invalid Logins Reached";
            }


            bll.LogUserLogin("PORTAL", ip, user.UserId, this.Session.SessionID, "555", msg, "LOGIN");

            ShowMessage(msg, true);
            return;
        }

        //user has to reset password
        if (user.ResetPassword)
        {
            msg = "RESET PASSWORD";
            bll.LogUserLogin("PORTAL", ip, user.UserId, this.Session.SessionID, "111", msg, "LOGIN");

            bll.InsertIntoAuditLog("LOGIN", "", user.CompanyCode, user.UserId, "Unsuccessfull login of User with ID :" + user.UserId + " Error: " + msg);
            CallResetPassword(user);
            ShowMessage(msg, true);
            return;
        }

        //user password has expired
        //if (bll.PasswordExpired(user.UserId, user.CompanyCode, ip))
        //{
        //    msg = "YOUR PASSWORD EXPIRED AND NEEDS TO BE CHANGED";
        //    bll.LogUserLogin("PORTAL", "", ip, user.UserId, this.Session.SessionID, "222", msg, "LOGIN");

        //    CallResetPassword(user);

        //    bll.ShowMessage(lblmsg, msg, true);
        //    return;
        //}


        AssignSessionVariables(user);
        
        ShowMessage("System Logon denied", true);
    }

    private void AssignSessionVariables(SystemUser user)//, Company usersCompany, InterConnect.UBAApi.UserType usersRole)
    {
        //string LastLoginDateTime = bll.GetLastLoginDateTime(user.Id,user.BankCode);
        Session["User"] = user;
        Session["RoleCode"] = user.RoleCode;
        Session["Username"] = user.Name;
        //Session["UsersCompany"] = usersCompany;
        //Session["UsersRole"] = usersRole;

        //bll.InsertIntoAuditLog("LOGIN", "", user.CompanyCode, user.UserId, "Successfull login of User with ID :" + user.UserId + " at " + DateTime.Now);
        Server.Transfer("Admin.aspx", false);
    }

    private bool MaxInvalidLoginsIsExceeded()
    {
        string loginCount = ViewState["InvalidLoginCount"] == null ? "0" : ViewState["InvalidLoginCount"].ToString();
        int InvalidLoginCount = GetInt(loginCount);
        InvalidLoginCount = InvalidLoginCount + 1;

        ViewState["InvalidLoginCount"] = "" + InvalidLoginCount;

        SystemSetting setting = bll.GetSystemSettingById("MAX_INVALID_LOGINS", "Lesh");
        int max_invalid_logs = GetInt(setting.SettingValue);
        if (InvalidLoginCount > max_invalid_logs)
        {
            return true;
        }
        return false;
    }

    private int GetInt(string aString)
    {
        int count = 0;
        try
        {
            count = Convert.ToInt32(aString);
        }
        catch (Exception ex)
        {
        }
        return count;
    }
    private void CallLogin(DataTable data_table)
    {
        Session["UserId"] = data_table.Rows[0]["UserId"].ToString();
        Session["Username"] = data_table.Rows[0]["Username"].ToString();
        Session["FullName"] = data_table.Rows[0]["FullName"].ToString();
        Session["RoleCode"] = data_table.Rows[0]["RoleCode"].ToString();
        Session["UserRole"] = data_table.Rows[0]["UserRole"].ToString();
        Session["Vendor"] = data_table.Rows[0]["Vendor"].ToString();
        Session["SenderId"] = data_table.Rows[0]["SenderId"].ToString();
        Session["Phone"] = data_table.Rows[0]["Phone"].ToString();
        Session["Mask"] = data_table.Rows[0]["Mask"].ToString();
        Session["VendorCode"] = data_table.Rows[0]["VendorCode"].ToString();
        Session["VendorName"] = data_table.Rows[0]["VendorName"].ToString();
        Session["Email"] = data_table.Rows[0]["Email"].ToString();

        if (Session["Username"] != null)
        {
            Response.Redirect("Admin.aspx");
        }
        
    }

    private void CallResetPassword(SystemUser user)
    {
        try
        {
            ShowMessage("System Password Reset Required, Please Reset Password and Continue", true);
            MultiView1.SetActiveView(View1);
            Session["PassUser"] = user;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
   
 
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        MultiView1.ActiveViewIndex = 0;
        ShowMessage(".", true);
    }

    protected void btnChangenewPassword_Click(object sender, EventArgs e)
    {
        try
        {
            string newpassword = txtnewpassword.Text;
            string confirmPassword = txtConfirmnewpassword.Text;
            if (newpassword.Equals(""))
            {
                ShowMessage("ENTER NEW PASSWORD", true);
                txtnewpassword.Focus();
            }
            else if (confirmPassword.Equals(""))
            {
                ShowMessage("CONFIRM NEW PASSWORD", true);
                txtConfirmnewpassword.Focus();
            }
            else
            {
                if (newpassword.Equals(confirmPassword))
                {

                    if (Session["PassUser"] != null)
                    {
                        SystemUser user = Session["PassUser"] as SystemUser;
                        if (!user.UserId.Equals(""))
                        {
                            if (SharedCommons.GenerateMD5Hash(newpassword) == user.Password)
                            {
                                ShowMessage("YOUR NEW PASSWORD CANNOT BE THE SAME AS THE PREVIOUS ONE", true);
                            }
                            else if (!bll.ObeysPasswordPolicy(newpassword, user.CompanyCode))
                            {
                                ShowMessage("Your new password should contain atleast one uppercase and lowercase letters, a special character,a number and Should be atleast 8 characters in Length", true);
                            }
                            else if (bll.PasswordHasBeenUsed(user.UserId, SharedCommons.GenerateMD5Hash(newpassword)))
                            {
                                ShowMessage("You have used this password before, please create another one", true);
                            }
                            else
                            {
                                string oldPassword = user.Password;
                                user.Password = SharedCommons.GenerateMD5Hash(newpassword);
                                Result result = bll.ChangeUsersPassword(user.UserId, user.CompanyCode, user.Password, user.RoleCode);
                                if (result.StatusCode == "0")
                                {
                                    bll.Log("PasswordTracker_Update", new string[] { user.UserId, oldPassword, ip });
                                    string msg = "Password Changed Successfully";
                                    ShowMessage(msg, false);
                                    MultiView1.ActiveViewIndex = 0;
                                    clearControls();
                                }
                                else
                                {
                                    string msg = result.StatusDesc;
                                    ShowMessage(msg, true);
                                }
                            }
                        }
                        else
                        {
                            ShowMessage("FAILED TO DETERMINE USER DETAILS", true);
                        }
                    }
                    else
                    {
                        ShowMessage("FAILED TO DETERMINE USER DETAILS", true);
                    }
                }
                else
                {
                    ShowMessage("PASSWORD MISMATCH", true);
                }
            }
        }
        catch (Exception ex)
        {
            ShowMessage("FAILED: " + ex.Message, true);
        }
    }

    public void clearControls()
    {
        txtnewpassword.Text = "";
        txtConfirmnewpassword.Text = "";
    }
    protected void btnlogin_Click(object sender, EventArgs e)
    {
        try
        {
            //
            string pass = Encryption.encrypt.DecryptString("jXU+xuRePS4KqcaRDwPW4Q==", "25011Pegsms2322");
            string userId = txtUsername.Text.Trim();
            string passwd = txtpassword.Text.Trim();
            if (userId.Equals(""))
            {
                ShowMessage("UserName Required", true);
                txtUsername.Focus();
            }
            else if (passwd.Equals(""))
            {
                ShowMessage("Password Required", true);
                txtpassword.Focus();
            }
            else
            {
                System_login(userId, passwd);
            }
        }
        catch (Exception ex)
        {
            ShowMessage(ex.Message, true);
        }
    }

    protected void LoadForgotPassword(object sender, EventArgs e)
    {
        CallForgotPassword();
    }

    protected void LoadRegisterClient(object sender, EventArgs e)
    {
        Server.Transfer("~/RegisterClient.aspx");
    }
    private void CallForgotPassword()
    {
        try
        {
            MultiView1.SetActiveView(View3);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void btnForgotPassword_Click(object sender, EventArgs e)
    {
        SystemUser user = bll.GetSystemUserByUserId(txtUserId.Text);
        try
        {
            // validate the Captcha to check we're not dealing with a bot
            //string userInput = txtCaptcha.Text.Trim().ToUpper();
            //bool isHuman = ExampleCaptcha.Validate(userInput);
            //txtCaptcha.Text = null; // clear previous user input

            //if (isHuman)
            //{
            //    // TODO: proceed with protected action
            //}
            //else
            //{
            //    ShowMessage("INCORRECT CAPTCHA", false);
            //    return;
            //}
            //get user using his UserId
            

            //unable to find user
            if (user.StatusCode != "0")
            {
                string msg = user.StatusDesc;
                bll.LogUserLogin("PORTAL", ip, user.UserId, this.Session.SessionID, user.StatusCode, user.StatusDesc, "LOGIN");

                bll.InsertIntoAuditLog("LOGIN", "", user.CompanyCode, user.UserId, "Unsuccessfull Password Reset of User with ID :" + user.UserId + " Error: " + msg);
                ShowMessage(msg, true);
                return;
            }
            ////use redis to save to cache
            //string host = "localhost";

            //string key = user.UserId;
            //// Retrieve data from the cache using the key
            //string data = Get(host, key);
            //int i = 1;

            //if (string.IsNullOrEmpty(data))
            //{
            //    // Store data in the cache
            //    Save(host, key, i.ToString());
            //}
            //else
            //{
            //    if (Convert.ToInt16(data) >= 3)
            //    {
            //        bll.ShowMessage(lblmsg, "Password cannot be changed more than 3 times in 24 hours", true);
            //        return;
            //    }
            //    else
            //    {
            //        // Store data in the cache with increased count
            //        i = Convert.ToInt16(data) + 1;
            //        Save(host, key, i.ToString());
            //    }
            //}
            //generate a new password for the user
            string Password = bll.GeneratePassword();
            user.Password = SharedCommons.GenerateMD5Hash(Password);
            ////user.ResetPassword = true;

            //update the password of the user at Pegasus
            Result result = bll.UpdateUserPassword(user);

            //failed to update
            if (result.StatusCode != "0")
            {
                ShowMessage("FAILED: " + result.StatusDesc, false);
                return;
            }

            //send the user the new credentials
            Result sendResult = bll.ResendCredentials(user, "Password", Password);

            //failed to send mail
            if (sendResult.StatusCode != "0")
            {
                //ShowMessage("FAILED: PASSWORD WAS RESET BUT EMAIL SEND TO [" + user.Email + "] FAILED : " + result.StatusDesc, false);
                //with no mail displayed to the user
                ShowMessage("FAILED: PASSWORD WAS RESET BUT EMAIL SEND TO YOUR ASSOCIATED MAIL ACCOUNT FAILED : " + result.StatusDesc, false);
                return;
            }

            //we are good
            //ShowMessage("YOUR PASSWORD HAS BEEN RESET AND AN EMAIL HAS BEEN SENT TO " + user.Email, false);
            //with no mail displayed to the user
            ShowMessage("YOUR PASSWORD HAS BEEN RESET AND AN EMAIL HAS BEEN SENT TO YOUR ASSOCIATED MAIL ACCOUNT", false);
            MultiView1.SetActiveView(View2);
        }
        catch (Exception ex)
        {
            bll.LogError(user.CompanyCode, "", "FORGOT-PWD" + ex.Message + ex.StackTrace, "", "EXCEPTION", "");
            ShowMessage("FAILED: INTERNAL ERROR", true);
        }
    }
}

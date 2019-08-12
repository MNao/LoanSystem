using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Encryption;
using InterConnect.LeshLaonApi;
using InterConnect;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

public partial class AddUser : System.Web.UI.Page
{   
    SystemUser user;
    BusinessLogic bll = new BusinessLogic();
    LeshLoanAPI Client = new LeshLoanAPI();
    //PhoneValidator phone_validity = new PhoneValidator();
    protected void Page_Load(object sender, EventArgs e)
    {
        //System.Net.ServicePointManager.ServerCertificateValidationCallback = RemoteCertificateValidation;
        //System.Net.ServicePointManager.Expect100Continue = false;
        try
        {
            user = Session["User"] as SystemUser;
            if (IsPostBack == false)
            {
                if ((Session["Username"] == null))
                {
                    Response.Redirect("Default.aspx");
                }
                MultiView2.ActiveViewIndex = -1;

                string UserID = Request.QueryString["UserID"];
                string CompanyCode = Request.QueryString["CompanyCode"];
                string UserType = Request.QueryString["UserType"];
                string Type = Request.QueryString["Type"];
                string Status = Request.QueryString["Status"];
                LoadData();
                if (Request.QueryString["transferid"] != null)
                {
                    //string UserCode = Encryption.encrypt.DecryptString(Request.QueryString["transferid"].ToString(), "25011Pegsms2322");
                    //LoadControls(UserCode);
                }
                else if (!string.IsNullOrEmpty(UserID))
                {
                    LoadEntityData(UserID, CompanyCode, UserType, Type, Status);
                }
            }
        }
        catch (Exception ex)
        {
            ShowMessage(ex.Message, true);
        }
    }

    protected void LoadData()
    {
        bll.LoadRolesIntoDropDown(user.CompanyCode, user, ddlUserType);
        //bll.LoadBranchesIntoDropDown(user.CompanyCode, user, ddBranchCode);
        btnEdit.Visible = false;
        btnApprove.Visible = false;
    }

    private bool RemoteCertificateValidation(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
    {
        throw new NotImplementedException();
    }

    private void LoadEntityData(string UserId, string BankCode, string UserType, string Type, string Status)
    {
        btnOK.Visible = false;
        btnEdit.Visible = true;
        MultiView2.SetActiveView(View2);
        
        SystemUser Edituser = bll.GetSystemUserByUserId(UserId);
        string [] name = Edituser.Name.Split(' ');
        txtfname.Text = name[0];
        txtlname.Text = name[1];
        txtemail.Text = Edituser.Email;//UserId;
        ddlUserType.SelectedItem.Text = UserType;
        //string UserRole = bll.GetUserRoleName(UserType);
        ddlUserType.SelectedItem.Value = UserType;

        if (Status == "True")
        {
            ChkDeactivate.Visible = true;
            chkActive.Visible = false;
        }
        else
        {
            ChkDeactivate.Visible = false;
            chkActive.Visible = true;
        }

        if (Type == "Approve")
        {
            ChkReset.Visible = false;
            btnApprove.Visible = true;
            btnEdit.Visible = false;
            ChkDeactivate.Visible = false;

            ddlUserType.Enabled = false;
            //ddBranchCode.Enabled = false;
            txtemail.Enabled = false;
            txtfname.Enabled = false;
            txtlname.Enabled = false;
            //txtphone.Enabled = false;
        }
        else
        {
            txtemail.Enabled = false;
        }
        //ddlUserType.Enabled = false;
        //ddBranchCode.Enabled = false;
        
        //txtfname.Enabled = false;
        //txtlname.Enabled = false;
        //txtphone.Enabled = false;

        //if (IsActive == "true")
        //{
        //    ddIsActive.SelectedItem.Value = IsActive;
        //    ddIsActive.SelectedItem.Text = bool.Parse(IsActive);
        //}
        //else
        //{
        //    IsActive = false;
        //}
    }
    
    private void ShowMessage(string Message, bool Error)
    {
        Label lblmsg = (Label)Master.FindControl("lblmsg");
        if (Error) { lblmsg.ForeColor = System.Drawing.Color.Red; lblmsg.Font.Bold = false; }
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
    protected void ddlUserType_DataBound(object sender, EventArgs e)
    {
        ddlUserType.Items.Insert(0, new ListItem(" Select User Role ", "0"));
    }
    protected void btnOK_Click(object sender, EventArgs e)
    {
        try
        {
            SystemUser RegUser = GetSystemUserDetails();
            string Password = RegUser.Password;
            RegUser.Password = SharedCommons.GenerateUserPassword(RegUser.Password);
        //    bool reset = CheckBox1.Checked;
            string check_status = validate_input(RegUser.Name,RegUser.Name, RegUser.UserId, RegUser.RoleCode);

            
            if (!check_status.Equals("OK"))
            {
                ShowMessage(check_status, true);
            }
            else
            {
                
                Result user_save = Client.SaveSystemUser(RegUser);

                if (user_save.StatusCode != "0")
                {
                    //MultiView2.ActiveViewIndex = 0;
                    ShowMessage(user_save.StatusDesc, true);
                    return;
                }
                    ShowMessage("USER SAVED SUCCESSFULLY", false);
                    Clear_contrls();
                //bll.SendCredentialsToUser(RegUser, Password);
                bll.InsertIntoAuditLog("USER-CREATION","SYSTEMUSERS", user.CompanyCode, user.UserId, "USER CREATED SUCCESSFULLY");
            }
        }
        catch (Exception ex)
        {
            ShowMessage(ex.Message, true);
        }
    }

    public SystemUser GetSystemUserDetails()
    {
        SystemUser Details = new SystemUser();

            Details.CompanyCode = user.CompanyCode;
            Details.UserId = txtemail.Text.Trim();
            string fname = txtfname.Text.Trim();
            string lname = txtlname.Text.Trim();
            Details.Name = String.Concat(fname,' ', lname);
            Details.Email = txtemail.Text.Trim();
            Details.RoleCode = ddlUserType.SelectedValue.ToString();
            Details.IsActive = "True";//chkActive.Checked.ToString();
            Details.Password = bll.GeneratePassword();
            Details.ModifiedBy = this.user.UserId;
            return Details;
    }

    public SystemUser GetSystemUserToEdit()
    {
        SystemUser Details = new SystemUser();

        Details.CompanyCode = user.CompanyCode;
        Details.UserId = txtemail.Text.Trim();
        string fname = txtfname.Text.Trim();
        string lname = txtlname.Text.Trim();
        Details.Name = String.Concat(fname, ' ', lname);
        Details.Email = txtemail.Text.Trim();
        Details.RoleCode = ddlUserType.SelectedValue.ToString();
        Details.IsActive = chkActive.Checked.ToString();
        Details.Password = bll.GeneratePassword();
        Details.ModifiedBy = this.user.UserId;
        Details.ResetPassword = ChkReset.Checked;
        return Details;
    }

    private void Clear_contrls()
    {
        lblCode.Text = "0";
        txtUserName.Text = "";
        //txtphone.Text = "";
        txtlname.Text = "";
        txtfname.Text = "";
        txtemail.Text = "";
        //ddlAreas.SelectedIndex = ddlAreas.Items.IndexOf(ddlAreas.Items.FindByValue("0"));
        ddlUserType.SelectedIndex = ddlUserType.Items.IndexOf(ddlUserType.Items.FindByValue("0"));
        MultiView2.ActiveViewIndex = -1;
    }

    private string validate_input(string fname, string lname, string email, string role)
    {
        string output = "";
        if (fname.Equals(""))
        {
            output = "First Name Required";
            txtfname.Focus();
        }
        else if (lname.Equals(""))
        {
            output = "Last Name Required";
            txtlname.Focus();
        }
        else if (email.Equals(""))
        {
            output = "Email/UserID Required";
            txtemail.Focus();
        }
        //else if (!phone_validity.PhoneNumbersOk(phone))
        //{
        //    output = "Enter Valid Mobile Phone Number Required";
        //    txtphone.Focus();
        //}
        //else if (vendor.Equals("0"))
        //{
        //    output = "Select a vendor";
        //}
        else if (role.Equals(""))
        {
            output = "Select User Role";
        }
        else
        {
            output = "OK";
        }
        return output;
    }
    //protected void ddlAreas_DataBound(object sender, EventArgs e)
    //{
    //    ddlAreas.Items.Insert(0, new ListItem(" Select Vendor ", "0"));
    //}

    protected void btnEdit_Click(object sender, EventArgs e)
    {
        Label lblmsg = (Label)Master.FindControl("lblmsg");
        try
        {
            Result result;
            string msg = "";
            SystemUser aclient = GetSystemUserToEdit();
            string Password = aclient.Password;
            aclient.Password = SharedCommons.GenerateUserPassword(aclient.Password);
            if (ChkReset.Checked)
            {
                result = bll.ReActivateUser(aclient, user.UserId, "RESET");
                if (result.StatusCode != Globals.SUCCESS_STATUS_CODE)
                {
                    msg = "FAILED: " + result.StatusDesc;
                    bll.ShowMessage(lblmsg, msg, true, Session);
                    return;
                }

                msg = "SYSTEM USER DETAILS RESET SUCCESSFULLY";
                bll.ShowMessage(lblmsg, msg, false, Session);
                Clear_contrls();
                bll.SendCredentialsToUser(aclient, Password);
                bll.InsertIntoAuditLog("RESET-USER", "SYSTEMUSERS", user.CompanyCode, user.UserId, "USER PASSWORD RESET SUCCESSFULLY");
                return;
            }

            if (chkActive.Checked)
            {
                result = bll.ReActivateUser(aclient, user.UserId, "JUSTACTIVATE");
                if (result.StatusCode != Globals.SUCCESS_STATUS_CODE)
                {
                    msg = "FAILED: " + result.StatusDesc;
                    bll.ShowMessage(lblmsg, msg, true, Session);
                    return;
                }

                msg = "SYSTEM USER ACTIVATED SUCCESSFULLY.";
                bll.ShowMessage(lblmsg, msg, false, Session);
                Clear_contrls();
                bll.SendCredentialsToUser(aclient, Password);
                bll.InsertIntoAuditLog("ACTIVATE-USER", "SYSTEMUSERS", user.CompanyCode, user.UserId, "USER ACTIVATED SUCCESSFULLY");
                return;
            }
           if(ChkDeactivate.Checked)
            {
                result = bll.ReActivateUser(aclient, user.UserId, "DEACTIVATE");
                if (result.StatusCode != Globals.SUCCESS_STATUS_CODE)
                {
                    msg = "FAILED: " + result.StatusDesc;
                    bll.ShowMessage(lblmsg, msg, true, Session);
                    return;
                }

                msg = "SYSTEM USER DETAILS DE-ACTIVATED SUCCESSFULLY.";
                bll.ShowMessage(lblmsg, msg, false, Session);
                Clear_contrls();
                //bll.SendCredentialsToUser(aclient, Password);
                bll.InsertIntoAuditLog("DEACTIVATE-USER", "SYSTEMUSERS", user.CompanyCode, user.UserId, "USER DE-ACTIVATED SUCCESSFULLY");
                return;
            }

            result = bll.AddEditedUserToTable(aclient, user.UserId);
            if (result.StatusCode != Globals.SUCCESS_STATUS_CODE)
            {
                msg = "FAILED: " + result.StatusDesc;
                bll.ShowMessage(lblmsg, msg, true, Session);
                return;
            }
            msg = "SYSTEM USER DETAILS EDITED SUCCESSFULLY PENDING APPROVAL";
            Clear_contrls();
            bll.InsertIntoAuditLog("EDIT-USER", "SYSTEMUSERS", user.CompanyCode, user.UserId, "USER EDITED SUCCESSFULLY");
            bll.ShowMessage(lblmsg, msg, false, Session);
        }
        catch (Exception ex)
        {
            bll.LogError(user.CompanyCode, "", "EDIT-SYSTEM USER" + ex.Message + ex.StackTrace, "EXCEPTION", "", "");
            bll.ShowMessage(lblmsg, ex.Message, true, Session);
        }
    }

    protected void btnApprove_Click()
    {
        Label lblmsg = (Label)Master.FindControl("lblmsg");
        try
        {
            string msg = "";
            SystemUser aclient = GetSystemUserToEdit();
            string Password = aclient.Password;
            aclient.Password = SharedCommons.GenerateUserPassword(aclient.Password);
            if (chkActive.Checked)
            {
                Result result = bll.ReActivateUser(aclient, user.UserId, "ACTIVATE");
                if (result.StatusCode != Globals.SUCCESS_STATUS_CODE)
                {
                    msg = "FAILED: " + result.StatusDesc;
                    bll.ShowMessage(lblmsg, msg, true, Session);
                    return;
                }

                msg = "SYSTEM USER DETAILS ACTIVATED SUCCESSFULLY. AN EMAIL WITH CREDENTIALS HAS BEEN SENT TO THE USER EMAIL";
                bll.ShowMessage(lblmsg, msg, false, Session);
                Clear_contrls();
                bll.SendCredentialsToUser(aclient, Password);
                bll.InsertIntoAuditLog("APPROVE-USER", "SYSTEMUSERS", user.CompanyCode, user.UserId, "USER ACTIVATED AND APPROVED SUCCESSFULLY");
                return;
            }

            msg = "SYSTEM USER STILL INACTIVE";
            bll.ShowMessage(lblmsg, msg, true, Session);
        }
        catch (Exception ex)
        {
            bll.LogError(user.CompanyCode, "", "APPROVE-SYSTEM USER" + ex.Message + ex.StackTrace, "EXCEPTION", "", "");

            bll.ShowMessage(lblmsg, ex.Message, true, Session);
        }
    }
}

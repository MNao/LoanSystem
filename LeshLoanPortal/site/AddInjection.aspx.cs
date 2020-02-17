using InterConnect.LeshLaonApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class AddInjection : System.Web.UI.Page
{
    SystemUser user;
    BusinessLogic bll = new BusinessLogic();
    LeshLoanAPI Client = new LeshLoanAPI();
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            user = Session["User"] as SystemUser;
            if (IsPostBack == false)
            {
                if ((Session["Username"] == null) || (Session["RoleCode"] == null))
                {
                    Response.Redirect("Default.aspx");
                }
                //MultiView2.ActiveViewIndex = -1;


                string InjectionID = Request.QueryString["InjectionID"];
                string CompanyCode = Request.QueryString["CompanyCode"];
                LoadData();
                if (!string.IsNullOrEmpty(InjectionID))
                {
                    LoadEntityData(InjectionID, CompanyCode);
                }
                else if (Request.QueryString["transferid"] != null)
                {
                    //string UserCode = Encryption.encrypt.DecryptString(Request.QueryString["transferid"].ToString(), "25011Pegsms2322");
                    //LoadControls(UserCode);
                }
                
            }
        }
        catch (Exception ex)
        {
            ShowMessage(ex.Message, true);
        }
    }

    private void LoadEntityData(string injectionID, string companyCode)
    {
        btnSubmit.Visible = false;
        btnEdit.Visible = true;
        btnBack.Visible = true;

        InterConnect.LeshLaonApi.Injection InjectionDet = bll.GetInjectionDetails(user, injectionID);
        txtInjectionNo.Text = injectionID;
        txtname.Text = InjectionDet.InjectorName;
        txtInjectedAmount.Text = InjectionDet.Amount;
        txtInjectionDate.Text = InjectionDet.InjectionDate;
        txtInjRepayAmnt.Text = InjectionDet.RepaymentAmount;
        txtInjRepayDate.Text = InjectionDet.RepaymentDate;
        txtEmail.Text = InjectionDet.Email;
        txtPhoneNo.Text = InjectionDet.PhoneNo;
        

        txtInjectionNo.Enabled = false;
    }

    public void LoadData()
    {

        txtInjectionNo.Text = bll.GenerateSystemCode("INJ");
        txtInjectionNo.Enabled = false;
        btnEdit.Visible = false;
        btnBack.Visible = false;
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

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        try
        {

            Injection Inj = GetInjectionDetails();
            //validate Injection details input
            string check_status = validate_input(Inj.InjectorName,Inj.Amount, Inj.InjectionDate, Inj.RepaymentAmount, Inj.RepaymentDate);


            if (!check_status.Equals("OK"))
            {
                ShowMessage(check_status, true);
            }
            else
            {
                //save client additional details
                Result user_save = Client.SaveInjection(Inj);

                if (user_save.StatusCode != "0")
                {
                    //MultiView2.ActiveViewIndex = 0;
                    ShowMessage(user_save.StatusDesc, true);
                    return;
                }
                ShowMessage("INJECTION DETAILS SAVED SUCCESSFULLY", false);
                Clear_contrls();
                btnBack.Visible = true;
                //bll.InsertIntoAuditLog("USER-CREATION", "SYSTEMUSERS", user.CompanyCode, user.UserId, "USER CREATED SUCCESSFULLY");
                //MultiView1.SetActiveView(LoanDetails);

            }

        }
        catch (Exception ex)
        {

        }
    }

    private void Clear_contrls()
    {
        
        txtInjectionNo.Text = "";
        txtname.Text="";
        txtInjectedAmount.Text="";
        txtInjectionDate.Text="";
        txtInjRepayAmnt.Text="";
        txtInjRepayDate.Text="";
    }

    private Injection GetInjectionDetails()
    {
        Injection Inj = new Injection();
        Inj.CompanyCode = user.CompanyCode;
        Inj.InjectionNumber = txtInjectionNo.Text;
        Inj.InjectorName = txtname.Text;
        Inj.Amount =txtInjectedAmount.Text;
        Inj.InjectionDate = txtInjectionDate.Text;
        Inj.RepaymentAmount = txtInjRepayAmnt.Text;
        Inj.RepaymentDate = txtInjRepayDate.Text;
        Inj.PhoneNo = txtPhoneNo.Text;
        Inj.Email = txtEmail.Text;
        Inj.ModifiedBy = user.UserId;
        return Inj;
    }

    private string validate_input(string Name, string Amount, string Date, string RepayAmount, string RepayDate)
    {
        string output = "";
        if (Name.Equals(""))
        {
            output = "Injector's Name Required";
            txtname.Focus();
        }
        else if (Amount.Equals(""))
        {
            output = "Injection Amount Required";
            txtInjectedAmount.Focus();
        }
        else if (Date.Equals(""))
        {
            output = "Date Required";
            txtInjectionDate.Focus();
        }
        else if (RepayAmount.Equals(""))
        {
            output = "Repayment Amount Required";
            txtInjRepayAmnt.Focus();
        }
        else if (RepayDate.Equals(""))
        {
            output = "Repayment Date Required";
            txtInjRepayDate.Focus();
        }

        else
        {
            output = "OK";
        }
        return output;
    }

    protected void btnEdit_Click(object sender, EventArgs e)
    {
        try
        {

            Injection Inj = GetInjectionDetails();
            //validate Injection details input
            string check_status = validate_input(Inj.InjectorName, Inj.Amount, Inj.InjectionDate, Inj.RepaymentAmount, Inj.RepaymentDate);


            if (!check_status.Equals("OK"))
            {
                ShowMessage(check_status, true);
            }
            else
            {
                //save client additional details
                Result user_save = Client.SaveInjection(Inj);

                if (user_save.StatusCode != "0")
                {
                    //MultiView2.ActiveViewIndex = 0;
                    ShowMessage(user_save.StatusDesc, true);
                    return;
                }
                ShowMessage("INJECTION DETAILS EDITED SUCCESSFULLY", false);
                Clear_contrls();
                btnBack.Visible = true;
                //bll.InsertIntoAuditLog("USER-CREATION", "SYSTEMUSERS", user.CompanyCode, user.UserId, "USER CREATED SUCCESSFULLY");
                //MultiView1.SetActiveView(LoanDetails);

            }

        }
        catch (Exception ex)
        {
        }
        }

    protected void btnBack_Click(object sender, EventArgs e)
    {
        Response.Redirect("ViewInjections.aspx");
    }
}
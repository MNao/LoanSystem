using InterConnect.LeshLaonApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class AddSystemSetting : System.Web.UI.Page
{
    SystemUser user;
    LeshLoanAPI client = new LeshLoanAPI();
    BusinessLogic bll = new BusinessLogic();
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {

            user = Session["User"] as SystemUser;
            Session["IsError"] = null;
            string Id = Request.QueryString["BankCode"];

            //Session is invalid
            if (user == null)
            {
                Response.Redirect("Default.aspx?Msg=SESSION HAS EXPIRED");
            }
            //Page posting back user request
            else if (IsPostBack)
            {

            }
            //Load Old details
            else if (!string.IsNullOrEmpty(Id))
            {
                LoadEntityData(Id);
            }
            //First time Request
            else
            {
                LoadData();
            }
        }
        catch (Exception ex)
        {
            //something is wrong...show the error
            ShowMessage(ex.Message, true);
        }
    }

    private void LoadData()
    {
        btnSubmit.Visible = true;
        btnEdit.Visible = false;
        //bll.LoadCompaniesIntoDropDown(user, ddCompanies);

    }

    private void LoadEntityData(string id)
    {
        btnSubmit.Visible = false;
        btnEdit.Visible = true;

    }

    protected void btnEdit_Click(object sender, EventArgs e)
    {
        try
        {

        }
        catch (Exception ex)
        {
           ShowMessage(ex.Message, true);
        }
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            string msg = "";
            SystemSetting aclient = GetSystemSetting();
            Result result = client.SaveSystemSetting(aclient);

            if (result.StatusCode != Globals.SUCCESS_STATUS_CODE)
            {
                msg = "FAILED: " + result.StatusDesc;
                ShowMessage(msg,true);
                return;
            }


            msg = "SYSTEM SETTING DETAILS SAVED SUCCESSFULLY";
            ShowMessage(msg, false);
            Clear_Controls();
        }
        catch (Exception ex)
        {
            bll.LogError(user.CompanyCode,"",ex.Message, "SAVE-SYSTEM SETTING", "EXCEPTION", ex.StackTrace);
            ShowMessage(ex.Message, true);
        }
    }

    private void Clear_Controls()
    {
       
    }

    private SystemSetting GetSystemSetting()
    {
        SystemSetting user = new SystemSetting();
        user.CompanyCode = ddCompanies.SelectedValue;
        user.SettingCode = txtSettingCode.Text;
        user.SettingValue = txtSettingValue.Text;
        user.ModifiedBy = this.user.UserId;
        return user;
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
}
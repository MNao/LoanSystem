using InterConnect.LeshLaonApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class AddInterest : System.Web.UI.Page
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
            string Id = Request.QueryString["CompanyCode"];
            string InterestCode = Request.QueryString["InterestCode"];

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
                LoadEntityData(Id,InterestCode);
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
        bll.LoadCompanysIntoDropDownALL(user, ddCompanies);

    }

    private void LoadEntityData(string id, string InterestCode)
    {
        btnSubmit.Visible = false;
        btnEdit.Visible = true;

        SystemSetting Setting = bll.GetInterestSetting(id, InterestCode);
        if (Setting.StatusCode == "0")
        {
            ddCompanies.SelectedItem.Value = id;
            ddCompanies.SelectedItem.Text = id;
            ddCompanies.Enabled = false;

            txtSettingName.Text = Setting.SettingName;
            txtSettingCode.Text = Setting.SettingCode;
            txtSettingValue.Text = Setting.SettingValue;
            txtSettingCode.Enabled = false;
        }
        else
        {
            ShowMessage(Setting.StatusDesc, true);
        }
        

    }

    protected void btnEdit_Click(object sender, EventArgs e)
    {
        try
        {
            string msg = "";
            SystemSetting setting = GetSystemSetting();
            if(string.IsNullOrEmpty(setting.SettingCode) || string.IsNullOrEmpty(setting.SettingValue))
            {
                ShowMessage("Please Provide Interest Details", true);
                return;
            }
            Result result = bll.SaveInterestSetting(user, setting);

            if (result.StatusCode != Globals.SUCCESS_STATUS_CODE)
            {
                msg = "FAILED: " + result.StatusDesc;
                ShowMessage(msg, true);
                return;
            }


            msg = "INTEREST DETAILS EDITED SUCCESSFULLY";
            ShowMessage(msg, false);
            Clear_Controls();
            Response.Redirect("ViewInterest.aspx");
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
            SystemSetting setting = GetSystemSetting();
            if (string.IsNullOrEmpty(setting.SettingCode) || string.IsNullOrEmpty(setting.SettingValue))
            {
                ShowMessage("Please Provide Interest Details", true);
                return;
            }
            Result result = bll.SaveInterestSetting(user,setting);

            if (result.StatusCode != Globals.SUCCESS_STATUS_CODE)
            {
                msg = "FAILED: " + result.StatusDesc;
                ShowMessage(msg, true);
                return;
            }


            msg = "INTEREST DETAILS SAVED SUCCESSFULLY";
            ShowMessage(msg, false);
            Clear_Controls();
        }
        catch (Exception ex)
        {
            bll.LogError(user.CompanyCode, "", ex.Message, "SAVE-INTEREST SETTING", "EXCEPTION", ex.StackTrace);
            ShowMessage(ex.Message, true);
        }
    }

    private void Clear_Controls()
    {
        txtSettingName.Text = "";
        txtSettingCode.Text = "";
        txtSettingValue.Text = "";
    }

    private SystemSetting GetSystemSetting()
    {
        SystemSetting setting = new SystemSetting();
        setting.CompanyCode = ddCompanies.SelectedValue;
        setting.SettingName = txtSettingName.Text;
        setting.SettingCode = txtSettingCode.Text;
        setting.SettingValue = txtSettingValue.Text;
        setting.ModifiedBy = this.user.UserId;
        return setting;
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
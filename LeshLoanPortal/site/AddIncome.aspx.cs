using InterConnect.LeshLaonApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class AddIncome : System.Web.UI.Page
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
                if ((Session["Username"] == null))
                {
                    Response.Redirect("Default.aspx");
                }
                //MultiView2.ActiveViewIndex = -1;


                string UserID = Request.QueryString["UserID"];
                string CompanyCode = Request.QueryString["CompanyCode"];
                string IncomeNo = Request.QueryString["IncomeID"];
                string Type = Request.QueryString["Type"];
                string Status = Request.QueryString["Status"];
                LoadData();
                if (Request.QueryString["transferid"] != null)
                {
                    //string UserCode = Encryption.encrypt.DecryptString(Request.QueryString["transferid"].ToString(), "25011Pegsms2322");
                    //LoadControls(UserCode);
                }
                else if (!string.IsNullOrEmpty(IncomeNo))
                {
                    LoadEntityData(CompanyCode, IncomeNo);
                }
            }
        }
        catch (Exception ex)
        {
            ShowMessage(ex.Message, true);
        }
    }

    private void LoadEntityData(string CompanyCode, string IncomeNo)
    {
        btnSubmit.Visible = false;
        btnEdit.Visible = true;
        Income Inc = bll.GetIncome(CompanyCode,IncomeNo);
        //ddCompany.SelectedItem.Text = Inc.CompanyCode;
        txtIncomeNo.Text = Inc.IncomeID;
        txtAmount.Text = Inc.Amount;
        txtIncomeDate.Text = Inc.IncomeDate;
        txtIncomeDesc.Text = Inc.Description;
        txtIncType.Text = Inc.Type;

        txtIncomeNo.Enabled = false;
        //txtAmount.Enabled = false;
        //txtIncomeDate.Enabled = false;
        //txtIncomeDesc.Enabled = false;
        //txtIncType.Enabled = false;
        
    }

    private void LoadData()
    {
        txtIncomeNo.Text = bll.GenerateSystemCode("INCO");
        txtIncomeNo.Enabled = false;
        btnEdit.Visible = false;
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

            Income Inco = GetIncomeDetails();
            //validate Injection details input
            string check_status = validate_input(Inco.Amount, Inco.IncomeDate, Inco.Description, Inco.Type);


            if (!check_status.Equals("OK"))
            {
                ShowMessage(check_status, true);
            }
            else
            {
                //save client additional details
                Result user_save = Client.SaveIncome(Inco);

                if (user_save.StatusCode != "0")
                {
                    //MultiView2.ActiveViewIndex = 0;
                    ShowMessage(user_save.StatusDesc, true);
                    return;
                }
                ShowMessage("INCOME DETAILS SAVED SUCCESSFULLY", false);
                Clear_contrls();
                //bll.InsertIntoAuditLog("USER-CREATION", "SYSTEMUSERS", user.CompanyCode, user.UserId, "USER CREATED SUCCESSFULLY");
                //MultiView1.SetActiveView(LoanDetails);
                Response.Redirect("AddIncome.aspx");

            }

        }
        catch (Exception ex)
        {

        }
    }

    private void Clear_contrls()
    {

        txtIncomeNo.Text = "";
        txtAmount.Text = "";
        txtIncomeDate.Text = "";
        txtIncomeDesc.Text = "";
        txtIncType.Text = "";
    }

    private Income GetIncomeDetails()
    {
        Income Inco = new Income();
        Inco.CompanyCode = user.CompanyCode;
        Inco.IncomeID = txtIncomeNo.Text;
        Inco.Amount = txtAmount.Text;
        Inco.IncomeDate = txtIncomeDate.Text;
        Inco.Description = txtIncomeDesc.Text;
        Inco.Type = txtIncType.Text;
        Inco.ModifiedBy = user.UserId;
        return Inco;
    }

    private string validate_input(string Amount, string Date, string Descr, string Type)
    {
        string output = "";
        if (Amount.Equals(""))
        {
            output = "Income Amount Required";
            txtAmount.Focus();
        }
        else if (Date.Equals(""))
        {
            output = "Date Required";
            txtIncomeDate.Focus();
        }
        else if (Descr.Equals(""))
        {
            output = "Income Description Required";
            txtIncomeDesc.Focus();
        }
        else if (Type.Equals(""))
        {
            output = "Income Type Required";
            txtIncType.Focus();
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

            Income Inco = GetIncomeDetails();
            //validate Injection details input
            string check_status = validate_input(Inco.Amount, Inco.IncomeDate, Inco.Description, Inco.Type);


            if (!check_status.Equals("OK"))
            {
                ShowMessage(check_status, true);
            }
            else
            {
                //save client additional details
                Result user_save = Client.SaveIncome(Inco);

                if (user_save.StatusCode != "0")
                {
                    //MultiView2.ActiveViewIndex = 0;
                    ShowMessage(user_save.StatusDesc, true);
                    return;
                }
                ShowMessage("INCOME DETAILS EDITED SUCCESSFULLY", false);
                Clear_contrls();
                //bll.InsertIntoAuditLog("USER-CREATION", "SYSTEMUSERS", user.CompanyCode, user.UserId, "USER CREATED SUCCESSFULLY");
                //MultiView1.SetActiveView(LoanDetails);
                Response.Redirect("AddIncome.aspx");

            }

        }
        catch (Exception ex)
        {

        }
    }
}
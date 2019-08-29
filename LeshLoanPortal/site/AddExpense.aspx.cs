using InterConnect.LeshLaonApi;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class AddExpense : System.Web.UI.Page
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
                string ExpenseID = Request.QueryString["ExpenseID"];
                string Type = Request.QueryString["Type"];
                string Status = Request.QueryString["Status"];
                LoadData();
                if (Request.QueryString["transferid"] != null)
                {
                    //string UserCode = Encryption.encrypt.DecryptString(Request.QueryString["transferid"].ToString(), "25011Pegsms2322");
                    //LoadControls(UserCode);
                }
                else if (!string.IsNullOrEmpty(ExpenseID))
                {
                    LoadEntityData(CompanyCode, ExpenseID);
                }
            }
        }
        catch (Exception ex)
        {
            ShowMessage(ex.Message, true);
        }
    }

    private void LoadEntityData(string CompanyCode, string ExpenseNo)
    {
        btnSubmit.Visible = false;
        btnEdit.Visible = true;
        Expense Exp = bll.GetExpense(CompanyCode, ExpenseNo);
        //ddCompany.SelectedItem.Text = Inc.CompanyCode;
        txtExpNo.Text = Exp.ExpenseID;
        txtAmount.Text = Exp.Amount;
        txtExpDate.Text = Exp.ExpenseDate;
        txtExpDesc.Text = Exp.Description;
        txtExpType.Text = Exp.Type;
        txtReceipt.Text = Exp.ReceiptNumber;

        txtExpNo.Enabled = false;
        //txtAmount.Enabled = false;
        //txtExpDate.Enabled = false;
        //txtExpDesc.Enabled = false;
        //txtExpType.Enabled = false;

    }

    private void LoadData()
    {
        txtExpNo.Text = bll.GenerateSystemCode("EXP");
        txtExpNo.Enabled = false;
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

            Expense Exp = GetExpenseDetails();
            //validate Injection details input
            string check_status = validate_input(Exp.Amount, Exp.ExpenseDate, Exp.Description, Exp.Type, Exp.ReceiptNumber);


            if (!check_status.Equals("OK"))
            {
                ShowMessage(check_status, true);
            }
            else
            {
                //save client additional details
                Result user_save = Client.SaveExpense(Exp);

                if (user_save.StatusCode != "0")
                {
                    //MultiView2.ActiveViewIndex = 0;
                    ShowMessage(user_save.StatusDesc, true);
                    return;
                }
                ShowMessage("EXPENSE DETAILS SAVED SUCCESSFULLY", false);
                Clear_contrls();
                //bll.InsertIntoAuditLog("USER-CREATION", "SYSTEMUSERS", user.CompanyCode, user.UserId, "USER CREATED SUCCESSFULLY");
                //MultiView1.SetActiveView(LoanDetails);
                Response.Redirect("AddExpense.aspx");

            }

        }
        catch (Exception ex)
        {

        }
    }

    private void Clear_contrls()
    {

        txtExpNo.Text = "";
        txtAmount.Text = "";
        txtExpDate.Text = "";
        txtExpDesc.Text = "";
        txtExpType.Text = "";
        txtReceipt.Text = "";
    }

    private Expense GetExpenseDetails()
    {
        Expense Exp = new Expense();
        Exp.CompanyCode = user.CompanyCode;
        Exp.ExpenseID = txtExpNo.Text;
        Exp.Amount = txtAmount.Text;
        Exp.ExpenseDate = txtExpDate.Text;
        Exp.Description = txtExpDesc.Text;
        Exp.Type = txtExpType.Text;
        Exp.ReceiptNumber = txtReceipt.Text;
        Exp.ModifiedBy = user.UserId;
        return Exp;
    }

    private string validate_input(string Amount, string Date, string Descr, string Type, string ReceiptNo)
    {
        string output = "";
        if (Amount.Equals(""))
        {
            output = "Expense Amount Required";
            txtAmount.Focus();
        }
        else if (Date.Equals(""))
        {
            output = "Date Required";
            txtExpDate.Focus();
        }
        else if (Descr.Equals(""))
        {
            output = "Expense Description Required";
            txtExpDesc.Focus();
        }
        else if (Type.Equals(""))
        {
            output = "Expense Type Required";
            txtExpType.Focus();
        }
        else if (ReceiptNo.Equals(""))
        {
            output = "Expense Receipt No Required";
            txtReceipt.Focus();
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
            Expense Exp = GetExpenseDetails();
            //validate Injection details input
            string check_status = validate_input(Exp.Amount, Exp.ExpenseDate, Exp.Description, Exp.Type, Exp.ReceiptNumber);


            if (!check_status.Equals("OK"))
            {
                ShowMessage(check_status, true);
            }
            else
            {
                //save client additional details
                Result user_save = Client.SaveExpense(Exp);

                if (user_save.StatusCode != "0")
                {
                    //MultiView2.ActiveViewIndex = 0;
                    ShowMessage(user_save.StatusDesc, true);
                    return;
                }
                ShowMessage("EXPENSE DETAILS EDITED SUCCESSFULLY", false);
                Clear_contrls();
                //bll.InsertIntoAuditLog("USER-CREATION", "SYSTEMUSERS", user.CompanyCode, user.UserId, "USER CREATED SUCCESSFULLY");
                //MultiView1.SetActiveView(LoanDetails);
                Response.Redirect("AddExpense.aspx");

            }

        }
        catch (Exception ex)
        {

        }
    }
}
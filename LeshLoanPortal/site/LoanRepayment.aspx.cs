using InterConnect.LeshLaonApi;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class LoanRepayment : System.Web.UI.Page
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
                string BankCode = Request.QueryString["BankCode"];
                string UserType = Request.QueryString["UserType"];
                string Type = Request.QueryString["Type"];
                string Status = Request.QueryString["Status"];
                //LoadData();
                if (Request.QueryString["transferid"] != null)
                {
                    //string UserCode = Encryption.encrypt.DecryptString(Request.QueryString["transferid"].ToString(), "25011Pegsms2322");
                    //LoadControls(UserCode);
                }
                else if (!string.IsNullOrEmpty(UserID))
                {
                    //LoadEntityData(UserID, BankCode, UserType, Type, Status);
                }
            }
        }
        catch (Exception ex)
        {
            ShowMessage(ex.Message, true);
        }
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

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        try
        {
            SearchDB();
        }
        catch (Exception ex)
        {
            ShowMessage(ex.Message, true);
        }
    }

    public void SearchDB()
    {
        string[] Params = GetSearchParameters();
        DataTable dt = bll.SearchLoanDetailsTableForRepayment(Params);
        if (dt.Rows.Count > 0)
        {
            dataGridResults.DataSource = dt;
            dataGridResults.DataBind();
            string msg = "Found " + dt.Rows.Count + " Records Matching Search Criteria";
            MultiView2.ActiveViewIndex = 0;
            Label lblmsg = (Label)Master.FindControl("lblmsg");
            bll.ShowMessage(lblmsg, msg, false, Session);
        }
        else
        {
            dataGridResults.DataSource = null;
            dataGridResults.DataBind();
            MultiView2.ActiveViewIndex = -1;
            string msg = "No Records Found Matching Search Criteria";
            Label lblmsg = (Label)Master.FindControl("lblmsg");
            bll.ShowMessage(lblmsg, msg, true, Session);
        }
    }

    private string[] GetSearchParameters()
    {
        List<string> searchCriteria = new List<string>();
        string ClientNo = txtClientNoSearch.Text.Trim();
        string LoanNo = txtLoanNoSearch.Text.Trim();
        string UserId = user.UserId;
        string Status = ddStatus.SelectedValue;
        string StartDate = txtStartDate.Text;
        string EndDate = txtEndDate.Text;
        
        searchCriteria.Add(ClientNo);
        searchCriteria.Add(LoanNo);
        searchCriteria.Add(UserId);
        searchCriteria.Add(Status);
        searchCriteria.Add(StartDate);
        searchCriteria.Add(EndDate);
        return searchCriteria.ToArray();
    }

    protected void dataGridResults_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        int index = 0;
        GridViewRow row;
        GridView grid = sender as GridView;
        index = Convert.ToInt32(e.CommandArgument);
        row = grid.Rows[index];
        string ClientID = row.Cells[1].Text;
        string LoanNo = row.Cells[2].Text;
        string LoanAmount = row.Cells[3].Text;
        string TotalAmount = row.Cells[7].Text;
        string AmountPerMonth = row.Cells[8].Text;
        string LoanBalance = row.Cells[9].Text;
        Label lblmsg = (Label)Master.FindControl("lblmsg");

        if (e.CommandName.Equals("Repay"))
        {
            InterConnect.LeshLaonApi.ClientDetails Det = bll.GetClientDetails(user, ClientID);
            MultiView3.SetActiveView(RepayDetails);
            txtLoanNo.Text = LoanNo;
            txtClientNo.Text = ClientID;
            txtfname.Text = Det.ClientName;
            txtMobileNo.Text = Det.ClientPhoneNumber;
            txtEmail.Text = Det.ClientEmail;

            txtClientNo.Enabled = false;
            txtLoanNo.Enabled = false;
            txtfname.Enabled = false;
            txtMobileNo.Enabled = false;
            txtEmail.Enabled = false;

            //bll.ShowMessage(lblmsg, "Loan Missing details", true, Session);

        }
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            InterConnect.LeshLaonApi.LoanDetails Payment = GetLoanRepaymentDetails();
            //validate client details input
            string check_status = validate_input(Payment.LoanDate, Payment.LoanAmount);


            if (!check_status.Equals("OK"))
            {
                ShowMessage(check_status, true);
            }
            else
            {   
                if(Payment.LoanAmount < AmountPerMonth)
                {

                }
                //save Payment details
                Result user_save = bll.SavePaymentDetails(Payment);

                if (user_save.StatusCode != "0")
                {
                    //MultiView2.ActiveViewIndex = 0;
                    ShowMessage(user_save.StatusDesc, true);
                    return;
                }
                ShowMessage("LOAN PAYMENT DETAILS SAVED SUCCESSFULLY", false);
                ClearControls();
                //Print Payment Receipt
                Response.Redirect("LoanRepayment.aspx");
            }
        } 
        catch (Exception ex)
        {
            ShowMessage(ex.Message, true);
        }
    }

    public LoanDetails GetLoanRepaymentDetails()
    {
        LoanDetails LoanRepayDet = new LoanDetails();

        LoanRepayDet.ClientID = txtClientNo.Text;
        LoanRepayDet.LoanNo = txtLoanNo.Text;
        LoanRepayDet.LoanDate = txtRepaymentDate.Text;
        LoanRepayDet.LoanAmount = txtPaidAmount.Text.Replace(",","");
        LoanRepayDet.Observations = txtRemarks.Text;
        LoanRepayDet.CreatedBy = user.UserId;
        return LoanRepayDet;
    }

    public void ClearControls()
    {
        txtClientNo.Text = "";
        txtLoanNo.Text = "";
        txtPaidAmount.Text = "";
        txtRepaymentDate.Text = "";
        txtEmail.Text = "";
        txtfname.Text = "";
        txtMobileNo.Text = "";
        txtRemarks.Text = "";
    }

    public string validate_input(string LoanDate, string LoanAmount)
    {
        string output = "";
        if (LoanDate.Equals(""))
        {
            output = "Date of Payment Required";
            txtRepaymentDate.Focus();
        }
        else if (LoanAmount.Equals(""))
        {
            output = "Payment Amount Required";
            txtPaidAmount.Focus();
        }

        else
        {
            output = "OK";
        }
        return output;

    }
}
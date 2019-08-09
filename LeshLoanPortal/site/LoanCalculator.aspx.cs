using InterConnect.LeshLaonApi;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class LoanCalculator : System.Web.UI.Page
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
                if ((Session["Username"] == null) || (Session["RoleCode"].ToString() == null) || (Session["RoleCode"].ToString() == ""))
                {
                    Response.Redirect("Default.aspx");
                }
                MultiView1.ActiveViewIndex = 1;

            }
        }
        catch (Exception ex)
        {
            //do nothing
        }
    }
    protected void btnCalculateLoan_Click(object sender, EventArgs e)
    {
        double Principal = Convert.ToInt32(txtLoanAmount.Text.Replace(",",""));
        double InterestIn = Convert.ToInt32(txtInterest.Text);
        double NOofMonths = Convert.ToInt32(txtMonths.Text);

        double Interest = (InterestIn / 100);
        
        double top = (Principal * Interest * (Math.Pow((1 + Interest) , NOofMonths)));
        double bottom = ((Math.Pow((1 + Interest), NOofMonths)) - 1);
        double EMI = Math.Round(top / bottom); 

        DataTable dt = new DataTable();
        dt.Clear();
        dt.Columns.Add("Installments");
        dt.Columns.Add("Principal");
        dt.Columns.Add("Interest");
        dt.Columns.Add("EMI");

        
        //Remaining balance LoanAmount-Principal per month;
        //Principal = EMI-InterestRate*remaining balance

        for (int i=1; i<=NOofMonths; i++)
        {
            double InterestPerMonth = Math.Round(Interest * Principal);
            double PrincipalPerMonth = Math.Round(EMI - InterestPerMonth);
            Principal = Math.Round((Principal - PrincipalPerMonth), MidpointRounding.ToEven);
            //double LoanBalance = ;
            dt.Rows.Add(i, PrincipalPerMonth.ToString("#,##0"), InterestPerMonth.ToString("#,##0"), EMI.ToString("#,##0"));
        }
        dataGridResults.DataSource = dt;
        dataGridResults.DataBind();
        MultiView2.ActiveViewIndex = 0;
    }

    public void SearchDB()
    {
        string[] Params = { };// GetSearchParameters();
        DataTable dt = bll.SearchLoanDetailsTableForApproval(Params);
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
    
}
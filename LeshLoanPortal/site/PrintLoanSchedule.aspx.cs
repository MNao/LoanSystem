using CrystalDecisions.CrystalReports.Engine;
using InterConnect.LeshLaonApi;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class PrintLoanSchedule : System.Web.UI.Page
{
    LeshLoanAPI api = new LeshLoanAPI();
    BusinessLogic bll = new BusinessLogic();
    SystemUser user;
    ReportDocument GenerateVoucher = new ReportDocument();
    ReportDocument GenerateLoanSummary = new ReportDocument();
    ReportDocument GenerateLoanCollateral = new ReportDocument();
    ReportDocument GenerateLoanAgreement = new ReportDocument();
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            user = Session["User"] as SystemUser;

            string LoanId = Request.QueryString["Id"];
            string CompanyCode = Request.QueryString["CompanyCode"];
            string ClientCode = Request.QueryString["ClientCode"];
            LoadLoanDetails(LoanId, CompanyCode,ClientCode);
            LoadLoanSummaryDetails(LoanId, CompanyCode, ClientCode);
            LoadCollateralDetails(LoanId, CompanyCode, ClientCode);
            LoadAgreementDetails(LoanId, CompanyCode, ClientCode);
        }
        catch (Exception ex)
        {
            //something is wrong...show the error
            bll.ShowMessage(lblmsg, ex.Message, true, Session);
        }
    }

    private void LoadLoanDetails(string LoanId, string CompanyCode, string ClientCode)
    {
        string[] Params = { ClientCode, LoanId };
        DataSet ds = new DataSet();
        //ds.Reset();
        DataTable Loan = api.ExecuteDataSet("GetLoanDetailsForSchedule", Params).Tables[0];
        LoanDetails response = GetClientNameByCode(CompanyCode, ClientCode);

        imgUrlGuarantoorProof.Text = Loan.Rows[0]["GuarantorProof"].ToString();
        imgUrlGuarantoorProof.Visible = false;

        //string[] BaseText = Purchase.Rows[0]["GuarantorProof"].ToString().Split(',');

        //byte[] imageBytes = Convert.FromBase64String(BaseText[1]);

        //string filepath = "E:/Projects/LeshLoanSystem/LeshLoanPortal/site/Images/" + CompanyCode + ".jpeg";

        //using (var imageFile = new FileStream(filepath, FileMode.Create))
        //{
        //    imageFile.Write(imageBytes, 0, imageBytes.Length);
        //    imageFile.Flush();
        //}

        double Principal = Convert.ToInt32(Loan.Rows[0]["LoanAmount"].ToString().Replace(",", ""));
        double InterestIn = Convert.ToInt32(Loan.Rows[0]["InterestRate"].ToString().Replace(",", ""));
        double NOofMonths = Convert.ToInt32(Loan.Rows[0]["PaymentPeriod"].ToString().Replace(",", ""));

        double Interest = (InterestIn / 100);

        //double top = (Principal * Interest * (Math.Pow((1 + Interest), NOofMonths)));
        //double bottom = ((Math.Pow((1 + Interest), NOofMonths)) - 1);
        //double EMI = Math.Round(top / bottom);

        DataTable LoanSchedule = new DataTable();
        LoanSchedule.Clear();
        LoanSchedule.Columns.Add("Installments");
        LoanSchedule.Columns.Add("Principal");
        LoanSchedule.Columns.Add("InterestPerInstallment");
        LoanSchedule.Columns.Add("EMI");

        double TotalInterestAmount = (Interest * Principal * NOofMonths);
        double LoanAmountPerMonth = Math.Round(((Principal + TotalInterestAmount) / NOofMonths), MidpointRounding.ToEven);
        double InterestPerMonth = Math.Round(Interest * Principal);
        

        for (int i = 1; i <= NOofMonths; i++)
        {
            LoanSchedule.Rows.Add(i, Principal.ToString("#,##0"), InterestPerMonth.ToString("#,##0"), LoanAmountPerMonth.ToString("#,##0"));
            Principal = Math.Round((Principal - (LoanAmountPerMonth - InterestPerMonth)), MidpointRounding.ToEven);
        }


        //for (int i = 1; i <= NOofMonths; i++)
        //{
        //    double InterestPerMonth = Math.Round(Interest * Principal);
        //    double PrincipalPerMonth = Math.Round(EMI - InterestPerMonth);
        //    Principal = Math.Round((Principal - PrincipalPerMonth), MidpointRounding.ToEven);
        //    //double LoanBalance = ;
        //    LoanSchedule.Rows.Add(i, PrincipalPerMonth.ToString("#,##0"), InterestPerMonth.ToString("#,##0"), EMI.ToString("#,##0"));
        //}

        ds.Tables.Add(Loan.Copy());
        ds.Tables.Add(LoanSchedule);

        if (Loan.Rows.Count > 0)
        {
            GenerateVoucher.Load(@"E:\Projects\LeshLoanSystem\LeshLoanPortal\site\bin\reports\LoanReport.rpt");
            GenerateVoucher.SetDataSource(ds);
            GenerateVoucher.Database.Tables[0].SetDataSource(Loan);
            GenerateVoucher.Database.Tables[1].SetDataSource(LoanSchedule);
            GenerateVoucher.SetParameterValue("ImgUrl", "E:\\Projects\\LeshLoanSystem\\LeshLoanPortal\\site\\Images\\" + CompanyCode + ".jpeg");
            CrystalReportViewer1.ReportSource = GenerateVoucher;
        }
    }

    private void LoadLoanSummaryDetails(string LoanId, string CompanyCode, string ClientCode)
    {
        string[] Params = { ClientCode, LoanId };

        DataTable LoanSummary = api.ExecuteDataSet("GetLoanSummaryDetails", Params).Tables[0];

        if (LoanSummary.Rows.Count > 0)
        {
            GenerateLoanSummary.Load(@"E:\Projects\LeshLoanSystem\LeshLoanPortal\site\bin\reports\LoanSummary_1.rpt");
            GenerateLoanSummary.SetDataSource(LoanSummary);
            CrystalReportViewer2.ReportSource = GenerateLoanSummary;
        }
    }

    private void LoadCollateralDetails(string LoanId, string CompanyCode, string ClientCode)
    {
        string[] Params = { ClientCode, LoanId };

        DataTable LoanSummary = api.ExecuteDataSet("GetLoanSummaryDetails", Params).Tables[0];

        if (LoanSummary.Rows.Count > 0)
        {
            GenerateLoanCollateral.Load(@"E:\Projects\LeshLoanSystem\LeshLoanPortal\site\bin\reports\LoanCollateral.rpt");
            GenerateLoanCollateral.SetDataSource(LoanSummary);
            CrystalReportViewer3.ReportSource = GenerateLoanCollateral;
        }
    }

    private void LoadAgreementDetails(string LoanId, string CompanyCode, string ClientCode)
    {
        string[] Params = { ClientCode, LoanId };

        DataTable LoanSummary = api.ExecuteDataSet("GetLoanSummaryDetails", Params).Tables[0];

        if (LoanSummary.Rows.Count > 0)
        {
            GenerateLoanAgreement.Load(@"E:\Projects\LeshLoanSystem\LeshLoanPortal\site\bin\reports\LoanAgreement.rpt");
            GenerateLoanAgreement.SetDataSource(LoanSummary);
            CrystalReportViewer4.ReportSource = GenerateLoanAgreement;
        }
    }
    private LoanDetails GetClientNameByCode(string CompanyCode, string clientCode)
    {
        LoanDetails aclient = new LoanDetails();// result as LoanDetails;
        return aclient;
    }

    protected void btnLoanSch_Click(object sender, EventArgs e)
    {
        MultiView2.SetActiveView(LoanSchedule);
    }

    protected void btnLoanDet_Click(object sender, EventArgs e)
    {
        MultiView2.SetActiveView(LoanDetails);
    }

    protected void btnBack_Click(object sender, EventArgs e)
    {
        Response.Redirect("ViewLoans.aspx");
    }

    protected void btnGuaratorProof_Click(object sender, EventArgs e)
    {
        string[] BaseText = imgUrlGuarantoorProof.Text.Split(',');

        if (BaseText[0].Contains("pdf"))
        {

            byte[] imageBytes = Convert.FromBase64String(BaseText[1]);

            Response.Clear();
            Response.ContentType = "application/pdf";
            Response.AppendHeader("Content-Disposition", "inline;filename=data.pdf");
            Response.BufferOutput = true;
            
            Response.BinaryWrite(imageBytes);
            Response.End();
        }
        else
        {
            Image1.Visible = true;
            Image1.ImageUrl = imgUrlGuarantoorProof.Text;
        }
        Image1.Width = Unit.Percentage(50);
        Image1.Height = Unit.Percentage(50);
    }


    protected void btnLoanSumm_Click(object sender, EventArgs e)
    {
        MultiView2.SetActiveView(LoanSummary);
    }

    protected void btnLoanCollateral_Click(object sender, EventArgs e)
    {
        MultiView2.SetActiveView(CollateralView);
    }

    protected void btnLoanAgmt_Click(object sender, EventArgs e)
    {
        MultiView2.SetActiveView(AgmtView);
    }

    protected void LinkButton1_Click(object sender, EventArgs e)
    {
        Session.Clear();
        Session.Abandon();
        Response.Redirect("./Default.aspx");
    }
}
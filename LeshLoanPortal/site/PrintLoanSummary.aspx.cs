using CrystalDecisions.CrystalReports.Engine;
using InterConnect.LeshLaonApi;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class PrintLoanSummary : System.Web.UI.Page
{
    LeshLoanAPI api = new LeshLoanAPI();
    BusinessLogic bll = new BusinessLogic();
    SystemUser user;
    ReportDocument GenerateLoanSummary = new ReportDocument();
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            user = Session["User"] as SystemUser;
            LoadLoanSummaryDetails();
        }
        catch (Exception ex)
        {
            //something is wrong...show the error
            bll.ShowMessage(lblmsg, ex.Message, true, Session);
        }
    }

    private void LoadLoanSummaryDetails()
    {
        string LoanId = Request.QueryString["LoanId"];
        string CompanyCode = Request.QueryString["CompanyCode"];
        string ClientCode = Request.QueryString["ClientCode"];

        string[] Params = { ClientCode, LoanId };

        DataTable LoanSummary = api.ExecuteDataSet("GetLoanSummaryDetails", Params).Tables[0];
        
        if (LoanSummary.Rows.Count > 0)
        {
            GenerateLoanSummary.Load(@"E:\Projects\LeshLoanSystem\LeshLoanPortal\site\bin\reports\LoanSummary_1.rpt");
            GenerateLoanSummary.SetDataSource(LoanSummary);
            CrystalReportViewer1.ReportSource = GenerateLoanSummary;
        }
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
            //MemoryStream ms = new MemoryStream(imageBytes, 0,
            //  imageBytes.Length);

            //// Convert byte[] to Image
            //ms.Write(imageBytes, 0, imageBytes.Length);

            Response.Clear();
            Response.ContentType = "application/pdf";
            Response.AppendHeader("Content-Disposition", "inline;filename=data.pdf");
            Response.BufferOutput = true;

            ////Response.AddHeader("Content-Length", response.Length.ToString());
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

}
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
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            user = Session["User"] as SystemUser;
            LoadLoanDetails();
        }
        catch (Exception ex)
        {
            //something is wrong...show the error
            bll.ShowMessage(lblmsg, ex.Message, true, Session);
        }
    }

    private void LoadLoanDetails()
    {
        string LoanId = Request.QueryString["Id"];
        string CompanyCode = Request.QueryString["CompanyCode"];
        string ClientCode = Request.QueryString["ClientCode"];

        string[] Params = { ClientCode, LoanId };
        DataSet ds = new DataSet();
        //ds.Reset();
        DataTable Loan = api.ExecuteDataSet("GetLoanDetailsForSchedule", Params).Tables[0];//bll.GetLoanDetails(user, LoanId, ClientCode);
        LoanDetails response = GetClientNameByCode(CompanyCode, ClientCode);

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

        double top = (Principal * Interest * (Math.Pow((1 + Interest), NOofMonths)));
        double bottom = ((Math.Pow((1 + Interest), NOofMonths)) - 1);
        double EMI = Math.Round(top / bottom);

        DataTable LoanSchedule = new DataTable();
        LoanSchedule.Clear();
        LoanSchedule.Columns.Add("Installments");
        LoanSchedule.Columns.Add("Principal");
        LoanSchedule.Columns.Add("InterestPerInstallment");
        LoanSchedule.Columns.Add("EMI");

        for (int i = 1; i <= NOofMonths; i++)
        {
            double InterestPerMonth = Math.Round(Interest * Principal);
            double PrincipalPerMonth = Math.Round(EMI - InterestPerMonth);
            Principal = Math.Round((Principal - PrincipalPerMonth), MidpointRounding.ToEven);
            //double LoanBalance = ;
            LoanSchedule.Rows.Add(i, PrincipalPerMonth.ToString("#,##0"), InterestPerMonth.ToString("#,##0"), EMI.ToString("#,##0"));
        }

        ds.Tables.Add(Loan.Copy());
        ds.Tables.Add(LoanSchedule);

        if (Loan.Rows.Count > 0)
        {
            //foreach (DataRow row in Loan.Rows)
            //{
            //    row["ClientName"] = response.LoanNo;
            //    row["ClientAddress"] = response.LoanDesc;
            //    row["ClientPhone"] = response.LoanAmount; //invoice.Phone;

            //}

            //PaymentVoucher voucher = result as PaymentVoucher;

            //DataTable paymentVoucher = new DataTable();
            //paymentVoucher.Columns.Add("ClientName");
            //paymentVoucher.Columns.Add("VoucherAmount");
            //paymentVoucher.Columns.Add("InvoiceNumber");
            //paymentVoucher.Columns.Add("VoucherCode");
            //paymentVoucher.Columns.Add("Reason");
            //DataRow row = paymentVoucher.NewRow();
            //row["ClientName"] = GetClientNameByCode(CompanyCode, ClientCode);
            //row["VoucherAmount"] = voucher.VoucherAmount;
            //row["InvoiceNumber"] = voucher.InvoiceNumber;
            //row["VoucherCode"] = voucher.VoucherCode; ;
            //row["Reason"] = voucher.Reason;
            //paymentVoucher.Rows.Add(row);
            GenerateVoucher.Load(@"E:\Projects\LeshLoanSystem\LeshLoanPortal\site\bin\reports\LoanReport.rpt");
            GenerateVoucher.SetDataSource(ds);
            GenerateVoucher.Database.Tables[0].SetDataSource(Loan);
            GenerateVoucher.Database.Tables[1].SetDataSource(LoanSchedule);
            GenerateVoucher.SetParameterValue("ImgUrl", "E:\\Projects\\LeshLoanSystem\\LeshLoanPortal\\site\\Images\\" + CompanyCode + ".jpeg");
            CrystalReportViewer1.ReportSource = GenerateVoucher;
        }
    }

    private LoanDetails GetClientNameByCode(string CompanyCode, string clientCode)
    {
        //Entity result = api.GetById(CompanyCode, "CLIENTORSUPPLIER", clientCode);
        //if (result.StatusCode != Globals.SUCCESS_STATUS_CODE)
        //{
        //    return result as LoanDetails;
        //}
        LoanDetails aclient = new LoanDetails();// result as LoanDetails;
        return aclient;
    }
}
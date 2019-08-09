using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using InterConnect.LeshLaonApi;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Web;
//using System.Web.Mail;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class PrintReceipt : System.Web.UI.Page
{
    LeshLoanAPI api = new LeshLoanAPI();
    BusinessLogic bll = new BusinessLogic();
    SystemUser user;

    ReportDocument GenerateReceipt = new ReportDocument();
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            user = Session["User"] as SystemUser;
            LoadRecieptDetails();
        }
        catch (Exception ex)
        {
            //something is wrong...show the error
            string msg = "RECEIPT " + ex.Message + ": PLEASE SUPPLY THE SAME MONTHLY/LOAN AMOUNT";
            bll.ShowMessage(lblmsg, ex.Message, true, Session);
        }
    }

    private void LoadRecieptDetails()
    {
        string ReceiptNo = Request.QueryString["Id"];
        string CompanyCode = Request.QueryString["CompanyCode"];
        string ClientCode = Request.QueryString["ClientCode"];


        Receipt result = bll.GetReceiptDetails(user, ReceiptNo, ClientCode); ;
        //api.GetById(CompanyCode, "RECIEPT", RecieptId);

        if (result.StatusCode != Globals.SUCCESS_STATUS_CODE)
        {
            throw new Exception(result.StatusDesc);
        }

        Receipt receipt = result as Receipt;

        //string[] clientDetails = GetClientNameByCode(receipt.CompanyCode, ClientCode);
        //string ClientName = clientDetails[0];
        string pdfFile = "E:\\PePay\\PegasusBussinessManagementSystem-master\\application\\apps\\Receipts\\" + Receipt.ClientName + " Receipt.pdf";
        //string pdfFile = "C:\\Users\\MNO\\Desktop\\application\\apps\\Receipts\\Reciept.pdf";

        DataTable PaymentReceipt = new DataTable();
        PaymentReceipt.Columns.Add("ClientName");
        PaymentReceipt.Columns.Add("ReceiptNumber");
        PaymentReceipt.Columns.Add("LoanNumber");
        PaymentReceipt.Columns.Add("PaymentType");
        PaymentReceipt.Columns.Add("PaymentDate");
        PaymentReceipt.Columns.Add("Amount");
        PaymentReceipt.Columns.Add("Currency");
        DataRow row = PaymentReceipt.NewRow();
        row["ClientName"] = ClientName;
        row["ReceiptNumber"] = receipt.ReceiptNumber;
        row["LoanNumber"] = receipt.LoanNumber; ; //invoice.quantity;
        row["PaymentType"] = receipt.PaymentType; //invoice.unitprice;
        //row["BankName"] = Convert.ToInt64(receipt.ReceiptAmount) + " Ugandan Shillings"; //reciept.BankAccountName;
        row["PaymentDate"] = receipt.PaymentDate;
        row["Amount"] = receipt.ReceiptAmount;
        row["Currency"] = receipt.CurrencyCode;
        PaymentReceipt.Rows.Add(row);
        GenerateReceipt.Load(@"E:\Projects\LeshLoanSystem\LeshLoanPortal\site\Bin\reports\PaymentReceipt.rpt");
        GenerateReceipt.SetDataSource(PaymentReceipt);
        GenerateReceipt.SetParameterValue("ImgUrl", "E:\\Projects\\LeshLoanSystem\\LeshLoanPortal\\site\\Images\\" + CompanyCode + ".jpeg");
        CrystalReportViewer1.ReportSource = GenerateReceipt;
        GenerateReceipt.ExportToDisk(ExportFormatType.PortableDocFormat, pdfFile);

        //GenerateReceipt.ExportToHttpResponse
        //   (CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, Response, true, "PersonDetails");

        //string mail = clientDetails[1];
        //sendMail(mail, pdfFile);
        //MultiView1.SetActiveView(reportview);
    }

    public Result sendMail(string mail, string pdfFile)
    {
        Result result = new Result();
        try
        {
            //http://192.168.23.15:5099/MailApi/Messenger.asmx?WSDL
            InterConnect.MailApi.Messenger mailApi = new InterConnect.MailApi.Messenger();
            InterConnect.MailApi.Email email = new InterConnect.MailApi.Email();
            InterConnect.MailApi.Attachment attachment = new InterConnect.MailApi.Attachment();
            email.From = "notifications@pegasustechnologies.co.ug";
            email.Subject = "CUSTOMER RECEIPT";
            email.Message = "Customer Receipt uptil " + DateTime.Now.ToString();
            InterConnect.MailApi.EmailAddress address = new InterConnect.MailApi.EmailAddress();
            address.Address = mail;
            address.AddressType = InterConnect.MailApi.EmailAddressType.To;
            address.Name = mail;
            attachment.AttachmentName = pdfFile;
            attachment.MimeType = "application/pdf";
            //attachment.ByteArray = FileStream();


            email.MailAddresses = new InterConnect.MailApi.EmailAddress[] { address };
            email.Attachments = new InterConnect.MailApi.Attachment[] { attachment };
            InterConnect.MailApi.Result resp = mailApi.PostEmail(email);
            string script = "<script>alert('Mail Sent Successfully')</script>";
            ClientScript.RegisterStartupScript(this.GetType(), "mailSent", script);
            result.StatusCode = resp.StatusCode;
            result.StatusDesc = resp.StatusDesc;
        }
        catch (Exception ex)
        {
            result.StatusCode = Globals.FAILURE_STATUS_CODE;
            result.StatusDesc = "EXCEPTION: " + ex.Message;
        }
        return result;
    }
    //MailMessage msg = new MailMessage();
    //    try
    //    {
    //        msg.From = new MailAddress("osbertmugabe@gmail.com");
    //        msg.To.Add(mail);
    //        msg.Body = "Customer Reciept";
    //        System.Net.Mail.Attachment attachment;
    //        attachment = new System.Net.Mail.Attachment(pdfFile);
    //        msg.Attachments.Add(attachment);
    //        //msg.Attachments.Add(new Attachment(pdfFile));
    //        msg.IsBodyHtml = true;
    //        msg.Subject = "Customer Receipt uptil " + DateTime.Now.ToString() + " date";
    //        SmtpClient smt = new SmtpClient("smtp.gmail.com");
    //        smt.Port = 587;
    //        smt.Credentials = new NetworkCredential("osbertmugabe@gmail.com", "");
    //        smt.EnableSsl = true;
    //        smt.Send(msg);
    //        string script = "<script>alert('Mail Sent Successfully')</script>";
    //        ClientScript.RegisterStartupScript(this.GetType(), "mailSent", script);
    //    }
    //    catch (Exception ex)
    //    {
    //        return;
    //    }
    //    finally
    //    {
    //        email.Dispose();
    //    }
    //}
}
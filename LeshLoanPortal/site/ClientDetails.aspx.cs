using InterConnect.LeshLaonApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class ClientDetails : System.Web.UI.Page
{
    BusinessLogic bll = new BusinessLogic();
    SystemUser user;
    LeshLoanAPI LeshLoanApi = new LeshLoanAPI();
    protected void Page_Load(object sender, EventArgs e)
    {
        //Label lblmsg = (Label)Master.FindControl("lblmsg");
        try
        {
            user = Session["User"] as SystemUser;
            string ClientID = Request.QueryString["ClientID"];
            string CustName = Request.QueryString["CustName"];
            string PhoneNumber = Request.QueryString["PhoneNo"];
            string Status = Request.QueryString["Status"];
            GenerateKYCDoc(user, CustName, PhoneNumber, Status, ClientID);

        }
        catch (Exception ex)
        {
            string msg = "FAILED: " + ex.Message;
            bll.LogError(user.CompanyCode, "", "ACESS-CLIENT-DETAILS" + ex.Message + ex.StackTrace, "EXCEPTION", "", "");
            bll.ShowMessage(lblMsg, msg, true, Session);
            Multiview1.SetActiveView(EmptyView);
            return;
        }
    }

    protected void btnReturn_Click(object sender, EventArgs e)
    {
        if (user.RoleCode == "003")
        {
            Response.Redirect("ViewKYCDetails.aspx");
        }
        if (user.RoleCode == "004")
        {
            Response.Redirect("ApproveKYCDetails.aspx");
        }

    }

    private void GenerateKYCDoc(SystemUser user, string CustName, string PhoneNo, string status, string ClientID)
    {
        Reason.Visible = false;
        btnRejectWithReason.Visible = false;
        if (user.RoleCode == "003")
        {
            btnApprove.Visible = false;
            btnVerify.Visible = true;
            if ((status == "VERIFIED") || (status == "REJECTED"))
            {
                btnReject.Visible = false;
                btnVerify.Visible = false;
            }
        }
        if (user.RoleCode == "004")
        {
            btnApprove.Visible = true;
            btnVerify.Visible = false;

            if ((status == "APPROVED") || (status == "REJECTED"))
            {
                btnReject.Visible = false;
                btnApprove.Visible = false;
            }
        }

        InterConnect.LeshLaonApi.ClientDetails[] Details = LeshLoanApi.GetClientDetails(user.CompanyCode, "", ClientID);


        foreach (InterConnect.LeshLaonApi.ClientDetails req in Details)
        {
            lblCustName.Text = CustName;
            lblPhoneNo.Text = PhoneNo;
            lblKycId.Text = ClientID;
            lblGender.Text = Details[0].Gender; //GetCommaAmount(tr.TotalTranAmount) + " UGX";
            lblIdNo.Text = Details[0].IDNumber;
            //lblDOB.Text = req.DateOfBirth;//Details[0].DateOfBirth;
            lblLocation.Text = Details[0].ClientAddress;
            lblCapturedBy.Text = Details[0].ModifiedBy;
            lblCapturedOn.Text = Details[0].ModifiedOn;
            Image1.ImageUrl = Details[0].ClientPhoto;
            Image1.Width = Unit.Pixel(300);
            Image1.Height = Unit.Pixel(300);
            lblStatus.Text = status;//Request.QueryString["Status"];
        }

        //lblItemDesc.Text = "Payment Amount";

        //lblCustName.Text = CustName;
        //lblAmountInWords.Text = CustName;//GetAmountInWords(tr.TotalTranAmount.Split('.')[0]) + " Uganda Shillings Only";
    }

    protected void btnVerify_Click(object sender, EventArgs e)
    {
        if (lblStatus.Text == "VERIFIED")
        {
            bll.ShowMessage(Label9, "KYC Already Verified", true, Session);
            return;
        }
        else if (lblStatus.Text == "REJECTED")
        {
            bll.ShowMessage(Label9, "KYC Already Rejected", true, Session);
            return;
        }

        if (user.RoleCode == "003")
        {
            string CustomerName = lblCustName.Text;
            string PhoneNumber = lblPhoneNo.Text;
            string IDNumber = lblIdNo.Text;
            string KYCID = lblKycId.Text;
            //bll.UpdateKYCStatus(KYCID, CustomerName, PhoneNumber, user.RoleCode, "", user.UserId);
            bll.ShowMessage(Label9, "KYC Successfully Verified", false, Session);

            btnReject.Visible = false;
            btnVerify.Visible = false;
            //sendMail to Approver
            string[] Details = bll.GetUserEmail(user, "004");
            bll.SendNotification(Details, "Approved", "");
            return;
            //Response.Redirect("ViewKYCDetails.aspx");
        }
        //bll.SendNotification(user, "Verified", "");
        Response.Redirect("ViewKYCDetails.aspx");
        return;
    }

    protected void btnReject_Click(object sender, EventArgs e)
    {

        //if (lblStatus.Text == "APPROVED")
        // {
        //     bll.ShowMessage(Label9, "KYC Already Approved", true, Session);
        //     return;
        // }
        // else if (lblStatus.Text == "REJECTED")
        // {
        //     bll.ShowMessage(Label9, "KYC Already Rejected", true, Session);
        //     return;
        // }
        Reason.Visible = true;
        btnRejectWithReason.Visible = true;
        btnReject.Visible = false;
        btnApprove.Visible = false;
        btnVerify.Visible = false;
        bll.ShowMessage(Label9, "Supply Rejection Reason Below", false, Session);

    }

    protected void btnApprove_Click(object sender, EventArgs e)
    {
        if (lblStatus.Text == "PENDING")
        {
            bll.ShowMessage(Label9, "KYC Not Already Verified", true, Session);
            return;
        }
        if (lblStatus.Text == "APPROVED")
        {
            bll.ShowMessage(Label9, "KYC Already Approved", true, Session);
            return;
        }
        if (lblStatus.Text == "REJECTED")
        {
            bll.ShowMessage(Label9, "KYC Already Rejected", true, Session);
            return;
        }

        if (user.RoleCode == "004")
        {
            string CustomerName = lblCustName.Text;
            string PhoneNumber = lblPhoneNo.Text;
            string IDNumber = lblIdNo.Text;
            string KYCID = lblKycId.Text;
            //bll.UpdateKYCStatus(KYCID, CustomerName, PhoneNumber, user.RoleCode, "", user.UserId);
            bll.ShowMessage(Label9, "KYC Successfully Approved", false, Session);

            btnReject.Visible = false;
            btnApprove.Visible = false;

            //sendNotification to bank user to activate Account in core Banking //Also send sms notification to customer 
            string[] Details = bll.GetUserEmail(user, "004");
            bll.SendNotification(Details, "Approved", "");

            string Message = "Dear " + CustomerName + " your Account with UBA has been Approved.";
            Random Id = new Random();
            string vendorTranID = Id.Next(1000, 99999).ToString();
            bll.SendSMSNotificationtoUser(PhoneNumber, Message, vendorTranID);
            return;
            //Response.Redirect("ApproveKYCDetails.aspx");
        }
        //bll.SendNotification(user, "Verified", "");
        //Response.Redirect("ApproveKYCDetails.aspx");


    }

    protected void btnRejectWithReason_Click(object sender, EventArgs e)
    {

        string RejReason = txtRejectionReason.Text;

        if (string.IsNullOrEmpty(RejReason))
        {
            bll.ShowMessage(Label9, "Please Supply a Rejection Reason", true, Session);
            Reason.Visible = true;
            btnRejectWithReason.Visible = true;
            return;
        }
        if (user.RoleCode == "003")
        {
            string CustomerName = lblCustName.Text;
            string PhoneNumber = lblPhoneNo.Text;
            string IDNumber = lblIdNo.Text;
            string KYCID = lblKycId.Text;
            //bll.UpdateKYCStatus(KYCID, CustomerName, PhoneNumber, user.RoleCode, RejReason, user.UserId);
            bll.ShowMessage(Label9, "KYC has been Rejected", false, Session);

            btnReject.Enabled = false;
            btnVerify.Visible = false;

            //sendMail to Approver
            string[] Details = bll.GetUserEmail(user, "005");
            bll.SendNotification(Details, "Rejected", RejReason);
            return;
            //Response.Redirect("ViewKYCDetails.aspx");
        }
        //bll.SendNotification(user, "Verified", "");
        //Response.Redirect("ViewKYCDetails.aspx");


        if (user.RoleCode == "004")
        {
            string CustomerName = lblCustName.Text;
            string PhoneNumber = lblPhoneNo.Text;
            string IDNumber = lblIdNo.Text;
            string KYCID = lblKycId.Text;
            //bll.UpdateKYCStatus(KYCID, CustomerName, PhoneNumber, user.RoleCode, RejReason, user.UserId);
            bll.ShowMessage(Label9, "KYC has been Rejected", false, Session);

            btnReject.Enabled = false;
            btnApprove.Visible = false;

            //sendMail to Approver
            string[] Details = bll.GetUserEmail(user, "005");
            bll.SendNotification(Details, "Rejected", RejReason);
            return;
            //Response.Redirect("ApproveKYCDetails.aspx");
        }
        //bll.SendNotification(user, "Verified", "");
        //Response.Redirect("ApproveKYCDetails.aspx");
        //return;
    }
    //protected void btnViewImg_Click()
    //{
    //    string[] BaseText = imgUrl.Text.Split(',');

    //    if (BaseText[0].Contains("pdf"))
    //    {

    //        byte[] imageBytes = Convert.FromBase64String(BaseText[1]);
    //        //MemoryStream ms = new MemoryStream(imageBytes, 0,
    //        //  imageBytes.Length);

    //        //// Convert byte[] to Image
    //        //ms.Write(imageBytes, 0, imageBytes.Length);

    //        Response.Clear();
    //        Response.ContentType = "application/pdf";
    //        Response.AppendHeader("Content-Disposition", "inline;filename=data.pdf");
    //        Response.BufferOutput = true;

    //        ////Response.AddHeader("Content-Length", response.Length.ToString());
    //        Response.BinaryWrite(imageBytes);
    //        Response.End();
    //    }
    //}
}
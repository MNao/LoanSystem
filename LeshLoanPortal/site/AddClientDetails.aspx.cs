﻿using InterConnect;
using InterConnect.LeshLaonApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class AddClientDetails : System.Web.UI.Page
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
                MultiView2.ActiveViewIndex = -1;
                
                btnEdit.Visible = false;
                btnReject.Visible = false;
                btnBack.Visible = false;
                btnEditDetails.Visible = false;
                Reason.Visible = false;

                string ClientID = Request.QueryString["ClientID"];
                string CompanyCode = Request.QueryString["CompanyCode"];
                string UserType = Request.QueryString["UserType"];
                string Type = Request.QueryString["Type"];
                string Status = Request.QueryString["Status"];
                string Action = Request.QueryString["Action"];
                LoadData();
                if (Request.QueryString["transferid"] != null)
                {
                    //string UserCode = Encryption.encrypt.DecryptString(Request.QueryString["transferid"].ToString(), "25011Pegsms2322");
                    //LoadControls(UserCode);
                }
                else if (!string.IsNullOrEmpty(ClientID) && Action == "Verify")
                {
                    LoadEntityData(ClientID, CompanyCode, UserType, Type, Status);
                }
                else if (Action == "Edit")
                {
                    LoadEntityDataForEdit(ClientID, CompanyCode, UserType, Type, Status);
                }
            }
        }
        catch (Exception ex)
        {
            ShowMessage(ex.Message,true);
        }
    }

    private void LoadEntityData(string ClientID, string bankCode, string userType, string type, string status)
    {
        ChkSec.Visible = true;
        ViewPhotos.Visible = true;
        btnSubmit.Visible = false;
        btnEdit.Visible = true;
        Reason.Visible = true;
        btnReject.Visible = true;
        btnBack.Visible = false;
        UploadPhotos.Visible = false;

        InterConnect.LeshLaonApi.ClientDetails ClientDet = bll.GetClientDetails(user, ClientID);
        txtClientNo.Text = ClientID;
        txtName.Text = ClientDet.ClientName;
        txtPhoneNo.Text = ClientDet.ClientPhoneNumber;
        txtReferee.Text = ClientDet.Referee;
        txtRefereePhone.Text = ClientDet.RefrereePhoneNo;
        ddIDType.SelectedItem.Text = ClientDet.IDType;
        ddIDType.SelectedItem.Value = ClientDet.IDType;
        txtDOB.Text = ClientDet.DOB;
        txtIDNo.Text = ClientDet.IDNumber;
        txtEmail.Text = ClientDet.ClientEmail;
        ddGender.SelectedItem.Text = ClientDet.Gender;
        ddGender.SelectedItem.Value = ClientDet.Gender;
        imgUrlClientPhoto.Text = ClientDet.ClientPhoto;
        imgUrlClientPhoto.Visible = false;
        ImgUrlIDPhoto.Text = ClientDet.IDPhoto;
        ImgUrlIDPhoto.Visible = false;



        txtClientNo.Enabled = false;
        txtName.Enabled = false;
        txtPhoneNo.Enabled = false;
        txtDOB.Enabled = false;
        txtReferee.Enabled = false;
        txtRefereePhone.Enabled = false;
        txtIDNo.Enabled = false;
        txtEmail.Enabled = false;
        ddIDType.Enabled = false;
        ddGender.Enabled = false;
    }

    private void LoadEntityDataForEdit(string ClientID, string bankCode, string userType, string type, string status)
    {
        //ChkSec.Visible = true;
        ViewPhotos.Visible = true;
        btnSubmit.Visible = false;
        //Reason.Visible = false;
        btnBack.Visible = false;
        btnEditDetails.Visible = true;
        UploadPhotos.Visible = false;

        InterConnect.LeshLaonApi.ClientDetails ClientDet = bll.GetClientDetails(user, ClientID);
        txtClientNo.Text = ClientID;
        txtName.Text = ClientDet.ClientName;
        txtPhoneNo.Text = ClientDet.ClientPhoneNumber;
        txtReferee.Text = ClientDet.Referee;
        txtRefereePhone.Text = ClientDet.RefrereePhoneNo;
        ddIDType.SelectedItem.Text = ClientDet.IDType;
        ddIDType.SelectedItem.Value = ClientDet.IDType;
        txtIDNo.Text = ClientDet.IDNumber;
        txtEmail.Text = ClientDet.ClientEmail;
        txtDOB.Text = ClientDet.DOB;
        ddGender.SelectedItem.Text = ClientDet.Gender;
        ddGender.SelectedItem.Value = ClientDet.Gender;
        imgUrlClientPhoto.Text = ClientDet.ClientPhoto;
        imgUrlClientPhoto.Visible = false;
        ImgUrlIDPhoto.Text = ClientDet.IDPhoto;
        ImgUrlIDPhoto.Visible = false;
        
    }

    public void LoadData()
    {
        txtClientNo.Text = bll.GenerateSystemCode("CLI");
        txtClientNo.Enabled = false;
        ChkSec.Visible = false;
        ViewPhotos.Visible = false;
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
            InterConnect.LeshLaonApi.ClientDetails clientDet = GetClientDetails();
            if(string.IsNullOrEmpty(clientDet.ClientPhoto) || string.IsNullOrEmpty(clientDet.IDPhoto))
            {
                ShowMessage("Please Upload Photo(s)", true);
                return;
            }
            string Password = clientDet.ClientPassword;
            clientDet.ClientPassword = SharedCommons.GenerateUserPassword(clientDet.ClientPassword);
            Result client_save = Client.SaveClientDetails(clientDet);

            if (client_save.StatusCode != "0")
            {
                //MultiView2.ActiveViewIndex = 0;
                ShowMessage(client_save.StatusDesc, true);
                return;
            }
            ShowMessage("CLIENT SAVED SUCCESSFULLY", false);
            Clear_controls();
            bll.SendCredentialsToClientUser(clientDet, Password);
            bll.InsertIntoAuditLog("USER-CREATION", "SYSTEMUSERS", user.CompanyCode, user.UserId, "USER " + clientDet.ClientNo +" CREATED SUCCESSFULLY");

        }
        catch (Exception ex)
        {

        }
    }

    public void Clear_controls()
    {
        txtEmail.Text = "";
        txtName.Text = "";
        txtIDNo.Text = "";
        txtPhoneNo.Text = "";
        txtReferee.Text = "";
        txtRefereePhone.Text = "";
        txtDOB.Text = "";
    }

    public InterConnect.LeshLaonApi.ClientDetails GetClientDetails()
    {
        InterConnect.LeshLaonApi.ClientDetails clients = new InterConnect.LeshLaonApi.ClientDetails();

        clients.ClientNo = txtClientNo.Text;
        clients.ClientName = txtName.Text;
        clients.ClientPhoneNumber = txtPhoneNo.Text;
        clients.ClientPhoto = bll.GetImageUploadedInBase64String(ClientPhoto);
        clients.IDPhoto = bll.GetImageUploadedInBase64String(IDPhoto);
        clients.Referee = txtReferee.Text;
        clients.RefrereePhoneNo = txtRefereePhone.Text;
        clients.DOB = txtDOB.Text;
        clients.IDType = ddIDType.SelectedValue;
        clients.IDNumber = txtIDNo.Text;
        clients.Gender = ddGender.SelectedValue;
        clients.ClientEmail = txtEmail.Text;
        clients.ClientAddress = "Kampala";
        clients.ModifiedBy = user.UserId;

        clients.ClientPassword = bll.GeneratePassword();
        return clients;
    }

    protected void btnViewPR_Click(object sender, EventArgs e)
    {
        string[] BaseText = imgUrlClientPhoto.Text.Split(',');

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
            Image1.ImageUrl = imgUrlClientPhoto.Text;
        }
        Image1.Width = Unit.Percentage(50);
        Image1.Height = Unit.Percentage(50);
    }

    protected void btnViewID_Click(object sender, EventArgs e)
    {
        string[] BaseText = ImgUrlIDPhoto.Text.Split(',');

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
            Image2.Visible = true;
            Image2.ImageUrl = ImgUrlIDPhoto.Text;
        }
        Image2.Width = Unit.Percentage(50);
        Image2.Height = Unit.Percentage(50);
    }
    
    protected void btnEdit_Click(object sender, EventArgs e)
    {
        try
        {
            if (chkApprove.Checked)
            {
                string ClientNo = txtClientNo.Text;
                bll.UpdateClientStatus(ClientNo, "", user.UserId);
                ShowMessage("Client Approved by Approver", true);

                //Response.Redirect("ViewClientDetails.aspx");
                btnEdit.Enabled = false;
                btnReject.Enabled = false;
                btnBack.Visible = true;
            }
            else
            {
                ShowMessage("Client Cannot be Approved", true);
            }
               
        }
        catch (Exception ex)
        {

        }
    }

    protected void btnReject_Click(object sender, EventArgs e)
    {
        
        btnEdit.Visible = false;
        try
        {
            string reason = txtReasonforRejection.Text;
            if (string.IsNullOrEmpty(reason))
            {
                ShowMessage("Please Provide a Reason for Rejection", true);
                return;
            }


            ShowMessage("Client Rejected by Approver", true);
            btnReject.Visible = false;
            btnBack.Visible = true;
            //btnRejectWithReason.Enabled = false;

        }
        catch (Exception Ex)
        {
            throw Ex;
        }
    }

    protected void btnBack_Click(object sender, EventArgs e)
    {
        Response.Redirect("ViewAllClientDetails.aspx");
    }

    protected void btnEditDetails_Click(object sender, EventArgs e)
    {
        try
        {
            InterConnect.LeshLaonApi.ClientDetails clientDet = GetClientDetails();

            clientDet.ClientPhoto = "null";
            clientDet.IDPhoto = "null";
            Result client_save = Client.SaveClientDetails(clientDet);

            if (client_save.StatusCode != "0")
            {
                //MultiView2.ActiveViewIndex = 0;
                ShowMessage(client_save.StatusDesc, true);
                return;
            }
            ShowMessage("CLIENT DETAILS EDITED SUCCESSFULLY", false);
            Clear_controls();
            //bll.SendCredentialsToClientUser(clientDet, Password);
            bll.InsertIntoAuditLog("USER-EDIT", "CLIENTS", user.CompanyCode, user.UserId, "CLIENT " + clientDet.ClientNo +" EDITED SUCCESSFULLY");
            btnBack.Visible = true;
            btnEditDetails.Visible = false;

        }
        catch (Exception ex)
        {

        }
    }
}
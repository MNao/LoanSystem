using InterConnect;
using InterConnect.LeshLaonApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class RegisterClient : System.Web.UI.Page
{
    BusinessLogic bll = new BusinessLogic();
    LeshLoanAPI Client = new LeshLoanAPI();
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (IsPostBack == false)
            {
                lblCaptchaError.Visible = false;
                LoadData();
            }
        }
        catch (Exception ex)
        {
            ShowMessage(ex.Message, true);
        }
    }

    public void LoadData()
    {
        txtClientNo.Text = bll.GenerateSystemCode("CLI");
        txtClientNo.Enabled = false;
    }
    protected void btnBack_Click(object sender, EventArgs e)
    {
        Response.Redirect("Default.aspx");
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            bool isHuman = captchaBox.Validate(txtCaptcha.Text);
            txtCaptcha.Text = null;
            if (!isHuman)
            {
                //The Captcha entered by user is Invalid.
                //ShowMessage("Captcha doesnot match", true);
                lblCaptchaError.Visible = true;
                lblCaptchaError.Text = "Captcha doesnot match";
                lblCaptchaError.ForeColor = System.Drawing.Color.Red; lblmsg.Font.Bold = true;
                return;
            }
            //The Captcha entered by user is Valid.
            lblCaptchaError.Visible = false;

            InterConnect.LeshLaonApi.ClientDetails clientDet = GetClientDetails();

            string Password = clientDet.ClientPassword;
            clientDet.ClientPassword = SharedCommons.GenerateUserPassword(clientDet.ClientPassword);
            Result client_save = Client.SaveClientDetails(clientDet);

            if (client_save.StatusCode != "0")
            {
                //MultiView2.ActiveViewIndex = 0;
                ShowMessage(client_save.StatusDesc, true);
                return;
            }
            //ShowMessage("", false);
            lblmsg.Text = "CLIENT SAVED SUCCESSFULLY";
            lblmsg.ForeColor = System.Drawing.Color.Green; lblmsg.Font.Bold = true;
            Clear_controls();
            bll.SendCredentialsToClientUser(clientDet, Password);
            bll.InsertIntoAuditLog("USER-CREATION", "SYSTEMUSERS", "Lensh", txtClientNo.Text, "USER "+ clientDet.ClientNo + "CREATED SUCCESSFULLY");
            Response.Redirect("Default.aspx");

        }
        catch (Exception ex)
        {

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
    public void Clear_controls()
    {
        txtEmail.Text = "";
        txtName.Text = "";
        txtIDNo.Text = "";
        txtCaptcha.Text = "";
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
        clients.ModifiedBy = txtClientNo.Text;

        clients.ClientPassword = bll.GeneratePassword();
        return clients;
    }
}
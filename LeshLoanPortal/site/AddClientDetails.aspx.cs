using InterConnect;
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
                btnRejectWithReason.Visible = false;
                Reason.Visible = false;

                string UserID = Request.QueryString["UserID"];
                string BankCode = Request.QueryString["BankCode"];
                string UserType = Request.QueryString["UserType"];
                string Type = Request.QueryString["Type"];
                string Status = Request.QueryString["Status"];
                LoadData();
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
            ShowMessage(ex.Message,true);
        }
    }

    public void LoadData()
    {
        txtClientNo.Text = bll.GenerateSystemCode("CLI");
        txtClientNo.Enabled = false;
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
            InterConnect.LeshLaonApi.ClientDetails client = GetClientDetails();

            Result client_save = Client.SaveClientDetails(client);

            if (client_save.StatusCode != "0")
            {
                //MultiView2.ActiveViewIndex = 0;
                ShowMessage(client_save.StatusDesc, true);
                return;
            }
            ShowMessage("USER SAVED SUCCESSFULLY", false);
            Clear_controls();
            //bll.SendCredentialsToUser(RegUser, Password);
            bll.InsertIntoAuditLog("USER-CREATION", "SYSTEMUSERS", user.CompanyCode, user.UserId, "USER CREATED SUCCESSFULLY");

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
    }

    public InterConnect.LeshLaonApi.ClientDetails GetClientDetails()
    {
        InterConnect.LeshLaonApi.ClientDetails clients = new InterConnect.LeshLaonApi.ClientDetails();

        clients.ClientNo = txtClientNo.Text;
        clients.ClientName = txtName.Text;
        clients.ClientPhoneNumber = txtPhoneNo.Text;
        clients.ClientPhoto = bll.GetImageUploadedInBase64String(ClientPhoto);
        clients.IDPhoto = bll.GetImageUploadedInBase64String(IDPhoto); ;
        clients.Referee = txtReferee.Text;
        clients.RefrereePhoneNo = txtRefereePhone.Text;
        clients.IDType = ddIDType.SelectedValue;
        clients.IDNumber = txtIDNo.Text;
        clients.Gender = ddGender.SelectedValue;
        clients.ClientEmail = txtEmail.Text;
        clients.ClientAddress = "Kampala";
        clients.ModifiedBy = user.UserId;

        string Password = bll.GeneratePassword();

        clients.ClientPassword = SharedCommons.GenerateUserPassword(Password);
        return clients;
    }
    
    protected void btnEdit_Click(object sender, EventArgs e)
    {

    }

    protected void btnReject_Click(object sender, EventArgs e)
    {

    }

    protected void btnRejectWithReason_Click(object sender, EventArgs e)
    {

    }
}
using InterConnect.LeshLaonApi;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Admin : System.Web.UI.Page
{

    //private readonly Processfile _processFile = new Processfile();
    SystemUser user;
    BusinessLogic bll = new BusinessLogic();
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            user = Session["User"] as SystemUser;
            if (IsPostBack) return;
            if ((Session["Username"] == null))
            {
                Response.Redirect("Default.aspx");
            }
            //var credit = _processFile.GetUserCredit();
            //lblCredit.Text =  credit.ToString("#,##0");
            LoadData();
        }
        catch (Exception ex)
        {
            ShowMessage(ex.Message, true);
        }
    }

    private void LoadData()
    {
        FullNames.Text = Session["Username"].ToString();
        AreaRole.Text = user.RoleCode.ToString() + " - " + user.CompanyCode.ToString();
        
       //string[] DisplayDet = bll.GetStatsToDisplay(user);

       // lblPending.Text = DisplayDet[0];
       // lblVerified.Text = DisplayDet[1];
       // lblApproved.Text = DisplayDet[2];
       // lblRejected.Text = DisplayDet[3];
    }

    private void ShowMessage(string Message, bool Error)
    {
        var lblmsg = (Label)Master.FindControl("lblmsg");
        if (Error)
        {
            lblmsg.ForeColor = Color.Red;
            lblmsg.Font.Bold = false;
        }
        else
        {
            lblmsg.ForeColor = Color.Green;
            lblmsg.Font.Bold = true;
        }

        if (Message == ".")
            lblmsg.Text = ".";
        else
            lblmsg.Text = "MESSAGE: " + Message.ToUpper();
    }

    private ArrayList GetNumbers(string nums)
    {
        var tels = new ArrayList();

        return tels;
    }


}

using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

public partial class MasterSetting : System.Web.UI.MasterPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if ((Session["Username"] != null))
            {
                string name = Session["FullName"].ToString();
                string area = Session["Area"].ToString();
                string UserType = Session["UserType"].ToString();
                string title = "Logged on : " + name + "[ " + UserType + "] Area :" + area;
                lbluserid.Text = title;
                TogglePermissions();
            }
            else
            {
                Response.Redirect("Default.aspx", false);
            }
        }

        catch (NullReferenceException exe)
        {
            Response.Redirect("Default.aspx", false);

        }
        catch (Exception ex)
        {
            Response.Redirect("Default.aspx", false);
        }
    }

    private void TogglePermissions()
    {
        string role_code = Session["Username"].ToString();
        if (role_code.Equals("001"))
        {
            lblsmsPanel.Visible = true;
            lblReporting.Visible = true;
            lblSetup.Visible = true;
            lbtnSetting.Visible = true;
        }
        else
        {
            lblsmsPanel.Visible = true;
            lblReporting.Visible = true;
            lblSetup.Visible = true;
            lbtnSetting.Visible = false;
        }
    }
     protected void lblRegistration_Click(object sender, EventArgs e)
    {
        Response.Redirect("./SmsSending.aspx");
    }
    protected void lblReporting_Click(object sender, EventArgs e)
    {
        Response.Redirect("./ViewListSmsSent.aspx"); 
    }
    protected void lbtnIncome_Click(object sender, EventArgs e)
    {
        Response.Redirect("./AddUser.aspx");
    }
    protected void lblSetup_Click(object sender, EventArgs e)
    {
        Response.Redirect("./Password.aspx");
    }
    protected void LinkButton1_Click(object sender, EventArgs e)
    {
        Session.Clear();
        Session.Abandon();
        Response.Redirect("./Default.aspx");
    }
}

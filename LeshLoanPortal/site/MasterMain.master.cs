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
using InterConnect.LeshLaonApi;

public partial class MasterMain : System.Web.UI.MasterPage
{
    SystemUser user;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            user = Session["User"] as SystemUser;
            if ((user.UserId != null))
            {
              
                lbluserid.Text = user.UserId;
                
                //TogglePermissions();
            }
            else if ((Session["Username"] == null) || (Session["RoleCode"] == null))
            {
                Response.Redirect("Default.aspx");
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
            //lblsmsPanel.Visible = true;
            //lblReporting.Visible = true;
            //lblSetup.Visible = true;
            //lbtnSetting.Visible = true;
        }
        else
        {
            //lblsmsPanel.Visible = true;
            //lblReporting.Visible = true;
            //lblSetup.Visible = true;
            //lbtnSetting.Visible = false;
        }

        lblmsg.Text = "";
    }

    protected void LinkButton1_Click(object sender, EventArgs e)
    {
        Session.Clear();
        Session.Abandon();
        Response.Redirect("./Default.aspx");
    }
}

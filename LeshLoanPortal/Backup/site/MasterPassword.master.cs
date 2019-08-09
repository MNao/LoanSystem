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

public partial class MasterPassword : System.Web.UI.MasterPage
{
    Processfile process_file = new Processfile();
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {

            if ((Session["TypeCode"] != null))
            {
                string name = Session["FullName"].ToString();
                string area = Session["Area"].ToString();
                string UserType = Session["UserType"].ToString();
                string title = "Logged on : " + name + "[ " + UserType + "] Area :" + area;
                lbluserid.Text = title;            
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
    protected void LinkButton1_Click(object sender, EventArgs e)
    {
        Session.Clear();
        Session.Abandon();
        Response.Redirect("./Default.aspx");
    }
    protected void LinkButton2_Click(object sender, EventArgs e)
    {
        Response.Redirect("./SmsSending.aspx");
    }
    protected void LinkButton3_Click(object sender, EventArgs e)
    {
        Response.Redirect("./AddUser.aspx");
    }
    protected void lbtnPassword_Click(object sender, EventArgs e)
    {
        Response.Redirect("./Password.aspx");
    }
}

using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

public partial class _Default : System.Web.UI.Page 
{
    Processfile process_file = new Processfile();
    DataTable data_table = new DataTable();
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                //DisableBtnsOnClick();
                MultiView1.ActiveViewIndex = 0;
            }
        }
        catch (Exception ex)
        {
            ShowMessage(ex.Message, true);
        }
    }
    private void ShowMessage(string Message, bool Error)
    {
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
    private void DisableBtnsOnClick()
    {
        string strProcessScript = "this.value='Working...';this.disabled=true;";
        btnlogin.Attributes.Add("onclick", strProcessScript + ClientScript.GetPostBackEventReference(btnlogin, "").ToString());  
        btnchange.Attributes.Add("onclick", strProcessScript + ClientScript.GetPostBackEventReference(btnchange, "").ToString());         
    }
    protected void Btnlogin_Click(object sender, EventArgs e)
    {
        try
        {
            string userId = txtUsername.Text.Trim();
            string passwd = txtpassword.Text.Trim();
            if (userId.Equals(""))
            {
                ShowMessage("UserName Required", true);
                txtUsername.Focus();
            }
            else if (passwd.Equals(""))
            {
                ShowMessage("Password Required", true);
                txtpassword.Focus();
            }
            else
            {
                System_login(userId, passwd);
            }
        }
        catch (Exception ex)
        {
            ShowMessage(ex.Message, true);
        }
    }

    private void System_login(string userId, string passwd)
    {
        data_table = process_file.LoginDetails(userId, passwd);
        if (data_table.Rows.Count > 0)
        {
            int user_Id = int.Parse(data_table.Rows[0]["UserId"].ToString());
            bool isactive = bool.Parse(data_table.Rows[0]["Active"].ToString());
            bool reset = bool.Parse(data_table.Rows[0]["Reset"].ToString());
            if (isactive)
            {
                if (reset)
                {
                    CallReset(user_Id);
                }
                else
                {
                    CallLogin(data_table);
                }
            }
            else
            {
                ShowMessage("System Logins disabled", true);
            }
        }
        else
        {
            ShowMessage("System Logon denied", true);
        }
    }

    private void CallLogin(DataTable data_table)
    {
        Session["UserId"] = data_table.Rows[0]["UserId"].ToString();
        Session["Username"] = data_table.Rows[0]["Username"].ToString();
        Session["FullName"] = data_table.Rows[0]["FullName"].ToString();
        Session["TypeCode"] = data_table.Rows[0]["TypeCode"].ToString();
        Session["UserType"] = data_table.Rows[0]["UserType"].ToString();
        Session["AreaID"] = data_table.Rows[0]["AreaID"].ToString();
        Session["Area"] = data_table.Rows[0]["Area"].ToString();
        Session["Phone"] = data_table.Rows[0]["Phone"].ToString();
        Session["Mask"] = data_table.Rows[0]["Mask"].ToString();
        if (Session["AreaID"].ToString().Equals("47"))
        {
            Server.Transfer("SmsSending.aspx");
        }
        else
        {
        Server.Transfer("SmsSending.aspx");
        }
    }

    private void CallReset(int user_Id)
    {
        ShowMessage("System Password Reset Required, Please Reset Password and Continue", true);
        lblUsercode.Text = user_Id.ToString();
        MultiView1.ActiveViewIndex = 1;
    }
   
 
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        MultiView1.ActiveViewIndex = 0;
        ShowMessage(".", true);
    }
    protected void btnchange_Click(object sender, EventArgs e)
    {
        try
        {
            //ShowMessage("Testing", true);
            string user_code = lblUsercode.Text.Trim();
            string password = txtResetPasswd.Text.Trim();
            string confirm = txtResetConfirm.Text.Trim();
            if (password.Equals(""))
            {
                ShowMessage("New Password Required", true);
                txtResetPasswd.Focus();
            }
            else if (confirm.Equals(""))
            {
                ShowMessage("Confirm Password Provide", true);
                txtResetConfirm.Focus();
            }
            else
            {
                if (!user_code.Equals("0"))
                {
                    if (password == confirm)
                    {
                        string reset_status = process_file.Reset_Passwd(user_code, password, false);
                        MultiView1.ActiveViewIndex = 0;
                        ShowMessage("RESET DONE SUCCESSFULLY, NOW LOGIN WITH YOUR NEW PASSWORD", false);
                    }
                    else
                    {
                        ShowMessage("Passwords dont match", true);
                        txtResetPasswd.Focus();
                    }
                }
                else
                {
                    ShowMessage("System failed to alocate User Id", true);
                }
            }
        }
        catch (Exception ex)
        {
            ShowMessage(ex.Message, true);
        }
    }
    protected void btnlogin_Click(object sender, EventArgs e)
    {
        try
        {
            //
            string pass = Encryption.encrypt.DecryptString("jXU+xuRePS4KqcaRDwPW4Q==", "25011Pegsms2322");
            string userId = txtUsername.Text.Trim();
            string passwd = txtpassword.Text.Trim();
            if (userId.Equals(""))
            {
                ShowMessage("UserName Required", true);
                txtUsername.Focus();
            }
            else if (passwd.Equals(""))
            {
                ShowMessage("Password Required", true);
                txtpassword.Focus();
            }
            else
            {
                System_login(userId, passwd);
            }
        }
        catch (Exception ex)
        {
            ShowMessage(ex.Message, true);
        }
    }
}

using System;
using System.IO;
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
using InterConnect;

public partial class Password : System.Web.UI.Page
{
    BusinessLogic bll = new BusinessLogic();
    SystemUser user;
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
                MultiView1.ActiveViewIndex = 0;
                //LinkButton MenuSms = (LinkButton)Master.FindControl("lblsmsPanel");
                //LinkButton MenuReport = (LinkButton)Master.FindControl("lblReporting");
                //LinkButton MenuProfile = (LinkButton)Master.FindControl("lblSetup");
                //LinkButton MenuSettting = (LinkButton)Master.FindControl("lbtnSetting");
                //MenuSms.Font.Italic = false;
                //MenuReport.Font.Italic = false;
                //MenuSettting.Font.Italic = false;
                //MenuProfile.Font.Italic = true;
                //string strProcessScript = "this.value='Working...';this.disabled=true;";
                //Button1.Attributes.Add("onclick", strProcessScript + ClientScript.GetPostBackEventReference(Button1, "").ToString());
            }
        }
        catch (Exception ex)
        {
            ShowMessage(ex.Message, true);
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
    protected void Button1_Click(object sender, EventArgs e)
    {
        Label LblMsg = (Label)Master.FindControl("lblMsg");
        try
        {
            string OldPassword = txtOldPasswd.Text.Trim();
            string NewPassword = txtNewPasswd.Text.Trim();
            string ConfirmedPassword = txtConfirm.Text.Trim();

            if (NewPassword != ConfirmedPassword)
            {
                string msg = "Msg: Your New Password Doesnt match the confirmed Password";
                bll.ShowMessage(LblMsg, msg, true);
                txtOldPasswd.Focus();
            }

            else if (SharedCommons.GenerateMD5Hash(OldPassword) != user.Password)
            {
                string msg = "Msg: Your Old Password Is Incorrect";
                txtOldPasswd.Focus();
                bll.ShowMessage(LblMsg, msg, true);

            }
            if (OldPassword.Equals(""))
            {
                ShowMessage("Please Enter your Old Password", true);
                txtOldPasswd.Focus();
            }
            else if (NewPassword.Equals(""))
            {
                ShowMessage("Please Enter your New Password", true);
                txtNewPasswd.Focus();
            }
            else if (ConfirmedPassword.Equals(""))
            {
                ShowMessage("Please Confirm your New Password", true);
                txtConfirm.Focus();
            }

            else
            {
                    if (SharedCommons.GenerateMD5Hash(NewPassword) == SharedCommons.GenerateMD5Hash(OldPassword))
                    {
                        string msg = "Your new password can't be Similar to the Old One";
                        bll.ShowMessage(LblMsg, msg, true);
                    }
                    else if (!bll.ObeysPasswordPolicy(NewPassword, user.CompanyCode))
                    {
                        string msg = "Your new password should have a mixture of uppercase & lowercase letters, special characters i.e ?,$ and numbers";
                        bll.ShowMessage(LblMsg, msg, true);
                    }
                    else if (bll.PasswordHasBeenUsed(user.UserId, SharedCommons.GenerateMD5Hash(NewPassword)))
                    {
                        bll.ShowMessage(LblMsg, "Your New Password can't be Similar To The Recent Two Passwords", true);
                    }
                    else
                    {
                        user.Password = SharedCommons.GenerateMD5Hash(NewPassword);
                        user.ModifiedBy = user.UserId;
                        Result result = bll.ChangeUsersPassword(user.UserId, user.CompanyCode, user.Password, user.RoleCode);//, false, "PASSWORD");
                        if (result.StatusCode == "0")
                        {
                            bll.Log("PasswordTracker_Update", new string[] { user.UserId, SharedCommons.GenerateMD5Hash(OldPassword), user.UserId, bll.getIp() });
                            string msg = "Password Changed Successfully";
                            bll.ShowMessage(LblMsg, msg, false);
                        }
                        else
                        {
                            string msg = result.StatusDesc;
                            bll.ShowMessage(LblMsg, msg, true);
                        }
                    }
            }
        }
        catch (Exception ex)
        {
            ShowMessage(ex.Message, true);
        }
    } 
}

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

public partial class AddUser : System.Web.UI.Page
{
    Databasefile data_file = new Databasefile();
    Processfile process_file = new Processfile();
    DataTable data_table = new DataTable();
    DataTable d_table = new DataTable();
    PhoneValidator phone_validity = new PhoneValidator();
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (IsPostBack == false)
            {
                LoadAreas();
                LoadRoles();
                MultiView2.ActiveViewIndex = -1;
                if (Request.QueryString["transferid"] != null)
                {
                    string UserCode = Request.QueryString["transferid"].ToString();
                    LoadControls(UserCode);
                }
                LinkButton MenuSms = (LinkButton)Master.FindControl("lblsmsPanel");
                LinkButton MenuReport = (LinkButton)Master.FindControl("lblReporting");
                LinkButton MenuProfile = (LinkButton)Master.FindControl("lblSetup");
                LinkButton MenuSettting = (LinkButton)Master.FindControl("lbtnSetting");
                MenuSms.Font.Italic = false;
                MenuReport.Font.Italic = false;
                MenuSettting.Font.Italic = true;
                MenuProfile.Font.Italic = false;
                string strProcessScript = "this.value='Working...';this.disabled=true;";
                btnOK.Attributes.Add("onclick", strProcessScript + ClientScript.GetPostBackEventReference(btnOK, "").ToString());
            }
        }
        catch (Exception ex)
        {
            ShowMessage(ex.Message, true);
        }
    }

    private void LoadControls(string user_code)
    {
        int user_id = int.Parse(user_code);
        data_table = data_file.GetUserDetails(user_id);
        if (data_table.Rows.Count > 0)
        {
            lblCode.Text = data_table.Rows[0]["UserId"].ToString();
            txtlname.Text = data_table.Rows[0]["FirstName"].ToString();
            txtfname.Text = data_table.Rows[0]["LastName"].ToString();
            txtphone.Text = data_table.Rows[0]["Phone"].ToString();
            txtemail.Text = data_table.Rows[0]["Email"].ToString();
            string area_code = data_table.Rows[0]["AreaID"].ToString();
            string type_code = data_table.Rows[0]["UserType"].ToString();

            bool isactive = bool.Parse(data_table.Rows[0]["Active"].ToString());
            ddlAreas.SelectedIndex = ddlAreas.Items.IndexOf(ddlAreas.Items.FindByValue(area_code));
            ddlUserType.SelectedIndex = ddlUserType.Items.IndexOf(ddlUserType.Items.FindByValue(type_code));
            chkActive.Checked = isactive;

            MultiView2.ActiveViewIndex = 1;
        }
    }

    private void LoadRoles()
    {
        d_table = data_file.GetSystemRoles();
        ddlUserType.DataSource = d_table;
        ddlUserType.DataValueField = "TypeCode";
        ddlUserType.DataTextField = "UserType";
        ddlUserType.DataBind();
    }

    private void LoadAreas()
    {
        data_table = data_file.GetAreas();
        ddlAreas.DataSource = data_table;
        ddlAreas.DataValueField = "AreaID";
        ddlAreas.DataTextField = "Area";
        ddlAreas.DataBind();
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
    protected void ddlUserType_DataBound(object sender, EventArgs e)
    {
        ddlUserType.Items.Insert(0, new ListItem(" Select User Type ", "0"));
    }
    protected void btnOK_Click(object sender, EventArgs e)
    {
        try
        {
            string username = txtUserName.Text.Trim();
            string user_code = lblCode.Text.Trim();
            string fname = txtfname.Text.Trim();
            string lname = txtlname.Text.Trim();
            string phone = txtphone.Text.Trim();
            string email = txtemail.Text.Trim();
            string area_code = ddlAreas.SelectedValue.ToString();
            string type_code = ddlUserType.SelectedValue.ToString();
            bool is_active = chkActive.Checked;
            bool reset = CheckBox1.Checked;
            string check_status = validate_input(fname, lname, phone, area_code, type_code);
            if (!check_status.Equals("OK"))
            {
                ShowMessage(check_status, true);
            }
            else
            {
                string res_save = process_file.SaveUser(user_code,username, fname, lname, phone, email, area_code, type_code, is_active, reset);
                if (res_save.Contains("USERNAME EXISTS"))
                {
                    MultiView2.ActiveViewIndex = 0;
                    txtUserName.Focus();
                    ShowMessage(res_save, true);
                }
                else
                {
                    if (res_save.Contains("SUCCESSFULLY"))
                    {
                        ShowMessage(res_save, false);
                        Clear_contrls();
                    }
                    else
                    {
                        ShowMessage(res_save, true);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ShowMessage(ex.Message, true);
        }
    }

    private void Clear_contrls()
    {
        lblCode.Text = "0";
        txtUserName.Text = "";
        txtphone.Text = "";
        txtlname.Text = "";
        txtfname.Text = "";
        txtemail.Text = "";
        ddlAreas.SelectedIndex = ddlAreas.Items.IndexOf(ddlAreas.Items.FindByValue("0"));
        ddlUserType.SelectedIndex = ddlUserType.Items.IndexOf(ddlUserType.Items.FindByValue("0"));
        MultiView2.ActiveViewIndex = -1;
    }

    private string validate_input(string fname, string lname, string phone, string area, string type)
    {
        string output = "";
        if (fname.Equals(""))
        {
            output = "First Name Required";
            txtfname.Focus();
        }
        else if (lname.Equals(""))
        {
            output = "Last Name Required";
            txtlname.Focus();
        }
        else if (phone.Equals(""))
        {
            output = "Mobile Phone Number Required";
            txtphone.Focus();
        }
        else if (!phone_validity.PhoneNumbersOk(phone))
        {
            output = "Enter Valid Mobile Phone Number Required";
            txtphone.Focus();
        }
        else if (area.Equals("0"))
        {
            output = "Select Operating Area";
        }
        else if (type.Equals("0"))
        {
            output = "Select User Type";
        }
        else
        {
            output = "OK";
        }
        return output;
    }
    protected void ddlAreas_DataBound(object sender, EventArgs e)
    {
        ddlAreas.Items.Insert(0, new ListItem(" Select Area ", "0"));
    }
}

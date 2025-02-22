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

public partial class ViewUsers : System.Web.UI.Page
{
    Databasefile data_file = new Databasefile();
    Processfile Process_file = new Processfile();
    DataTable data_table = new DataTable();
    DataTable d_table = new DataTable();
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (IsPostBack == false)
            {
                MultiView2.ActiveViewIndex = 0;
                LoadAreas();
                LoadRoles();
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
    protected void btnOK_Click(object sender, EventArgs e)
    {
        try
        {
            LoadUsers();
        }
        catch (Exception ex)
        {
            ShowMessage(ex.Message, true);
        }
    }

    private void LoadUsers()
    {
        string area_code = ddlAreas.SelectedValue.ToString();
        string user_type_code = ddlUserType.SelectedValue.ToString();
        string name = txtSearch.Text.Trim();
        data_table = Process_file.GetUsers(area_code, user_type_code, name);
        if (data_table.Rows.Count > 0)
        {
            ShowMessage(".", true);
        }
        else
        {
            ShowMessage("No Results Returned", true);
        }
        DataGrid1.DataSource = data_table;
        DataGrid1.CurrentPageIndex = 0;
        DataGrid1.DataBind();
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
     protected void DataGrid1_ItemCommand(object source, DataGridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "btnEdit")
            {
                string user_code = e.Item.Cells[0].Text;
                Response.Redirect("./AddUser.aspx?transferid=" + user_code, false);                
            }
            else if (e.CommandName == "btnCredit")
            {
                string user_code = e.Item.Cells[0].Text;
                string username = e.Item.Cells[1].Text;
                string name = e.Item.Cells[5].Text;
                LoadCreditControl(user_code,username,name);
            }
        }
        catch (Exception ex)
        {
            ShowMessage(ex.Message, true);
        }
    }

    private void LoadCreditControl(string user_code, string username, string name)
    {
        MultiView2.ActiveViewIndex = 1;
        lblPhoneCode.Text = user_code;
        txtUserName.Text = username;
        txtName.Text = name;
        lblCredit.Text = GetCredit(username);
    }

    private string GetCredit(string username)
    {
        string response = "";
        try
        {
            int money1 = 0;
            DataTable dt = data_file.GetCurrentCredit(username);
            if (dt.Rows.Count > 0)
            {
                money1 = int.Parse(dt.Rows[0]["Credit"].ToString());
            }
            response = "YOUR CURRENT CREDIT IS: " + money1.ToString("#,##0");
        }
        catch (Exception ex)
        {
            throw ex;
        }
        return response;
    }
    protected void DataGrid1_PageIndexChanged(object source, DataGridPageChangedEventArgs e)
    {
        try
        {
            string area_code = ddlAreas.SelectedValue.ToString();
            string user_type_code = ddlUserType.SelectedValue.ToString();
            string name = txtSearch.Text.Trim();
            data_table = Process_file.GetUsers(area_code, user_type_code, name);
            DataGrid1.DataSource = data_table;
            DataGrid1.CurrentPageIndex = e.NewPageIndex;
            DataGrid1.DataBind();
        }
        catch (Exception ex)
        {
            ShowMessage(ex.Message,true);
        }
    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        string username = txtUserName.Text.Trim();
        string name = txtName.Text.Trim();
        string credit = txtCredit.Text.Trim();
        if (credit.Equals(""))
        {
            ShowMessage("Please Enter Credit to add", true);
            txtCredit.Focus();
        }
        else
        {
            string res = Process_file.AddCredit(username, credit, name);
            if (res.Contains("SUCCESSFULLY"))
            {
                MultiView2.ActiveViewIndex = 0;
                ShowMessage(res, false);
            }
            else
            {
                ShowMessage(res, true);
            }
            
        }
    }
    protected void ddlUserType_DataBound(object sender, EventArgs e)
    {
        ddlUserType.Items.Insert(0, new ListItem(" All User Types ", "0"));
    }
    protected void ddlAreas_DataBound(object sender, EventArgs e)
    {
        ddlAreas.Items.Insert(0, new ListItem(" All Areas ", "0"));
    }
}

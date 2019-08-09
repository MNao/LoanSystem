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

public partial class Areas : System.Web.UI.Page
{
    Databasefile data_file = new Databasefile();
    Processfile Process_file = new Processfile();
    DataTable data_table = new DataTable();

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (IsPostBack == false)
            {
                MultiView1.ActiveViewIndex = 0;
                LoadLocations();
                LinkButton MenuSms = (LinkButton)Master.FindControl("lblsmsPanel");
                LinkButton MenuReport = (LinkButton)Master.FindControl("lblReporting");
                LinkButton MenuProfile = (LinkButton)Master.FindControl("lblSetup");
                LinkButton MenuSettting = (LinkButton)Master.FindControl("lbtnSetting");
                MenuSms.Font.Italic = false;
                MenuReport.Font.Italic = false;
                MenuSettting.Font.Italic = true;
                MenuProfile.Font.Italic = false;
               string strProcessScript = "this.value='Working...';this.disabled=true;";
                Button1.Attributes.Add("onclick", strProcessScript + ClientScript.GetPostBackEventReference(Button1, "").ToString());
            }
        }
        catch (Exception ex)
        {
            ShowMessage(ex.Message, true);
        }
    }

    private void LoadLocations()
    {
        MultiView2.ActiveViewIndex = 0;
        data_table = data_file.GetLocationslist();
        DataGrid1.DataSource = data_table;
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
                string Location_code = e.Item.Cells[0].Text;
                string Location = e.Item.Cells[1].Text;
                lblCode.Text = Location_code;
                txtLocation.Text = Location;
                ShowMessage(".", true);
            }           
        }
        catch (Exception ex)
        {
            ShowMessage(ex.Message, true);
        }
    }    
    protected void Button1_Click(object sender, EventArgs e)
    {
        try
        {
            string location_code = lblCode.Text.Trim();
            string location = txtLocation.Text.Trim();
            if (location.Equals(""))
            {
                ShowMessage("Please Enter Location Name", true);
                txtLocation.Focus();
            }
            else
            {
                string res_tariff = Process_file.Save_Location(location_code, location);
                ShowMessage("LOCATION DETAILS SAVE SUCCESSFULLY", false);
                txtLocation.Text = "";
                lblCode.Text = "0";
                LoadLocations();
            }
        }
        catch (Exception ex)
        {
            ShowMessage(ex.Message, true);
        }
    }
    protected void DataGrid1_PageIndexChanged(object source, DataGridPageChangedEventArgs e)
    {
        try
        {
            data_table = data_file.GetLocationslist();
            DataGrid1.DataSource = data_table;
            DataGrid1.CurrentPageIndex = e.NewPageIndex;
            DataGrid1.DataBind();
        }
        catch (Exception ex)
        {
            ShowMessage(ex.Message, true);
        }
    }
   
}

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

public partial class Tariff : System.Web.UI.Page
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
                LoadRates();
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

    private void LoadRates()
    {
        MultiView2.ActiveViewIndex = 0;
        data_table = data_file.GetTariffRates();
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
                string network = e.Item.Cells[1].Text;
                string rate = e.Item.Cells[3].Text;
                txtNetwork.Text = network;
                txtRate.Text = rate;
                txtNetwork.Enabled = false;
                txtRate.Enabled = true;
                ShowMessage(".", true);
            }
            else if (e.CommandName == "btnAdd")
            {
                string listCode = e.Item.Cells[0].Text;
                string listName = e.Item.Cells[3].Text;
                string active = e.Item.Cells[4].Text;
                if (active.Equals("YES"))
                {
                    Response.Redirect("./PhoneNumber.aspx?transfereid=" + listCode, false);
                }
                else
                {
                    ShowMessage("List "+listName+" is not active to add numbers on", true);
                }
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
            string network = txtNetwork.Text.Trim();
            string rate = txtRate.Text.Trim();
            if (network.Equals(""))
            {
                ShowMessage("Please Enter Network", true);
                txtNetwork.Focus();
            }
            else if (rate.Equals(""))
            {
                ShowMessage("Please Enter Network Tariff Rate", true);
                txtRate.Focus();
            }
            else
            {
                string res_tariff = Process_file.Save_Tariff(network, rate);
                ShowMessage("NETWORK TARIFF SAVE SUCCESSFULLY", false);
                txtNetwork.Text = "";
                txtRate.Text = "";
                txtNetwork.Enabled = true;
                txtRate.Enabled = true;
                LoadRates();
            }
        }
        catch (Exception ex)
        {
            ShowMessage(ex.Message, true);
        }
    }
    protected void DataGrid1_PageIndexChanged(object source, DataGridPageChangedEventArgs e)
    {

    }
   
}

using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

public partial class SmsSending : System.Web.UI.Page
{
    Processfile process_file = new Processfile();
    DataTable data_table = new DataTable();
    Databasefile data_file = new Databasefile();
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (IsPostBack == false)
            {
                int credit = process_file.GetUserCredit();
                if (!credit.Equals(0))
                {
                    MultiView1.ActiveViewIndex = 0;
                    lblCredit.Text = "YOUR AVAILABLE SMS CREDIT IS " + credit.ToString("#,##0");
                    LoadActivelists();
                    txtMessage.Attributes["onkeydown"] = String.Format("count('{0}')", txtMessage.ClientID); 
                }
                else
                {
                    MultiView1.ActiveViewIndex = 1;
                    lblerror.Text = "YOUR SMS CREDIT BALANCE IS ZERO, PLEASE CONTACT ADMINISTRATOR";
                }
                LinkButton MenuSms = (LinkButton)Master.FindControl("lblsmsPanel");
                LinkButton MenuReport = (LinkButton)Master.FindControl("lblReporting");
                LinkButton MenuProfile = (LinkButton)Master.FindControl("lblSetup");
                LinkButton MenuSettting = (LinkButton)Master.FindControl("lbtnSetting");
                MenuSms.Font.Italic = true;
                MenuReport.Font.Italic = false;
                MenuSettting.Font.Italic = false;
                MenuProfile.Font.Italic = false;
                string strProcessScript = "this.value='Working...';this.disabled=true;";
                btnOK.Attributes.Add("onclick", strProcessScript + ClientScript.GetPostBackEventReference(btnOK, "").ToString());
                LoadLocations();
                chkAll.Visible = false;
            }
        }
        catch (Exception ex)
        {
            ShowMessage(ex.Message, true);
        } 
    }

    private void LoadActivelists()
    {
        data_table = process_file.GetActiveLists();
        ddlists.DataSource = data_table;
        ddlists.DataValueField = "ListID";
        ddlists.DataTextField = "ListName";
        ddlists.DataBind();
    }
    private void LoadContacts(string AreaID)
    {
        try
        {
            data_table = data_file.GetFieldEngineersByLocation(AreaID);
            lbContacts.DataSource = data_table;
            lbContacts.DataTextField = "Name";
            lbContacts.DataValueField = "Contact";
            lbContacts.DataBind();
            
        }
        catch(Exception ex)
        {
            throw ex;
        }
    }
    private void LoadLocations()
    {
       // data_table = data_file.GetLocationslist();
       data_table= data_file.GetAreas();
        ddlLocation.DataSource = data_table;
        ddlLocation.DataTextField = "Area";
        ddlLocation.DataValueField = "AreaID";
        ddlLocation.DataBind();
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
    protected void btnOK_Click(object sender, EventArgs e)
    {
        try
        {
            string list_code = ddlists.SelectedValue.ToString();
            string location = ddlLocation.SelectedValue.ToString();
            string list_name = ddlists.SelectedItem.ToString();
            string prefix = ddlPrefix.SelectedItem.ToString();
            string incidenceNo=txtIncidenceNo.Text;
            string message = "Inc: "+ incidenceNo+' '+txtMessage.Text.Trim();
           // string enterednumbers = txtPhones.Text.Trim();
            string enterednumbers = GetNumbers();
            int numberCount=lbContacts.Items.Count;
            if (location.Equals("0"))
            {
                ShowMessage("Select Location ", true);
            }
            else if (list_code.Equals("0") && enterednumbers.Equals(""))
            {
                ShowMessage("Select Number(s) or Select List to Send Message to", true);
            }
            else if (incidenceNo.Length!=7)
            {
                ShowMessage("Incidence Number should a numeric value of 7 digits", true);
            }
            //else if (list_code.Equals("0"))
            //{
            //    ShowMessage("Please Select List to Send Message to", true);
            //}
            else if (message.Trim().Equals(""))
            {
                ShowMessage("Please Enter Message to send", true);
                txtMessage.Focus();
            }
            else
            {
                //string nums = txtPhones.Text.Trim();
                string textNumbers = GetNumbers();
                if (process_file.SufficientCredit(list_code))
                {
                    Toggle(list_name, prefix, message);
                }
                else
                {
                    ShowMessage("Insufficient Credit for List " + list_name, true);
                }
            }
            int credit = process_file.GetUserCredit();
            lblCredit.Text = "YOUR AVAILABLE SMS CREDIT IS " + credit.ToString("#,##0");
        }
        catch (Exception ex)
        {
            ShowMessage(ex.Message, true);
        }
    }

    private string GetNumbers()
    {
        string tels = "";
        //foreach (Object selectedItem in lbContacts.)
        //{
        //    string value = selectedItem as String;
        //    tels = value + ",";
        //}
        if (lbContacts.Items.Count > 0)
        {
            for (int i = 0; i < lbContacts.Items.Count; i++) 
            {
                string value = lbContacts.Items[i].Value.ToString();
                bool selected=lbContacts.Items[i].Selected;
                if (selected)
                {
                    tels = tels + value + ",";
                }
            }
        }
        tels = tels.TrimEnd(',');
        return tels;
    }

    private void Toggle(string list_name, string prefix, string message)
    {
        MultiView1.ActiveViewIndex = 2;
        //string EnteredNumber = txtPhones.Text.Trim();
        string EnteredNumber = GetNumbers();
        if (list_name.Trim().Equals("Select List"))
        {
            list_name = "";
            txtviewlistname.Text = EnteredNumber;
        }
        else if (EnteredNumber.Equals(""))
        {
            txtviewlistname.Text = list_name;
            lblList.Text = "Selected List: " + list_name;
        }
        else 
        {
            txtviewlistname.Text = list_name + ", " + EnteredNumber;
            lblList.Text = "Selected List: " + list_name;
        }
        string[] contact = EnteredNumber.Split(',');
        string details = "";
        foreach (string con in contact)
        {
            data_table = data_file.GetFieldEngineerDetailsByContact(con);
            details += data_table.Rows[0]["Name"].ToString()+"\n";
        }
        txtViewContacts.Text = details;
        txtviewprefix.Text = prefix;
        txtViewMessage.Text = message;
        Label1.Text = "Please Confirm Details Below";
        ShowMessage("Please Confirm and Continue", false);
    }
    protected void ddlists_DataBound(object sender, EventArgs e)
    {
        ddlists.Items.Insert(0, new ListItem(" Select List ", "0"));
    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        try
        {
            string list_code = ddlists.SelectedValue.ToString();
            string otherPhones = txtviewlistname.Text.Trim();
            string prefix = txtviewprefix.Text.Trim();
            string message = txtViewMessage.Text.Trim();
            string areaID = Session["AreaID"].ToString();
            int numberCount = lbContacts.Items.Count;

            if (list_code.Equals("0") && numberCount.Equals(0))
            {
                ShowMessage("Please Select List To Send to", true);
                MultiView1.ActiveViewIndex = 0;
            }
            else
            {
                string res_log = process_file.LogSMS(list_code, prefix, message, otherPhones, areaID);
                if (res_log.Contains("Successfully"))
                {
                    ShowMessage(res_log, false);
                    Clear_contrls();
                }
                else
                {
                    ShowMessage(res_log, true);
                }
                MultiView1.ActiveViewIndex = 0;
            }
        }
        catch (Exception ex)
        {
            ShowMessage(ex.Message, true);
        }
    }

    private void Clear_contrls()
    {
        ddlists.SelectedIndex = ddlists.Items.IndexOf(ddlists.Items.FindByValue("0"));
        ddlPrefix.SelectedIndex = ddlPrefix.Items.IndexOf(ddlPrefix.Items.FindByValue("0"));
        ddlLocation.SelectedIndex = ddlLocation.Items.IndexOf(ddlLocation.Items.FindByValue("0"));
        txtMessage.Text = "";
        txtIncidenceNo.Text = "";
        LoadContacts("0");
        chkAll.Checked = false;
        chkAll.Visible = false;
        
    }
    protected void Button2_Click(object sender, EventArgs e)
    {
        MultiView1.ActiveViewIndex = 0;
        ShowMessage(".", true);
    }
    protected void ddlPrefix_SelectedIndexChanged(object sender, EventArgs e)
    {
        string prefix = ddlPrefix.SelectedValue.ToString();
        if (prefix.Equals("0"))
        {
            lblMessageLength.Text = "SMS MESSAGE LENGTH : 160";
        }
        else if (prefix.Equals("1"))
        {
            lblMessageLength.Text = "SMS MESSAGE LENGTH : 155";
        }
        else
        {
            lblMessageLength.Text = "SMS MESSAGE LENGTH : 154";
        }
    }
    protected void ddlLocation_DataBound(object sender, EventArgs e)
    {
        ddlLocation.Items.Insert(0, new ListItem(" Select Location ", "0"));
    }
    protected void ddlLocation_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            string location = ddlLocation.SelectedValue.ToString();
            LoadContacts(location);
            chkAll.Visible = true;
            chkAll.Checked = false;
        }
        catch(Exception ex)
        {
            ShowMessage(ex.Message,true);
        }
    }
    protected void lbContacts_DataBound(object sender, EventArgs e)
    {
        //lbContacts.Items.Insert(0, new ListItem(" Select Contacts ", "0"));
    }
    protected void chkAll_CheckedChanged(object sender, EventArgs e)
    {

        bool check = chkAll.Checked;
        if (check)
        {
            for (int i = 0; i < lbContacts.Items.Count; i++)
            {
                 lbContacts.Items[i].Selected=true;
               
            }
        }
        else 
        {
            for (int i = 0; i < lbContacts.Items.Count; i++)
            {
                lbContacts.Items[i].Selected = false;

            }
        }
    }
}

using System;
using System.Data;
using System.Collections;
using System.Drawing;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using System.Web;
using System.IO;
using InterConnect.LeshLaonApi;
using Ionic.Zip;

public partial class ApproveLoan : Page
{

    FileUpload uploadedFile;
    SystemUser user;
    BusinessLogic bll = new BusinessLogic();


    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            user = Session["User"] as SystemUser;
            if (IsPostBack) return;
            if ((Session["Username"] == null))
            {
                Response.Redirect("Default.aspx");
            }
            MultiView1.ActiveViewIndex = 0;

            LoadData();
            //LoadMessageTemplates();

        }
        catch (Exception ex)
        {
            ShowMessage(ex.Message, true);
        }
    }

    protected void LoadData()
    {
        //bll.LoadAgentsIntopDropDown(user, ddAgents);

        if (user.RoleCode == "003")
        {
            ddStatus.SelectedItem.Value = "PENDING";
        }
        SearchDB();
    }

    private void ShowMessage(string Message, bool Error)
    {
        var lblmsg = (Label)Master.FindControl("lblmsg");
        if (Error)
        {
            lblmsg.ForeColor = Color.Red;
            lblmsg.Font.Bold = false;
        }
        else
        {
            lblmsg.ForeColor = Color.Green;
            lblmsg.Font.Bold = true;
        }

        if (Message == ".")
            lblmsg.Text = ".";
        else
            lblmsg.Text = "MESSAGE: " + Message.ToUpper();
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            SearchDB();
        }
        catch (Exception ex)
        {
            ShowMessage(ex.Message, true);
        }
    }

    public void SearchDB()
    {
        string[] Params = GetSearchParameters();
        DataTable dt = bll.SearchLoanDetailsTableForApproval(Params);
        if (dt.Rows.Count > 0)
        {
            dataGridResults.DataSource = dt;
            dataGridResults.DataBind();
            string msg = "Found " + dt.Rows.Count + " Records Matching Search Criteria";
            MultiView2.ActiveViewIndex = 0;
            Label lblmsg = (Label)Master.FindControl("lblmsg");
            bll.ShowMessage(lblmsg, msg, false, Session);
        }
        else
        {
            dataGridResults.DataSource = null;
            dataGridResults.DataBind();
            MultiView2.ActiveViewIndex = -1;
            string msg = "No Records Found Matching Search Criteria";
            Label lblmsg = (Label)Master.FindControl("lblmsg");
            bll.ShowMessage(lblmsg, msg, true, Session);
        }
    }

    private string[] GetSearchParameters()
    {
        List<string> searchCriteria = new List<string>();
        string ClientNo = txtClientNo.Text.Trim();
        string UserId = user.UserId;
        string Status = ddStatus.SelectedValue;
        string StartDate = txtStartDate.Text;
        string EndDate = txtEndDate.Text;

        searchCriteria.Add(ClientNo);
        searchCriteria.Add(UserId);
        searchCriteria.Add(Status);
        searchCriteria.Add(StartDate);
        searchCriteria.Add(EndDate);
        return searchCriteria.ToArray();
    }

    private ArrayList GetNumbers(string nums)
    {
        var tels = new ArrayList();
        //PhoneValidator pv = new PhoneValidator();
        //try
        //{
        //    string phones = nums.Split(',');
        //    foreach (string s in phones)
        //    {
        //        if (pv.PhoneNumbersOk(s))
        //        {
        //            s = pv.Format(s);
        //            tels.Add(s);
        //        }
        //    }
        //}
        //catch (Exception ex)
        //{
        //    ShowMessage(ex.Message, true);
        //}
        return tels;
    }

    //private void Toggle(string listCode, string prefix, string message)
    //{

    //    MultiView1.ActiveViewIndex = 2;

    //    string contactsToDisplay = "";
    //    var listName = ddlists.SelectedItem.ToString();
    //    var enteredNumber = txtPhones.Text.Trim();      

    //    var receipientType = ddlReceipient.SelectedValue;

    //    //User entered a list of numbers comma separated
    //    if (receipientType == RECEIPTION_TYPE_PHONE_NO) {
    //        contactsToDisplay = txtPhones.Text.Trim();
    //    }

    //    //User selected a list to send to
    //    else if (receipientType == RECEIPTION_TYPE_LIST)
    //    {
    //        contactsToDisplay = listName;
    //    }

    //    //User uploaded a file
    //    else {
    //        contactsToDisplay = FileUpload1.FileName;
    //        uploadedFile = FileUpload1;
    //        Session["uploadedFile"] = uploadedFile;

    //    }

    //    ////Not a receiption type list so we the phone numbers
    //    //if (listName.Trim().Equals("Select List"))
    //    //{
    //    //    listName = "";           
    //    //    //Get the list of phone numbers
    //    //    txtviewlistname.Text = enteredNumber;
    //    //}
    //    //else if (enteredNumber.Equals(""))
    //    //{
    //    //    //No phone numbers input
    //    //    txtviewlistname.Text = listName;
    //    //}
    //    //else
    //    //{
    //    //    txtviewlistname.Text = listName + ", " + enteredNumber;
    //    //}


    //    txtviewlistname.Text = contactsToDisplay;
    //    txtviewprefix.Text = prefix;
    //    txtViewMessage.Text = message;
    //    Label1.Text = "Please Confirm Details Below";
    //    ShowMessage("Please Confirm and Continue", false);

    //}

    //protected void ddlists_DataBound(object sender, EventArgs e)
    //{
    //    ddlists.Items.Insert(0, new ListItem(" Select Group ", "0"));
    //}

    //protected void Button1_Click(object sender, EventArgs e)
    //{
    //    try
    //    {
    //var Receipient = ddlReceipient.SelectedItem.ToString();
    //var ReceipientType = ddlReceipient.SelectedValue;

    //var Phones = txtPhones.Text;
    //var ContactGroup = ddlists.SelectedValue;
    //var FileUpload = FileUpload1.FileName;
    //var message = txtViewMessage.Text.Trim();
    //var VendorCode = Session["VendorCode"].ToString();


    //if (Receipient.Equals(""))
    //{
    //    ShowMessage("Please Select a contact group To Send to", true);
    //    MultiView1.ActiveViewIndex = 0;
    //}else
    //if (txtMessage.Equals(""))
    //{
    //    ShowMessage("Please type a message to send", true);
    //    MultiView1.ActiveViewIndex = 0;
    //}
    //else
    //{
    //    var resLog = "";

    //    if (ReceipientType.Equals(RECEIPTION_TYPE_PHONE_NO) && (txtPhones.Text != null || txtPhones.Text != ""))
    //    {

    //        String[] phoneNumberArr = Phones.Split(',');
    //        resLog = _processFile.LogSMSCommaSeparatedList(phoneNumberArr, message, VendorCode);

    //    }

    //    else if (ReceipientType.Equals(RECEIPTION_TYPE_LIST) && (ddlists.SelectedValue.ToString() != null || ddlists.SelectedValue.ToString() != ""))
    //    {
    //        string listId = ddlists.SelectedValue.ToString();
    //        string user = Session["Username"].ToString();
    //        string mask = Session["Mask"].ToString();
    //        string SenderId = Session["SenderId"].ToString();
    //        string filepath = "";
    //        resLog = _processFile.LogSMSFileUpload(listId, filepath, message, VendorCode, user, mask, SenderId, "ListSMS");

    //    }

    //    else if (ReceipientType.Equals(RECEIPTION_TYPE_FILE))

    //    {

    //          uploadedFile = Session["uploadedFile"] as FileUpload;
    //        //string filename = Path.GetFileName(FileUpload1.FileName);
    //        string filename = Path.GetFileName(uploadedFile.FileName);
    //        string extension = Path.GetExtension(filename);

    //        string pathToFile = @"E:\SMSUploads\FilessToSend";
    //        DateTime todaydate = DateTime.Now;
    //        string datetoday = todaydate.ToString().Replace("/", "-").Replace(":", "-").Replace(" ", "-");
    //        string filepath = pathToFile + filename + "_" + datetoday + extension;
    //        if (!Directory.Exists(pathToFile))
    //            Directory.CreateDirectory(pathToFile);

    //        uploadedFile.SaveAs(filepath);
    //        //FileUpload1.SaveAs(filepath);

    //        var user = HttpContext.Current.Session["Username"].ToString();
    //        var mask = HttpContext.Current.Session["Mask"].ToString();
    //        var SenderId = HttpContext.Current.Session["SenderId"].ToString();
    //        var listId = "";

    //        resLog = _processFile.LogSMSFileUpload(listId, filepath, message, VendorCode, user, mask, SenderId, "FileSMS");

    //    }


    //    if (resLog.Contains("Successfully"))
    //    {
    //        ShowMessage(resLog, false);
    //        Clear_contrls();
    //    }
    //    else
    //    {
    //        ShowMessage(resLog, true);
    //    }

    //    MultiView1.ActiveViewIndex = 0;
    //}
    //    }
    //    catch (Exception ex)
    //    {
    //        ShowMessage(ex.Message, true);
    //    }
    //}

    //private void Clear_contrls()
    //{
    //    ddlists.SelectedIndex = ddlists.Items.IndexOf(ddlists.Items.FindByValue("0"));
    //    ddlReceipient.SelectedIndex = ddlReceipient.Items.IndexOf(ddlReceipient.Items.FindByValue("0"));
    //    txtMessage.Text = "";
    //}

    //protected void Button2_Click(object sender, EventArgs e)
    //{
    //    MultiView1.ActiveViewIndex = 0;
    //    ShowMessage(".", true);
    //}



    //protected void ddlReceipient_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    var prefix = ddlReceipient.SelectedValue;
    //    if (prefix.Equals("0")){
    //        MultiView2.ActiveViewIndex = 0;
    //    }
    //    else if (prefix.Equals("1"))
    //    {
    //        //lblMessageLength.Text = "SMS MESSAGE LENGTH : 155";
    //        //LoadActivelists();
    //        MultiView2.ActiveViewIndex = 1;
    //    }
    //    else if (prefix.Equals("2"))
    //    {
    //        //lblMessageLength.Text = "SMS MESSAGE LENGTH : 154";
    //        MultiView2.ActiveViewIndex = 2;
    //    }
    //    lblMessageLength.Text = "SMS MESSAGE LENGTH : 160";
    //}


    //protected void ddlMessageTemplates_SelectedIndexChanged(object sender, EventArgs e)
    //{

    //    var messageTemplate = ddlMessageTemplates.SelectedValue;
    //    txtMessage.Text = messageTemplate;

    //}


    protected void dataGridResults_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        int index = 0;
        GridViewRow row;
        GridView grid = sender as GridView;
        index = Convert.ToInt32(e.CommandArgument);
        row = grid.Rows[index];
        string ClientID = row.Cells[1].Text;
        string LoanNo = row.Cells[2].Text;
        string PhoneNumber = row.Cells[4].Text;
        string IDNumber = row.Cells[6].Text;
        string status = row.Cells[8].Text;
        Label lblmsg = (Label)Master.FindControl("lblmsg");

        if (e.CommandName.Equals("Verify"))
        {
            if (IDNumber != "")
            {
                Server.Transfer("~/AddLoan.aspx?ClientID=" + ClientID + "&LoanNo=" + LoanNo + "&Status=" + status);
                //return;
            }
            else
            {
                bll.ShowMessage(lblmsg, "Loan Missing details", true, Session);
            }

        }
        if (e.CommandName.Equals("Download"))
        {
            string OutPutFileName = LoanNo + ".zip";
            Response.Clear();
            Response.ContentType = "application/zip";
            Response.AddHeader("Content-Disposition", "attachment; filename=" + OutPutFileName);
            using (ZipFile zipfile = new ZipFile())
            {
                zipfile.AddSelectedFiles("*.*", Server.MapPath("Images"), false);
                zipfile.Save(Response.OutputStream);
            }
            Response.Close();
        }
    }
}
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

public partial class PhoneNumber : System.Web.UI.Page
{
    Databasefile data_file = new Databasefile();
    Processfile Process_file = new Processfile();
    DataTable data_table = new DataTable();
    PhoneValidator phone_validity = new PhoneValidator();
    DataFile df;
    private ArrayList fileContents;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (IsPostBack == false)
            {
                MultiView1.ActiveViewIndex = 0;
                LoadLists();
                if (Request.QueryString["transfereid"] != null)
                {
                    string list_code = Request.QueryString["transfereid"].ToString();
                    ddllists.SelectedIndex = ddllists.Items.IndexOf(ddllists.Items.FindByValue(list_code));
                    ddllists.Enabled = false;
                }
                else
                {
                    ddllists.Enabled = true;
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
                Button1.Attributes.Add("onclick", strProcessScript + ClientScript.GetPostBackEventReference(Button1, "").ToString());
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
    private void LoadLists()
    {
        data_table = Process_file.GetActiveLists();
        ddllists.DataSource = data_table;
        ddllists.DataValueField = "ListID";
        ddllists.DataTextField = "ListName";
        ddllists.DataBind();
    }
    protected void ddllists_DataBound(object sender, EventArgs e)
    {
        ddllists.Items.Insert(0, new ListItem(" Select List ", "0"));
    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        try
        {
            string list_code = ddllists.SelectedValue.ToString();
            string phone = txtPhoneNumber.Text.Trim();
            if (list_code.Equals("0"))
            {
                ShowMessage("Select List To add Numbers to", true);
            }
            else if (FileUpload1.FileName.Trim().Equals("") && phone.Equals(""))
            {
                ShowMessage("Browse file to Upload or Enter Phone Number", true);
            }
            else
            {
                UploadNumbers(phone);
            }
        }
        catch (Exception ex)
        {
            ShowMessage(ex.Message, true);
        }
    }

    private void UploadNumbers(string phone)
    {
        if (!FileUpload1.FileName.Trim().Equals(""))
        {
            ReadFile();
        }
        else
        {
            string name = txtName.Text.Trim();
            string list_code = ddllists.SelectedValue.ToString();
            if (phone_validity.PhoneNumbersOk(phone))
            {
                Process_file.SavePhoneNumber(phone, name, list_code);

                txtName.Text = "";
                txtPhoneNumber.Text = "";
                ShowMessage(phone + " Add to lists Successfully", false);
            }
            else
            {
                ShowMessage(phone + " is not a valid Phone Number", true);
            }
        }
    }
    private void ReadFile()
    {
        HttpFileCollection uploads;
        uploads = HttpContext.Current.Request.Files;
        string c = FileUpload1.FileName;
        string file_ext = Path.GetExtension(c);
        string cNoSpace = c.Replace(" ", "-");
        string User = Session["Username"].ToString().Replace(" ", "-");
        string Date = DateTime.Now.ToString().Replace("/", "-");
        Date = Date.Replace(":", "-");
        string c1 = User + "_" + Date + "_" + cNoSpace;
        c1 = c1.Replace(" ", "");
        string PathFrom = "C:\\Sms\\files\\";
        Process_file.CheckPath(PathFrom);
        string FullPath = (PathFrom + "" + c1);
        FileUpload1.PostedFile.SaveAs(FullPath);

        if (file_ext == ".csv" || file_ext == ".txt")
        {
            int count = 0;
            int position = 0;
            df = new DataFile();
            fileContents = df.readFile(FullPath);
            for (int i = 0; i < fileContents.Count; i++)
            {
                position = i+1;
                string line = fileContents[i].ToString();
                string[] sLine = line.Split(',');
                //line = line.Replace("", "");
                if (sLine.Length == 1 || sLine.Length == 2)
                {
                    string phone = sLine[0].ToString();
                    if (phone_validity.PhoneNumbersOk(phone))
                    {
                        count = i + 1;
                    }
                    else
                    {
                        throw new Exception("Invalid Phone Number at line " + position);
                    }
                }
                else
                {
                    throw new Exception("File Format is not OK, Columns must be 1 or 2..");
                }

            }
            lblPath.Text = FullPath;
            Toggle(count, true);

        }
        else
        {
            Process_file.RemoveFile(FullPath);
            ShowMessage("File format " + file_ext + " is not supported", true);
        }
    }


    private void Toggle(int count, bool Check)
    {
        MultiView1.ActiveViewIndex = 1;
        ddllists.Enabled = false;
        lblQn.Text = "Are you sure you want to upload a file of " + count + " Number(s) to " + ddllists.SelectedItem.ToString();
    }


    protected void btnYes_Click(object sender, EventArgs e)
    {
        try
        {
            int count =0;
            string list_code = ddllists.SelectedValue.ToString();
            string list_name = ddllists.SelectedItem.ToString();
            string FullPath = lblPath.Text.Trim();
            df = new DataFile();
            fileContents = df.readFile(FullPath);
            for (int i = 0; i < fileContents.Count; i++)
            {
                count++;
                string phone = "";
                string name = "";
                string line = fileContents[i].ToString();
                string[] sLine = line.Split(',');
                string[] StrArray = line.Split(Convert.ToChar(","));
                phone = StrArray[0].ToString();
                if (sLine.Length == 2)
                {
                    name = StrArray[1].ToString().ToUpper();
                }
                Process_file.SavePhoneNumber(phone, name, list_code);
                MultiView1.ActiveViewIndex = 0;
            }
            string msg = count + " Phone Number(s) have been add to list(" + list_name + ")";
            ShowMessage(msg, true);
        }
        catch (Exception ex)
        {
            ShowMessage(ex.Message, true);
        }
    }
    protected void btnNo_Click(object sender, EventArgs e)
    {
        MultiView1.ActiveViewIndex = 0;
    }
}

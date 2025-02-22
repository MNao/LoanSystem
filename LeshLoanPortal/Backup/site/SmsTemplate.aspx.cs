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
using InterConnect.SMSService;
public partial class SmsTemplate : System.Web.UI.Page
{
    int flag = 0;
    int noOfCol = 0;
    int noOfParams = 0;
    Processfile process_file = new Processfile();
    DataTable data_table = new DataTable();
    

    DataTable networkRates = new DataTable();
    DataTable networkCodes = new DataTable();

    PhoneValidator phone_validity;
    private Databasefile dp;

    ArrayList Codes = new ArrayList();
    Hashtable rates = new Hashtable();
    Hashtable nCodes = new Hashtable();

    DataFile df = new DataFile();

    ArrayList csv = new ArrayList();

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                int credit = process_file.GetUserCredit();
                if (!credit.Equals(0))
                {
                    MultiView1.ActiveViewIndex = 0;
                    lblCredit.Text = "YOUR AVAILABLE SMS CREDIT IS " + credit.ToString("#,##0");

                    txtMessage.Attributes["onkeydown"] = String.Format("count('{0}')", txtMessage.ClientID);
                }
                else
                {
                    MultiView1.ActiveViewIndex = 1;
                    lblerror.Text = "YOUR SMS CREDIT BALANCE IS ZERO, PLEASE CONTACT ADMINISTRATOR";
                }

                txtMessage.Attributes["onkeydown"] = String.Format("count('{0}')", txtMessage.ClientID); 

                DisableBtnsOnClick();

                LinkButton MenuSms = (LinkButton)Master.FindControl("lblsmsPanel");
                LinkButton MenuReport = (LinkButton)Master.FindControl("lblReporting");
                LinkButton MenuProfile = (LinkButton)Master.FindControl("lblSetup");
                LinkButton MenuSettting = (LinkButton)Master.FindControl("lbtnSetting");
                MenuSms.Font.Italic = true;
                MenuReport.Font.Italic = false;
                MenuSettting.Font.Italic = false;
                MenuProfile.Font.Italic = false;
            }            
           SetNetworkCodes();
           SetNetworkRates();


        }
        catch (Exception ex)
        {
            ShowMessage(ex.Message, true);
        }
    }

    private void SetNetworkCodes()
    {

        NetworkCode[] ncodes1 = process_file.GetNetworkCodes();        
        networkCodes.Columns.Add("Network", typeof(string));
        networkCodes.Columns.Add("Code", typeof(string));
        //networkRates.Columns.Add("Network", typeof(string));
        //networkRates.Columns.Add("Rate(UShs.)", typeof(string));
        foreach (NetworkCode code in ncodes1)
        {
            networkCodes.Rows.Add(code.Network, code.Code);
            //networkRates.Rows.Add(rate.Network, rate.Rate);
        }

        nCodes.Clear();
        //rates.Clear();

        //DataGrid1.DataSource = networkRates;
        //DataGrid1.DataBind();

        for (int i = 0; i < networkCodes.Rows.Count; i++)
        {
            string network = networkCodes.Rows[i]["Network"].ToString();
            string nCode = networkCodes.Rows[i]["Code"].ToString();

            nCodes.Add(nCode,network);
        }
    }
    private void DisableBtnsOnClick()
    {
        string strProcessScript = "this.value='Working...';this.disabled=true;";
        btnOK.Attributes.Add("onclick", strProcessScript + ClientScript.GetPostBackEventReference(btnOK, "").ToString());
    }
    protected void btnOK_Click(object sender, EventArgs e)
    {
        try
        {
            CallFileProc();
        }
        catch (Exception ex)
        {
            ShowMessage(ex.Message, true);
        }
    }

    private void CallFileProc()
    {
        if (!FileUpload1.FileName.Trim().Equals(""))
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
            process_file.CheckPath(PathFrom);
            string FullPath = (PathFrom + "" + c1);
            FileUpload1.PostedFile.SaveAs(FullPath);
            if (file_ext == ".csv" || file_ext == ".txt")
            {
                csv = df.readFile(FullPath);
                Proc_Template(csv);
            }
            else
            {
                process_file.RemoveFile(FullPath);
                ShowMessage("File format " + file_ext + " is not supported", true);
            }
        }
        else
        {
            ShowMessage("Please Browse for the Template file", true);
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
    private int Proc_Template(ArrayList csv)
    {
        flag = 0;
        if (txtMessage.Text.Trim().Equals(""))
        {
            flag = 1;
            ShowMessage("Please type sms to send to receipients.",true);
        }
        else if (csv.Count > 0)
        {
            noOfParams = 0;
            int credit = process_file.GetUserCredit();
            int totalCost = 0;
            //GetNumber of Params in the text message
            string sms = txtMessage.Text.Trim().Replace("%s", "%S");
            char[] delimiter = new char[] { ' ' };
            string[] smsWords = sms.Split(delimiter, StringSplitOptions.RemoveEmptyEntries);
            foreach (string word in smsWords)
            {
                if (word.Trim().ToUpper().Contains("%S"))
                {
                    noOfParams++;
                }
            }
            //GetNumber of Columns in csv file
            string csvLine = csv[0].ToString();
            char[] delimiter1 = new char[] { ',' };
            string[] csvWords = csvLine.Split(delimiter1, StringSplitOptions.RemoveEmptyEntries);
            noOfCol = csvWords.Length;
            //compare and validate file against sms parameters
            if (noOfParams != noOfCol - 1)
            {
                flag = 1;
                ShowMessage("The number of columns in the csv file don't tally with the parameters in the sms", true);  
            }
            else
            {
                //Get phones to send to
                int p = 0;
                string[] phones = new string[csv.Count];
                foreach (string line in csv)
                {
                    csvWords = line.Split(delimiter1, StringSplitOptions.RemoveEmptyEntries);
                    if (csvWords.Length == noOfCol)
                    {
                        phones[p] = csvWords[0];
                    }
                    p++;
                }
                //Get Sms Cost
                totalCost = GetTotalCost(phones);
                if (totalCost < credit || totalCost == credit)
                {
                    process_file.ProcessSMS(sms, csv);// ProcessSMS(sms);
                    //update credit
                    if (totalCost > 0)
                    {
                        string username = Session["UserName"].ToString();
                        int newCredit = credit - totalCost;
                        process_file.UpdateUserCredit(username, newCredit); 
                    }
                    ShowMessage("SMSs Have Been Successfully Processed ",true);
                }
                else
                {
                    int difference = totalCost - credit;
                    flag = 1;
                    ShowMessage("Your current credit is not enough. Add an extra Ushs " + difference.ToString("#,##") + " to send the SMS",true);
                }

            }
        }
        else
        {
            flag = 1;
            ShowMessage("Load the csv/text file or enter a valid phone number", true);
        }
        return flag;
    }

    private void SetNetworkRates()
    {
        NetworkRate[] nrates = process_file.GetNetworkRates(); 
        networkRates.Columns.Add("Network", typeof(string));
        networkRates.Columns.Add("Rate(UShs.)", typeof(string));
        foreach (NetworkRate rate in nrates)
        {
            networkRates.Rows.Add(rate.Network, rate.Rate);
        }
        rates.Clear();

        DataGrid1.DataSource = networkRates;
        DataGrid1.DataBind();

        for (int i = 0; i < networkRates.Rows.Count; i++)
        {
            string network = networkRates.Rows[i]["Network"].ToString();
            string nRate = networkRates.Rows[i]["Rate(UShs.)"].ToString();
            rates.Add(network, nRate);
        }
    }
    private int GetMessageCost(string phone)
    {
        int intCost = 0;
        string code = "";
        phone_validity = new PhoneValidator();

        if (phone_validity.PhoneNumbersOk(phone))
        {
            if (phone.StartsWith("0"))
            {
                 code = phone.Substring(1, 3);
            }
            else if(phone.StartsWith("256"))
            {
                 code = phone.Substring(3, 3);
            }
            
            string ntwk = nCodes[code].ToString();
            string cost = rates[ntwk].ToString();
            intCost = int.Parse(cost);
            intCost = int.Parse(cost);
        }
        return intCost;
    }
    private int GetTotalCost(string[] phones)
    {
        int messageCost = 0;
        phone_validity = new PhoneValidator();
        foreach (string phone in phones)
        {
            string s = phone;
            if (phone != null)
            {
                s = process_file.formatPhone(s);
                if (phone_validity.PhoneNumbersOk(s))
                {
                    messageCost = messageCost + GetMessageCost(s);
                }
            }
        }
        return messageCost;
    }
}

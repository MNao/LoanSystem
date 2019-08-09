using System;
using System.Data;
using System.IO;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Collections;
using System.Text.RegularExpressions;
using InterConnect.SMSService;

/// <summary>
/// Summary description for Processfile
/// </summary>
public class Processfile
{
    SMSService smsapi;

    DataTable data_table = new DataTable();
    Databasefile data_file = new Databasefile();
    DataFile df = new DataFile();
    PhoneValidator phone_validator;
    private ArrayList fileContent;
    public Processfile()
    {
        //
        // TODO: Add constructor logic here
        //
    }
    public DataTable LoginDetails(string username, string password)
    {
        //string pass = Encryption.encrypt.DecryptString("ORsy4hbcqhEqC5NHlKoptw==", "25011Pegsms2322");
        password = EncryptString(password);
        data_table = data_file.GetUserAccessibility(username,password);
        return data_table;
    }

    public string EncryptString(string ClearText)
    {
        string ret = "";
        ret = Encryption.encrypt.EncryptString(ClearText, "25011Pegsms2322");
        return ret;
    }

    public string Reset_Passwd(string user_code, string password,bool reset)
    {
        int user_Id = int.Parse(user_code);
        password = EncryptString(password);
        data_file.ResetPassword(user_Id, password,reset);
        return "RESET";
    }

    public int GetUserCredit()
    {
        int credit = 0;
        string user = HttpContext.Current.Session["Username"].ToString();
        data_table = data_file.GetCurrentCredit(user);
        if (data_table.Rows.Count > 0)
        {
            credit = int.Parse(data_table.Rows[0]["Credit"].ToString());
        }
        return credit;
    }

    public string SaveList(string list_code, string list_name, bool is_active)
    {
        int list_id = int.Parse(list_code);
        string area_code = HttpContext.Current.Session["AreaID"].ToString();
        string user = HttpContext.Current.Session["Username"].ToString();
        data_file.SaveList(list_id, list_name,is_active, area_code, user);
        if (list_id.Equals(0))
        {
            return "SAVED";
        }
        else
        {

            return "EDITED";
        }
    }

    public DataTable GetActiveLists()
    {
        string area_code = HttpContext.Current.Session["AreaID"].ToString();
        data_table = data_file.GetActivelists(area_code);
        return data_table;
    }
    public DataTable GetAllLists()
    {
        string area_code = HttpContext.Current.Session["AreaID"].ToString();
        data_table = data_file.GetAllLists(area_code);
        return data_table;
    }
    public void CheckPath(string Path)
    {
        if (!Directory.Exists(Path))
        {
            Directory.CreateDirectory(Path);
        }
    }
    public void RemoveFile(string Path)
    {
        if (File.Exists(Path))
        {
            File.Delete(Path);
        }
    }
    public void SavePhoneNumber(string phone, string name, string list_code)
    {
        string area_code = HttpContext.Current.Session["AreaID"].ToString();
        string user = HttpContext.Current.Session["Username"].ToString();
        int list_id = int.Parse(list_code);
        phone = formatPhone(phone);
        data_file.SavePhoneToList(phone, name.ToUpper(), list_id, area_code, user);
    }

    public DataTable GetListDetails(string list_code, string phone, string name)
    {
        int list_id = int.Parse(list_code);
        data_table = data_file.GetListDetails(list_id, phone, name);
        return data_table;
    }

    public void ChangePhoneStatus(string phone_code, string status)
    {
        int phone_id = int.Parse(phone_code);
        bool active = false;
        if (status.Equals("NO"))
        {
            active = true;
        }
        data_file.ChangePhoneStatus(phone_id, active);
    }

    public void UpdatePhoneDetails(string phone_code, string phone, string phone_name, bool active)
    {
        int phone_id = int.Parse(phone_code);
        data_file.UpdatePhoneDetails(phone_id, phone, phone_name, active);
    }

    public string SaveUser(string user_code,string user_name, string fname, string lname, string phone, string email, string area_code, string type_code, bool active, bool reset)
    {
        string output = "";
        int user_id = int.Parse(user_code);
        int area_id = int.Parse(area_code);
        string user = HttpContext.Current.Session["Username"].ToString();
        user_name = GetUserName(fname, lname, user_name);
        if (UserNameExists(user_name, user_code))
        {
            output = user_name+" USERNAME EXISTS, PLEASE ENTER A USERNAME";
        }
        else
        {
            string passwd = EncryptString(user_name);
            data_file.Save_user(user_id, user_name,passwd, fname, lname, phone, email, area_id, type_code, active, reset, user);
            if (user_code.Equals("0"))
            {
                output = "USER SAVED SUCCESSFULLY";
            }
            else
            {
                output = "USER DETAILS UPDATED SUCCESSFULLY";
            }
        }
        return output;
    }

    private bool UserNameExists(string user_name, string user_code)
    {
        if (user_code.Equals("0"))
        {
            data_table = data_file.GetUserDetailsByUserName(user_name);
            if (data_table.Rows.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }
    private string GetUserName(string Fname,string Sname,string user_name)
    {
        string ret = "";
        if (user_name.Equals(""))
        {
            string initial = Fname.Substring(0, 1);
            ret = initial + "." + Sname;
            ret = ret.ToLower();
        }
        else
        {
            ret = user_name;
        }
        return ret;
    }
    public DataTable GetUsers(string area_code, string type_code, string name)
    {
        int area_id = int.Parse(area_code);
        int type_id = int.Parse(type_code);
        data_table = data_file.GetUsers(area_id, type_id, name);
        return data_table;
    }
    //public int GetTotalCost(string[] phones)
    //{
    //    int messageCost = 0;
    //    foreach (string s in phones)
    //    {
    //        messageCost = messageCost + GetMessageCost(s);
    //    }
    //    return messageCost;
    //}
    //private int GetMessageCost(string phone)
    //{
    //    int intCost = 0;
    //    string code = "";
    //    string ntwk = "";
    //    string cost = "";
    //    if (phone.Trim().StartsWith("0") && phone.Trim().Length == 10)
    //    {
    //        code = phone.Substring(1, 3);
    //        ArrayList phoneCodes = new ArrayList(nCodes.Keys);
    //        if (phoneCodes.Contains(code))
    //        {
    //            ntwk = nCodes[code].ToString();
    //            cost = rates[ntwk].ToString();
    //            intCost = int.Parse(cost);
    //        }
    //        else
    //        {
    //            intCost = 0;
    //        }
    //    }
    //    else if (phone.Trim().StartsWith("256") && phone.Trim().Length == 12)
    //    {
    //        code = phone.Substring(3, 3);
    //        ArrayList phoneCodes = new ArrayList(nCodes.Keys);
    //        if (phoneCodes.Contains(code))
    //        {
    //            ntwk = nCodes[code].ToString();
    //            cost = rates[ntwk].ToString();
    //            intCost = int.Parse(cost);
    //        }
    //        else
    //        {
    //            intCost = 0;
    //        }
    //    }
    //    else
    //    {
    //        intCost = 0;
    //    }
    //    return intCost;
    //}
    public string AddCredit(string user_name, string credit, string name)
    {
        string output = "";
        int credit_toadd = int.Parse(credit);
        if (credit_toadd.Equals(0))
        {
            output = "Credit to add cannot be zero";
        }
        else if (user_name.Equals(""))
        {
            output = "System failed to alocate User details";
        }
        else
        {
            string user = HttpContext.Current.Session["Username"].ToString();
            data_file.AddCredit(user_name, credit_toadd, user);
            output = "CREDIT OF " + credit_toadd.ToString("#,##0") + " HAS BEEN ADDED SUCCESSFULLY TO " + name;
        }
        return output;
    }

    public bool SufficientCredit(string list_code)
    {
        int list_id = int.Parse(list_code);
        int list_total = data_file.GetListTotalCost(list_id);
        int credit = GetUserCredit();
        if (credit >= list_total)
        {
            return true;
        }
        else
        {
            return false;
        }

    }

    public string LogSMS(string list_code, string prefix, string message, string otherPhones, string areaID)
    {
        string output = "";
        phone_validator = new PhoneValidator();
        int list_id = int.Parse(list_code);

        string[] ArrayPhones = otherPhones.Split(',');
        data_table = data_file.GetActiveListNumbers(list_id);

        if ((data_table.Rows.Count > 0)||(ArrayPhones.Length > 0))
        {
            string user = HttpContext.Current.Session["Username"].ToString();
            string mask = HttpContext.Current.Session["Mask"].ToString();
            int i = 0;
            int count = 0;
            foreach (DataRow dr in data_table.Rows)
            {
                string phone = phone_validator.Format(dr["PhoneNumber"].ToString().Trim());
                string name = dr["PhoneName"].ToString();
                string msg = GetformatMessage(phone, name, prefix, message);
                data_file.InsertSmsToSend(phone, msg, mask, user, areaID);
                count++;

                Reduct_credit(list_id, message, mask, user, phone);
            }
            if (count > 0) { i = 1; }

            for (; i < ArrayPhones.Length;i++,count++)
            {
                string phone = phone_validator.Format(ArrayPhones[i].ToString().Trim());                
                string msg = GetformatMessage(phone, "", prefix, message);
                data_file.InsertSmsToSend(phone, msg, mask, user, areaID);

                Reduct_creditforOtherPhones(ArrayPhones, message, mask, user,"YES");
                
            }
            
            output = "A list of " + count + " has been logged Successfully";
        }       
        else
        {
            output = "No Active Phone number(s) on list";
        }
        return output;
    }
    private void Reduct_creditforOtherPhones(string [] OtherPhones, string message, string mask, string user,string indicator)
    {
        int list_total = GetOtherPhonesinDetails(OtherPhones, indicator);           
        int credit = GetUserCredit();
        int new_balance = credit - list_total;
        string Area_code = HttpContext.Current.Session["AreaID"].ToString();
        int Area_Id = int.Parse(Area_code);

        data_file.reduce_credit(new_balance, 0, list_total, message, Area_Id, mask, user);
    }

    private int GetOtherPhonesinDetails(string[] OtherPhones, string indicator)
    {
        int x = 0;
        if (indicator.Equals("YES")) { x = 1; }
        int TotalCost=0;

        for (; x < OtherPhones.Length; x++)
        {
            string Phone = phone_validator.Format(OtherPhones[x].ToString());
            string NetworkCode = Phone.Substring(1, 3);
            int Tarrif = data_file.GetParticularTarrifWithCode(NetworkCode);
            TotalCost += Tarrif;
        }
        return TotalCost;
    }

    private void Reduct_credit(int list_id,string message,string mask,string user,string Element)
    {
        //int list_total = data_file.GetListTotalCost(list_id);
     
        string NetworkCode = Element.Substring(3, 3);
        int list_total = data_file.GetParticularTarrifWithCode(NetworkCode);

        int credit = GetUserCredit();
        int new_balance = credit - list_total;
        string Area_code = HttpContext.Current.Session["AreaID"].ToString();
        int Area_Id = int.Parse(Area_code);

        data_file.reduce_credit(new_balance, list_id, list_total,message,Area_Id,mask, user);

    }
    public void ProcessSMS(string message, ArrayList csv)
    {
        string user = HttpContext.Current.Session["Username"].ToString();
        string mask = HttpContext.Current.Session["Mask"].ToString();
        string Areaid = HttpContext.Current.Session["AreaID"].ToString();
        
        Hashtable smsToSend = new Hashtable();
        string[] smsWords = Regex.Split(message, "%S");
        int p = 0;
        int noOfCol = smsWords.Length;
        char[] delimiter = new char[] { ',' };

        foreach (string line in csv)
        {
            try
            {
                string[] words = line.Split(delimiter, StringSplitOptions.RemoveEmptyEntries);
                if (noOfCol >= words.Length)
                {
                    string phone = words[0].ToString();
                    if (phone != null)
                    {
                        phone = formatPhone(phone);
                        string sms = "";

                        phone_validator = new PhoneValidator();
                        if (phone_validator.PhoneNumbersOk(phone))
                        {
                            if (words.Length == 1)
                            {
                                sms = sms = sms + smsWords[0].Trim();
                            }
                            else
                            {
                                for (int i = 1; i < words.Length; i++)
                                {
                                    sms = sms + smsWords[i - 1].Trim() + " " + words[i].Trim() + " ";
                                    if (i == words.Length - 1)
                                    {
                                        sms = sms = sms + smsWords[i].Trim();
                                    }
                                }
                            }
                            smsToSend.Add(phone, sms);
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }
        if (smsToSend.Count > 0)
        {
            ArrayList tels = new ArrayList(smsToSend.Keys);
            foreach (string tel in tels)
            {
                string telephone = tel;
                telephone = formatPhone(telephone);

                if ((telephone.StartsWith("07") || telephone.StartsWith("03")) && telephone.Trim().Length == 10)
                {
                    telephone = telephone.Remove(0, 1);
                    telephone = "256" + telephone;
                }
                data_file.InsertSmsToSend(telephone, smsToSend[tel].ToString(), mask, user, Areaid);
                //SendSms(telephone, smsToSend[tel].ToString(), "UMEME", username);
            }
        }
    }
    public string formatPhone(string phone)
    {
        if (phone.Trim().StartsWith("256") && phone.Trim().Length == 12)
        {
            phone = phone.Remove(0, 3);
            phone = "0" + phone;
        }
        else if (phone.Trim().StartsWith("+256") && phone.Trim().Length == 13)
        {
            phone = phone.Remove(0, 4);
            phone = "0" + phone;
        }
        else if ((phone.Trim().StartsWith("7") || phone.Trim().StartsWith("3")) && phone.Trim().Length == 9)
        {
            phone = "0" + phone;
        }
        else if ((phone.StartsWith("07") || phone.StartsWith("03")) && phone.Trim().Length == 10)
        {
            phone = phone.Remove(0, 1);
            phone = "256" + phone;
        }
        return phone;
    }
    private string GetformatMessage(string phone, string name, string prefix, string message)
    {
        string output = "";
        if (prefix.Equals("NONE"))
        {
            output = message;
        }
        else
        {
            if (name.Equals("NONE"))
            {
                output = message;
            }
            else
            {
                output = prefix + " " + name + " " + message;
            }
        }
        return output;
    }

    public string Change_Passwd(string oldPasswd, string newPasswd, string confirm)
    {
        string output = "";
        if (newPasswd == confirm)
        {
            string userId = HttpContext.Current.Session["Username"].ToString();
            //oldPasswd = EncryptString(oldPasswd);
            data_table = LoginDetails(userId, oldPasswd);
            if (data_table.Rows.Count > 0)
            {
                newPasswd = EncryptString(newPasswd);
                string usercode = HttpContext.Current.Session["UserID"].ToString();
                int user_id = int.Parse(usercode);
                data_file.ResetPassword(user_id, newPasswd, false);
                output = "Password Changed Successfully";
            }
            else
            {
                output = "Invalid Old Password";
            }
        }
        else
        {
            output = "Passwords do not match";
        }
        return output;
    }

    public string Save_Tariff(string network, string rate)
    {
        int tariff = int.Parse(rate);
        data_file.Save_Tariff(network, tariff);
        return "SAVED";
    }

    public string Save_Area(string area_code, string area, string mask)
    {
        int area_id = int.Parse(area_code);
        data_file.Save_Area(area_id, area, mask);
        return "SAVED";
    }
    public DateTime ReturnDate(string date, int type)
    {
        DateTime dates;

        if (type == 1)
        {

            if (date == "")
            {
                dates = DateTime.Parse("January 1, 2012");
            }
            else
            {
                dates = DateTime.Parse(date);
            }
        }
        else
        {
            if (date == "")
            {
                dates = DateTime.Now;
            }
            else
            {
                dates = DateTime.Parse(date);
            }
        }

        return dates;
    }

    public DataTable GetSmslogs(string list_code, string area_code, string user, string from, string end)
    {
        int listid = int.Parse(list_code);
        int area_id = int.Parse(area_code);
        DateTime from_date = ReturnDate(from, 1);
        DateTime end_date = ReturnDate(end, 2);
        data_table = data_file.GetSmslogs(listid, area_id,user, from_date, end_date);
        return data_table;
    }

    public DataTable GetSmsSent(string area_code, string user,string sent, string from, string end)
    {
       // int listid = int.Parse(list_code);
        //int area_id = area_code;
        DateTime from_date = ReturnDate(from, 1);
        DateTime end_date = ReturnDate(end, 2);
        //data_table = data_file.GetSmsSent(area_code, user,sent, from_date, end_date);
        //return data_table;
        using (DataTable data_table1 = data_file.GetSmsSent(area_code, user, sent, from_date, end_date))
        {
            return data_table1;
        }
    }
    public DataTable GetCreditHistory(string area_code, string user, string from, string end)
    {
        // int listid = int.Parse(list_code);
        //int area_id = area_code;
        DateTime from_date = ReturnDate(from, 1);
        DateTime end_date = ReturnDate(end, 2);
        data_table = data_file.GetCreditHistory(area_code, user, from_date, end_date);
        return data_table;
    }

    public DataTable GetSmsSentPR(string area_code, string user, string sent, string from, string end)
    {
        // int listid = int.Parse(list_code);
        //int area_id = area_code;
        DateTime from_date = ReturnDate(from, 1);
        DateTime end_date = ReturnDate(end, 2);
        //data_table = data_file.GetSmsSentPR(area_code, user, sent, from_date, end_date);
        //return data_table;
        using (DataTable data_table1 = data_file.GetSmsSentPR(area_code, user, sent, from_date, end_date))
        {
            return data_table1;
        }
    }

    public void SendSms(string phone, string message, string mask, string smsSender)
    {
        try
        {
            smsapi = new SMSService();
            smsapi.SendSms(phone, message, mask, smsSender, "EMEMU", "T3rr1613");
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public void UpdateUserCredit(string username, int newCredit)
    {
        try
        {
            smsapi = new SMSService();
            smsapi.UpdateUserCredit(username, newCredit);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    public NetworkRate[] GetNetworkRates()
    {
        try
        {
            smsapi = new SMSService();
            NetworkRate[] nrates = smsapi.GetNetworkRates();
            return nrates;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    public NetworkCode[] GetNetworkCodes()
    {
        try
        {
            smsapi = new SMSService();
            NetworkCode[] ncodes = smsapi.GetNetworkCodes();
            return ncodes;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
   
    public Hashtable SetNetworkRates()
    {
        Hashtable rates = new Hashtable();
        NetworkRate[] nrates = GetNetworkRates();
        DataTable networkRates = new DataTable();
        networkRates.Columns.Add("Network", typeof(string));
        networkRates.Columns.Add("Rate(UShs.)", typeof(string));
        foreach (NetworkRate rate in nrates)
        {
            networkRates.Rows.Add(rate.Network, rate.Rate);
        }        
        rates.Clear();
        for (int i = 0; i < networkRates.Rows.Count; i++)
        {
            string network = networkRates.Rows[i]["Network"].ToString();
            string nRate = networkRates.Rows[i]["Rate(UShs.)"].ToString();
            rates.Add(network, nRate);
        }

        return rates;
    }


    public string Save_Location(string location_code, string location)
    {
        int location_id = int.Parse(location_code);
        data_file.Save_Location(location_id, location);
        return "SAVED";
    }

    public string Save_FieldEngineer(string RecordId, string Name, string Phone, string location,bool Active)
    {
        int EngineerId = int.Parse(RecordId);
        int location_id = int.Parse(location);
        data_file.Save_FieldEngineer(EngineerId, Name, Phone, location_id, Active);
        return "SAVED";

    }

    public string UploadEngineerList(string NewfilePath, string location)
    {
        string output = "";
        try
        {
            string res=ValidateEngineerList(NewfilePath);
            if (res.Equals("NONE"))
            {
                DataFile fileData = new DataFile();
                fileContent = df.readFile(NewfilePath);
                for (int i = 1; i < fileContent.Count; i++)
                {
                    string fileLine = fileContent[i].ToString();
                    string[] fLine = fileLine.Split(',');
                    string EngineerName = fLine[0].ToString();
                    string EngineerContact = fLine[1].ToString();
                    bool Active=true;
                    if (EngineerContact.Length.Equals(9))
                    {
                        EngineerContact = "0" + EngineerContact;
                    }
                    int RecordId = 0;
                    int location_id = int.Parse(location);
                    data_file.Save_FieldEngineer(RecordId, EngineerName, EngineerContact, location_id, Active);
                }
                output = "ENGINEERS LIST HAS BEEN SUCCESSFULLY SAVED ";
            }
            else 
            {
                output = res;
            }
            return output;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    public string ValidateEngineerList(string filepath)
    {
        string output = "";
        try
        {
            if (File.Exists(filepath))
            {
                

                DataFile fileData = new DataFile();
                fileContent = df.readFile(filepath);
                int count;
                for (int i = 1; i < fileContent.Count; i++)
                {
                    count = i + 1;
                    string fileLine = fileContent[i].ToString();
                    string[] fLine = fileLine.Split(',');
                    if (fLine.Length == 2)
                    {
                        string EngineerName = fLine[0].ToString();
                        string EngineerContact = fLine[1].ToString();
                        if (EngineerContact.Length.Equals(9))
                        {
                            EngineerContact = "0" + EngineerContact;
                        }
                        data_table = data_file.EngineerNumberExists(EngineerContact);
                        if (data_table.Rows.Count < 1)
                        {
                            if (EngineerName.Equals(""))
                            {
                                output = "The Engineer Name at Line " + count + " is Incorrect";
                                return output;
                            }
                            else if (EngineerContact.Equals(""))
                            {
                                output = "The Engineer Contact can not be empty at Line " + count ;
                                return output;
                            }
                        }
                        else 
                        {
                            output = "The Engineer Contact at Line " + count+" Already Exists";
                            return output;
                        }
                    }
                    else
                    {
                        output = "Invalid Number of Columns on line count All the file line Should have 2 Columns";
                        return output;
                    }
                }

                output = "NONE";
                return output;
            }
            else
            {
                output = "The File Name Does Not Exist";
                return output;
            }
           
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public DataTable GetDistrictTotalSmsSent(string area_code, string Sent, string from, string end)
    {
        DateTime from_date = ReturnDate(from, 1);
        DateTime end_date = ReturnDate(end, 2);
        string from_date1 = formatDate(from_date);
        string end_date1 = formatDate(end_date);
        data_table = data_file.GetDistrictTotalSmsSent(area_code, Sent, from_date1, end_date1);
        return data_table;
    }
    public DataTable GetDistrictTotalSmsSentNew(string area_code, string Sent, string from, string end)
    {
        DateTime from_date = ReturnDate(from, 1);
        DateTime end_date = ReturnDate(end, 2);
        string from_date1 = formatDate(from_date);
        string end_date1 = formatDate(end_date);
        data_table = data_file.GetDistrictTotalSmsSentNew(area_code, Sent, from_date1, end_date1);
        return data_table;
    }
    public string formatDate(DateTime Date)
    {
        string[] newDate = Date.ToString().Split(' ');
        string dateOnly = newDate[0];
        string[] DateParts = dateOnly.Split('/');
        string month = DateParts[0];
        string day = DateParts[1];
        string year = DateParts[2];
        string fulldate = year + '-' + month + '-' + day;
        return fulldate;
    }
}

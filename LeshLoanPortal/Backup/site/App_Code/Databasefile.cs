using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Data.Common;
using System.Collections;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Microsoft.Practices.EnterpriseLibrary.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;
/// <summary>
/// Summary description for Databasefile
/// </summary>
public class Databasefile
{
    private Database Sms_DB;
    private DbCommand procommand;
    private DataTable data_table;
    //string oradb = "Data Source=ORCL;User Id=hr;Password=hr;";

    //private OracleConnection conn;
    private string ReturnConsring()
    {
        //string constring = "Jab";
        string constring = "jab";
        //string constring = "TestPegPay";
        return constring;
    }

    public Databasefile()
    {
        try
        {
            Sms_DB = DatabaseFactory.CreateDatabase(ReturnConsring());
            //conn = new OracleConnection(oradb); 
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
  
    internal DataTable GetUserAccessibility(string username, string password)
    {
        try
        {
            procommand = Sms_DB.GetStoredProcCommand("Login", username, password);
            data_table = Sms_DB.ExecuteDataSet(procommand).Tables[0];
            return data_table;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    //internal DataTable GetStatement(string account, DateTime sDate, DateTime eDate)
    //{
    //    try
    //    {
    //        conn.Open();
    //        OracleCommand cmd = new OracleCommand();
    //        cmd.Connection = conn;
    //        //cmd.CommandText = "select department_name from departments where department_id = 10";
    //        cmd.CommandText = "select department_id, department_name, city"
    //              + " from departments d, locations l"
    //              + " where d.location_id = l.location_id";


    //        cmd.CommandType = CommandType.Text;
    //        OracleDataReader dr = cmd.ExecuteReader();
    //        dr.Read();

    //        data_table.Load(dr);

    //        conn.Dispose();
    //        return data_table;
    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //}

    internal void ResetPassword(int user_Id, string password,bool reset)
    {
        try
        {
            procommand = Sms_DB.GetStoredProcCommand("ResetPassword", user_Id, password, reset);
            Sms_DB.ExecuteNonQuery(procommand);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public DataTable GetLists(string area_code,string listName)
    {        
        try
        {
            int area_id = int.Parse(area_code);
            procommand = Sms_DB.GetStoredProcCommand("GetLists", area_id, listName);
            data_table = Sms_DB.ExecuteDataSet(procommand).Tables[0];
            return data_table;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    internal void SaveList(int list_id, string list_name,bool is_active, string area_code, string user)
    {
        try
        {
            int area_id = int.Parse(area_code);
            procommand = Sms_DB.GetStoredProcCommand("Save_list", list_id, list_name, is_active, area_id, user);
            Sms_DB.ExecuteNonQuery(procommand);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public DataTable Get_list(int list_Id)
    {
        try
        {
            procommand = Sms_DB.GetStoredProcCommand("GetList", list_Id);
            data_table = Sms_DB.ExecuteDataSet(procommand).Tables[0];
            return data_table;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    internal DataTable GetActivelists(string area_code)
    {     
        try
        {
            int area_id = int.Parse(area_code);
            procommand = Sms_DB.GetStoredProcCommand("GetActiveLists", area_id);
            data_table = Sms_DB.ExecuteDataSet(procommand).Tables[0];
            return data_table;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    public DataTable GetAllListsByArea(string area_code)
    {
        try
        {
            int area_id = int.Parse(area_code);
            procommand = Sms_DB.GetStoredProcCommand("GetListsByArea", area_id);
            data_table = Sms_DB.ExecuteDataSet(procommand).Tables[0];
            return data_table;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    internal DataTable GetAllLists(string area_code)
    {
        try
        {
            int area_id = int.Parse(area_code);
            procommand = Sms_DB.GetStoredProcCommand("GetAllLists", area_id);
            data_table = Sms_DB.ExecuteDataSet(procommand).Tables[0];
            return data_table;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    internal ArrayList GetBlackListedNumbers()
    {
        throw new Exception("The method or operation is not implemented.");
    }
    internal Hashtable GetNetworkCodes()
    {
        Hashtable networkCodes = new Hashtable();
        try
        {
            procommand = Sms_DB.GetStoredProcCommand("GetNetworkCodes");
            DataSet ds = Sms_DB.ExecuteDataSet(procommand);
            int recordCount = ds.Tables[0].Rows.Count;
            if (recordCount != 0)
            {
                for (int i = 0; i < recordCount; i++)
                {
                    DataRow dr = ds.Tables[0].Rows[i];
                    string network = dr["Network"].ToString();
                    string code = dr["Code"].ToString();
                    networkCodes.Add(code, network);
                }
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
        return networkCodes;
    }
    internal string[,] GetNetCodes()
    {
        string [,]networkCodes =new string[100,2];
        try
        {
            procommand = Sms_DB.GetStoredProcCommand("GetNetworkCodes");
            DataSet ds = Sms_DB.ExecuteDataSet(procommand);
            int recordCount = ds.Tables[0].Rows.Count;
            if (recordCount != 0)
            {
                for (int i = 0; i < recordCount; i++)
                {
                    DataRow dr = ds.Tables[0].Rows[i];
                    string network = dr["Network"].ToString();
                    string code = dr["Code"].ToString();

                    for (int j = 0; j < recordCount; i++)
                    {
                        if (j > 0)
                        {
                            networkCodes[i, j] = network;
                        }
                        else 
                        {
                            networkCodes[i, j] = code;
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
        return networkCodes;
    }
 
    internal void SavePhoneToList(string phone, string name, int list_id, string area_code, string user)
    {
        try
        {
            int area_id = int.Parse(area_code);
            procommand = Sms_DB.GetStoredProcCommand("SavePhoneToList", phone, name, list_id, user);
            Sms_DB.ExecuteNonQuery(procommand);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    internal DataTable GetListDetails(int list_id, string phone, string name)
    {
        try
        {
            procommand = Sms_DB.GetStoredProcCommand("GetListDetails", phone, name, list_id);
            data_table = Sms_DB.ExecuteDataSet(procommand).Tables[0];
            return data_table;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    public DataTable GetSystemRoles()
    {
        try
        {
            procommand = Sms_DB.GetStoredProcCommand("Get_SystemRoles");
            data_table = Sms_DB.ExecuteDataSet(procommand).Tables[0];
            return data_table;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    public DataTable GetAreas()
    {
        try
        {
            procommand = Sms_DB.GetStoredProcCommand("Get_Areas");
            data_table = Sms_DB.ExecuteDataSet(procommand).Tables[0];
            return data_table;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    public DataTable GetCurrentCredit(string user)
    {
        try
        {
            procommand = Sms_DB.GetStoredProcCommand("GetCurrentCredit", user);
            data_table = Sms_DB.ExecuteDataSet(procommand).Tables[0];
            return data_table;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    public DataTable GetUserDetails(int userid)
    {
        try
        {
            procommand = Sms_DB.GetStoredProcCommand("Get_UserDetails", userid);
            data_table = Sms_DB.ExecuteDataSet(procommand).Tables[0];
            return data_table;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    internal DataTable GetUserDetailsByUserName(string user_name)
    {
        try
        {
            procommand = Sms_DB.GetStoredProcCommand("GetUserDetailsByUserName", user_name);
            data_table = Sms_DB.ExecuteDataSet(procommand).Tables[0];
            return data_table;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    internal DataTable GetUsers(int area_id, int type_id, string name)
    {
        try
        {
            procommand = Sms_DB.GetStoredProcCommand("Get_Users", name, area_id, type_id);
            data_table = Sms_DB.ExecuteDataSet(procommand).Tables[0];
            return data_table;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    public DataTable GetUsersByArea(int area_id)
    {
        try
        {
            procommand = Sms_DB.GetStoredProcCommand("GetUsersByArea", area_id);
            data_table = Sms_DB.ExecuteDataSet(procommand).Tables[0];
            return data_table;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    public DataTable GetActiveListNumbers(int list_id)
    {
        try
        {
            procommand = Sms_DB.GetStoredProcCommand("GetActiveListNumbers", list_id);
            data_table = Sms_DB.ExecuteDataSet(procommand).Tables[0];
            return data_table;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    public DataTable GetAreaslist()
    {
        try
        {
            procommand = Sms_DB.GetStoredProcCommand("Get_Areas");
            data_table = Sms_DB.ExecuteDataSet(procommand).Tables[0];
            return data_table;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    public DataTable GetLocationslist()
    {
        try
        {
            procommand = Sms_DB.GetStoredProcCommand("Get_Locations");
            data_table = Sms_DB.ExecuteDataSet(procommand).Tables[0];
            return data_table;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    public DataTable GetEngineers(string name, string Contact, int Area )
    {
        try
        {
            procommand = Sms_DB.GetStoredProcCommand("Get_Engineers", name, Contact, Area);
            data_table = Sms_DB.ExecuteDataSet(procommand).Tables[0];
            return data_table;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    public DataTable GetTariffRates()
    {
        try
        {
            procommand = Sms_DB.GetStoredProcCommand("GetNetworkRates");
            data_table = Sms_DB.ExecuteDataSet(procommand).Tables[0];
            return data_table;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    internal DataTable GetSmslogs(int listid, int area_id,string user, DateTime from_date, DateTime end_date)
    {
        try
        {
            procommand = Sms_DB.GetStoredProcCommand("Get_Smslogs", listid, area_id, user, from_date, end_date);
            data_table = Sms_DB.ExecuteDataSet(procommand).Tables[0];
            return data_table;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    public DataTable GetSmsSent(string area_id, string user, string sent, DateTime from_date, DateTime end_date)
    {
        try
        {
            procommand = Sms_DB.GetStoredProcCommand("Get_SmsSent", area_id, user,sent, from_date, end_date);
            procommand.CommandTimeout = 0;
            //data_table = Sms_DB.ExecuteDataSet(procommand).Tables[0];
            //return data_table;
            using (DataTable data_table1 = Sms_DB.ExecuteDataSet(procommand).Tables[0])
            {
                return data_table1;
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    public DataTable GetCreditHistory(string area_id, string user, DateTime from_date, DateTime end_date)
    {
        try
        {
            procommand = Sms_DB.GetStoredProcCommand("Get_CreditHistory", area_id, user, from_date, end_date);
            data_table = Sms_DB.ExecuteDataSet(procommand).Tables[0];
            return data_table;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    public DataTable GetSmsSentPR(string area_id, string user, string sent, DateTime from_date, DateTime end_date)
    {
        try
        {
            procommand = Sms_DB.GetStoredProcCommand("Get_SmsSentPR", area_id, user, sent, from_date, end_date);
            //data_table = Sms_DB.ExecuteDataSet(procommand).Tables[0];
            //return data_table;
            using (DataTable data_table1 = Sms_DB.ExecuteDataSet(procommand).Tables[0])
            {
                return data_table1;
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    internal int GetListTotalCost(int list_id)
    {
        int total = 0;
        try
        {            
            procommand = Sms_DB.GetStoredProcCommand("GetListTotalCost", list_id);
            data_table = Sms_DB.ExecuteDataSet(procommand).Tables[0];
            if (data_table.Rows.Count > 0)
            {
                total = int.Parse(data_table.Rows[0]["TotalCost"].ToString());
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
        return total;
    }
    internal void ChangePhoneStatus(int phone_id, bool active)
    {
        try
        {
            procommand = Sms_DB.GetStoredProcCommand("ChangePhoneStatus", phone_id, active);
            Sms_DB.ExecuteNonQuery(procommand);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    internal void UpdatePhoneDetails(int phone_id, string phone, string phone_name, bool active)
    {
        try
        {
            procommand = Sms_DB.GetStoredProcCommand("UpdatePhoneDetails", phone_id, phone, phone_name, active);
            Sms_DB.ExecuteNonQuery(procommand);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    internal void Save_user(int user_id, string user_name,string passwd, string fname, string lname, string phone, string email, int area_id, string type_code, bool active, bool reset, string user)
    {
        try
        {
            procommand = Sms_DB.GetStoredProcCommand("Save_user", user_id, user_name, fname, lname, passwd, phone, email, area_id, type_code, active, user);
            Sms_DB.ExecuteNonQuery(procommand);
            if (reset)
            {
                ResetPassword(user_id, passwd,reset);
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }


    internal void AddCredit(string user_name, int credit_toadd, string user)
    {
        try
        {
            procommand = Sms_DB.GetStoredProcCommand("AddCredit", user_name, credit_toadd, user);
            Sms_DB.ExecuteNonQuery(procommand);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    internal void InsertSmsToSend(string phone, string message, string mask, string user,string areaID)
    {
        try
        {
            procommand = Sms_DB.GetStoredProcCommand("InsertSmsToSend", phone, message, mask, user, areaID);
            Sms_DB.ExecuteNonQuery(procommand);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    internal void reduce_credit(int new_balance, int list_id, int list_total,string message,int Area_id,string mask, string user)
    {
        try
        {
            procommand = Sms_DB.GetStoredProcCommand("Reduce_credit", list_id, list_total, new_balance,message,Area_id,mask, user);
            Sms_DB.ExecuteNonQuery(procommand);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    internal Hashtable GetNetworkRates()
    {
        throw new Exception("The method or operation is not implemented.");
    }

    internal void Save_Tariff(string network, int rate)
    {
        try
        {
            procommand = Sms_DB.GetStoredProcCommand("Save_Tariff", network, rate);
            Sms_DB.ExecuteNonQuery(procommand);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    internal void Save_Area(int area_id, string area, string mask)
    {

        try
        {
            procommand = Sms_DB.GetStoredProcCommand("Save_Area", area_id, area, mask);
            Sms_DB.ExecuteNonQuery(procommand);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    internal int GetParticularTarrifWithCode(string NetworkCode)
    {
        int total = 0;
        try
        {
            procommand = Sms_DB.GetStoredProcCommand("GetParticularTarrifWithCode", NetworkCode);
            data_table = Sms_DB.ExecuteDataSet(procommand).Tables[0];
            if (data_table.Rows.Count > 0)
            {
                total = int.Parse(data_table.Rows[0]["Tarrif"].ToString());
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
        return total;
    }

    public void Save_Location(int location_id, string location)
    {
        try
        {
            procommand = Sms_DB.GetStoredProcCommand("Save_Location", location_id, location);
            Sms_DB.ExecuteNonQuery(procommand);
        }
        catch (Exception ex)
        {
            throw ex;
        }

    }
    internal void Save_FieldEngineer(int EngineerId, string Name, string Phone, int location_id,bool Active)
    {
        try
        {
            procommand = Sms_DB.GetStoredProcCommand("Save_FieldEngineer", EngineerId, Name, Phone, location_id, Active);
            Sms_DB.ExecuteNonQuery(procommand);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public DataTable GetFieldEngineersByLocation(string location)
    {

        try
        {
            procommand = Sms_DB.GetStoredProcCommand("GetFieldEngineersByLocation", location);
            data_table = Sms_DB.ExecuteDataSet(procommand).Tables[0];
            return data_table;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    public DataTable GetFieldEngineerDetailsByContact(string Contact)
    {

        try
        {
            procommand = Sms_DB.GetStoredProcCommand("GetFieldEngineerDetailsByContact", Contact);
            data_table = Sms_DB.ExecuteDataSet(procommand).Tables[0];
            return data_table;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    public string GetSystemParameter(int valueCode)
    {

        string ret = "";
        try
        {
            procommand = Sms_DB.GetStoredProcCommand("GetSys_Parameter", valueCode);
            data_table = Sms_DB.ExecuteDataSet(procommand).Tables[0];
            if (data_table.Rows.Count > 0)
            {
                ret = data_table.Rows[0]["ParameterValue"].ToString();
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
        return ret;
    }

    public DataTable EngineerNumberExists(string EngineerContact)
    {

        try
        {
            procommand = Sms_DB.GetStoredProcCommand("GetFieldEngineersByContact", EngineerContact);
            data_table = Sms_DB.ExecuteDataSet(procommand).Tables[0];
            return data_table;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    internal DataTable GetDistrictTotalSmsSent(string area_code, string sent, string from_date, string end_date)
    {
        try
        {

            procommand = Sms_DB.GetStoredProcCommand("GetDistrictTotalSmsSent", area_code, sent, from_date, end_date);
            procommand.CommandTimeout = 120;
            data_table = Sms_DB.ExecuteDataSet(procommand).Tables[0];
            return data_table;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    internal DataTable GetDistrictTotalSmsSentNew(string area_code, string sent, string from_date, string end_date)
    {
        try
        {

            procommand = Sms_DB.GetStoredProcCommand("GetDistrictTotalSmsSentNew", area_code, sent, from_date, end_date);
            procommand.CommandTimeout = 120;
            data_table = Sms_DB.ExecuteDataSet(procommand).Tables[0];
            return data_table;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
}

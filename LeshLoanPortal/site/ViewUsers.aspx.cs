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
using Encryption;
using System.Collections.Generic;
using InterConnect.LeshLaonApi;

public partial class ViewUsers : System.Web.UI.Page
{
    SystemUser user;
    BusinessLogic bll = new BusinessLogic();
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            user = Session["User"] as SystemUser;
            if (IsPostBack == false)
            {
                if ((Session["Username"] == null))
                {
                    Response.Redirect("Default.aspx");
                }
                MultiView2.ActiveViewIndex = 0;

                //bll.LoadBanksIntoDropDownALL(user, ddlAreas);
                bll.LoadRolesIntoDropDown(ddCompany.SelectedValue, user, ddlUserType);
                //bll.LoadBranchesIntoDropDown(user.BankCode, user, ddBranch);
                //bll.LoadBranchesForSearchIntoDropDown();
                string urole = Session["RoleCode"].ToString();
                SearchDB();
            }
        }
        catch (Exception ex)
        {
            ShowMessage(ex.Message, true);
        }
    }
    protected void btnOK_Click(object sender, EventArgs e)
    {
        SearchDB();
    }

    public void SearchDB()
    {
        try
        {
            string[] Params = GetSearchParameters();
            DataTable dt = bll.SearchSystemUsers(Params);
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
                //MultiView2.ActiveViewIndex = -1;
                string msg = "No Records Found Matching Search Criteria";
                Label lblmsg = (Label)Master.FindControl("lblmsg");
                bll.ShowMessage(lblmsg, msg, true, Session);
            }

        }
        catch (Exception ex)
        {
            ShowMessage(ex.Message, true);
        }
    }
    private string[] GetSearchParameters()
    {   
        List<string> searchCriteria = new List<string>();

        string CompanyCode = ddCompany.SelectedValue;
        string Role_code = ddlUserType.SelectedValue.ToString();
        string UserId = txtSearch.Text.Trim();

        searchCriteria.Add(CompanyCode);
        searchCriteria.Add(Role_code);
        searchCriteria.Add(UserId);
        return searchCriteria.ToArray();
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

    private void LoadCreditControl(string user_code, string username, string name)
    {
        MultiView2.ActiveViewIndex = 1;
        lblPhoneCode.Text = user_code;
        txtUserName.Text = username;
        txtName.Text = name;
        //lblCredit.Text = GetCredit(username);
    }

    //private string GetCredit(string username)
    //{
    //    string response = "";
    //    try
    //    {
    //        int money1 = 0;
    //        DataTable dt = data_file.GetCurrentCredit(username);
    //        if (dt.Rows.Count > 0)
    //        {
    //            money1 = int.Parse(dt.Rows[0]["Credit"].ToString());
    //        }
    //        response = "CURRENT CREDIT IS: " + money1.ToString("#,##0");
    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //    return response;
    //}
    protected void DataGrid1_PageIndexChanged(object source, DataGridPageChangedEventArgs e)
    {
    //    try
    //    {
    //        string vendor_code = ddlAreas.SelectedValue.ToString();
    //        string user_role = ddlUserType.SelectedValue.ToString();
    //        string name = txtSearch.Text.Trim();
    //        data_table = Process_file.GetUsers(vendor_code, user_role, name);
    //        DataGrid1.DataSource = data_table;
    //        DataGrid1.CurrentPageIndex = e.NewPageIndex;
    //        DataGrid1.DataBind();
    //    }
    //    catch (Exception ex)
    //    {
    //        ShowMessage(ex.Message,true);
    //    }
    }


    protected void dataGridResults_RowCommand(object sender, GridViewCommandEventArgs e)
    {
    try
    {
        int index = 0;
        GridViewRow row;
        GridView grid = sender as GridView;
        index = Convert.ToInt32(e.CommandArgument);
        row = grid.Rows[index];
        string BankCode = row.Cells[1].Text;
        string UserID = row.Cells[2].Text;
        string UserType = row.Cells[4].Text;
        string IsActive;
        CheckBox chbx = row.Cells[10].FindControl("chkbx") as CheckBox;
            if (chbx.Checked)
            {
                IsActive = true.ToString();
            }
            else
            {
                IsActive = false.ToString();
            }
            Label lblmsg = (Label)Master.FindControl("lblmsg");
        if (e.CommandName == "EditUser")
        {
            Server.Transfer("AddUser.aspx?BankCode=" + BankCode + "&UserID=" + UserID + "&UserType=" + UserType + "&Type=" + "Reset" + "&Status=" + IsActive);
            //string user_code = encrypt.EncryptString(e.Item.Cells[0].Text, "25011Pegsms2322"); 
            //Response.Redirect("./AddUser.aspx?transferid=" + user_code, false);                
        }
    }
        catch (Exception ex)
        {
            ShowMessage(ex.Message, true);
        }
    }

    protected void dataGridResults_RowCreated(object sender, GridViewRowEventArgs e)
    {
        GridViewRow row = e.Row;
        List<TableCell> columns = new List<TableCell>();

        //Get the first Cell /Column
        TableCell cell = row.Cells[1];
        // Then Remove it after
        row.Cells.Remove(cell);
        //And Add it to the List Collections
        columns.Add(cell);

        // Add cells
        row.Cells.AddRange(columns.ToArray());
        e.Row.Cells[10].Visible = false;
    }
}
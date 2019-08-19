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

public partial class ViewInterest : System.Web.UI.Page
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

                bll.LoadCompanysIntoDropDownALL(user, ddCompany);
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
            DataTable dt = bll.SearchInterestSettingTable(Params);
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
        string Interestcode = txtInterestCode.Text;

        searchCriteria.Add(CompanyCode);
        searchCriteria.Add(Interestcode);
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
        
    }
    
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
            string CompanyCode = row.Cells[1].Text;
            string InterestCode = row.Cells[2].Text;
            
            Label lblmsg = (Label)Master.FindControl("lblmsg");
            if (e.CommandName == "EditUser")
            {
                Server.Transfer("AddInterest.aspx?CompanyCode=" + CompanyCode + "&InterestCode=" + InterestCode);
                //string user_code = encrypt.EncryptString(e.Item.Cells[0].Text, "25011Pegsms2322"); 
                //Response.Redirect("./AddUser.aspx?transferid=" + user_code, false);                
            }
        }
        catch (Exception ex)
        {
            ShowMessage(ex.Message, true);
        }
    }
    
}

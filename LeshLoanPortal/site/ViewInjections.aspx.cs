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

public partial class ViewInjections : Page
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
        DataTable dt = bll.SearchInjectionDetailsTable(Params);
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
        string CompanyCode = user.CompanyCode;
        string InjectorName = txtInjectorNo.Text.Trim();
        string UserId = user.UserId;
        string StartDate = txtStartDate.Text;
        string EndDate = txtEndDate.Text;

        searchCriteria.Add(CompanyCode);
        searchCriteria.Add(InjectorName);
        searchCriteria.Add(UserId);
        searchCriteria.Add(StartDate);
        searchCriteria.Add(EndDate);
        return searchCriteria.ToArray();
    }
    

    }
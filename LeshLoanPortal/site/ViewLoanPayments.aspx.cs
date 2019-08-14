﻿using System;
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

public partial class ViewLoanPayments : Page
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

            string ClientID = Request.QueryString["ClientID"];
            string LoanID = Request.QueryString["LoanID"];
            LoadData(LoanID);
            //LoadMessageTemplates();

        }
        catch (Exception ex)
        {
            ShowMessage(ex.Message, true);
        }
    }

    protected void LoadData(string LoanID)
    {
        //bll.LoadAgentsIntopDropDown(user, ddAgents);
        txtLoanNo.Text = LoanID;
        txtLoanNo.Enabled = false;

        if (user.RoleCode == "003")
        {
            ddStatus.SelectedItem.Value = "PENDING";
        }
        else if (user.RoleCode == "004")
        {
            
            //CEOSelect.Visible = false;
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
        DataTable dt = bll.SearchLoanPayments(Params);
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
        string LoanID = txtLoanNo.Text.Trim();
        string UserId = user.UserId;
        string Status = ddStatus.SelectedValue;
        string StartDate = txtStartDate.Text;
        string EndDate = txtEndDate.Text;
        
        searchCriteria.Add(LoanID);
        searchCriteria.Add(UserId);
        searchCriteria.Add(Status);
        searchCriteria.Add(StartDate);
        searchCriteria.Add(EndDate);
        return searchCriteria.ToArray();
    }

    protected void dataGridResults_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        int index = 0;
        GridViewRow row;
        GridView grid = sender as GridView;
        index = Convert.ToInt32(e.CommandArgument);
        row = grid.Rows[index];
        string ClientID = row.Cells[1].Text;
        string LoanID = row.Cells[2].Text;
        string AmountPerMonth = row.Cells[8].Text;
        Label lblmsg = (Label)Master.FindControl("lblmsg");

        if (e.CommandName.Equals("ViewPayments"))
        {
            if (LoanID != "")
            {
                Server.Transfer("~/ViewLoanPayments.aspx?ClientID=" + ClientID + "&LoanID=" + LoanID + "&AmountPerMonth=" + AmountPerMonth);
                //return;
            }
            else
            {
                bll.ShowMessage(lblmsg, "Loan Missing details", true, Session);
            }

        }
        if (e.CommandName.Equals("Download"))
        {
            string OutPutFileName = LoanID + ".zip";
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
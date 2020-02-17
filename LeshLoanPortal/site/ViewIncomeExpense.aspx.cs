using System;
using System.Data;
using System.Collections;
using System.Drawing;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using System.Web;
using System.IO;
using OfficeOpenXml;
using InterConnect.LeshLaonApi;

public partial class ViewIncomeExpense : System.Web.UI.Page
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
        //SearchDB();
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
            if (ddType.SelectedValue == "")
            {
                ShowMessage("Please Select Type to View", true);
                return;
            }
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
        DataTable dt = bll.SearchIncomeExpenseDetailsForreport(Params);

        if(ddType.SelectedValue == "IncomeExpense")
        {
            dataGridResultsIncomeStat.Visible = true;
            dataGridResults.Visible = false;
            if (dt.Rows.Count > 0)
            {
                dataGridResultsIncomeStat.DataSource = dt;
                dataGridResultsIncomeStat.DataBind();
                string msg = "Found " + dt.Rows.Count + " Records Matching Search Criteria";
                MultiView2.ActiveViewIndex = 0;
                Label lblmsg = (Label)Master.FindControl("lblmsg");
                bll.ShowMessage(lblmsg, msg, false, Session);
            }
            else
            {
                dataGridResultsIncomeStat.DataSource = null;
                dataGridResultsIncomeStat.DataBind();
                MultiView2.ActiveViewIndex = -1;
                string msg = "No Records Found Matching Search Criteria";
                Label lblmsg = (Label)Master.FindControl("lblmsg");
                bll.ShowMessage(lblmsg, msg, true, Session);
            }
            
        }
        else
        {
            dataGridResults.Visible = true;
            dataGridResultsIncomeStat.Visible = false;
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

    }
    private string[] GetSearchParameters()
    {
        List<string> searchCriteria = new List<string>();
        string CompanyCode = ddCompany.SelectedValue.ToString();
        string Type = ddType.SelectedValue;
        string SearchNo = txtSearchNo.Text;
        string UserId = user.UserId;
        string StartDate = txtStartDate.Text;
        string EndDate = txtEndDate.Text;

        searchCriteria.Add(CompanyCode);
        searchCriteria.Add(Type);
        searchCriteria.Add(SearchNo);
        searchCriteria.Add(UserId);
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
        string CompanyCode = row.Cells[1].Text;
        string IncomeNo = row.Cells[2].Text;
        string Amount = row.Cells[3].Text;
        Label lblmsg = (Label)Master.FindControl("lblmsg");

        if (e.CommandName.Equals("DeleteRecord"))
        {
            lblID.Text = IncomeNo;
            lblID.Visible = false;
            MultiView2.SetActiveView(ConfirmView);
        }
        else if (e.CommandName.Equals("EditRecord"))
        {
            if (IncomeNo != "")
            {
                if (ddType.SelectedValue == "Income")
                {
                    Server.Transfer("~/AddIncome.aspx?IncomeID=" + IncomeNo + "&CompanyCode=" + CompanyCode + "&Amount=" + Amount);
                }
                else if (ddType.SelectedValue == "Expense")
                {
                    Server.Transfer("~/AddExpense.aspx?ExpenseID=" + IncomeNo + "&CompanyCode=" + CompanyCode + "&Amount=" + Amount);
                }
                else
                {
                    ShowMessage("No Record To Edit", true);
                }
            }
            else
            {
                bll.ShowMessage(lblmsg, "Income Missing details", true, Session);
            }

        }
        else
        {
            bll.ShowMessage(lblmsg, "Record Missing details", true, Session);
        }
    }

protected void btnExport_Click(object sender, EventArgs e)
    {
        string[] searchParams = GetSearchParameters();
        DataTable dt = bll.SearchIncomeExpenseDetailsForreport(searchParams);
        if (dt.Rows.Count > 0)
        {
            ExcelPackage package = new ExcelPackage();
            ExcelWorksheet ws = package.Workbook.Worksheets.Add("sheet1");

            //set heading
            int excelColumn = 1;
            foreach (DataColumn dc in dt.Columns)
            {
                ws.Cells[1, excelColumn].Value = dc.ColumnName;
                excelColumn++;
            }

            ws.Cells[1, 1, 1, dt.Columns.Count].Style.Font.Bold = true;

            int i = 2;//row position in excel sheet

            foreach (DataRow dr in dt.Rows)
            {
                int dataColumn = 1;
                int tableColumnNumber = 0;
                foreach (DataColumn dc in dt.Columns)
                {
                    ws.Cells[i, dataColumn].Value = dr[tableColumnNumber].ToString();
                    dataColumn++;
                    tableColumnNumber++;
                }
                i++;
            }

            package.Workbook.Properties.Title = "Attempts";
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.AddHeader(
                      "content-disposition",
                      string.Format("attachment;  filename={0}", "Report.xlsx"));
            Response.BinaryWrite(package.GetAsByteArray());
        }
    }

    protected void btnConfirm_Click(object sender, EventArgs e)
    {
        Entity result = bll.UpdateIncomeExpenseStatus(user.CompanyCode,ddType.SelectedValue,lblID.Text,user.UserId);
        if (result.StatusCode != "0")
        {
            ShowMessage(result.StatusDesc, true);
        }
        else
        {
            ShowMessage("Record has been Deleted Successfully", false);
            MultiView2.SetActiveView(EmptyView);
        }
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("ViewIncomeExpense.aspx");
    }

    int Total = 0;
    protected void dataGridResultsIncomeStat_RowDataBound(object sender, GridViewRowEventArgs e)
    {


        int val;
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            //e.Row.Cells[1].Text = "<i>" + e.Row.Cells[1].Text + "</i>";
            val = int.Parse(e.Row.Cells[2].Text);
            Total += val;
            
        }
        //lblIncomeStat.Text = Total.ToString();

        if (e.Row.RowType == DataControlRowType.Footer)
        {
            //e.Row.Cells[0].Text = "";
            e.Row.Cells[1].Text = "Total Income-Expenditure";
            e.Row.Cells[2].Text = Total.ToString();
        }

    }
}
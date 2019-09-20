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
using OfficeOpenXml;
using iTextSharp.text;
using iTextSharp.text.pdf;

public partial class ViewClientReport : Page
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
            if ((Session["Username"] == null) || (Session["RoleCode"] == null))
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
        else
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
        DataTable dt = bll.SearchClientDetailsTable(Params);
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
        return tels;
    }

    protected void dataGridResults_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        int index = 0;
        GridViewRow row;
        GridView grid = sender as GridView;
        index = Convert.ToInt32(e.CommandArgument);
        row = grid.Rows[index];
        string CompanyCode = "";
        string ClientID = row.Cells[1].Text;
        string CustomerName = row.Cells[2].Text;
        string PhoneNumber = row.Cells[4].Text;
        string IDNumber = row.Cells[6].Text;
        string status = row.Cells[11].Text;
        string Gender = row.Cells[8].Text;
        Label lblmsg = (Label)Master.FindControl("lblmsg");

        if (e.CommandName.Equals("Details"))
        {
            if (IDNumber != "")
            {
                MultiView2.SetActiveView(DetailsView);

                InterConnect.LeshLaonApi.ClientDetails Cli = bll.GetClientDetails(user, ClientID);
                imgUrlClientPhoto.Text = Cli.ClientPhoto;
                ImgUrlIDPhoto.Text = Cli.IDPhoto;
                imgUrlClientPhoto.Visible = false;
                ImgUrlIDPhoto.Visible = false;

                lblClientNo.Text = ClientID;
                lblClientName.Text = CustomerName;
                lblTelNo.Text = PhoneNumber;
                lblGender.Text = Gender;
            }
            else
            {
                bll.ShowMessage(lblmsg, "Client Missing details", true, Session);
            }

        }
    }

    protected void btnBack_Click(object sender, EventArgs e)
    {
        Response.Redirect("ViewClientReport.aspx");
    }

    protected void btnViewPR_Click(object sender, EventArgs e)
    {
        string[] BaseText = imgUrlClientPhoto.Text.Split(',');

        if (BaseText[0].Contains("pdf"))
        {

            byte[] imageBytes = Convert.FromBase64String(BaseText[1]);

            Response.Clear();
            Response.ContentType = "application/pdf";
            Response.AppendHeader("Content-Disposition", "inline;filename=data.pdf");
            Response.BufferOutput = true;

            ////Response.AddHeader("Content-Length", response.Length.ToString());
            Response.BinaryWrite(imageBytes);
            Response.End();
        }
        else
        {
            Image1.Visible = true;
            Image1.ImageUrl = imgUrlClientPhoto.Text;
        }
        Image1.Width = Unit.Percentage(50);
        Image1.Height = Unit.Percentage(50);
    }

    protected void btnViewID_Click(object sender, EventArgs e)
    {
        string[] BaseText = ImgUrlIDPhoto.Text.Split(',');

        if (BaseText[0].Contains("pdf"))
        {

            byte[] imageBytes = Convert.FromBase64String(BaseText[1]);

            Response.Clear();
            Response.ContentType = "application/pdf";
            Response.AppendHeader("Content-Disposition", "inline;filename=data.pdf");
            Response.BufferOutput = true;

            ////Response.AddHeader("Content-Length", response.Length.ToString());
            Response.BinaryWrite(imageBytes);
            Response.End();
        }
        else
        {
            Image2.Visible = true;
            Image2.ImageUrl = imgUrlClientPhoto.Text;
        }
        Image2.Width = Unit.Percentage(50);
        Image2.Height = Unit.Percentage(50);
    }

    protected void btnExport_Click(object sender, EventArgs e)
    {
        bool ExcelChecked = rdExcel.Checked;
        bool PdfChecked = rdPdf.Checked;
        if (ExcelChecked)
        {
            Excel_Export();
        }
        else if (PdfChecked)
        {
            PDF_Export();
        }
        else
        {
            ShowMessage("Choose Export Type", true);
        }
    }

    //private void PDF_Export()
    //{
    //    //ShowMessage("Kindly Wait, Its under Implementation", true);

    //    try
    //    {
    //        string Name = "ClientReport";
    //        string[] searchParams = GetSearchParameters();
    //        DataTable dataTable = bll.SearchClientDetailsTable(searchParams);

    //            string[] columnNames = (from dc in dataTable.Columns.Cast<DataColumn>()
    //                                    select dc.ColumnName).ToArray();
    //            int Cell = 0;
    //            int count = columnNames.Length;
    //            object[] array = new object[count];

    //            dataTable.Rows.Add(array);

    //            Document pdfDoc = new Document(PageSize.A2, 10f, 10f, 10f, 0f);
    //            System.IO.MemoryStream mStream = new System.IO.MemoryStream();
    //            PdfWriter writer = PdfWriter.GetInstance(pdfDoc, mStream);
    //            int cols = dataTable.Columns.Count;
    //            int rows = dataTable.Rows.Count;


    //            HeaderFooter header = new HeaderFooter(new Phrase(Name), false);

    //            // Remove the border that is set by default  
    //            header.Border = iTextSharp.text.Rectangle.TITLE;
    //            // Align the text: 0 is left, 1 center and 2 right.  
    //            header.Alignment = Element.ALIGN_CENTER;
    //            pdfDoc.Header = header;
    //            // Header.  
    //            pdfDoc.Open();
    //        //iTextSharp.text.Table pdfTable = new iTextSharp.text.Table(cols, rows);
    //            PdfPTable pdfTable = new PdfPTable(cols);
    //            pdfTable.BorderWidth = 1; pdfTable.Width = 100;
    //            pdfTable.Padding = 1; pdfTable.Spacing = 4;

    //            //creating table headers  
    //            for (int i = 0; i < cols; i++)
    //            {
    //                //Cell cellCols = new Cell();
    //                PdfPCell  cellCols = new PdfPCell();
    //                Chunk chunkCols = new Chunk();
    //                cellCols.BackgroundColor = new iTextSharp.text.Color(System.Drawing.ColorTranslator.FromHtml("#548B54"));
    //                iTextSharp.text.Font ColFont = FontFactory.GetFont(FontFactory.HELVETICA, 14, iTextSharp.text.Font.BOLD, iTextSharp.text.Color.WHITE);

    //                chunkCols = new Chunk(dataTable.Columns[i].ColumnName, ColFont);

    //                cellCols.Add(chunkCols);
    //                pdfTable.AddCell(cellCols);
    //            }
    //            //creating table data (actual result)   

    //            for (int k = 0; k < rows; k++)
    //            {
    //                for (int j = 0; j < cols; j++)
    //                {
    //                    Cell cellRows = new Cell();
    //                    if (k % 2 == 0)
    //                    {
    //                        cellRows.BackgroundColor = new iTextSharp.text.Color(System.Drawing.ColorTranslator.FromHtml("#cccccc")); ;
    //                    }
    //                    else { cellRows.BackgroundColor = new iTextSharp.text.Color(System.Drawing.ColorTranslator.FromHtml("#ffffff")); }
    //                    iTextSharp.text.Font RowFont = FontFactory.GetFont(FontFactory.HELVETICA, 12);
    //                    Chunk chunkRows = new Chunk(dataTable.Rows[k][j].ToString(), RowFont);
    //                    cellRows.Add(chunkRows);

    //                    pdfTable.AddCell(cellRows);
    //                }
    //            }

    //            pdfDoc.Add(pdfTable);
    //            pdfDoc.Close();
    //            Response.ContentType = "application/octet-stream";
    //            Response.AddHeader("Content-Disposition", "attachment; filename=" + Name + "_" + DateTime.Now.ToString() + ".pdf");
    //            Response.Clear();
    //            Response.BinaryWrite(mStream.ToArray());
    //            Response.End();

    //        }
    //    catch (Exception ex)
    //    {

    //    }
    //}

    protected void PDF_Export()
    {
        string filename = "ClientReport";
        string[] searchParams = GetSearchParameters();
        DataTable dt = bll.SearchClientDetailsTable(searchParams);
        if (dt.Rows.Count > 0)
        {
            Document pdfDoc = new Document(PageSize.A4, 10, 10, 10, 10);
            PdfWriter.GetInstance(pdfDoc, System.Web.HttpContext.Current.Response.OutputStream);
            pdfDoc.Open();
            Chunk c = new Chunk
                ("Client Report",
                FontFactory.GetFont("Verdana", 15));
            Paragraph p = new Paragraph();
            p.Alignment = Element.ALIGN_CENTER;
            p.Add(c);
            pdfDoc.Add(p);
            try
            {
                iTextSharp.text.Font fnt1 = FontFactory.GetFont(FontFactory.HELVETICA, 9, iTextSharp.text.Font.BOLD);
                iTextSharp.text.Font fnt = FontFactory.GetFont("Times New Roman", 9);
                if (dt != null)
                {
                    PdfPTable PdfTable = new PdfPTable(dt.Columns.Count);
                    PdfPCell PdfPCell = null;
                    for (int rows = 0; rows < dt.Rows.Count; rows++)
                    {
                        if (rows == 0)
                        {
                            for (int column = 0; column < dt.Columns.Count; column++)
                            {

                                PdfPCell = new PdfPCell(new Phrase(new Chunk(dt.Columns[column].ColumnName.ToString(), fnt1)));
                                PdfTable.AddCell(PdfPCell);
                            }
                        }
                    }
                    for (int rows = 0; rows < dt.Rows.Count; rows++)
                    {
                        for (int column = 0; column < dt.Columns.Count; column++)
                        {
                            PdfPCell = new PdfPCell(new Phrase(new Chunk(dt.Rows[rows][column].ToString(), fnt)));
                            PdfTable.AddCell(PdfPCell);
                        }
                    }

                    pdfDoc.Add(PdfTable);
                }
                pdfDoc.Close();
                Response.ContentType = "application/pdf";
                Response.AddHeader("content-disposition", "attachment; filename=" + DateTime.Now.ToString("yyyyMMdd") + ".pdf");
                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                Response.Flush();
                Response.End();

            }
            catch (Exception ex)
            {
                Response.Write(ex.ToString());

            }
        }

    }

    protected void Excel_Export()
    {
        string[] searchParams = GetSearchParameters();
        DataTable dt = bll.SearchClientDetailsTable(searchParams);
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
                      string.Format("attachment;  filename={0}", "ClientReport.xlsx"));
            Response.BinaryWrite(package.GetAsByteArray());
        }
    }
}
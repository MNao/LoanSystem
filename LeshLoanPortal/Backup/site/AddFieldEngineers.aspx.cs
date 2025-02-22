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

public partial class Areas : System.Web.UI.Page
{
    Databasefile data_file = new Databasefile();
    Processfile Process_file = new Processfile();
    DataTable data_table = new DataTable();

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (IsPostBack == false)
            {
                MultiView1.ActiveViewIndex = 0;
                btnCancel.Visible = false;
                LoadLocations();
                LoadFieldEngineers();
                LinkButton MenuSms = (LinkButton)Master.FindControl("lblsmsPanel");
                LinkButton MenuReport = (LinkButton)Master.FindControl("lblReporting");
                LinkButton MenuProfile = (LinkButton)Master.FindControl("lblSetup");
                LinkButton MenuSettting = (LinkButton)Master.FindControl("lbtnSetting");
                MenuSms.Font.Italic = false;
                MenuReport.Font.Italic = false;
                MenuSettting.Font.Italic = true;
                MenuProfile.Font.Italic = false;
               string strProcessScript = "this.value='Working...';this.disabled=true;";
                Button1.Attributes.Add("onclick", strProcessScript + ClientScript.GetPostBackEventReference(Button1, "").ToString());

                if(Request.QueryString["Bulk"]!=null)
                {
                   
                    MultiView3.ActiveViewIndex = 1;
                }
                else
                {
                    MultiView3.ActiveViewIndex = 0;
                }
            }
        }
        catch (Exception ex)
        {
            ShowMessage(ex.Message, true);
        }
    }

    private void LoadLocations()
    {
        //data_table = data_file.GetLocationslist();
        data_table = data_file.GetAreas();
        ddlLocation.DataSource = data_table;
        ddlLocation.DataTextField = "Area";
        ddlLocation.DataValueField = "AreaID";
        ddlLocation.DataBind();

        ddlLocation2.DataSource = data_table;
        ddlLocation2.DataTextField = "Area";
        ddlLocation2.DataValueField = "AreaID";
        ddlLocation2.DataBind();

        ddlAreas.DataSource = data_table;
        ddlAreas.DataTextField = "Area";
        ddlAreas.DataValueField = "AreaID";
        ddlAreas.DataBind();
    }
    private void LoadFieldEngineers()
    {
        string name=txtSearch.Text;
        string contact=txtSearchContact.Text;
        int areaId = int.Parse(ddlAreas.SelectedValue.ToString());
        MultiView2.ActiveViewIndex = 0;
        data_table = data_file.GetEngineers(name, contact, areaId);
        DataGrid1.DataSource = data_table;
        DataGrid1.DataBind();
    }
    private void validatefile()
    {
        try
        {
            string location =ddlLocation2.SelectedValue.ToString();
            if (location.Equals("0"))
            {
                ShowMessage("Select Enginners Location", true);
            }
            else 
            {
                if (FileUpload1.FileName.Trim().Equals(""))
                {
                    ShowMessage("Please Browse File to Upload", true);
                }
                else
                {
                    string path = Path.GetFullPath(FileUpload1.FileName);
                    string fileName = Path.GetFileName(FileUpload1.FileName);
                    string fileExtension = Path.GetExtension(fileName);
                    string extension = fileExtension.ToUpper();
                    if (!extension.Equals(".CSV"))
                    {
                        ShowMessage("Invalid File Selected", true);
                    }
                    else
                    {
                        string folder = data_file.GetSystemParameter(1);
                        string NewfilePath = folder + fileName;
                        FileUpload1.SaveAs(NewfilePath);
                        string output = Process_file.UploadEngineerList(NewfilePath, location);
                        if (output.Contains("SUCCESSFULLY"))
                        {

                            ShowMessage(output, false);
                            MultiView1.ActiveViewIndex = 0;
                            MultiView2.ActiveViewIndex = 0;
                            LoadFieldEngineers();
                        }
                        else
                        {
                            ShowMessage(output, true);
                        }
                        
                    }
                }
            }
        }
        catch(Exception ex)
        {
            ShowMessage(ex.Message , true);
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
    protected void DataGrid1_ItemCommand(object source, DataGridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "btnEdit")
            {
                string RecordId = e.Item.Cells[0].Text;
                string Name = e.Item.Cells[1].Text;
                string Contact = e.Item.Cells[4].Text;
                string LocationId = e.Item.Cells[6].Text;
                string Active=e.Item.Cells[7].Text;
                lblCode.Text = RecordId;
                txtName.Text = Name;
                if (Active.Trim().ToUpper().Equals("YES"))
                {
                    ChkActive.Checked = true;
                }
                else
                {
                    ChkActive.Checked = false;
                }
                txtPhone.Text = Contact;
                ddlLocation.SelectedValue = LocationId;
                MultiView3.ActiveViewIndex = 0;
                MultiView4.ActiveViewIndex = 0;
                ShowMessage(".", true);
            }           
        }
        catch (Exception ex)
        {
            ShowMessage(ex.Message, true);
        }
    }    
    protected void Button1_Click(object sender, EventArgs e)
    {
        try
        {
            string RecordId = lblCode.Text.Trim();
            string Name = txtName.Text.Trim();
            string Phone = txtPhone.Text;
            string location =ddlLocation.SelectedValue.ToString();
            if (Name.Equals(""))
            {
                ShowMessage("Please Enter Name", true);
                txtName.Focus();
            }
            else if (Phone.Equals(""))
            {
                ShowMessage("Please Enter Contact", true);
                txtName.Focus();
            }
            else if (location.Equals("0"))
            {
                ShowMessage("Please Select Location", true);
            }
            else
            {
                if (RecordId.Equals("0"))
                {
                    ChkActive.Checked = true;
                }
                bool Active = ChkActive.Checked;
                string res_tariff = Process_file.Save_FieldEngineer(RecordId, Name, Phone, location, Active);
                ShowMessage("FIELD ENGINEER DETAILS SAVED SUCCESSFULLY", false);
                txtName.Text = "";
                txtPhone.Text = "";
                ddlLocation.SelectedValue = "0";
                lblCode.Text = "0";
                MultiView4.ActiveViewIndex = -1;
                LoadFieldEngineers();
            }
        }
        catch (Exception ex)
        {
            ShowMessage(ex.Message, true);
        }
    }
    protected void DataGrid1_PageIndexChanged(object source, DataGridPageChangedEventArgs e)
    {
        try
        {
            string name = txtSearch.Text;
            string contact = txtSearchContact.Text;
            int areaId = int.Parse(ddlAreas.SelectedValue.ToString());
            data_table = data_file.GetEngineers(name, contact, areaId);
            DataGrid1.DataSource = data_table;
            DataGrid1.CurrentPageIndex = e.NewPageIndex;
            DataGrid1.DataBind();
        }
        catch (Exception ex)
        {
            ShowMessage(ex.Message, true);
        }
    }

    protected void ddlLocation_DataBound(object sender, EventArgs e)
    {
        ddlLocation.Items.Insert(0, new ListItem(" Select District ", "0"));
    }
    protected void ddlAreas_DataBound(object sender, EventArgs e)
    {
        ddlAreas.Items.Insert(0, new ListItem(" Select District ", "0"));
    }
    
    protected void ddlLocation2_DataBound(object sender, EventArgs e)
    {
        ddlLocation.Items.Insert(0, new ListItem(" Select District ", "0"));
    }
    protected void btnUpload_Click(object sender, EventArgs e)
    {
        try 
        {
            validatefile();
        }
        catch(Exception ex)
        {
            ShowMessage(ex.Message, true);
        }
    }


    protected void btnfiedEngineer_Click(object sender, EventArgs e)
    {
        try
        {
            MultiView3.ActiveViewIndex = 1;
            btnCancel.Visible = true;
            btnfiedEngineer.Visible = false;
        }
        catch(Exception ex)
        {
            ShowMessage(ex.Message,true);
        }
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            MultiView3.ActiveViewIndex = 0;
            btnfiedEngineer.Visible = true;
            btnCancel.Visible = false;
        }
        catch (Exception ex)
        {
            ShowMessage(ex.Message, true);
        }
    }
    protected void btnOK_Click(object sender, EventArgs e)
    {
        try 
        {
            LoadFieldEngineers();
        }
        catch(Exception ex)
        {
            ShowMessage(ex.Message,true);
        }
    }
}

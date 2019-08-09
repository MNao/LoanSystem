using InterConnect.LeshLaonApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class AddLoan : System.Web.UI.Page
{
    SystemUser user;
    BusinessLogic bll = new BusinessLogic();
    LeshLoanAPI Client = new LeshLoanAPI();
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            user = Session["User"] as SystemUser;
            if (IsPostBack == false)
            {
                if ((Session["Username"] == null) || (Session["RoleCode"].ToString() == null) || (Session["RoleCode"].ToString() == ""))
                {
                    Response.Redirect("Default.aspx");
                }
                MultiView2.ActiveViewIndex = -1;
                btnApproveLoan.Visible = false;
                LoanDets.Visible = false;

                string UserID = Request.QueryString["UserID"];
                string ClientID = Request.QueryString["ClientID"];
                string LoanNo = Request.QueryString["LoanNo"];
                string UserType = Request.QueryString["UserType"];
                string Type = Request.QueryString["Type"];
                string Status = Request.QueryString["Status"];
                
                if (ClientID != null)
                {
                    MultiView1.SetActiveView(LoanDetails);
                    btnApproveLoan.Visible = true;
                    btnSubmitLoanDetails.Visible = false;
                    LoadLoanDetails(ClientID, LoanNo);
                    //string UserCode = Encryption.encrypt.DecryptString(Request.QueryString["transferid"].ToString(), "25011Pegsms2322");
                    //LoadControls(UserCode);
                }
                else //if (!string.IsNullOrEmpty(UserID))
                {
                    LoadData();
                }
            }
        }
        catch (Exception ex)
        {
            ShowMessage(ex.Message, true);
        }
    }

    public void LoadLoanDetails(string ClientID, string LoanNo)
    {
        
        LoanDetails LoanDet = bll.GetLoanDetails(user, ClientID, LoanNo);
        txtLoanNo.Text = LoanNo;
        txtLoanDate.Text = Convert.ToDateTime(LoanDet.LoanDate).ToString("dd-MM-yyyy");
        txtLoanAmount.Text = string.Format("{0:n0}", Convert.ToInt32(LoanDet.LoanAmount)); 
        txtLoanPurp.Text = LoanDet.LoanDesc;
        txtorg.Text = LoanDet.Organization;
        //txtApprov.Text = string.Format("{0:n0}", Convert.ToInt32(LoanDet.ApprovedAmount));
        txtInterest.Text = LoanDet.InterestRate;

        LoanDets.Visible = true;

        txtLoanNo.Enabled = false;
        txtLoanDate.Enabled = false;
        txtLoanAmount.Enabled = false;
        txtLoanPurp.Enabled = false;
        txtorg.Enabled = false;
        //txtApprov.Enabled = false;
        txtInterest.Enabled = false;

        //txtAmountToPayPerMonth.Enabled = false;
        //txtAppLoanAmount.Enabled = false;
    }
    
    public void LoadData()
    {

        if (user.RoleCode != "001")
        {
            ddClientNo.SelectedItem.Value = user.UserId;
            ddClientNo.SelectedItem.Text = user.UserId;
            //ddClientNo.Enabled = false;
        }
        else
        {
            bll.LoadClientsIntoDropDown(user.CompanyCode, "", ddClientNo);
        }
        ddClientNoSelectedIndexChanged(null,null);
    }
    protected void txtSearch_TextChanged(object sender, EventArgs e)
    {
        ListItemCollection ClientSearch = bll.GetClientSearchDetails(txtSearch.Text);
        ListView lvItem = new ListView();
        
        //ddlSearch.Items.Add(ClientSearch.ToString());
        //ddlSearch.Items.Add(new ListItem(ClientSearch));

        //if (txtSearch.Text != "")
        //{
        //    foreach (ListViewItem item in listView1.Items)
        //    {
        //        if (item.Text.ToLower().Contains(txtSearch.Text.ToLower()))
        //        {
        //            item.Selected = true;
        //        }
        //        else
        //        {
        //            listView1.Items.Remove(item);
        //        }

        //    }
        //    if (listView1.SelectedItems.Count == 1)
        //    {
        //        listView1.Focus();
        //    }
        //}
        //else
        //{
        //    //LoadContacts();
        //    //RefreshAll();
        //}
    }

    protected void ddClientNoSelectedIndexChanged(object sender, EventArgs e)
    {
       if(ddClientNo.SelectedValue == "")
        {

        }
        else
        {
            if (user.RoleCode != "001")
            {
                ddClientNo.SelectedItem.Value = user.UserId;
                ddClientNo.SelectedItem.Text = user.UserId;
                ddClientNo.Enabled = false;
            }

            InterConnect.LeshLaonApi.ClientDetails GetClient = bll.GetClientDetails(user, ddClientNo.SelectedValue);

            txtClientname.Text = GetClient.ClientName;
            ddGender.SelectedItem.Value = GetClient.Gender;
            ddGender.SelectedItem.Text = GetClient.Gender;
            txtIDNumber.Text = GetClient.IDNumber;
            txtEmail.Text = GetClient.ClientEmail;
            txtHomeAddress.Text = GetClient.ClientAddress;
            txtMobileNo.Text = GetClient.ClientPhoneNumber;
            txtNameofReferee.Text = GetClient.Referee;


            ddGender.Enabled = false;
            txtClientname.Enabled = false;
            txtIDNumber.Enabled = false;
            txtEmail.Enabled = false;
            txtHomeAddress.Enabled = false;
            txtMobileNo.Enabled = false;
            txtNameofReferee.Enabled = false;

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

    protected void menuTabs_MenuItemClick(object sender, MenuEventArgs e)
    {
        Menu menuTabs = sender as Menu;
        MultiView multiTabs = this.FindControl("MultiView1") as MultiView;
        multiTabs.ActiveViewIndex = Int32.Parse(menuTabs.SelectedValue);

    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        try
        {

        InterConnect.LeshLaonApi.ClientDetails AddnClient = GetAddnClientDetails();
        //validate client details input
        string check_status = validate_input(AddnClient.DOB, AddnClient.BusinessLoc, AddnClient.Occupation, AddnClient.NoOfBeneficiaries, AddnClient.EducLevel, AddnClient.MonthlyIncome);


        if (!check_status.Equals("OK"))
        {
            ShowMessage(check_status, true);
        }
        else
        {
            //save client additional details
            Result user_save = Client.SaveAdditionalClientDetails(AddnClient);

            if (user_save.StatusCode != "0")
            {
                //MultiView2.ActiveViewIndex = 0;
                ShowMessage(user_save.StatusDesc, true);
                return;
            }
            ShowMessage("ADDITIONAL DETAILS SAVED SUCCESSFULLY", false);
            //Clear_contrls();
            //bll.InsertIntoAuditLog("USER-CREATION", "SYSTEMUSERS", user.CompanyCode, user.UserId, "USER CREATED SUCCESSFULLY");
            MultiView1.SetActiveView(LoanDetails);
            txtLoanNo.Text = bll.GenerateSystemCode("LOAN");

             SystemSetting setting = bll.GetSystemSettingById("INTEREST_RATE", "Lesh");
             txtInterest.Text = setting.SettingValue;
            txtLoanNo.Enabled = false;
            txtInterest.Enabled = false;

            }

        }
        catch(Exception ex)
        {

        }

    }

    protected InterConnect.LeshLaonApi.ClientDetails GetAddnClientDetails()
    {
        InterConnect.LeshLaonApi.ClientDetails AddnClient = new InterConnect.LeshLaonApi.ClientDetails();
        AddnClient.ClientNo = ddClientNo.SelectedValue;
        AddnClient.DOB = txtBirthDate.Text.ToString();
        AddnClient.BusinessLoc = txtBusLoc.Text.Trim().ToString();
        AddnClient.Occupation = txtOccup.Text.Trim().ToString();
        AddnClient.NoOfBeneficiaries = txtBenf.Text.Trim().ToString();
        AddnClient.EducLevel = txtEduc.Text.Trim().ToString();
        AddnClient.MonthlyIncome = txtMonthlyInc.Text.ToString().Replace(",","");
        AddnClient.ModifiedBy = user.UserId;
        return AddnClient;
    }

    
    private string validate_input(string DOB, string BusiLoc, string Occup, string NoofBen, string EducLv, string MonthlyInc)
    {
        string output = "";
        if (DOB.Equals(""))
        {
            output = "Date of Birth Required";
            txtBirthDate.Focus();
        }
        else if (BusiLoc.Equals(""))
        {
            output = "Business Location Required";
            txtBusLoc.Focus();
        }
        else if (Occup.Equals(""))
        {
            output = "Occupation Required";
            txtOccup.Focus();
        }
        else if (NoofBen.Equals(""))
        {
            output = "Number of Beneficials Required";
            txtBenf.Focus();
        }
        else if (EducLv.Equals(""))
        {
            output = "Education Level Required";
            txtEduc.Focus();
        }
        else if (MonthlyInc.Equals(""))
        {
            output = "Monthly Income Required";
            txtMonthlyInc.Focus();
        }

        else
        {
            output = "OK";
        }
        return output;
    }

    protected void ClientDetailsBtn_Click(object sender, EventArgs e)
    {
        MultiView1.SetActiveView(ClientDetails);
    }

    protected void LoanDetailsBtn_Click(object sender, EventArgs e)
    {
        //MultiView1.SetActiveView(LoanDetails);

        if (String.IsNullOrEmpty(txtLoanNo.Text))
        {
            MultiView1.SetActiveView(ClientDetails);
        }
        else
        {
            MultiView1.SetActiveView(LoanDetails);
        }

        //LoanDetailsBtn.BackColor = System.Drawing.Color.Blue;
        //ClientDetailsBtn.BackColor = System.Drawing.Color.GhostWhite;
        //LoanAgmtBtn.BackColor = System.Drawing.Color.Blue;
    }
    protected void CollateralDetailsBtn_Click(object sender, EventArgs e)
    {
        MultiView1.SetActiveView(CollateralView);
    }

    protected void LoanAgmtBtn_Click(object sender, EventArgs e)
    {
    //    if (String.IsNullOrEmpty(txtLoanNo.Text))
    //    {
    //        MultiView1.SetActiveView(ClientDetails);
    //    }
    //    else
    //    {   
    //        //Load loan agreement view if the loan view is submitted
    //        MultiView1.SetActiveView(LoanAgmtView);
    //    }
        MultiView1.SetActiveView(LoanAgmtView);
    }

    protected void btnSubmitLoanDetails_Click(object sender, EventArgs e)
    {
        try
        {

            LoanDetails Loan = GetLoanDetails();
            //validate loan details input
            string check_status = validate_LoanInputDetails(Loan.LoanDate, Loan.LoanAmount, Loan.InterestRate,Loan.LoanDesc, Loan.Organization);


            if (!check_status.Equals("OK"))
            {
                ShowMessage(check_status, true);
            }
            else
            {
                //save loan details
                Result user_save = Client.SaveLoanDetails(Loan);

                if (user_save.StatusCode != "0")
                {
                    //MultiView2.ActiveViewIndex = 0;
                    ShowMessage(user_save.StatusDesc, true);
                    return;
                }
                ShowMessage("LOAN DETAILS SAVED SUCCESSFULLY", false);
                //Clear_contrls();
                //bll.InsertIntoAuditLog("USER-CREATION", "SYSTEMUSERS", user.CompanyCode, user.UserId, "USER CREATED SUCCESSFULLY");

                //redirect to Collateral view
                MultiView1.SetActiveView(CollateralView);

            }

        }
        catch (Exception ex)
        {

        }

    }

    protected InterConnect.LeshLaonApi.LoanDetails GetLoanDetails()
    {
        LoanDetails Loan = new LoanDetails();
        Loan.ClientID = ddClientNo.SelectedValue;
        Loan.LoanNo = txtLoanNo.Text;
        Loan.LoanDate = txtLoanDate.Text.ToString();
        Loan.LoanAmount = txtLoanAmount.Text.Trim().ToString().Replace(",", "");
        Loan.LoanDesc = txtLoanPurp.Text.Trim().ToString();
        Loan.InterestRate = txtInterest.Text.Trim().ToString();
        //Loan.ApprovedAmount = txtApprov.Text.ToString().Replace(",","");
        //Loan.LastLoanAmount = txtLastLoanRcvdAmount.Text.Trim().ToString();
        //Loan.EasyPaidAmountPerMonth = txtPayLoan.Text;
        //Loan.CurrentDebt = txtDebtAmount.Text;
        Loan.Organization = txtorg.Text;
        Loan.ModifiedBy = user.UserId;
        return Loan;
    }


    private string validate_LoanInputDetails(string LoanDate, string LoanAmount, string InterestRate, string LoanDesc, string Org)
    {
        string output = "";
        if (LoanDate.Equals(""))
        {
            output = "Date of Loan Required";
            txtLoanDate.Focus();
        }
        else if (LoanAmount.Equals(""))
        {
            output = "Loan Amount Required";
            txtBusLoc.Focus();
        }
        else if (InterestRate.Equals(""))
        {
            output = "Interest Rate Required";
            txtInterest.Focus();
        }
        //else if (ApprovedAmount.Equals(""))
        //{
        //    output = "Approved Amount Required";
        //    txtBenf.Focus();
        //}
        else if (LoanDesc.Equals(""))
        {
            output = "Loan Description Required";
            txtLoanPurp.Focus();
        }

        else
        {
            output = "OK";
        }
        return output;
    }

    protected void btnAcceptTc_Click(object sender, EventArgs e)
    {
        bool Tc = chckAgreeTc.Checked; //this.FindControl("chckAgreeTc") as CheckBox; 

        if (Tc)
        {
            string LoanNo = txtLoanNo.Text;
            string ClientID = ddClientNo.SelectedValue;
            bll.UpdateLoanStatus(LoanNo, ClientID, user.RoleCode, user.UserId);

            ShowMessage("YOUR LOAN HAS BEEN ACCEPTED FOR REVIEW, WE WILL SHORTLY GET BACK TO YOU AFTER REVIEW.", false);
            //where to redirect
            Response.Redirect("ViewLoans.aspx");
        }
        else
        {
            ShowMessage("You havent accepted Terms and Conditions, Your Loan Application will not be processed", true);
        }
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("Admin.aspx");
    }

    protected void btnApproveLoan_Click(object sender, EventArgs e)
    {
        LoanDetails Loan = GetApprovalLoanDetails();

        
        Result Approval_save = bll.SaveLoanApproval(Loan.LoanNo, Loan.Approved, Loan.ApprovedAmount, Loan.ModifiedBy, Loan.MonthsToPayIn, Loan.EasyPaidAmountPerMonth, Loan.ImageProof);

        if (Approval_save.StatusCode != "0")
        {
            //MultiView2.ActiveViewIndex = 0;
            ShowMessage(Approval_save.StatusDesc, true);
            return;
        }
        ShowMessage("LOAN "+Loan.LoanNo+" APPROVED SUCCESSFULLY", false);
        //Clear_contrls();
        bll.InsertIntoAuditLog("LOAN-APPROVAL", "LOANS", user.CompanyCode, user.UserId, "LOAN "+Loan.LoanNo+" APPROVED SUCCESSFULLY");

        //redirect to Approval page
        Response.Redirect("ApproveLoan.aspx");

    }

    public LoanDetails GetApprovalLoanDetails()
    {
        LoanDetails ApprovalLoanDetails = new LoanDetails();
        ApprovalLoanDetails.LoanNo = txtLoanNo.Text;
        ApprovalLoanDetails.Approved = "true";
        ApprovalLoanDetails.ApprovedAmount = txtAppLoanAmount.Text.Replace(",", "");
        ApprovalLoanDetails.EasyPaidAmountPerMonth = txtAmountToPayPerMonth.Text.Replace(",","");
        ApprovalLoanDetails.MonthsToPayIn = txtMonths.Text;
        ApprovalLoanDetails.ModifiedBy = user.UserId;
        ApprovalLoanDetails.ImageProof = bll.GetImageUploadedInBase64String(ImgGuarantor);
        return ApprovalLoanDetails;
    }



    protected void btnCollateral_Click(object sender, EventArgs e)
    {
        try
        {

           LoanDetails Loan = GetCollateralDetails();
            //validate loan details input
            string check_status = validate_CollateralInputDetails(Loan.Name, Loan.Type, Loan.Model, Loan.Make, Loan.SerialNumber, Loan.EstimatedPrice);


            if (!check_status.Equals("OK"))
            {
                ShowMessage(check_status, true);
            }
            else
            {
                //save Collateral details
                Result user_save = bll.SaveCollateralDetails(Loan.LoanNo, Loan.Name, Loan.Type, Loan.Model, Loan.Make, Loan.SerialNumber, Loan.EstimatedPrice, Loan.ImageProof, Loan.Observations, user.UserId);

                if (user_save.StatusCode != "0")
                {
                    //MultiView2.ActiveViewIndex = 0;
                    ShowMessage(user_save.StatusDesc, true);
                    return;
                }
                ShowMessage("Collateral DETAILS SAVED SUCCESSFULLY", false);
                //Clear_contrls();
                //bll.InsertIntoAuditLog("USER-CREATION", "SYSTEMUSERS", user.CompanyCode, user.UserId, "USER CREATED SUCCESSFULLY");

                //redirect to loan aggreement
                MultiView1.SetActiveView(LoanAgmtView);

            }

        }
        catch (Exception ex)
        {

        }
    }

    protected LoanDetails GetCollateralDetails()
    {
        LoanDetails Loan = new LoanDetails();
        Loan.LoanNo = txtLoanNo.Text;
        Loan.Name = txtColName.Text.ToString();
        Loan.Type = txtLoanAmount.Text.Trim();
        Loan.Model = txtLoanPurp.Text.Trim().ToString();
        Loan.Make = txtInterest.Text.Trim().ToString();
        Loan.SerialNumber = txtColSerialNo.Text.ToString();
       Loan.Observations = txtColObsv.Text;
        Loan.ImageProof = txtColImgProof.FileBytes.ToString();
        Loan.EstimatedPrice = txtorg.Text;
        Loan.ModifiedBy = user.UserId;
        return Loan;
    }

    private string validate_CollateralInputDetails(string Name, string Type, string Model, string Make, string SerialNo, string EstPrice)
    {
        string output = "";
        if (Name.Equals(""))
        {
            output = "Collateral Name Required";
            txtColName.Focus();
        }
        else if (Type.Equals(""))
        {
            output = "Collateral Type Required";
            txtColType.Focus();
        }
        else if (Model.Equals(""))
        {
            output = "Collateral Model Required";
            txtColModel.Focus();
        }
        else if (Make.Equals(""))
        {
            output = "Collateral Make Required";
            txtColMake.Focus();
        }
        else if (SerialNo.Equals(""))
        {
            output = "Collateral Serial Number Required";
            txtColSerialNo.Focus();
        }
        else if (EstPrice.Equals(""))
        {
            output = "Collateral Estimated Price Required";
            txtColEstPrice.Focus();
        }

        else
        {
            output = "OK";
        }
        return output;
    }
}
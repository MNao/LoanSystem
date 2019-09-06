<%@ Page Language="C#" MasterPageFile="~/MasterMain.master" AutoEventWireup="true" CodeFile="AddLoan.aspx.cs" Inherits="AddLoan" Title="SYSTEM USER" %>
 <%@ Register 
 Assembly="AjaxControlToolkit" 
 Namespace="AjaxControlToolkit" 
 TagPrefix="ajaxToolkit" %>
 <%@ Import
  Namespace="System.Threading" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
   
    <ajaxToolkit:ToolkitScriptManager runat="Server" EnableScriptGlobalization="true"
        EnableScriptLocalization="true" ID="ScriptManager1" />

         <div class="col-lg-12">
                    
             </div>
     

<div class="col-lg-12">
    <div class="container">
    <div class="card mb-3">
        <div class="card-header">
        <i class="fa fa fa-cog"></i> Loans <i class='fa fa-arrow-right'></i> Add Loan
        </div>
        <div class="card-body">
            <div class="row clearfix">

<%--               
                <asp:Menu  
                  ID="menuTabs"  
                  Orientation="Horizontal"  
                  Width="100%"  
                  runat="server" 
                   OnMenuItemClick="menuTabs_MenuItemClick" 
                  >  
                  <Items>  
                      <asp:MenuItem Text="Employee Info" Value="0" Selected="true"/>  
                      <asp:MenuItem Text="Contact Info" Value="1" />  
                      <asp:MenuItem Text="Salary Info" Value="2" />  
                  </Items>  
              </asp:Menu> --%>
                <div class="col-lg-12" style=" padding-bottom: 20px;">
                <ul class="nav nav-tabs nav-justified" >
                        <li id="ClientDetailsLink" runat="server" class="nav-item">
                            <asp:LinkButton ID="ClientDetailsBtn" runat="server" CssClass="list-group-item list-group-item-action" OnClick="ClientDetailsBtn_Click">Client Info</asp:LinkButton></li>
                        <li id="LoanDetailslink" runat="server" class="nav-item">
                            <asp:LinkButton ID="LoanDetailsBtn" runat="server" CssClass="list-group-item list-group-item-action" OnClick="LoanDetailsBtn_Click">Loan Details</asp:LinkButton></li>
                        <li id="CollateralDetailsLink" runat="server" class="nav-item">
                            <asp:LinkButton ID="CollateralDetailsBtn" runat="server" CssClass="list-group-item list-group-item-action" OnClick="CollateralDetailsBtn_Click">Collateral</asp:LinkButton></li>
                        <li id="LoanAgmtDetails" runat="server" class="nav-item">
                            <asp:LinkButton ID="LoanAgmtBtn" runat="server" CssClass="list-group-item list-group-item-action" OnClick="LoanAgmtBtn_Click">Loan Agreement</asp:LinkButton></li>
                    </ul>
                    </div>
                <div class="col-lg-12"></div>
            <asp:MultiView ID="MultiView1" runat="server" ActiveViewIndex="0">
                <asp:View ID="ClientDetails" runat="server">
                <div class="modal-content col-md-6  col-sm-6 col-xs-10"  style="margin:0 auto;">
		            <div class="modal-body">
                        <div class="row">
                        <div class="col-sm-6">
                        Client Number
                        <asp:DropDownList runat="server" ID="ddClientNo" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddClientNoSelectedIndexChanged">
                            <asp:ListItem>YES</asp:ListItem>
                            <asp:ListItem>NO</asp:ListItem>
                        </asp:DropDownList></div>
                    <div class="col-sm-6">   
                        Search
                        <asp:TextBox ID="txtSearch" runat="server" CssClass="form-control"></asp:TextBox>
                        
                        
                    </div>
                        </div>
                      Client Name
                      <asp:TextBox ID="txtClientname" runat="server" CssClass="form-control"  placeholder="Client Name"></asp:TextBox>
                       
                        
                      Date Of Birth
                      <asp:TextBox ID="txtBirthDate" runat="server"  CssClass="form-control"  placeholder="Date of Birth"></asp:TextBox>

                      Gender
                                            <asp:DropDownList ID="ddGender" runat="server" CssClass="form-control">
                                                <asp:ListItem Value="">--Select Your Gender--</asp:ListItem>
                                                <asp:ListItem Value="Male">MALE</asp:ListItem>
                                                <asp:ListItem Value="Female">FEMALE</asp:ListItem>
                                            </asp:DropDownList>

                      Name of Father/Next of Kin
                      <asp:TextBox ID="txtNameofReferee" runat="server" CssClass="form-control"  placeholder="Referee Name"></asp:TextBox>
                        
                      Business Location
                      <asp:TextBox ID="txtBusLoc" runat="server" CssClass="form-control"  placeholder="Client's Business Location"></asp:TextBox>

                      Occupation
                      <asp:TextBox ID="txtOccup" runat="server" CssClass="form-control"  placeholder="Client's Location"></asp:TextBox>

                    Monthly Income
                      <asp:TextBox ID="txtMonthlyInc" runat="server" CssClass="form-control"  placeholder="Client's Monthly Income" onkeyup="javascript:this.value=Comma(this.value);"></asp:TextBox>
                        
                      ID Number
                      <asp:TextBox ID="txtIDNumber" runat="server" CssClass="form-control" placeholder="Identification Number"></asp:TextBox>
                        
                      Mobile No. Own
                      <asp:TextBox ID="txtMobileNo" runat="server" CssClass="form-control"  placeholder="Client's Own Mobile Number"></asp:TextBox>

                      Email
                      <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control"  placeholder="Client's Email"></asp:TextBox>
                        
                      Home Address
                      <asp:TextBox ID="txtHomeAddress" runat="server" CssClass="form-control"  placeholder="Client's Home Address"></asp:TextBox>

                      Number of Beneficials
                      <asp:TextBox ID="txtBenf" runat="server" CssClass="form-control"  placeholder="How many Beneficials"></asp:TextBox>
                        
                      Education Level
                      <asp:TextBox ID="txtEduc" runat="server" CssClass="form-control"  placeholder="Education Level"></asp:TextBox>

                      <%--Vendor
                      <asp:DropDownList ID="ddlAreas" runat="server" OnDataBound="ddlAreas_DataBound" CssClass="form-control">
                      </asp:DropDownList>--%>


                      <%--<br/><asp:CheckBox ID="chkActive" runat="server" Text=" Is Active" /><br/>--%>

                      <asp:MultiView ID="MultiView2" runat="server">
                        <asp:View ID="View3" runat="server">
                                    User Name
                                    <asp:TextBox ID="txtUserName" runat="server" CssClass="form-control"></asp:TextBox>
                                           
                        </asp:View>
                        <asp:View ID="View2" runat="server">
                         <br/><asp:CheckBox ID="chkActive" runat="server" Text=" Tick To Activate User Account" /><br/>
                         <br/><asp:CheckBox ID="ChkDeactivate" runat="server" Text="Tick To De-Activate User Account" /><br/>
                         <br/><asp:CheckBox ID="ChkReset" runat="server" Text=" Tick To Reset Password" /> <br/>
                        </asp:View>
                    </asp:MultiView>
                    </div>
                    <div class="modal-footer">
                        <asp:Button ID="btnSubmit" runat="server" CssClass="btn btn-success" Text="SUBMIT DETAILS"  OnClick="btnSubmit_Click" />
                    </div>
                    
                    
                    <script type="text/javascript">

                        $(document).ready(function () {
                            $("#txtSearch").autocomplete({
                                source: ["c++", "java", "php", "coldfusion", "javascript", "asp", "ruby"]
                                //source: 'GetClientName.ashx'
                            });
                        });
                    </script>
                </div>
                    
                <ajaxToolkit:CalendarExtender ID="CalendarExtender1" runat="server" CssClass="cal_Theme1"
            Format="yyyy-MM-dd" PopupPosition="BottomRight" TargetControlID="txtBirthDate">
        </ajaxToolkit:CalendarExtender>
                    
            </asp:View>

                <asp:View ID="LoanDetails" runat="server">
                    <div class="modal-content col-md-6  col-sm-6 col-xs-10"  style="margin:0 auto;">
                   
		            <div class="modal-body">
                        Loan Number
                      <asp:TextBox ID="txtLoanNo" runat="server" CssClass="form-control"  placeholder="Loan Number"></asp:TextBox>

                        <%--Loan Type
                      <asp:DropDownList ID="ddlLoanType" runat="server" CssClass="form-control">
                      </asp:DropDownList>--%>
                        
                      Loan Date
                      <asp:TextBox ID="txtLoanDate" runat="server" CssClass="form-control"  placeholder="Loan Date"></asp:TextBox>

                        Loan Amount
                      <asp:TextBox ID="txtLoanAmount" runat="server" CssClass="form-control LoanAmnt" onchange="CalculateTotalLoanAmt();" onkeyup="javascript:this.value=Comma(this.value);"></asp:TextBox>

                      Interest Rate (%)
                      <asp:TextBox ID="txtInterest" runat="server" CssClass="form-control Interest" onchange="CalculateTotalLoanAmt();"></asp:TextBox>

                        Purpose of the Loan
                      <asp:TextBox ID="txtLoanPurp" runat="server"  CssClass="form-control" TextMode="multiline" Columns="10" Rows="2"></asp:TextBox>
                        
                      Organization
                      <asp:TextBox ID="txtorg" runat="server"  CssClass="form-control"></asp:TextBox>

                      <%--Approved Amount
                      <asp:TextBox ID="txtApprov" runat="server" CssClass="form-control TotalAmnt"></asp:TextBox>--%>
                        
                        <div runat="server" id="LoanDets">
                        Months To Pay In
                      <asp:TextBox ID="txtMonths" runat="server"  CssClass="form-control months" onchange="CalculateTotalLoanAmt();"></asp:TextBox>

                        Amount to Pay Per Month
                      <asp:TextBox ID="txtAmountToPayPerMonth" runat="server" Text="0"  CssClass="form-control AmntPerMonth"></asp:TextBox>
                        
                            Approved Loan Amount
                      <asp:TextBox ID="txtAppLoanAmount" runat="server" Text="0" CssClass="form-control AppLoanAmnt"></asp:TextBox>

                             Proof from Guarantor
                      <asp:FileUpload ID="ImgGuarantor" runat="server" CssClass="form-control"/>

                            Approver's Comment
                      <asp:TextBox ID="txtComment" runat="server" Textmode="MultiLine" Columns="5" Rows="2" CssClass="form-control"></asp:TextBox>
                        </div>
                       <%-- 
                      How much can you easily pay per month
                      <asp:TextBox ID="txtPayLoan" runat="server"  CssClass="form-control"></asp:TextBox>--%>
                      
                    </div>

                        <div class="modal-footer">
                        <asp:Button ID="btnSubmitLoanDetails" runat="server" CssClass="btn btn-success" Text="SUBMIT LOAN DETAILS" OnClick="btnSubmitLoanDetails_Click" />
                            <asp:Button ID="btnApproveLoan" runat="server" CssClass="btn btn-success" Text="APPROVE LOAN" OnClick="btnApproveLoan_Click" />
                    </div>
                        </div>

                    <ajaxToolkit:CalendarExtender ID="CalendarExtender2" runat="server" CssClass="cal_Theme1"
            Format="yyyy-MM-dd" PopupPosition="BottomRight" TargetControlID="txtLoanDate">
        </ajaxToolkit:CalendarExtender>

                    <script type="text/javascript">

            //function CalculateSubtotal() {
            //    try {
            //        var LoanAmnt = $(".LoanAmnt").val();
            //        var Interest = $(".Interest").val();
            //        var result = parseInt(removeCommas(LoanAmnt)) + parseInt((removeCommas(LoanAmnt) * (removeCommas(Interest)/100)));
            //        $(".TotalAmnt").val(Comma(result));
            //    }
            //    catch (err) {
            //        alert("EXCEPTION raised: ");
            //    }
            //}
            

            function CalculateTotalLoanAmt() {
                try {
                    var LoanAmnt = $(".LoanAmnt").val();
                    var InterestIn = $(".Interest").val();
                    var Months = $(".months").val();

                    var Interest = (parseInt(InterestIn) / 100);
                    var InterestPlus = (1 + parseFloat(Interest));
                    var InterestPow = Math.pow(InterestPlus, Months);
                    var top = (parseFloat(removeCommas(LoanAmnt)) * parseFloat(Interest) * parseFloat(InterestPow));//(Math.Pow((1 + Interest) , Months))
                    
                    var bottom = (parseFloat(InterestPow)) - 1;
                    var result = Math.round(parseFloat(top) / parseFloat(bottom));
                    
                    var WholeAmnt = (parseFloat(result) * parseFloat(Months));
                    $(".AmntPerMonth").val(Comma(result))
                    $(".AppLoanAmnt").val(Comma(WholeAmnt));
                }
                catch (err) {
                    alert("EXCEPTION raised: " + err.Message);
                }
            }

            function removeCommas(str) {
                while (str.search(",") >= 0) {
                    str = (str + "").replace(',', '');
                }
                return str;
            };
        </script>

                </asp:View>

                <asp:View ID="CollateralView" runat="server">
                    <div class="modal-content col-md-6  col-sm-6 col-xs-10"  style="margin:0 auto;">
                   
		            <div class="modal-body">
                        Name
                      <asp:TextBox ID="txtColName" runat="server" CssClass="form-control"  placeholder="Collateral Name"></asp:TextBox>

                        <%--Loan Type
                      <asp:DropDownList ID="ddlLoanType" runat="server" CssClass="form-control">
                      </asp:DropDownList>--%>
                        
                      Type of Collateral
                      <asp:TextBox ID="txtColType" runat="server" CssClass="form-control"  placeholder="Collateral Type"></asp:TextBox>

                        Model
                      <asp:TextBox ID="txtColModel" runat="server" CssClass="form-control"></asp:TextBox>

                      Make
                      <asp:TextBox ID="txtColMake" runat="server" CssClass="form-control"></asp:TextBox>

                        Serial Number
                      <asp:TextBox ID="txtColSerialNo" runat="server"  CssClass="form-control"></asp:TextBox>
                        
                     Estimated Price
                      <asp:TextBox ID="txtColEstPrice" runat="server"  CssClass="form-control"></asp:TextBox>

                      Proof of Ownership
                        <asp:FileUpload ID="txtColImgProof" runat="server" CssClass="form-control"/>
                       
                        Observations
                      <asp:TextBox ID="txtColObsv" runat="server"  CssClass="form-control" TextMode="MultiLine" Columns="5" Rows="2"></asp:TextBox>
                        
                      
                    </div>

                        <div class="modal-footer">
                        <asp:Button ID="btnCollateral" runat="server" CssClass="btn btn-success" Text="SUBMIT COLLATERAL DETAILS" OnClick="btnCollateral_Click" />
                    </div>
                        </div>

                </asp:View>

                <asp:View ID="LoanAgmtView" runat="server">
                    <div class="modal-content col-md-8  col-sm-6 col-xs-10"  style="margin:0 auto;">
                   <div class="modal-header">
                       <h5 class="modal-title text-center" id="exampleModalLabel">LOAN AGREEMENT</h5>
                       </div>
		            <div class="modal-body">
                        <div class="row">
                            <div class="col-lg-12">
                                This LOAN AGREEMENT is made between Lensh Microfinance Limited(Lender) of PO.Box 26687 Kampala, and Mr/Mrs/Ms <strong><asp:label runat="server" ID="lblCliName" Text="Name"></asp:label></strong> a resident of <strong><asp:label runat="server" ID="lblLocation" Text="Location"></asp:label></strong>, Tel:  <strong><asp:label runat="server" ID="lblTel" Text="Tel No"></asp:label></strong> (Hereinafter called the "Borrower") and provides as follows;
                            <ul runat="server">
                                <li>That Lensh Microfinance Limited has lent and the borrower has taken a loan of Uganda Shillings  at the rate of <strong><asp:label runat="server" ID="lblIntRate" Text="Interest"></asp:label>%</strong>  on reducing balance per month. </li>
                                <li>The borrower consents that the following deductions upfront upon the loan disbursement , a loan fees of <strong><asp:Label ID="lblLoanFee" runat="server" Text="Loan Fee"></asp:Label></strong> and insurance fees of <strong><asp:Label ID="lblInsFee" runat="server" Text="Insurance Fee"></asp:Label></strong> and a total deduction to be paid <strong><asp:Label ID="lblTotFee" runat="server" Text="Total Fee"></asp:Label></strong>. </li>
                                <li>The client has been granted for and is limited to <strong><asp:Label ID="lblLtdAmnt" runat="server" Text="Ltd Amount"></asp:Label></strong> only. </li>
                                <li>The borrower will repay the loan and an interest in total of <strong><asp:Label ID="lblTotAmnt" runat="server" Text="Total Amt"></asp:Label></strong> UGX Shillings in monthly installements as indicated on the repayment schedule attached. </li>
                                <li>The borrower will use the loan funds for the purpose for which the loan was granted.</li>
                                <li>The borrower will mantain proper books of accounts and other records relating to the utilisation of the loan funds.</li>
                                <li>The borrower will give free such information as may be required concerning the general management of the borrower's source of funds.<br />
                                    The borrower has provided as security the following whose particulars are indicated.<br />
                                    Also the borrower has given rights to the lender to take over the collateral security should the borrower default on the payments as per the agreed repayment schedule and rights of the lender over the security shall
                                    in all forms be uncontestable.
                                    <ul runat="server">
                                        <li>Name of Collateral Security: <strong><asp:Label ID="lblCollateral" runat="server" Text="Collateral"></asp:Label></strong></li>
                                        <li>Details of Collateral Security: <strong><asp:Label ID="lblCollSec" runat="server" Text="Collateral Details"></asp:Label></strong></li>
                                        <li>Guarantor: </li>
                                        <p>
                                        "The Guarantor by signing this agreement on behalf of the borrower shall have the same obligations to fulfill the obligations under this agreement as the borrower.
                                        <br />I <strong><asp:Label ID="lblGuarantor" runat="server"></asp:Label></strong> (Guarantor) of telephone ... of district .... of country ... of village ...
                                        Sign ....... agree to ensure Mr/Mrs/Ms <strong><asp:label runat="server" ID="lblClientName1" Text="Name"></asp:label></strong> (Borrower) pays all his/her loan as per the agreed loan schedule to completion. I hereby agree to produce 
                                        Mr/Mrs/Ms <strong><asp:label runat="server" ID="lblClientName2" Text="Name"></asp:label></strong> to Lensh Microfinance Limited should he/she fail to pay, I agree to settle the entire amount should Mr/Mrs/Ms <strong><asp:label runat="server" ID="lblClientName3" Text="Name"></asp:label></strong> fail to settle their
                                         loan balance as per this agreement".</p>

                                    </ul>
                                </li>
                                <li>The borrower has agreed that if any instalement of the principal sum or interest shall remain unpaid for one instalment after it becomes due, 
                                    the whole of the principal sum with all accrued interest thereon shall immediately become due to payable.
                                </li>
                                <li>In case the client (Borrower) is unable to reach the office or reach any LENSH loan officer the staff to repay the loan or loan installment, the client (borrower) can deposit the money on the office registered mobile numbers (+256) 775-650-413, (+256) 393-225-621,
                                    and specify their names and call to confirm the receipt. In such a case, the client (Borrower) must also add the withdrawal charges 
                                    with respect tothe loan installment they are paying for.
                                </li>
                                <li>Repayment in full of amounts which the borrower owes to Lensh Microfinance Limited as principal interest and any other costs that will have 
                                    been incurred in recovering this money. In addition, if the borrower delays to pay as per repayment schedule, then the total otstanding principal
                                    and interest shall attach a penalty fee of ....... Per day thereafter for each day in default.
                                </li>
                                <li>The terms and conditions of this agreement including the validity of the pledge here by granted by the borrower to Lensh Microfinance Limited shall remain 
                                    in force until the borrower repays in full and the money owed to <strong>Lensh Microfinance Limited</strong>.
                                </li>
                                <li>THE BORROWER HAS ALSO AGREED WITH LENSH MICROFINANCE LIMITED THAT IF THE BORROWER
                                    <ul runat="server">
                                        <li>Fails to observe and perform any of the conditions in this agreement: or</li>
                                        <li>Commits any act showing that he/she lost the liability to pay his/her loan or</li>
                                        <li>Enters into any arrangement with any other company or persons whom he/she owes money which may affect her ability to repay 
                                            the loan, then in any one or more such cases all outstanding monies shall become immediately payable and the Lensh Microfinance Limited may exercise
                                            its rights in the agreement or its powers under any mortage to recover his money. 
                                        </li>
                                        <li>Lensh Microfinance Limited or the borrower may give a written notice to event of any breach of this agreement. Any written notice 
                                            may be given in writing to the borrower by ordinary of registered letter post or by the post and shall be deemed to have been 
                                            served at the expiration of 14(fourteen) days from the time of posting. Notices to the Lensh Microfinance Limited may be delivered 
                                            to the Managing Director of <strong>Lensh Microfinance Limited</strong>.
                                        </li>
                                        <li>The legal fees for the preparation of this agreement and for other legal matters arising in connection there with shall be met by the borrower.</li>
                                        <li>Where the borrower is not able to attach his/her signature, a thumb print will be sufficient evidence that the agreement was fully 
                                            explained to him by the responsible official of <strong>Lensh Microfinance Limited</strong> or by a person nominated by him/her to translate this 
                                            agreement. (NOTE: In case of the later the name of the persona translating should be indicated) 
                                        </li>
                                        </ul>
                                </li>
                            </ul>
                                <p>
                                    <strong>IN WITNESS WHEREOF</strong> the parties have signed this agreement on this ..... day of ....... month ......., year to signify that they all agree 
                                    to all the terms of this agreement.
                                    Name:......................................... Signature:...................................... <br />
                                    <strong>The Borrower</strong> <br />
                                    Name:..............................<br />
                                    Signature:.........................<br />
                                    Telephone:......................... 
                                </p>
                                <p><asp:CheckBox ID="chckAgreeTc" runat="server"/> I agree to the Terms and Conditions specified in the Agreement above.</p>
                            </div>
                        </div>
                            
                        
		            </div>
                        <div class="modal-footer">
                            <asp:Button ID="btnAcceptTc" runat="server" CssClass="btn btn-success" Text="Continue" OnClick="btnAcceptTc_Click"/>
                            <asp:Button ID="btnCancel" runat="server" CssClass="btn btn-danger" Text="Cancel" OnClick="btnCancel_Click"/>
                        </div>
                        </div>
                    </asp:View>
        </asp:MultiView>
            </div>
        </div>
    </div>
    </div>
</div>
    <asp:Label ID="lblCode" runat="server" Text="0" Visible="False"></asp:Label>
    <asp:Label ID="lblUsername" runat="server" Text="0" Visible="False"></asp:Label><br />
    &nbsp;

</asp:Content>







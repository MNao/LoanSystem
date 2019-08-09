<%@ Page Title="" Language="C#" MasterPageFile="~/MasterMain.master" AutoEventWireup="true" CodeFile="AddClientDetails.aspx.cs" Inherits="AddClientDetails" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<%@ Register assembly="CrystalDecisions.Web, Version=13.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" namespace="CrystalDecisions.Web" tagprefix="CR" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<ajaxToolkit:ToolkitScriptManager runat="Server" EnableScriptGlobalization="true"
        EnableScriptLocalization="true" ID="ScriptManager1" />
<%--<!DOCTYPE html>
<html>
         <script type="text/javascript">
        function printDocument() {
            document.getElementById('Button3').style.visibility = 'hidden';
            document.getElementById('Button4').style.visibility = 'hidden';
            //document.getElementById('MsgDiv').style.visibility = 'hidden';
            window.print();
            document.getElementById('Button3').style.visibility = 'visible';
            document.getElementById('Button4').style.visibility = 'visible';
        }
    </script>
    <body>
        <div class="container" style="padding: 10px;">
            </div>
        </body>
     </html>--%>

<div class="col-lg-12">
    <div class="card mb-3">
        <div class="card-header">
        <i class="fa fa fa-group"></i> Client Details <i class='fa fa-arrow-right'></i> Add Client
        </div>
        <div class="card-body">
            <div class="row clearfix">
<asp:MultiView ID="MultiView1" ActiveViewIndex="0" runat="server">
    <asp:View ID="CaptureDetailsView" runat="server">


        <div class="modal-content col-md-8  col-sm-8 col-xs-10"  style="margin:0 auto;">
                   
		            <div class="modal-body">
                        
                        Client Number
                      <asp:TextBox ID="txtClientNo" runat="server" CssClass="form-control"></asp:TextBox>
                        <p class="help-block"></p>
                        <div class="row">
                                        <div class="col-lg-6">
                                            <label>Client Name</label>
                                            <asp:TextBox ID="txtName" runat="server" CssClass="form-control" placeholder="Enter Client Name"></asp:TextBox>
                                            <p class="help-block"></p>
                                        </div>
                                        <div class="col-lg-6">
                                            <label>Client PhoneNo</label>
                                            <asp:TextBox ID="txtPhoneNo" runat="server" CssClass="form-control" placeholder="Enter Phone Number" />
                                            <p class="help-block"></p>
                                        </div>
                                    </div>
                        <div class="row">
                                        <div class="col-lg-6">
                                            <label>Referee Name</label>
                                            <asp:TextBox ID="txtReferee" runat="server" CssClass="form-control" placeholder="Enter Referee Name"></asp:TextBox>
                                            <p class="help-block"></p>
                                        </div>
                                        <div class="col-lg-6">
                                            <label>Referee PhoneNo</label>
                                            <asp:TextBox ID="txtRefereePhone" runat="server" CssClass="form-control" placeholder="Enter Referee Phone Number" />
                                            <p class="help-block"></p>
                                        </div>
                        </div>
                      <%--Telephone
                      <asp:TextBox ID="txtphone" runat="server" CssClass="form-control"></asp:TextBox>--%>

                      <%--Vendor
                      <asp:DropDownList ID="ddlAreas" runat="server" OnDataBound="ddlAreas_DataBound" CssClass="form-control">
                      </asp:DropDownList>--%>

                      <div class="row">
                                        <div class="col-lg-6">
                                            <label>Client  Email</label>
                                            <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" placeholder="Enter Client Email Address" />
                                            <p class="help-block"></p>
                                        </div>
                                        <div class="col-lg-6">
                                            <label>Gender</label>
                                            <asp:DropDownList ID="ddGender" runat="server" CssClass="form-control">
                                                <asp:ListItem Value="">--Select Client Gender--</asp:ListItem>
                                                <asp:ListItem Value="Male">MALE</asp:ListItem>
                                                <asp:ListItem Value="Female">FEMALE</asp:ListItem>
                                            </asp:DropDownList>
                                           <p class="help-block"></p>
                                        </div>
                                    </div>

                        <div class="row">
                                        <div class="col-lg-6">
                                            <label>ID Type</label>
                                            <asp:DropDownList ID="ddIDType" runat="server" CssClass="form-control">
                                                <asp:ListItem Value="">--Select ID Type--</asp:ListItem>
                                                <asp:ListItem Value="National">National ID</asp:ListItem>
                                                <asp:ListItem Value="Passport">Passport</asp:ListItem>
                                                <asp:ListItem Value="Permit">Driving Permit</asp:ListItem>
                                            </asp:DropDownList>
                                            <p class="help-block"></p>
                                        </div>
                                        <div class="col-lg-6">
                                            <label>ID Number</label>
                                            <asp:TextBox ID="txtIDNo" Style="z-index: 10000;" runat="server" CssClass="form-control" placeholder="Enter ID No" />
                                            <p class="help-block"></p>
                                        </div>
                                    </div>
                        <div class="row">
                                        <div class="col-lg-6">
                                            <label>Client Photo</label>
                                            <asp:FileUpload ID="ClientPhoto" runat="server" CssClass="form-control"/>
                                            <p class="help-block"></p>
                                        </div>
                                        <div class="col-lg-6">
                                            <label>Client ID Photo</label>
                                            <asp:FileUpload ID="IDPhoto" runat="server" CssClass="form-control"/>
                                            <p class="help-block"></p>
                                        </div>
                                    </div>
                        <div class="row" id="Reason" runat="server">
                                        <div class="col-lg-6">
                                            <label>Reason for Rejection</label>
                                            <asp:TextBox ID="txtReasonforRejection" TextMode="MultiLine" runat="server" CssClass="form-control" placeholder="Enter Reason" />
                                            <p class="help-block"></p>
                                        </div>
                                    </div>

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
                        <div class="row">
                        <asp:Button ID="btnSubmit" runat="server" Text="Submit Details" Width="200px" CssClass="btn btn-success btn-lg" OnClick="btnSubmit_Click" />
                        <asp:Button ID="btnEdit" runat="server" Text="Approve Client" Width="200px" CssClass="btn btn-success btn-lg" OnClick="btnEdit_Click" />
                        <asp:Button ID="btnReject" runat="server" Text="Reject" Width="200px" CssClass="btn btn-danger btn-lg" OnClick="btnReject_Click"/>
                        <asp:Button ID="btnRejectWithReason" runat="server" Text="Reject" Width="200px" CssClass="btn btn-danger btn-lg" OnClick="btnRejectWithReason_Click"/>
                        </div>
                    </div>
                </div>


        <div id="page-wrapper">

            <div class="container-fluid">

                <div class="row">

                    <%------------------------------------------ Message Label ----------------------------------------%>
                    <div class="row">
                        <div class="col-lg-1"></div>
                        <div class="col-lg-10 text-center">
                            <% 
                                string IsError = Session["IsError"] as string;
                                if (IsError == null)
                                {
                                    Response.Write("<div>");

                                }
                                else if (IsError == "True")
                                {
                                    Response.Write("<div class=\"alert alert-danger\">");

                                }
                                else
                                {
                                    Response.Write("<div class=\"alert alert-success\">");
                                }
                            %>
                            <strong>
                                <asp:Label ID="lblmsg" runat="server"></asp:Label>
                            </strong>
                            <%Response.Write("</div>"); %>
                        </div>
                        <div class="col-lg-1"></div>
                    </div>

                    <%--<div class="container">
                        <div class="col-lg-2"></div>

                        <div class="col-lg-8">

                            <div class="card-footer">
                                <div class="text-center">
                                </div>
                            </div>
                            </div>
                            <div class="col-lg-2"></div>
                        </div>--%>
                    <div class="col-lg-2"></div>
                </div>
            </div>
        </div>

<%--        <ajaxToolkit:ToolkitScriptManager ID="ScriptManager1" runat="Server" EnableScriptGlobalization="true"
            EnableScriptLocalization="true">
        </ajaxToolkit:ToolkitScriptManager>
        <br />--%>
        <%--<ajaxToolkit:CalendarExtender ID="CalendarExtender1" runat="server" CssClass="cal_Theme1"
            Format="yyyy-MM-dd" PopupPosition="BottomRight" TargetControlID="txtxInvoiceDate">
        </ajaxToolkit:CalendarExtender>--%>

          <script type="text/javascript">
            function CalculateSubtotal() {
                try {
                    var SaleAmount = $(".total-subtotal").val();
                    var TaxAmount = $(".tax-amount").val();
                    var AnyOtherTax = $(".any-other-tax").val();
                    var DiscountAmount = $(".discount-amount").val();
                    var result = parseFloat(removeCommas(SaleAmount)) + parseFloat(removeCommas(TaxAmount)) + parseFloat(removeCommas(AnyOtherTax)) - parseFloat(removeCommas(DiscountAmount));
                    $(".total-sale-amount").val(Comma(result));
                }
                catch (err) {
                    alert("EXCEPTION rasied: ");
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

    <%------------------------------------------------------------ Confirm View ------------------------------- --%>
    <asp:View ID="ConfirmView" runat="server">
        <div class="container">
            <div class="text-center">
                <div class="row" style="padding-top: 70px;">
                    <div class="col-lg-2"></div>
                    <div class="col-lg-8">
                        <div class="alert alert-info">
                            Are You Sure You Want to be Paid to a Different Currency Account.
                            If Yes Click Continue and If NO, Click Cancel and Select Same Currency Account.
                        </div>
                    </div>
                    <div class="col-lg-2"></div>
                </div>
                <hr />
                <div class="row">
                    <asp:Button ID="btnConfirm" runat="server" CssClass="btn btn-success" Text="Continue" />
                    <asp:Button ID="btnCancel" runat="server" CssClass="btn btn-danger" Text="Cancel" />
                </div>
            </div>
            <hr />
        </div>
    </asp:View>
    <asp:View ID="reportview" runat="server">
        <div class="container">
            <div class="text-center">
                 <div class="col-lg-8">
                       <%-- Print Button --%>
            <div class="row" style="padding: 10px;">
                <div class="text-center">
                    <input id="Button3" accesskey="P" class="btn btn-success" onclick="printDocument()"
                        value="Print Invoice" />
                    <a href="LoggedInStartPage.aspx">
                        <input id="Button4" accesskey="P" class="btn btn-primary"
                            value="Return" /></a>
                </div>
            </div>
                    
        <CR:CrystalReportViewer ID="CrystalReportViewer1" runat="server" AutoDataBind="True" GroupTreeImagesFolderUrl="" Height="1174px" ReportSourceID="InvoiceReport" ToolbarImagesFolderUrl="" ToolPanelWidth="220px" Width="868px" EnableTheming="True" HasDrilldownTabs="False" HasDrillUpButton="False" HasGotoPageButton="False" HasPageNavigationButtons="False" HasSearchButton="False" HasToggleGroupTreeButton="False" HasToggleParameterPanelButton="False" HasZoomFactorList="False" ToolPanelView="None" BorderStyle="None" EnableToolTips="False" HasCrystalLogo="False" />
        <CR:CrystalReportSource ID="InvoiceReport" runat="server">
            <Report FileName="Bin\reports\PegInvoice1.rpt">
            </Report>
        </CR:CrystalReportSource>
                     </div>
                </div>
            </div>
    </asp:View>
</asp:MultiView>
                </div>
        </div>
    </div>
</div>

</asp:Content>



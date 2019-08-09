<%@ Page Title="" Language="C#" MasterPageFile="~/MasterMain.master" AutoEventWireup="true" CodeFile="AddInjection.aspx.cs" Inherits="AddInjection" %>

<%@ Register 
 Assembly="AjaxControlToolkit" 
 Namespace="AjaxControlToolkit" 
 TagPrefix="ajaxToolkit" %>


<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">


    <ajaxToolkit:ToolkitScriptManager runat="Server" EnableScriptGlobalization="true"
        EnableScriptLocalization="true" ID="ScriptManager1" />

<div class="col-lg-12">
    <div class="card mb-3">
        <div class="card-header">
        <i class="fa fa fa-cog"></i> Loans <i class='fa fa-arrow-right'></i> RePay Loan
        </div>
        <div class="card-body">
            <div class="row clearfix">
            <asp:MultiView ID="MultiView1" runat="server" ActiveViewIndex="0">
                <asp:View ID="View1" runat="server">
                <div class="modal-content col-md-6  col-sm-6 col-xs-10"  style="margin:0 auto;">
                   
		            <div class="modal-body">

                        Client Number
                      <asp:TextBox ID="txtClientNo" runat="server" CssClass="form-control"  placeholder="Client Number"></asp:TextBox>

                      Client Name
                      <asp:TextBox ID="txtfname" runat="server" CssClass="form-control"  placeholder="Client Name"></asp:TextBox>
                       
                        Loan Number
                      <asp:TextBox ID="txtLoanNo" runat="server" CssClass="form-control"  placeholder="Loan Number"></asp:TextBox>

                        Amount Paid
                      <asp:TextBox ID="txtBusLoc" runat="server" CssClass="form-control"  placeholder="Loan Amount Paid"></asp:TextBox>

                      Repayment Date
                      <asp:TextBox ID="txtRepaymentDate" runat="server" CssClass="form-control" placeholder="Repayment Date"></asp:TextBox>
                        
                      Mobile No. Own
                      <asp:TextBox ID="txtMobileNo" runat="server" CssClass="form-control"  placeholder="Client's Own Mobile Number"></asp:TextBox>

                      Email
                      <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control"  placeholder="Client's Email"></asp:TextBox>
                        
                      <%--Vendor
                      <asp:DropDownList ID="ddlAreas" runat="server" OnDataBound="ddlAreas_DataBound" CssClass="form-control">
                      </asp:DropDownList>--%>


                      <%--<br/><asp:CheckBox ID="chkActive" runat="server" Text=" Is Active" /><br/>--%>

                    </div>
                    <div class="modal-footer">
                        <asp:Button ID="btnSubmit" runat="server" CssClass="btn btn-success" Text="SUBMIT DETAILS" />
                    </div>
                </div>
                    
                    <ajaxToolkit:CalendarExtender ID="CalendarExtender2" runat="server" CssClass="cal_Theme1"
            Format="yyyy-MM-dd" PopupPosition="BottomRight" TargetControlID="txtRepaymentDate">
        </ajaxToolkit:CalendarExtender>
            </asp:View>

        </asp:MultiView>
            </div>
        </div>
    </div>
</div>
    <asp:Label ID="lblCode" runat="server" Text="0" Visible="False"></asp:Label>
    <asp:Label ID="lblUsername" runat="server" Text="0" Visible="False"></asp:Label><br />
    &nbsp;

</asp:Content>









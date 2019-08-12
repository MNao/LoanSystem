<%@ Page Title="" Language="C#" MasterPageFile="~/MasterMain.master" AutoEventWireup="true" CodeFile="AddExpense.aspx.cs" Inherits="AddExpense" %>

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
        <i class="fa fa fa-cog"></i> Expense <i class='fa fa-arrow-right'></i> Add Expense
        </div>
        <div class="card-body">
            <div class="row clearfix">
            <asp:MultiView ID="MultiView1" runat="server" ActiveViewIndex="0">
                <asp:View ID="View1" runat="server">
                <div class="modal-content col-md-6  col-sm-6 col-xs-10"  style="margin:0 auto;">
                   
		            <div class="modal-body">

                        Expense Number
                      <asp:TextBox ID="txtExpNo" runat="server" CssClass="form-control"  placeholder="Expense Number"></asp:TextBox>

                        Amount Paid
                      <asp:TextBox ID="txtAmount" runat="server" CssClass="form-control"  placeholder="Expense Amount Paid"></asp:TextBox>

                      Expense Date
                      <asp:TextBox ID="txtExpDate" runat="server" CssClass="form-control" placeholder="Expense Date"></asp:TextBox>
                        
                      Expense Description
                      <asp:TextBox ID="txtExpDesc" runat="server" CssClass="form-control"  placeholder="Expense Description"></asp:TextBox>

                      Expense Type
                      <asp:TextBox ID="txtExpType" runat="server" CssClass="form-control"></asp:TextBox>
                       
                        Expense Receipt
                      <asp:TextBox ID="txtReceipt" runat="server" CssClass="form-control"></asp:TextBox>


                      <%--<br/><asp:CheckBox ID="chkActive" runat="server" Text=" Is Active" /><br/>--%>

                    </div>
                    <div class="modal-footer">
                        <asp:Button ID="btnSubmit" runat="server" CssClass="btn btn-success" Text="SUBMIT DETAILS" OnClick="btnSubmit_Click"/>
                        <asp:Button ID="btnEdit" runat="server" CssClass="btn btn-success" Text="UPDATE DETAILS" OnClick="btnEdit_Click"/>
                    </div>
                </div>
                    
                    <ajaxToolkit:CalendarExtender ID="CalendarExtender2" runat="server" CssClass="cal_Theme1"
            Format="yyyy-MM-dd" PopupPosition="BottomRight" TargetControlID="txtExpDate">
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







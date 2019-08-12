<%@ Page Title="" Language="C#" MasterPageFile="~/MasterMain.master" AutoEventWireup="true" CodeFile="AddIncome.aspx.cs" Inherits="AddIncome" %>

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
        <i class="fa fa fa-cog"></i> Income <i class='fa fa-arrow-right'></i> Add Income
        </div>
        <div class="card-body">
            <div class="row clearfix">
            <asp:MultiView ID="MultiView1" runat="server" ActiveViewIndex="0">
                <asp:View ID="View1" runat="server">
                <div class="modal-content col-md-6  col-sm-6 col-xs-10"  style="margin:0 auto;">
                   
		            <div class="modal-body">

                        Income Number
                      <asp:TextBox ID="txtIncomeNo" runat="server" CssClass="form-control"  placeholder="Income Number"></asp:TextBox>

                        Amount Paid
                      <asp:TextBox ID="txtAmount" runat="server" CssClass="form-control"  placeholder="Income Amount Paid"></asp:TextBox>

                      Income Date
                      <asp:TextBox ID="txtIncomeDate" runat="server" CssClass="form-control" placeholder="Income Date"></asp:TextBox>
                        
                      Income Description
                      <asp:TextBox ID="txtIncomeDesc" runat="server" CssClass="form-control"  placeholder="Income Description"></asp:TextBox>

                      Income Type
                      <asp:TextBox ID="txtIncType" runat="server" CssClass="form-control"></asp:TextBox>
                        
                      <%--Vendor
                      <asp:DropDownList ID="ddlAreas" runat="server" OnDataBound="ddlAreas_DataBound" CssClass="form-control">
                      </asp:DropDownList>--%>


                      <%--<br/><asp:CheckBox ID="chkActive" runat="server" Text=" Is Active" /><br/>--%>

                    </div>
                    <div class="modal-footer">
                        <asp:Button ID="btnSubmit" runat="server" CssClass="btn btn-success" Text="SUBMIT DETAILS" OnClick="btnSubmit_Click"/>
                        <asp:Button ID="btnEdit" runat="server" CssClass="btn btn-success" Text="UPDATE DETAILS" OnClick="btnEdit_Click"/>
                    </div>
                </div>
                    
                    <ajaxToolkit:CalendarExtender ID="CalendarExtender2" runat="server" CssClass="cal_Theme1"
            Format="yyyy-MM-dd" PopupPosition="BottomRight" TargetControlID="txtIncomeDate">
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








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
        <i class="fa fa fa-cog"></i> Injection <i class='fa fa-arrow-right'></i> Add Injection
        </div>
        <div class="card-body">
            <div class="row clearfix">
            <asp:MultiView ID="MultiView1" runat="server" ActiveViewIndex="0">
                <asp:View ID="View1" runat="server">
                <div class="modal-content col-md-6  col-sm-6 col-xs-10"  style="margin:0 auto;">
                   
		            <div class="modal-body">

                        Injection Number
                      <asp:TextBox ID="txtInjectionNo" runat="server" CssClass="form-control"  placeholder="Injection Number"></asp:TextBox>

                      Name
                      <asp:TextBox ID="txtname" runat="server" CssClass="form-control"  placeholder="Injector Name"></asp:TextBox>
                       
                        Amount Injected
                      <asp:TextBox ID="txtInjectedAmount" runat="server" CssClass="form-control"  placeholder="Injected Amount"></asp:TextBox>

                      Injection Date
                      <asp:TextBox ID="txtInjectionDate" runat="server" CssClass="form-control" placeholder="Injection Date"></asp:TextBox>
                        
                      Injector's Phone Number
                      <asp:TextBox ID="txtPhoneNo" runat="server" CssClass="form-control"  placeholder="Injector's Phone Number"></asp:TextBox>

                      Injector's Email
                      <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control"  placeholder="Injector's Email"></asp:TextBox>
                        
                        Repayment Date
                      <asp:TextBox ID="txtInjRepayDate" runat="server" CssClass="form-control"  placeholder="Expected Repayment Date"></asp:TextBox>

                        Repayment Amount
                      <asp:TextBox ID="txtInjRepayAmnt" runat="server" CssClass="form-control"  placeholder="Expected Repayment Amount"></asp:TextBox>


                      <%--<br/><asp:CheckBox ID="chkActive" runat="server" Text=" Is Active" /><br/>--%>

                    </div>
                    <div class="modal-footer">
                        <asp:Button ID="btnSubmit" runat="server" CssClass="btn btn-success" Text="SUBMIT DETAILS" onclick="btnSubmit_Click"/>
                        <asp:Button ID="btnEdit" runat="server" CssClass="btn btn-success" Text="EDIT" OnClick="btnEdit_Click"/>
                    </div>
                </div>
                    
                    <ajaxToolkit:CalendarExtender ID="CalendarExtender2" runat="server" CssClass="cal_Theme1"
            Format="yyyy-MM-dd" PopupPosition="BottomRight" TargetControlID="txtInjectionDate">
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









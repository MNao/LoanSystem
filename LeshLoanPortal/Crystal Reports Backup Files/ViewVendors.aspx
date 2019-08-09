<%@ Page Language="C#" MasterPageFile="~/MasterMain.master" AutoEventWireup="true" CodeFile="ViewVendors.aspx.cs" Inherits="ViewVendors" Title="ViewVendors" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%--<%@ Register 
         Assembly="AjaxControlToolkit" 
         Namespace="AjaxControlToolkit" 
         TagPrefix="ajaxToolkit" %>
         <ajaxToolkit:ToolkitScriptManager runat="Server" EnableScriptGlobalization="true"
        EnableScriptLocalization="true" ID="ScriptManager1" />--%>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

<div class="col-lg-12">
    <div class="card mb-3">
        <div class="card-header">
        <i class="fa fa fa-cog"></i> System Tools <i class='fa fa-arrow-right'></i> View Vendors
        </div>
        <div class="card-body">
            <div class="row clearfix" style="overflow-x:auto;">

            <div class="col-md-4">
                        <label>Vendor</label>
                        <asp:TextBox ID="txtVendor" runat="server" CssClass="form-control "></asp:TextBox>
                    </div>

                    <div class="col-md-2">
                        <label>Serach..</label><br/>
                        <asp:Button ID="btnOK" runat="server" CssClass="btn btn-success" OnClick="btnOK_Click" Text="Search"/>
                    </div>

               <asp:MultiView ID="MultiView1" runat="server" ActiveViewIndex="0">
                                <asp:View ID="View1" runat="server">
                                   

                        <%--<asp:DataGrid ID="DataGrid1" runat="server" AllowPaging="True" AutoGenerateColumns="True"
                                UseAccessibleHeader="true" GridLines="None" CssClass="table table-striped table-hover" 
                                    HorizontalAlign="Justify" OnItemCommand="DataGrid1_ItemCommand"
                                OnPageIndexChanged="DataGrid1_PageIndexChanged" style="white-space:nowrap; margin-top:10px;">
                                <FooterStyle BackColor="InactiveCaption"   ForeColor="White" />
                                <PagerStyle  ForeColor="#4380B8"  HorizontalAlign="Center" Mode="NumericPages"  Font-Size="16"/>
                                    <Columns>
                                    <asp:ButtonColumn CommandName="btnEdit" HeaderText="Edit" Text="Area" DataTextField="VendorCode">
                                        
                                    </asp:ButtonColumn>
                                    
                                </Columns>
                                
                            </asp:DataGrid>--%>

                                </asp:View>
                        </asp:MultiView>

            </div>
        </div>
    </div>
</div>
</asp:Content>
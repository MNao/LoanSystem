﻿<%@ Page Title="" Language="C#" MasterPageFile="~/MasterMain.master" AutoEventWireup="true" CodeFile="ViewInjections.aspx.cs" Inherits="ViewInjections" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

<div class="col-lg-12">
   
    <%--<div class="card mb-3">
        <div class="card-header">
        <i class="fa fa fa-envelope"></i> Verification <i class='fa fa-arrow-right'></i> Verify KYC Details
        <a  href="Send_SMS_Template.csv" class="pull-right"> <i class='fa fa-file-excel-o'></i> Template</a>
        
        </div>
        <div class="card-body">
    
    </div>
</div>--%>

<asp:MultiView ID="MultiView1" ActiveViewIndex="0" runat="server">
    <asp:View ID="SearchView" runat="server">
        <%-------------------------------------------- Message Label ----------------------------------%>
               <!---------------------------------------------- Search Options --------------------------------->
        <div class="row">
            <div class="col-lg-3">
                <label>
                    Injector Name
                </label>
                <asp:TextBox ID="txtInjectorNo" runat="server" CssClass="form-control"></asp:TextBox>
            </div>
            <div class="col-lg-2">
                <label>
                    Start Date
                </label>
                <asp:TextBox ID="txtStartDate" runat="server" CssClass="form-control" placeholder="Select Date" />
            </div>
            <div class="col-lg-2">
                <label>
                    End Date
                </label>
                <asp:TextBox ID="txtEndDate" runat="server" CssClass="form-control" placeholder="Select Date"/>
            </div>
            <div class="col-lg-2" style="padding-top: 15px;">
            
                <asp:Button ID="btnSubmit" runat="server" Text="Search DB" CssClass="btn btn-success btn-lg"
                    OnClick="btnSubmit_Click" />
            </div>
        </div>
        <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="Server" EnableScriptGlobalization="true"
            EnableScriptLocalization="true">
        </ajaxToolkit:ToolkitScriptManager>
        <br />
        <ajaxToolkit:CalendarExtender ID="CalendarExtender1" runat="server" CssClass="cal_Theme1"
            Format="yyyy-MM-dd" PopupPosition="BottomRight" TargetControlID="txtStartDate">
        </ajaxToolkit:CalendarExtender>
        <ajaxToolkit:CalendarExtender ID="CalendarExtender2" runat="server" CssClass="cal_Theme1"
            Format="yyyy-MM-dd" PopupPosition="BottomRight" TargetControlID="txtEndDate">
        </ajaxToolkit:CalendarExtender>
        <hr />
        <%------------------------------------------- Search Results  -----------------------------------%>
        <asp:MultiView runat="server" ID="MultiView2" ActiveViewIndex="0">
            <asp:View runat="server" ID="resultView">
                <div class="row" style="overflow-x:auto;">
                    <div class="table-responsive">
                        <asp:GridView runat="server" Width="100%" CssClass="table table-bordered table-hover"
                            ID="dataGridResults" AutoGenerateColumns="true">
                            <AlternatingRowStyle BackColor="#FFF9FB" />
                            <HeaderStyle BackColor="#E44B4B" Font-Bold="false" ForeColor="white" Font-Italic="False"
                                Font-Overline="False" Font-Strikeout="False" Font-Underline="False" Height="30px" />
                            <Columns>
                                <%--<asp:TemplateField HeaderText="Details">
                                    <ItemTemplate>
                                        <asp:Button ID="btnMakeASale" runat="server" Text="Download KYC" CommandName="Download" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" />
                                    </ItemTemplate>
                                </asp:TemplateField>--%>
                            </Columns>
                        </asp:GridView>
                    </div>
                </div>
            </asp:View>
            <asp:View runat="server" ID="EmptyView">
            </asp:View>
        </asp:MultiView>

        <!------------------------------------------------- View2 -------------------------------------------------->
    </asp:View>
    <asp:View ID="View2" runat="server">
    </asp:View>
</asp:MultiView>
</div>
<script type="text/javascript">
    function count(clientId) {
        var txtMessage = document.getElementById(clientId);
        var spanDisplay = document.getElementById('spanDisplay');
        spanDisplay.innerHTML = txtMessage.value.length;
    }
</script>
</asp:Content>





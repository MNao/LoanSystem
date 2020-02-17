<%@ Page Title="" Language="C#" MasterPageFile="~/MasterMain.master" AutoEventWireup="true" CodeFile="ViewLoans.aspx.cs" Inherits="ViewLoans" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<%--<%@ Register 
 Assembly="AjaxControlToolkit" 
 Namespace="AjaxControlToolkit" 
 TagPrefix="ajaxToolkit" %>--%>
 <%--<asp:ScriptManager runat="Server" EnableScriptGlobalization="true"
        EnableScriptLocalization="true" ID="ScriptManager1" 
        />--%>
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
                    Client Number/Name
                </label>
                <asp:TextBox ID="txtSearch" runat="server" ClientIdMode="Static" placeholder="Search" CssClass="form-control"></asp:TextBox>
            </div>
            <div class="col-lg-2">
                <label>
                    Status
                </label>
                <asp:DropDownList ID="ddStatus" runat="server" CssClass="form-control">
                    <asp:ListItem Value="">--Select Status--</asp:ListItem>
                    <asp:ListItem Value="PENDING">PENDING</asp:ListItem>
                     <asp:ListItem Value="VERIFIED">VERIFIED</asp:ListItem>
                    <asp:ListItem Value="APPROVED">APPROVED</asp:ListItem>
                     <asp:ListItem Value="REJECTED">REJECTED</asp:ListItem>
                </asp:DropDownList>
            </div>
            <div class="col-lg-2">
                <label>
                    Start Date
                </label>
                <asp:TextBox ID="txtStartDate" runat="server" CssClass="form-control" AutoComplete="off" placeholder="Select Date" />
            </div>
            <div class="col-lg-2">
                <label>
                    End Date
                </label>
                <asp:TextBox ID="txtEndDate" runat="server" CssClass="form-control" AutoComplete="off" placeholder="Select Date"/>
            </div>
            <div class="col-lg-2" style="padding-top: 30px;">
                
                <asp:Button ID="btnSubmit" runat="server" Text="Search DB" CssClass="btn btn-success btn-md"
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
                            ID="dataGridResults" AutoGenerateColumns="true" OnRowCommand="dataGridResults_RowCommand" OnRowDataBound="dataGridResults_RowDataBound"> <%-- OnRowCreated="dataGridResults_RowCreated" arBFE4FF hc115E9B  --%>
                            <AlternatingRowStyle BackColor="#FFF9FB" /> <%--DE6868--%>
                            <HeaderStyle BackColor="#E44B4B" Font-Bold="false" ForeColor="white" Font-Italic="False"
                                Font-Overline="False" Font-Strikeout="False" Font-Underline="False" Height="30px" />
                            <Columns>
                               <asp:TemplateField HeaderText="View Payments">
                                    <ItemTemplate>
                                        <i style="color:dodgerblue;"><a class="fa fa-money"><asp:Button runat="server" ID="btnViewPayments" Text="View Payments" ForeColor="dodgerblue" CommandName="ViewPayments" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" BackColor="WhiteSmoke" BorderStyle="None"></asp:Button></a></i>
                                        
                                        <%--<asp:Button ID="btnViewPayments" runat="server" Text="View Payments" CommandName="ViewPayments" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" />--%>
                                    </ItemTemplate>
                                </asp:TemplateField>
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




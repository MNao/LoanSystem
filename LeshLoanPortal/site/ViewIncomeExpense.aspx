<%@ Page Title="View Reports" Language="C#" MasterPageFile="~/MasterMain.master" AutoEventWireup="true" CodeFile="ViewIncomeExpense.aspx.cs" Inherits="ViewIncomeExpense" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<div class="col-lg-12">

<asp:MultiView ID="MultiView1" ActiveViewIndex="0" runat="server">
    <asp:View ID="SearchView" runat="server">
        <%-------------------------------------------- Message Label ----------------------------------%>
               <!---------------------------------------------- Search Options --------------------------------->
        <div class="row">
            <div class="col-lg-2">
                <label>
                    Company
                </label>
                <asp:DropDownList ID="ddCompany" runat="server" CssClass="form-control">
                    <asp:ListItem Value="Lesh">LENSH</asp:ListItem>
                </asp:DropDownList>
            </div>
            <div class="col-lg-2">
                <label>
                    Type
                </label>
                <asp:DropDownList ID="ddType" runat="server" CssClass="form-control">
                    <asp:ListItem Value="">--Select Type--</asp:ListItem>
                    <asp:ListItem Value="Income">INCOME</asp:ListItem>
                     <asp:ListItem Value="Expense">EXPENSE</asp:ListItem>
                    <asp:ListItem Value="IncomeExpense">INCOME-EXPENSE STATEMENT</asp:ListItem>
                </asp:DropDownList>
            </div>
            <div class="col-lg-2">
                <label>
                    Search No
                </label>
                <asp:TextBox ID="txtSearchNo" runat="server" CssClass="form-control" placeholder="Enter text" />
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
            
                <asp:Button ID="btnSubmit" runat="server" Text="Search" CssClass="btn btn-success btn-lg"
                    OnClick="btnSubmit_Click" />
            </div>
            <div class="col-lg-2" style="padding-top: 15px;">
            
                <asp:Button ID="btnExport" runat="server" Text="Export" CssClass="btn btn-success btn-lg"
                    OnClick="btnExport_Click" />
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
                            ID="dataGridResults" AutoGenerateColumns="true" OnRowCommand="dataGridResults_RowCommand" ><%--OnRowCreated="dataGridResults_RowCreated" arBFE4FF hc115E9B  --%>
                            <AlternatingRowStyle BackColor="#FFF9FB" /> <%--DE6868--%>
                            <HeaderStyle BackColor="#E44B4B" Font-Bold="false" ForeColor="white" Font-Italic="False"
                                Font-Overline="False" Font-Strikeout="False" Font-Underline="False" Height="30px" />
                            <Columns>
                                <asp:TemplateField HeaderText="Actions">
                                    <ItemTemplate>
                                       <%-- <i><a class="fa fa-edit" href="AddIncome.aspx"> Edit</a></i>--%>
                                        <i style="color:dodgerblue;"><a class="fa fa-edit"> <asp:Button runat="server" ID="btnEdit" Text="Edit" ForeColor="dodgerblue" CommandName="EditRecord" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" BackColor="WhiteSmoke" BorderStyle="None"></asp:Button></a></i> |
                                        <i style="color:dodgerblue;"><a class="fa fa-trash"> <asp:Button runat="server" ID="btnDelete" Text="Delete" CommandName="DeleteRecord" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" ForeColor="dodgerblue" BackColor="WhiteSmoke" BorderStyle="None"></asp:Button></a></i>
                                        
                                        <%--<asp:Button ID="btnMakeASale" runat="server" Text="Download KYC" CommandName="Download" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" />--%>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>
                </div>
            </asp:View>
            <asp:View runat="server" ID="ConfirmView">
                <div class="container">
            <div class="text-center">
                <div class="row" style="padding-top: 30px;">
                    <div class="col-lg-2"><asp:Label ID="lblID" runat="server"></asp:Label></div>
                    <div class="col-lg-8">
                        <div class="alert alert-info">
                            You are about to Delete a Record !! Are you sure you want to Proceed?.
                        </div>
                    </div>
                    <div class="col-lg-2"></div>
                </div>
                <hr />
                <div class="row" style="justify-content:center">
                    <asp:Button ID="btnConfirm" runat="server" CssClass="btn btn-success" Text="Confirm Operation" OnClick="btnConfirm_Click" />
                    <asp:Button ID="btnCancel" runat="server" CssClass="btn btn-danger" Text="Cancel Operation" OnClick="btnCancel_Click" />
                </div>
            </div>
            <hr />
        </div>
            </asp:View>
            <asp:View ID="EmptyView" runat="server"></asp:View>
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





<%@ Page Language="C#" MasterPageFile="~/MasterMain.master" AutoEventWireup="true" CodeFile="ViewKYCDetails.aspx.cs" Inherits="ViewKYCDetails" Title="SMS SENDING PANEL" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<%--<%@ Register 
 Assembly="AjaxControlToolkit" 
 Namespace="AjaxControlToolkit" 
 TagPrefix="ajaxToolkit" %>--%>
 <asp:ScriptManager runat="Server" EnableScriptGlobalization="true"
        EnableScriptLocalization="true" ID="ScriptManager1" 
        />
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
                    Bank
                </label>
                <asp:DropDownList ID="ddBank" runat="server" CssClass="form-control">
                    <asp:ListItem>True</asp:ListItem>
                    <asp:ListItem>False</asp:ListItem>
                </asp:DropDownList>
            </div>
            <div class="col-lg-3">
                <label>
                    Agent
                </label>
                <asp:DropDownList ID="ddAgents" runat="server" CssClass="form-control">
                    <asp:ListItem>True</asp:ListItem>
                    <asp:ListItem>False</asp:ListItem>
                </asp:DropDownList>
                <%--<asp:TextBox ID="txtName" runat="server" CssClass="form-control" placeholder="Enter text" />--%>
            </div>
            <div class="col-lg-3" style="padding-top: 15px;">
            
                <asp:Button ID="btnSubmit" runat="server" Text="Search DB" CssClass="btn btn-success btn-lg"
                    OnClick="btnSubmit_Click" />
            </div>
        </div>

        <hr />
        <%------------------------------------------- Search Results  -----------------------------------%>
        <asp:MultiView runat="server" ID="Multiview2">
            <asp:View runat="server" ID="resultView">
                <div class="row">
                    <div class="table-responsive">
                        <asp:GridView runat="server" Width="100%" CssClass="table table-bordered table-hover"
                            ID="dataGridResults" > <%--OnRowCommand="dataGridResults_RowCommand" OnRowCreated="dataGridResults_RowCreated"--%>
                            <AlternatingRowStyle BackColor="#BFE4FF" />
                            <HeaderStyle BackColor="#115E9B" Font-Bold="false" ForeColor="white" Font-Italic="False"
                                Font-Overline="False" Font-Strikeout="False" Font-Underline="False" Height="30px" />
                            <Columns>
                                <asp:TemplateField HeaderText="Details">
                                    <ItemTemplate>
                                        <%--<asp:Button ID="btnedit" runat="server" Text="Edit" CommandName="EditEntity" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" />--%>
                                        <asp:Button ID="btnMakeASale" runat="server" Text="Edit Details" CommandName="EditCategory" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="IsActive">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chkbx" runat="server" Checked='<%#Convert.ToBoolean(Eval("IsActive")) %>'/>
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


<%@ Page Language="C#" MasterPageFile="~/MasterMain.master" AutoEventWireup="true" CodeFile="ViewUsers.aspx.cs" Inherits="ViewUsers" Title="VIEW USERS" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<%@ Register 
 Assembly="AjaxControlToolkit" 
 Namespace="AjaxControlToolkit" 
 TagPrefix="ajaxToolkit" %>
    <script runat="server">
</script>

    <ajaxToolkit:ToolkitScriptManager runat="Server" EnableScriptGlobalization="true"
        EnableScriptLocalization="true" ID="ScriptManager1" />
    <div class="col-lg-12">
    <div class="card mb-3">
        <div class="card-header">
        <i class="fa fa fa-cog"></i> System Tools <i class='fa fa-arrow-right'></i> View Users
        </div>
        <div class="card-body">
            <div class="row clearfix" style="overflow-x:auto;">

        <!---------------------------------------------- Search Options --------------------------------->
            <asp:MultiView ID="MultiView2" ActiveViewIndex="0" runat="server">
                <asp:View ID="View2" runat="server">
                <div class="row">
                <div class="col-md-2">
                    <label>Company</label>
                    <asp:DropDownList ID="ddCompany" runat="server" CssClass="form-control">
                    <asp:ListItem Value="Lesh">LENSH</asp:ListItem>
                </asp:DropDownList>
                </div>
                <div class="col-md-3">
                    <label>User Role</label>
                    <asp:DropDownList ID="ddlUserType" runat="server" AutoPostBack="True" CssClass="form-control">
                    </asp:DropDownList>
                </div>
                <div class="col-md-2">
                    <label>User ID</label>
                    <asp:TextBox ID="txtSearch" runat="server" CssClass="form-control"></asp:TextBox>
                </div>
                <div class="col-md-2">
                    <label></label><br/>
                    <asp:Button ID="btnOK" runat="server" CssClass="btn btn-success" OnClick="btnOK_Click" Text="Search" />
                </div>
                </div>

                <hr /><br /><br /><br /><br /><br /><br />
                <div class="row">
                <div class="col-md-10">
                <asp:GridView runat="server" Width="100%" CssClass="table table-bordered table-hover"
                            ID="dataGridResults" OnRowCommand="dataGridResults_RowCommand" OnRowCreated="dataGridResults_RowCreated"> <%--  115E9B  BFE4FF FF2F33--%>
                            <AlternatingRowStyle BackColor="#FFF9FB" />
                            <HeaderStyle BackColor="#E44B4B" Font-Bold="false" ForeColor="white" Font-Italic="False"
                                Font-Overline="False" Font-Strikeout="False" Font-Underline="False" Height="15px" />
                            <Columns>
                                <asp:TemplateField HeaderText="Details">
                                    <ItemTemplate>
                                        <asp:Button ID="btnedit" runat="server" Text="Edit Details" CommandName="EditUser" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" />
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
                        <div class="col-md-1"></div>
                        </div>
                <%--<asp:DataGrid ID="DataGrid1" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                    HorizontalAlign="Justify" OnItemCommand="DataGrid1_ItemCommand"
                    UseAccessibleHeader="true" GridLines="None" CssClass="table table-striped table-hover" 
                    OnPageIndexChanged="DataGrid1_PageIndexChanged" PageSize="50" ShowFooter="True" style="white-space:nowrap;">
                   <FooterStyle BackColor="InactiveCaption"   ForeColor="White" />
                    <PagerStyle  ForeColor="#4380B8"  HorizontalAlign="Center" Mode="NumericPages"  Font-Size="16"/>
                    <Columns>

                        <asp:BoundColumn DataField="UserId" HeaderText="Code" Visible="False"></asp:BoundColumn>
                        <asp:BoundColumn DataField="Username" HeaderText="Username" Visible="False"></asp:BoundColumn>
                      
                        <asp:ButtonColumn CommandName="btnCredit" HeaderText="Credit" Text="Username" DataTextField="Username" Visible="false">
                            <HeaderStyle Width="15%" />
                            <ItemStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                Font-Underline="False" ForeColor="Blue" />
                        </asp:ButtonColumn>


                        <asp:ButtonColumn DataTextField="Username" HeaderText="Edit" Text="Username" CommandName="btnEdit">
                            <HeaderStyle Width="15%" />
                            <ItemStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                Font-Underline="False" ForeColor="Blue" />
                        </asp:ButtonColumn>
                        <asp:BoundColumn DataField="fullName" HeaderText="Name">
                            <HeaderStyle Width="25%" />
                            <ItemStyle Width="120px" />
                        </asp:BoundColumn>
                        <asp:BoundColumn HeaderText="Phone" Visible="False">
                            <HeaderStyle Width="10%" />
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="UserRole" HeaderText="Role">
                            <HeaderStyle Width="10%" />
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="VendorCode" HeaderText="Vendor">
                            <HeaderStyle Width="10%" />
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="Active" HeaderText="Active">
                            <HeaderStyle Width="10%" />
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="CreationDate" HeaderText="Date" Visible="False">
                            <HeaderStyle Width="10%" />
                        </asp:BoundColumn>
                    </Columns>
                    
                </asp:DataGrid>--%>

                </asp:View>

                <asp:View ID="View1" runat="server">
                    <div class="modal-content col-md-6  col-sm-6 col-xs-10"  style="margin:0 auto;">
                            <div class="modal-header">
			                    <center>Credit User</center>
		                    </div>
		                    <div class="modal-body">
                             <asp:Label ID="lblCredit" runat="server" Font-Bold="True" Font-Names="Arial" ForeColor="Blue" Text="."></asp:Label>
                             <br/>
                             Username
                             <asp:TextBox ID="txtUserName" runat="server" MaxLength="12" CssClass="form-control" Enabled="False"></asp:TextBox>
		        

                            Name(s)
                            <asp:TextBox ID="txtName" runat="server" CssClass="form-control" Enabled="False"></asp:TextBox>

                            Credit
                            <asp:TextBox ID="txtCredit" runat="server" CssClass="form-control"></asp:TextBox>
                            </div>
                            <div class="modal-footer">
                                <%--<asp:Button ID="Button1" runat="server" CssClass="btn btn-success" OnClick="Button1_Click" Text="Add Credit" />--%>
                         </div>
                    </div>
                 </asp:View>
                 
            </asp:MultiView>
            </div>
        </div>
      </div>
    </div> 
     

                        
    <asp:Label ID="lblPhoneCode" runat="server" Text="0" Visible="False"></asp:Label><br />
    <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server"
        TargetControlID="txtCredit" ValidChars="0123456789">
    </ajaxToolkit:FilteredTextBoxExtender>
</asp:Content>




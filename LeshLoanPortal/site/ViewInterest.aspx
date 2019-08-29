<%@ Page Title="" Language="C#" MasterPageFile="~/MasterMain.master" AutoEventWireup="true" CodeFile="ViewInterest.aspx.cs" Inherits="ViewInterest" %>

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
        <i class="fa fa fa-cog"></i> System Tools <i class='fa fa-arrow-right'></i> View Interest
        </div>
        <div class="card-body">
            <div class="row clearfix" style="overflow-x:auto;">

        <!---------------------------------------------- Search Options --------------------------------->
            <asp:MultiView ID="MultiView2" ActiveViewIndex="0" runat="server">
                <asp:View ID="View2" runat="server">
                <div class="row">
                <div class="col-md-6">
                    <label>Company</label>
                    <asp:DropDownList ID="ddCompany" runat="server" CssClass="form-control">
                    <asp:ListItem Value="Lensh">LENSH</asp:ListItem>
                </asp:DropDownList>
                </div>
                <div class="col-md-3">
                    <label>Interest Code</label>
                    <asp:TextBox ID="txtInterestCode" runat="server" CssClass="form-control"></asp:TextBox>
                </div>
                <div class="col-md-3">
                    <label>.</label><br/>
                    <asp:Button ID="btnOK" runat="server" CssClass="btn btn-success" OnClick="btnOK_Click" Text="Search" />
                </div>
                </div>

                    <div class="row">
                <div class="col-md-8">
                    <asp:CheckBoxList runat="server" ID="Chk" TextAlign="Right"></asp:CheckBoxList>
                </div>
                    </div>

                <hr /><br /><br /><br /><br /><br /><br />
                <div class="row">
                <div class="col-md-10">
                <asp:GridView runat="server" Width="100%" CssClass="table table-bordered table-hover"
                            ID="dataGridResults" OnRowCommand="dataGridResults_RowCommand">
                            <AlternatingRowStyle BackColor="#FFF9FB" />
                            <HeaderStyle BackColor="#E44B4B" Font-Bold="false" ForeColor="white" Font-Italic="False"
                                Font-Overline="False" Font-Strikeout="False" Font-Underline="False" Height="15px" />
                            <Columns>
                                <asp:TemplateField HeaderText="Details">
                                    <ItemTemplate>
                                        <i style="color:dodgerblue;"><a class="fa fa-edit"><asp:Button runat="server" ID="btnedit" Text="Edit" ForeColor="dodgerblue" CommandName="EditUser" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" BackColor="WhiteSmoke" BorderStyle="None"></asp:Button></a></i>
                                        
                                        <%--<asp:Button ID="btnedit" runat="server" Text="Edit Details" CommandName="EditUser" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" />--%>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <%--<asp:TemplateField HeaderText="IsActive">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chkbx" runat="server" Checked='<%#Convert.ToBoolean(Eval("IsActive")) %>'/>
                                    </ItemTemplate>
                               </asp:TemplateField>--%>
                            </Columns>
                        </asp:GridView>
                        </div>
                        <div class="col-md-1"></div>
                        </div>

                </asp:View>

                 
            </asp:MultiView>
            </div>
        </div>
      </div>
    </div> 
     

                        
    <asp:Label ID="lblPhoneCode" runat="server" Text="0" Visible="False"></asp:Label><br />
    <%--<ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server"
        TargetControlID="txtCredit" ValidChars="0123456789">
    </ajaxToolkit:FilteredTextBoxExtender>--%>
</asp:Content>






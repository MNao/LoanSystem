<%@ Page Title="" Language="C#" MasterPageFile="~/MasterMain.master" AutoEventWireup="true" CodeFile="AddSystemSetting.aspx.cs" Inherits="AddSystemSetting" %>

<%@ Register 
 Assembly="AjaxControlToolkit" 
 Namespace="AjaxControlToolkit" 
 TagPrefix="ajaxToolkit" %>
 <%@ Import
  Namespace="System.Threading" %>
<script runat="server">

    protected void btnApprove_Click(object sender, EventArgs e)
    {

    }
</script>


<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">


    <ajaxToolkit:ToolkitScriptManager runat="Server" EnableScriptGlobalization="true"
        EnableScriptLocalization="true" ID="ScriptManager1" />

<div class="col-lg-12">
    <div class="card mb-3">
        <div class="card-header">
        <i class="fa fa fa-cog"></i> System Tools <i class='fa fa-arrow-right'></i> Add System Setting
        </div>
        <div class="card-body">
            <div class="row clearfix">
            <asp:MultiView ID="MultiView1" runat="server" ActiveViewIndex="0">
                <asp:View ID="View1" runat="server">
                <div class="modal-content col-md-6  col-sm-6 col-xs-10"  style="margin:0 auto;">
                   
		            <div class="modal-body">

                      Setting Name
                      <asp:TextBox ID="txtfname" runat="server" CssClass="form-control"></asp:TextBox>
                       
                      Setting Value
                      <asp:TextBox ID="txtlname" runat="server" CssClass="form-control"></asp:TextBox>

                      User Role
                      <asp:DropDownList ID="ddlUserType" runat="server" OnDataBound="ddlUserType_DataBound" CssClass="form-control">
                      </asp:DropDownList>
                        <div class="row">
                                        <div class="col-lg-6">
                                            <label>Company Code</label>
                                            <asp:DropDownList ID="ddCompanies" runat="server" CssClass="form-control">
                                                <asp:ListItem Value="TRUE">YES</asp:ListItem>
                                                <asp:ListItem Value="FALSE">NO</asp:ListItem>
                                            </asp:DropDownList>
                                            <p class="help-block">The Name of the Company</p>
                                        </div>
                                        
                                    </div>

                                    <div class="row">
                                        <div class="col-lg-6">
                                            <label>Setting Code</label>
                                            <asp:TextBox ID="txtSettingCode" runat="server" CssClass="form-control" placeholder="Enter text" />
                                            <p class="help-block">The Code associated with The Setting</p>
                                        </div>
                                        <div class="col-lg-6">
                                            <label>Setting Value</label>
                                            <asp:TextBox ID="txtSettingValue" runat="server" CssClass="form-control" placeholder="Enter text" />
                                            <p class="help-block">The Value of The Setting</p>
                                        </div>
                                    </div>
                    </div>
                    <div class="modal-footer">
                        <asp:Button ID="btnOK" runat="server" CssClass="btn btn-success" Text="SUBMIT DETAILS"  OnClick="btnOK_Click" />
                        <asp:Button ID="btnEdit" runat="server" CssClass="btn btn-success" Text="UPDATE DETAILS"  OnClick="btnEdit_Click" />
                    </div>
                </div>
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








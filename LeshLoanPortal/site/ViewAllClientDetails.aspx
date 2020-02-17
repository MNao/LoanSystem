<%@ Page Language="C#" MasterPageFile="~/MasterMain.master" AutoEventWireup="true" CodeFile="ViewAllClientDetails.aspx.cs" Inherits="ViewAllClientDetails" Title="Client Details" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<div class="col-lg-12">

<asp:MultiView ID="MultiView1" ActiveViewIndex="0" runat="server">
    <asp:View ID="SearchView" runat="server">
        <%-------------------------------------------- Message Label ----------------------------------%>
               <!---------------------------------------------- Search Options --------------------------------->
        <div class="row">
            <div class="col-lg-3">
                <label>
                    Client Name/Number
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
            <div class="col-lg-2" style="padding-top: 15px;">
            
                <asp:Button ID="btnSubmit" runat="server" Text="Search DB" CssClass="btn btn-success btn-md"
                    OnClick="btnSubmit_Click" />
            </div>
        </div>
        <hr />
        <div class="row">
                <div class="col-md-4"> 
                        <asp:RadioButton runat="server" ID="rdExcel" Text="Excel" GroupName="btnCheck" CssClass="form-control" />
                </div>
             <div class="col-md-4">
                 <asp:RadioButton runat="server" ID="rdPdf" Text="PDF" GroupName="btnCheck"  CssClass="form-control"/>
             </div>
             <div class="col-md-4">
                 <asp:Button ID="btnExport" runat="server" Text="Export" CssClass="btn btn-success btn-md" OnClick="btnExport_Click" />
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
                            ID="dataGridResults" AutoGenerateColumns="true" OnRowCommand="dataGridResults_RowCommand"> <%--OnRowCommand="dataGridResults_RowCommand" OnRowCreated="dataGridResults_RowCreated" arBFE4FF hc115E9B  --%>
                            <AlternatingRowStyle BackColor="#FFF9FB" /> <%--DE6868--%>
                            <HeaderStyle BackColor="#E44B4B" Font-Bold="false" ForeColor="white" Font-Italic="False"
                                Font-Overline="False" Font-Strikeout="False" Font-Underline="False" Height="30px" />
                            <Columns>
                                <asp:TemplateField HeaderText="Edit">
                                    <ItemTemplate>
                                        <i style="color:dodgerblue;"><a class="fa fa-info"><asp:Button runat="server" ID="btnEdit" Text="Edit" ForeColor="dodgerblue" CommandName="Edit" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" BackColor="WhiteSmoke" BorderStyle="None"></asp:Button></a></i>
                                        
                                      </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>
                </div>
            </asp:View>
            <asp:View runat="server" ID="EmptyView">
            </asp:View>
            <asp:View ID="DetailsView" runat="server">
        <div class="modal-content col-md-8  col-sm-8 col-xs-10"  style="margin:0 auto;">
                   
		            <div class="modal-body">
                         Client Number
                      <asp:label ID="lblClientNo" Text="ClientNo" runat="server" CssClass="form-control"></asp:label>
                        Client Name
                      <asp:label ID="lblClientName" Text="ClientName" runat="server" CssClass="form-control"></asp:label>
                        Client Phone Number
                      <asp:label ID="lblTelNo" Text="ClientPhoneNo" runat="server" CssClass="form-control"></asp:label>
                        Gender
                      <asp:label ID="lblGender" Text="ClientGender" runat="server" CssClass="form-control"></asp:label>

                        <div class="row" id="ViewPhotos" runat="server">
                                        <div class="col-lg-6">
                                            <asp:Label ID="imgUrlClientPhoto" runat="server"></asp:Label>
                                            <label>Client Photo</label><br />
                                            <asp:Button ID="btnViewPR" Text="View Client Image" OnClick="btnViewPR_Click" runat="server" CssClass="btn btn-success btn-sm"/>
                                            
                                            <br /><br />
                                         <asp:image ID="Image1" runat="server"/>
                                        </div>
                                <div class="col-lg-6">
                                            <asp:Label ID="ImgUrlIDPhoto" runat="server"></asp:Label>
                                            <label>ID Photo</label><br />
                                            <asp:Button ID="btnViewID" Text="View ID Image" OnClick="btnViewID_Click" runat="server" CssClass="btn btn-success btn-sm"/>
                                            
                                            <br /><br />
                                         <asp:image ID="Image2" runat="server"/>
                                        </div>
                                    </div>
                        
		            </div>
            <div class="modal-footer">
                        <div class="row">
                        <asp:Button ID="btnBack" runat="server" Text="Back" Width="200px" CssClass="btn btn-danger btn-sm" OnClick="btnBack_Click"/>
                        </div>
                    </div>
            </div>
    </asp:View>
        </asp:MultiView>

        <!------------------------------------------------- View2 -------------------------------------------------->
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




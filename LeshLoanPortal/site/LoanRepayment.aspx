<%@ Page Title="" Language="C#" MasterPageFile="~/MasterMain.master" AutoEventWireup="true" CodeFile="LoanRepayment.aspx.cs" Inherits="LoanRepayment" %>
<%@ Register 
 Assembly="AjaxControlToolkit" 
 Namespace="AjaxControlToolkit" 
 TagPrefix="ajaxToolkit" %>


<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">


    <ajaxToolkit:ToolkitScriptManager runat="Server" EnableScriptGlobalization="true"
        EnableScriptLocalization="true" ID="ScriptManager1" />
<div class="col-lg-12">

<asp:MultiView ID="MultiView2" ActiveViewIndex="0" runat="server">
    <asp:View ID="SearchView" runat="server">
        <%-------------------------------------------- Message Label ----------------------------------%>
               <!---------------------------------------------- Search Options --------------------------------->
        <div class="row">
            <div class="col-lg-2">
                <label>
                    Client Name/Number
                </label>
                <asp:TextBox ID="txtSearch" runat="server" ClientIdMode="Static" placeholder="Search" CssClass="form-control"></asp:TextBox>
            </div>
            <div class="col-lg-2">
                <label>
                    Loan Number
                </label>
                <asp:TextBox ID="txtLoanNoSearch" AutoComplete="Off" runat="server" CssClass="form-control"></asp:TextBox>
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
            <div class="col-lg-2" style="padding-top: 15px;">
            
                <asp:Button ID="btnSearch" runat="server" Text="Search DB" CssClass="btn btn-success btn-lg"
                    OnClick="btnSearch_Click" />
            </div>
        </div>
        <ajaxToolkit:CalendarExtender ID="CalendarExtender1" runat="server" CssClass="cal_Theme1"
            Format="yyyy-MM-dd" PopupPosition="BottomRight" TargetControlID="txtStartDate">
        </ajaxToolkit:CalendarExtender>
        <ajaxToolkit:CalendarExtender ID="CalendarExtender3" runat="server" CssClass="cal_Theme1"
            Format="yyyy-MM-dd" PopupPosition="BottomRight" TargetControlID="txtEndDate">
        </ajaxToolkit:CalendarExtender>
        <hr />
        <%------------------------------------------- Search Results  -----------------------------------%>
        <asp:MultiView runat="server" ID="MultiView3" ActiveViewIndex="0">
            <asp:View runat="server" ID="resultView">
                <div class="row" style="overflow-x:auto;">
                    <div class="table-responsive">
                        <asp:GridView runat="server" Width="100%" CssClass="table table-bordered table-hover"
                            ID="dataGridResults" AutoGenerateColumns="true" OnRowCommand="dataGridResults_RowCommand"> <%--OnRowCommand="dataGridResults_RowCommand" OnRowCreated="dataGridResults_RowCreated" arBFE4FF hc115E9B  --%>
                            <AlternatingRowStyle BackColor="#FFF9FB" /> <%--DE6868--%>
                            <HeaderStyle BackColor="#E44B4B" Font-Bold="false" ForeColor="white" Font-Italic="False"
                                Font-Overline="False" Font-Strikeout="False" Font-Underline="False" Height="30px" />
                            <Columns>
                                <asp:TemplateField HeaderText="Repay">
                                    <ItemTemplate>
                                        <i style="color:dodgerblue;"><a class="fa fa-money"><asp:Button runat="server" ID="btnRepayLoan" Text="Repay Loan" ForeColor="dodgerblue" CommandName="Repay" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" BackColor="WhiteSmoke" BorderStyle="None"></asp:Button></a></i>
                                        
                                        <%--<asp:Button ID="btnRepayLoan" runat="server" Text="Repay Loan" CommandName="Repay" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" />--%>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>
                </div>
            </asp:View>
            <asp:View ID="RepayDetails" runat="server">
                <div class="modal-content col-md-6  col-sm-6 col-xs-10"  style="margin:0 auto;">
                   
		            <div class="modal-body">
                       Receipt Number
                      <asp:TextBox ID="txtReceiptNo" runat="server" CssClass="form-control"  placeholder="Receipt Number"></asp:TextBox>


                        Client Number
                      <asp:TextBox ID="txtClientNo" runat="server" CssClass="form-control"  placeholder="Client Number"></asp:TextBox>

                      Client Name
                      <asp:TextBox ID="txtfname" runat="server" CssClass="form-control"  placeholder="Client Name"></asp:TextBox>
                       
                        Loan Number
                      <asp:TextBox ID="txtLoanNo" runat="server" CssClass="form-control"  placeholder="Loan Number"></asp:TextBox>

                        Amount Paid
                      <asp:TextBox ID="txtPaidAmount" runat="server" CssClass="form-control"  placeholder="Loan Amount Paid" onkeyup="javascript:this.value=Comma(this.value);"></asp:TextBox>

                        Payment Type
                      <asp:DropDownList ID="ddPaymentType" runat="server" CssClass="form-control">
                          <asp:ListItem Value="MOMO">MOBILE MONEY</asp:ListItem>
                          <asp:ListItem Value="CASH">CASH</asp:ListItem>
                      </asp:DropDownList>

                      Payment Date
                      <asp:TextBox ID="txtRepaymentDate" runat="server" CssClass="form-control" placeholder="Repayment Date"></asp:TextBox>
                        
                      Mobile No. Own
                      <asp:TextBox ID="txtMobileNo" runat="server" CssClass="form-control"  placeholder="Client's Own Mobile Number"></asp:TextBox>

                      Email
                      <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control"  placeholder="Client's Email"></asp:TextBox>

                        Remarks
                      <asp:TextBox ID="txtRemarks" runat="server" CssClass="form-control" TextMode="MultiLine" Columns="2" Rows="2" placeholder="Any Remarks about Payment"></asp:TextBox>
                        
                      <%--Vendor
                      <asp:DropDownList ID="ddlAreas" runat="server" OnDataBound="ddlAreas_DataBound" CssClass="form-control">
                      </asp:DropDownList>--%>


                      <%--<br/><asp:CheckBox ID="chkActive" runat="server" Text=" Is Active" /><br/>--%>

                    </div>
                    <div class="modal-footer">
                        <asp:Button ID="btnSubmit" runat="server" CssClass="btn btn-success" Text="SUBMIT DETAILS"  OnClick="btnSubmit_Click" />
                    </div>
                </div>
                    
                    <ajaxToolkit:CalendarExtender ID="CalendarExtender2" runat="server" CssClass="cal_Theme1"
            Format="yyyy-MM-dd" PopupPosition="BottomRight" TargetControlID="txtRepaymentDate">
        </ajaxToolkit:CalendarExtender>
            </asp:View>
        </asp:MultiView>

        <!------------------------------------------------- View2 -------------------------------------------------->
    </asp:View>
    <asp:View ID="View2" runat="server">
    </asp:View>
</asp:MultiView>
</div>

<%--<div class="col-lg-12">
    <div class="card mb-3">
        <div class="card-header">
        <i class="fa fa fa-cog"></i> Loans <i class='fa fa-arrow-right'></i> RePay Loan
        </div>
        <div class="card-body">
            <div class="row clearfix">
            <asp:MultiView ID="MultiView1" runat="server" ActiveViewIndex="0">
                

        </asp:MultiView>
            </div>
        </div>
    </div>
</div>--%>
    <asp:Label ID="lblCode" runat="server" Text="0" Visible="False"></asp:Label>
    <asp:Label ID="lblUsername" runat="server" Text="0" Visible="False"></asp:Label><br />
    &nbsp;

</asp:Content>







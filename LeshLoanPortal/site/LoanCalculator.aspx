<%@ Page Title="" Language="C#" MasterPageFile="~/MasterMain.master" AutoEventWireup="true" CodeFile="LoanCalculator.aspx.cs" Inherits="LoanCalculator" %>

 <%@ Register 
 Assembly="AjaxControlToolkit" 
 Namespace="AjaxControlToolkit" 
 TagPrefix="ajaxToolkit" %>
 <%@ Import
  Namespace="System.Threading" %>


<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">


    <ajaxToolkit:ToolkitScriptManager runat="Server" EnableScriptGlobalization="true"
        EnableScriptLocalization="true" ID="ScriptManager1" />

<div class="col-lg-12">
    <div class="container">
    <div class="card mb-3">
        <div class="card-header">
        <i class="fa fa fa-cog"></i> Loans <i class='fa fa-arrow-right'></i> Calculate Your Loan
        </div>
        <div class="card-body">
            <div class="row clearfix">
                <div class="col-lg-12"></div>
            <asp:MultiView ID="MultiView1" runat="server" ActiveViewIndex="0">
                <asp:View ID="LoanDetails" runat="server">
                    <div class="modal-content col-md-6  col-sm-6 col-xs-10"  style="margin:0 auto;">
                   
		            <div class="modal-body">
                        Loan Amount
                      <asp:TextBox ID="txtLoanAmount" runat="server" CssClass="form-control" autocomplete="off" onkeyup="javascript:this.value=Comma(this.value);"></asp:TextBox>

                      Interest Rate (%)
                      <asp:TextBox ID="txtInterest" runat="server" CssClass="form-control" autocomplete="off"></asp:TextBox>

                      Number of Months to PayIn
                      <asp:TextBox ID="txtMonths" runat="server" CssClass="form-control" autocomplete="off"></asp:TextBox>
                        
                      
                    </div>

                        <div class="modal-footer">
                        <asp:Button ID="btnCalculateLoan" runat="server" CssClass="btn btn-success" Text="CALCULATE" OnClick="btnCalculateLoan_Click" />
                            
                    </div>
                        </div>
                </asp:View>
                
        </asp:MultiView>

                    
            </div>
        </div>
    </div>
    </div>
</div>

         <div class="col-lg-12">
             <div class="container">
            <%--<div class="row clearfix">--%>
                    <asp:MultiView ID="MultiView2" runat="server">
                <asp:View ID="LoanTable" runat="server">
                    <div class="row" style="overflow-x:auto;">
                    <div class="table-responsive">
                        <asp:GridView runat="server" Width="100%" CssClass="table table-bordered table-hover"
                            ID="dataGridResults" AutoGenerateColumns="true"> <%--OnRowCommand="dataGridResults_RowCommand" OnRowCommand="dataGridResults_RowCommand" OnRowCreated="dataGridResults_RowCreated" arBFE4FF hc115E9B  --%>
                            <AlternatingRowStyle BackColor="#FFF9FB" /> <%--DE6868--%>
                            <HeaderStyle BackColor="#E44B4B" Font-Bold="false" ForeColor="white" Font-Italic="False"
                                Font-Overline="False" Font-Strikeout="False" Font-Underline="False" Height="30px" />
                            <%--<Columns>
                                <asp:TemplateField HeaderText="Details">
                                    <ItemTemplate>
                                        <asp:Button ID="btnApproveLoan" runat="server" Text="View Details" CommandName="Verify" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>--%>
                        </asp:GridView>
                    </div>
                </div>
                </asp:View></asp:MultiView>
                </div>
             </div>
    <asp:Label ID="lblCode" runat="server" Text="0" Visible="False"></asp:Label>
    <asp:Label ID="lblUsername" runat="server" Text="0" Visible="False"></asp:Label><br />
    &nbsp;
</asp:Content>









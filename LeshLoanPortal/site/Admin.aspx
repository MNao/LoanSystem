<%@ Page Language="C#" MasterPageFile="~/MasterMain.master" AutoEventWireup="true" CodeFile="Admin.aspx.cs" Inherits="Admin" Title="SYSTEM ADMINISTRATION" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<%@ Register 
 Assembly="AjaxControlToolkit" 
 Namespace="AjaxControlToolkit" 
 TagPrefix="ajaxToolkit" %>
 <ajaxToolkit:ToolkitScriptManager runat="Server" EnableScriptGlobalization="true"
        EnableScriptLocalization="true" ID="ScriptManager1" />

        

          <div class="col-lg-8">

            <!-- Example Bar Chart Card -->
            <div class="card mb-3">
              <div class="card-header">
                <i class="fa fa fa-sitemap"></i>
               <strong> LENSH LOAN SYSTEM</strong>
              </div>
              <div class="card-body">
                <div class="row">
                  <div class="col-sm-9 text-right my-auto">
                    <img src="Images/micro2.png" class="card-img-top img-fluid w-100" alt="Messages"><br/>
                  </div>
                  <div class="col-sm-3 text-right my-auto">
                    <div class="h4 mb-0 text-primary"><asp:Label ID="lblApproved" Text="117" runat="server"></asp:Label></div>
                    <div class="small text-muted">Approved Clients</div>
                    <hr/>
                    <div class="h4 mb-0 text-default"><asp:Label ID="lblVerified" Text="1235" runat="server"></asp:Label></div>
                    <div class="small text-muted">Verified Clients</div>
                    <hr/>
                    <div class="h4 mb-0 text-warning"><asp:Label ID="lblPending" Text="1235" runat="server"></asp:Label></div>
                    <div class="small text-muted">Pending Clients</div>
                    <hr/>
                    <div class="h4 mb-0 text-danger"><asp:Label ID="lblRejected" Text="53" runat="server"></asp:Label></div>
                    <div class="small text-muted">Rejected Clients</div>
                    <hr/>
                    <%--<div class="h4 mb-0 text-success"><asp:Label ID="lblTime" runat="server" Text="."></asp:Label></div>
                    <div class="small text-muted">Time</div>--%>
                    
                  </div>
                </div>
              </div>
              <div class="card-footer small text-muted">
                Track Your Clients and their Loans with ease, fast and convenient.
              </div>
            </div>

          </div>

          <div class="col-lg-4">
            <!-- Example Pie Chart Card -->
            <div class="card mb-3">
              <div class="card-header">
                <i class="fa fa-user-circle"></i>
                 <strong><asp:Label ID="FullNames" runat="server" Text=""></asp:Label></strong>
              </div>
              <div class="card-body">
                <img src="Images/avatar.png" class="card-img-top img-fluid w-100" alt="My Profile" width="10%" height="10%"/>
              </div>
               <div class="card-footer small text-muted">
                       <br/><p><strong>Branch:<asp:Label ID="Branch" runat="server" Text=""></asp:Label> </strong></p>
                      <br/><p><strong>Role Code:<asp:Label ID="AreaRole" runat="server" Text=""></asp:Label> </strong></p><br/>
               
                      
                </div>     
             
            </div>
            <!-- Example Notifications Card -->
            
          </div>
        

</asp:Content>


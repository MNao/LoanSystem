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
                PEGASUS SMS PORTAL
              </div>
              <div class="card-body">
                <div class="row">
                  <div class="col-sm-9 text-right my-auto">
                    <img src="Images/message.jpg" class="card-img-top img-fluid w-100" alt="Messages"><br/>
                  </div>
                  <div class="col-sm-3 text-right my-auto">
                    <div class="h4 mb-0 text-primary">117</div>
                    <div class="small text-muted">Received Messages</div>
                    <hr/>
                    <div class="h4 mb-0 text-default">1235</div>
                    <div class="small text-muted">Sent Messages</div>
                    <hr/>
                    <div class="h4 mb-0 text-warning">1235</div>
                    <div class="small text-muted">Pending Messages</div>
                    <hr/>
                    <div class="h4 mb-0 text-danger">53</div>
                    <div class="small text-muted">Failed Delivery</div>
                    <hr/>
                    <div class="h4 mb-0 text-success"><asp:Label ID="lblCredit" runat="server" Text="."></asp:Label></div>
                    <div class="small text-muted">Left SMS Credit</div>
                    
                  </div>
                </div>
              </div>
              <div class="card-footer small text-muted">
                Sending SMS with ease, fast and convinience.
              </div>
            </div>

          </div>

          <div class="col-lg-4">
            <!-- Example Pie Chart Card -->
            <div class="card mb-3">
              <div class="card-header">
                <i class="fa fa-user-circle"></i>
                 <strong><asp:Label ID="FullNames" runat="server" Text="."></asp:Label></strong>
              </div>
              <div class="card-body">
                <img src="Images/avatar.png" class="card-img-top img-fluid w-100" alt="My Profile" width="10%" height="10%"/>
              </div>
               <div class="card-footer small text-muted">
                      <br/><br/><p><strong><asp:Label ID="AreaRole" runat="server" Text="."></asp:Label> </strong></p><br/>
               
                      
                </div>     
             
            </div>
            <!-- Example Notifications Card -->
            
          </div>
        

</asp:Content>


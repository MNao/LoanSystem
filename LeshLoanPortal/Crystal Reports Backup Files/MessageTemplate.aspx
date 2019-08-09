<%@ Page Language="C#" MasterPageFile="~/MasterMain.master" AutoEventWireup="true" CodeFile="MessageTemplate.aspx.cs" Inherits="MessageTemplate" Title="SMS SENDING PANEL" %>
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
   
    <div class="card mb-3">
        <div class="card-header">
        <i class="fa fa fa-envelope"></i> SMS PANEL <i class='fa fa-arrow-right'></i> Message Template
        </div>
        <div class="card-body">
    <asp:MultiView ID="MultiView1" runat="server" ActiveViewIndex="0">
    <asp:View ID="View1" runat="server">
        
            <div class="modal-content col-md-6  col-sm-6 col-xs-10"  style="margin:0 auto;">
				 
                <div class="modal-body">                                       

                      
                       
						    <label>Template Title</label>
                                <div class="form-group input-group">							
								    <asp:TextBox ID="txtTitle" runat="server" CssClass="form-control"></asp:TextBox>
							    </div> 
                            <asp:TextBox ID="txtId" runat="server" CssClass="form-control" Visible="false"></asp:TextBox>

               

							<label> Message Body</label>
							<div class="form-group input-group">
								<asp:TextBox ID="txtMessage" runat="server" CssClass="form-control" Height="100" TextMode="MultiLine" onKeyDown="textCounter(document.txtMessage,document.txtCount,125)"
                                        onKeyUp="textCounter(document.txtMessage,document.txtCount,125)"></asp:TextBox>
                            </div>
                            <asp:Label ID="lblMessageLength" runat="server" Font-Bold="True" Text="SMS MESSAGE LENGTH : 160"></asp:Label> <br/>
                            Count: <span id="spanDisplay"> </span>
                      
				            </div>
							<div class="modal-footer">
					
					            <asp:Button ID="btnOK" runat="server" CssClass="btn btn-success pull-right" OnClick="btnOK_Click" Text="SAVE MESSAGE TEMPLATE" />
					        </div>
				
			</div>
        
    </asp:View>
     <asp:View ID="View2" runat="server">
     <div class=" modal-content col-md-6  col-sm-6  col-xs-10" style="margin:0 auto;">
		<div class="modal-header">
			<center><h4>SMS CREDIT MESSAGE</h4></center>
		</div>
		<div class="modal-body">
			<asp:Label ID="lblerror" runat="server" Font-Bold="True" ForeColor="#C00000" Text="."></asp:Label>
                                   
		</div>
		<div class="modal-footer">

		</div>
	</div>
      </asp:View>

      <asp:View ID="View3" runat="server">

      <div class=" modal-content col-md-6  col-sm-6 col-xs-10" style="margin:0 auto;">
		<div class="modal-header">
			<center><h4>SMS CONFIRMATION</h4></center>
		</div>
		<div class="modal-body">
            <div>
                <asp:Label ID="Label1" runat="server" Text="."></asp:Label> 
            </div>
			<div><label>List To Send To</label></div>
        <div class="form-group input-group">
			<asp:TextBox ID="txtviewlistname" runat="server" CssClass="form-control" Enabled="False"></asp:TextBox>
		</div> 
        <span style="display:none">
		    <label>Contact To send</label>
		    <div class="form-group input-group">
			    <asp:TextBox ID="txtviewprefix" runat="server" CssClass="form-control" Enabled="False"></asp:TextBox>
            </div>
        </span> 
		<label>Message To Send</label>
		<div class="form-group input-group">
			<asp:TextBox ID="txtViewMessage" runat="server" CssClass="form-control" TextMode="MultiLine"  Enabled="False" ReadOnly="True"></asp:TextBox>
        </div>
        Count: <asp:Label ID="TextBox3" runat="server"></asp:Label>
		</div>
		<div class="modal-footer">
        <asp:Button ID="Button2" runat="server" CssClass="btn btn-danger form-control pull-left" OnClick="Button2_Click" Text="CANCEL" />
			<asp:Button ID="Button1" runat="server" Font-Bold="True" CssClass="btn btn-success pull-right" OnClick="Button1_Click" Text="CONTINUE TO SEND" />
                                    
		</div>
	</div>

      </asp:View>
    </asp:MultiView>
    </div>
</div>
</div>
<script type="text/javascript">
    function count(clientId) {
        var txtMessage = document.getElementById(clientId);
        var spanDisplay = document.getElementById('spanDisplay');
        spanDisplay.innerHTML = txtMessage.value.length;
    }
</script>
</asp:Content>


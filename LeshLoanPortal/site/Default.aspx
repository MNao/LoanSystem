<%@ Page Language="C#" AutoEventWireup="true" 
CodeFile="Default.aspx.cs" 
Inherits="_Default"
EnableEventValidation="false"
Culture="auto" 
UICulture="auto" %>


 <%@ Register 
 Assembly="AjaxControlToolkit" 
 Namespace="AjaxControlToolkit" 
 TagPrefix="ajaxToolkit" %>
 <%@ Import
  Namespace="System.Threading" %>
  
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >

  <head>

    <meta charset="utf-8"/>
    <meta http-equiv="X-UA-Compatible" content="IE=edge"/>
    <meta name="viewport" content="width=device-width, initial-scale=1, shrink-to-fit=no"/>
	<meta name="description" content=""/>
    <meta name="author" content=""/>
    <title>LENSH LOAN PORTAL</title>
   <link rel="icon" href="Images/favicon.png"/>
	

    <!-- Bootstrap core CSS -->
    <link href="vendor/bootstrap/css/bootstrap.min.css" rel="stylesheet" type="text/css"/>

    <!-- Custom fonts for this template -->
    <link href="vendor/font-awesome/css/font-awesome.min.css" rel="stylesheet" type="text/css"/>

    <!-- Plugin CSS -->
    <link href="vendor/datatables/dataTables.bootstrap4.css" rel="stylesheet" type="text/css"/>

    <!-- Custom styles for this template -->
    <link href="css/sb-admin.css" rel="stylesheet" type="text/css"/>

  </head>

<body style="background-color: #9EB7E5;"> <%--#E2E2E2--%>
<form id="form1" runat="server">
    <div class="container col-md-4 col-md-offset-4 col-sm-6 col-sm-offset-3 col-xs-10 col-xs-offset-1 ">
                <div class="row " style="padding-top:100px;">
            <center><asp:Label ID="lblmsg" runat="server" ForeColor="Red" Text="."></asp:Label></center>
                <div class="modal-content">
                
                   
                     <div class="modal-header">
							<img class="modal-title" style="max-width: 100%;" src="Images/Lenshlogo1.png" alt="logo"/>
						</div>
						   <asp:MultiView ID="MultiView1" runat="server">
                                <asp:View ID="View2" runat="server">
                                
                                    <div class="modal-body text-danger">
                                        <center><h4>LENSH LOAN PORTAL</h4>
                                        <hr/>
                                        </center>
                                        
                                        <%--<div class="form-group input-group">
									        <span class="input-group-prepend"><i class="fa fa-user" ></i></span>
									        </div><br/>--%>

                                        <label>Username</label>
                                        <div class="input-group mb-3">
                                            <div class="input-group-prepend">
                                              <span class="input-group-text"><i class="fa fa-user" ></i></span>
                                            </div>
                                            <asp:TextBox ID="txtUsername" placeholder="Username" runat="server" CssClass="form-control" autocomplete="off" onblur="Change(this, event)" onfocus="Change(this, event)"></asp:TextBox>
								        </div>

                                        <label>Password</label>
                                        <div class="input-group mb-3">
                                            <div class="input-group-prepend">
                                              <span class="input-group-text"><i class="fa fa-lock" ></i></span>
                                            </div>
                                            <asp:TextBox ID="txtpassword" placeholder="Password" runat="server" CssClass="form-control" TextMode="Password"></asp:TextBox>
                                        </div>
                                    </div>
                                     <div class="modal-footer">
                                         <asp:LinkButton runat="server" ID="linkRegister" OnClick="LoadRegisterClient"
                                                Text="Register as Client" /> |
							                    <asp:LinkButton runat="server" ID="linkForgotPassword" OnClick="LoadForgotPassword" ForeColor="#ff0000"
                                                Text="Forgot Password?" />
                                                 <asp:Button ID="btnlogin" runat="server" CssClass="btn btn-success  col-md-4 pull-right" Text="Login" OnClick="btnlogin_Click" />
							        
							        </div>
                                
			                    <asp:Label ID="lblMessage" runat="server" Font-Bold="True" ForeColor="#C00000"></asp:Label>
                                </asp:View>

                        <%--<asp:View ID="ForgotPasswordView" runat="server">
                        <div id="Div3" style="margin-top: 50px;" class="mainbox">
                            <div class="panel panel-primary">
                                <div class="panel-heading">
                                    <div class="panel-title">
                                        <div class="text-center">
                                            Oops!! Forgot Your Password??</div>
                                    </div>
                                </div>
                                <div style="padding-top: 30px;" class="panel-body">
                                    <div style="display: none" id="Div4" class="alert alert-danger col-lg-12">
                                    </div>
                                    <form id="Form2" runat="server" class="form-horizontal" role="form">
                                    <div class="text-center" style="margin-bottom: 25px">
                                         <img id="Img3" class="img-responsive img-thumbnail" runat="server" height="150" width="150" src="Images/PegasusNewLogo.png" style="border-color: #DDDDDD;border-style:solid;border-width:1px;" />
                                        
                                    </div>
                                    <div style="margin-bottom: 25px" class="input-group">
                                        <span class="input-group-addon"><i class="glyphicon glyphicon-user"></i></span>
                                        <asp:TextBox ID="txtUserId" runat="server" TextMode="SingleLine" CssClass="form-control"
                                            Text="" placeholder="Username"></asp:TextBox>
                                    </div>
                                    <%--<div style="margin-bottom: 25px" class="input-group">
                                        <span class="input-group-addon"><i class="glyphicon glyphicon-user"></i></span>
                                        <asp:textbox ID="txtCaptcha" runat="server"  CssClass="form-control" TextMode="SingleLine" placeholder="Please Enter the Text as shown in the Image below" AutoComplete="false"></asp:textbox>
                                    </div>
                                   <div style="margin-bottom: 25px" class="input-group">
                                       <span class="input-group-addon"><i class="glyphicon glyphicon-user"></i></span>
                                        <BotDetect:WebFormsCaptcha ID="ExampleCaptcha" runat="server" />
                                    </div>--%><%--
                                    <div style="margin-top: 10px" class="form-group">
                                        <!-- Button -->
                                        <div class="row">
                                            <div class="col-lg-3 controls">
                                            </div>
                                            <div class="col-lg-6 controls">
                                                <div class="text-center">
                                                    <asp:Button ID="btnForgotPassword" runat="server" CssClass="btn btn-success btn-block btn-lg"
                                                        Text="Reset Password" OnClick="btnForgotPassword_Click" />
                                                </div>
                                            </div>
                                            <div class="col-lg-3 controls">
                                            </div>
                                        </div>
                                    </div>
                                    </form>
                                </div>
                            </div>
                        </div>
                    </asp:View>--%>

                               <asp:View ID="View3" runat="server">
                                    <div class="modal-body">
                                         <center><h4>RESET PASSWORD</h4>
                                        <hr/>
                                        </center>
								        <label>UserID</label>
                                        <div class="form-group input-group">
									        <span class="input-group-addon"><i class="fa fa-lock"  ></i></span>
									       <asp:TextBox ID="txtUserId" runat="server" CssClass="form-control"></asp:TextBox>
								        </div><br/>
								        <%--<label>Confirm Password</label>
								        <div class="form-group input-group">
									        <span class="input-group-addon"><i class="fa fa-lock"  ></i></span>
									       <asp:TextBox ID="TextBox2" runat="server" CssClass="form-control" TextMode="Password"></asp:TextBox>
                                        </div>--%>
                                    </div>
                                    <div class="modal-footer">
                                     
                                     <div class="text-center">
                                        <asp:Button ID="btnForgotPassword" runat="server" CssClass="btn btn-success btn-block btn-lg"
                                                        Text="Reset Password" OnClick="btnForgotPassword_Click" />
                                       </div>
                                    </div>
                                    
                                </asp:View>

                                 <asp:View ID="View1" runat="server">
                                    <div class="modal-body">
                                         <center><h4>RESET PASSWORD</h4>
                                        <hr/>
                                        </center>
								        <label>New Password</label>
                                        <div class="form-group input-group">
									        <span class="input-group-addon"><i class="fa fa-lock"  ></i></span>
									       <asp:TextBox ID="txtnewpassword" runat="server" CssClass="form-control" TextMode="Password"></asp:TextBox>
								        </div><br/>
								        <label>Confirm Password</label>
								        <div class="form-group input-group">
									        <span class="input-group-addon"><i class="fa fa-lock"  ></i></span>
									       <asp:TextBox ID="txtConfirmnewpassword" runat="server" CssClass="form-control" TextMode="Password"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="modal-footer">
                                     
                                     <asp:Button ID="btnchange" runat="server" CssClass="btn btn-success col-md-4 pull-left"  OnClick="btnChangenewPassword_Click" Text="Reset" />
                                     <asp:Button ID="btnCancel" runat="server" CssClass="btn btn-danger col-md-4 pull-right"  OnClick="btnCancel_Click" Text="Cancel"  />
				                       
                                    </div>
                                    
                                </asp:View>


                            </asp:MultiView>
					</div><!-- /.modal-content -->
                       <asp:Label ID="lblUsercode" runat="server" Text="0" Visible="False"></asp:Label>     
				</div>

    </div>
	<div id="footer-sec" style="text-align:center; margin-top: 20px;">
        &copy; 2019 - <%=DateTime.Now.ToString("yyyy") %> |  UG-TECH LTD</a>
    </div>
</form>
 <!-- Bootstrap core JavaScript -->
    <script src="vendor/jquery/jquery.min.js" type="text/javascript"></script>
    <script src="vendor/popper/popper.min.js" type="text/javascript"></script>
    <script src="vendor/bootstrap/js/bootstrap.min.js" type="text/javascript"></script>

    <!-- Plugin JavaScript -->
    <script src="vendor/jquery-easing/jquery.easing.min.js" type="text/javascript"></script>
    <script src="vendor/chart.js/Chart.min.js" type="text/javascript"></script>
    <script src="vendor/datatables/jquery.dataTables.js" type="text/javascript"></script>
    <script src="vendor/datatables/dataTables.bootstrap4.js" type="text/javascript"></script>

    <!-- Custom scripts for this template -->
    <script src="js/sb-admin.min.js" type="text/javascript"></script>
</body>
</html>

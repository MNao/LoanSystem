<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PrintReceipt.aspx.cs" Inherits="PrintReceipt" %>

<%@ Register assembly="CrystalDecisions.Web, Version=13.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" namespace="CrystalDecisions.Web" tagprefix="CR" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">


<!DOCTYPE html>
<html lang="en">

<head id="Head1" runat="server">

    <meta charset="utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <meta name="description" content="" />
    <meta name="author" content="" />

    <title>PRINT RECIEPT</title>

    <!-- Bootstrap Core CSS -->
    <link href="css/bootstrap.min.css" rel="stylesheet" />
    <%--<link href="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.6/css/bootstrap.min.css" rel="stylesheet" integrity="sha256-7s5uDGW3AHqw6xtJmNNtr+OBRJUlgkNJEo78P4b0yRw= sha512-nNo+yCHEyn0smMxSswnf/OnX6/KwJuZTlNZBjauKhTK0c+zT+q5JOCx0UFhXQ6rJR9jg6Es8gPuD2uZcYDLqSw==" crossorigin="anonymous">--%>

    <!-- Custom CSS -->
    <link href="css/PutCustomCssHere.css" rel="stylesheet" />

    <!-- Morris Charts CSS -->
    <link href="css/plugins/morris.css" rel="stylesheet" />

    <!-- Custom Fonts -->
    <link href="font-awesome/css/font-awesome.min.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .container12 {
            width: 320px;
            height: 240px;
            border: 1px solid #d3d3d3;
        }

            .container12 video {
                width: 100%;
                height: 100%;
            }

            .container12 .photoArea {
                border: 2px dashed white;
                width: 140px;
                height: 190px;
                relati e;
                margi: 0 a c n fl;
            }

        .controls {
            clear: both;
        }

        @media print {
            a {
                display: none;
            }
        }

        @page {
            size: auto; /* auto is the initial value */
            margin: 0mm; /* this affects the margin in the printer settings */
        }

        html {
            background-color: #FFFFFF;
            margin: 0px; /* this affects the margin on the HTML before sending to printer */
        }

        body {
            margin: 10mm 15mm 10mm 15mm; /* margin you want for the content */
        }
        .auto-style1 {
            position: relative;
            min-height: 1px;
            float: left;
            width: 66.66666667%;
            left: 0px;
            top: 9px;
            padding-left: 15px;
            padding-right: 15px;
        }
    </style>
    <script type="text/javascript">
        function printDocument() {
            document.getElementById('Button3').style.visibility = 'hidden';
            document.getElementById('Button4').style.visibility = 'hidden';
            //document.getElementById('MsgDiv').style.visibility = 'hidden';
            window.print();
            document.getElementById('Button3').style.visibility = 'visible';
            document.getElementById('Button4').style.visibility = 'visible';
        }


    </script>
</head>

<body style="font-size: 12px;">
    <form runat="server">
        <div class="container" style="padding: 10px;">

            <%-- Print Button --%>
            <div class="row" style="padding: 10px;">
                <div class="text-center">
                    <input id="Button3" accesskey="P" class="btn btn-success" onclick="printDocument()"
                        value="Print Receipt" />
                    <a href="Receipts.aspx">
                        <input id="Button4" accesskey="P" class="btn btn-primary"
                            value="Return" /></a>
                </div>
            </div>

            <%-- Message Label --%>
            <div class="row" id="MsgDiv">
                <div class="text-center">
                    <% 
                        string IsError = Session["IsError"] as string;
                        if (IsError == null)
                        {
                            Response.Write("<div>");

                        }
                        else if (IsError == "True")
                        {
                            Response.Write("<div class=\"alert alert-danger\">");

                        }
                        else
                        {
                            Response.Write("<div class=\"alert alert-success\">");
                        }
                    %>
                    <strong>
                        <asp:Label ID="lblmsg" runat="server"></asp:Label></strong>
                    <%Response.Write("</div>"); %>
                </div>
            </div>
            <%-- title section --%>
            <div class="row" style="padding: 15px; /*border: 1px solid gray;*/ border-top: none;">
                <div class="text-center">
                    <h4>
                        <asp:Label ID="lblHeading" runat="server"></asp:Label></h4>
                </div>
            </div>
        <div class="row">
          <div class="text-center">
        <CR:CrystalReportViewer ID="CrystalReportViewer1" runat="server" AutoDataBind="True" GroupTreeImagesFolderUrl="" Height="6588px" ReportSourceID="Receipt" ToolbarImagesFolderUrl="" ToolPanelWidth="200px" Width="970px" ToolPanelView="None" DisplayStatusbar="False" DisplayToolbar="False" EnableDatabaseLogonPrompt="False" EnableDrillDown="False" EnableParameterPrompt="False" EnableTheming="False" EnableToolTips="False" HasDrilldownTabs="False" HasDrillUpButton="False" HasExportButton="False" HasGotoPageButton="False" HasPageNavigationButtons="False" HasSearchButton="False" HasToggleGroupTreeButton="False" HasToggleParameterPanelButton="False" HasZoomFactorList="False" SeparatePages="False" />
        <CR:CrystalReportSource ID="Receipt" runat="server">
            <Report FileName="site\Bin\reports\PaymentReceipt.rpt">
            </Report>
        </CR:CrystalReportSource>
        </div>
       </div>
        </div>
    </form>
</body>
</html>

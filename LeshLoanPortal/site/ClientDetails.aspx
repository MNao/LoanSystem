<%@ Page Title="" Language="C#" AutoEventWireup="true" CodeFile="ClientDetails.aspx.cs" Inherits="ClientDetails" %>
<%--MasterPageFile="~/MasterMain.master"
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server"> --%>

<!doctype html>
<html>
<head>
    <meta charset="utf-8" />
    <title>KYC DETAILS</title>
    <!-- Bootstrap Core CSS -->
    <link href="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/css/bootstrap.min.css"
        rel="stylesheet" />
    <script type="text/javascript" src="https://code.jquery.com/jquery-1.10.2.js"></script><!--js/jquery-ui.js-->
    <style type="text/css">
        .invoice-box {
            max-width: 800px;
            margin: auto;
            padding: 30px;
            border: 1px solid #eee;
            box-shadow: 0 0 10px rgba(0, 0, 0, .15);
            font-size: 16px;
            line-height: 24px;
            font-family: 'Helvetica Neue', 'Helvetica', Helvetica, Arial, sans-serif;
            color: #555;
        }

            .invoice-box table {
                width: 100%;
                line-height: inherit;
                text-align: left;
            }

                .invoice-box table td {
                    padding: 5px;
                    vertical-align: top;
                }

                .invoice-box table tr td:nth-child(3) {
                    text-align: right;
                }

                .invoice-box table tr td:nth-child(2) {
                    text-align: center;
                    border-bottom: 1px solid #ddd;
                }

                .invoice-box table tr.top table td {
                    padding-bottom: 20px;
                }

                    .invoice-box table tr.top table td.title {
                        font-size: 45px;
                        line-height: 45px;
                        color: #333;
                    }

                .invoice-box table tr.information table td {
                    padding-bottom: 40px;
                }

                .invoice-box table tr.heading td {
                    background: #eee;
                    border-bottom: 1px solid #ddd;
                    font-weight: bold;
                }

                

                .invoice-box table tr.details td {
                    padding-bottom: 20px;
                }

                .invoice-box table tr.item td {
                    border-bottom: 1px solid #eee;
                }

                .invoice-box table tr.item.last td {
                    border-bottom: none;
                }

                .invoice-box table tr.total td:nth-child(3) {
                    border-top: 2px solid #eee;
                    font-weight: bold;
                }

        @media only screen and (max-width: 600px) {
            .invoice-box table tr.top table td {
                width: 100%;
                display: block;
                text-align: center;
            }

            .invoice-box table tr.information table td {
                width: 100%;
                display: block;
                text-align: center;
            }
        }

        .auto-style1 {
            width: 100%;
            height: 2px;
        }

        @media print {
            .hide-print {
                display: none;
            }
        }

        .pull-right {
            text-align: right;
        }
    </style>

</head>
<body>
    <form id="Form1" runat="server">
    
        
                <div class="row" style="padding-top: 100px; padding-bottom: 10px">
                    <div class="text-center">
                        <%--<input id="Button3" runat="server" accesskey="P" class="btn btn-success btn-md hide-print"
                            onclick="printDocument()" value="Print Customer KYC" style="padding-bottom: 5px;" />--%>
                        <%--<a href="Default.aspx" role="button" ID="btnReturn" style="width:150px" class="btn btn-primary btn-sm">Return Home</a>--%>
                        <asp:Button runat="server" ID="btnReturn" CssClass="btn btn-primary btn-md hide-print"
                            Style="width: 150px" OnClick="btnReturn_Click" Text="Go Back" />
                        <h3 class="display-3">
                        <asp:Label ID="Label9" runat="server" />
                    </h3>
                    </div>
                </div>
            <asp:MultiView ID="Multiview1" ActiveViewIndex="0" runat="server">
            <asp:View ID="RecieptView" runat="server">
                <div class="invoice-box table-responsive" style="padding-top: 50px">
                    <table class="table" cellpadding="0" cellspacing="0">
                        <tr class="item top">
                            <td>
                                <img src="Images/UBA.png" width="150" height="150" />
                            </td>
                            <td style="text-align:right"><h1>KYC DETAILS</h1>
                            </td>
                            <td>
                                <img src="Images/UBA.png" width="150" height="150" />
                            </td>
                        </tr>
                        <tr class="heading">
                            <td>CUSTOMER KYC DETAILS
                            </td>
                            <td></td>
                            <td>Value
                            </td>
                        </tr>
                        <tr class="item">
                            <td>
                                <asp:Label ID="Label3" runat="server">Customer Name</asp:Label>
                            </td>
                            <td></td>
                            <td>
                                <asp:Label ID="lblCustName" runat="server">$10.00</asp:Label>
                            </td>
                        </tr>
                        <tr class="item">
                            <td>
                                <asp:Label ID="Label" runat="server">Phone Number</asp:Label>
                            </td>
                            <td></td>
                            <td>
                                <asp:Label ID="lblPhoneNo" runat="server">$10.00</asp:Label>
                            </td>
                        </tr>
                        <tr class="heading">
                            <td>Description
                            </td>
                            <td></td>
                            <td>Value
                            </td>
                        </tr>
                        <tr class="item">
                            <td>
                                <asp:Label ID="Label15" runat="server">KYC ID</asp:Label>
                            </td>
                            <td></td>
                            <td>
                                <asp:Label ID="lblKycId" runat="server">$10.00</asp:Label>
                            </td>
                        </tr>
                        <tr class="item">
                            <td>
                                <asp:Label ID="Label2" runat="server">Gender</asp:Label>
                            </td>
                            <td></td>
                            <td>
                                <asp:Label ID="lblGender" runat="server">$10.00</asp:Label>
                            </td>
                        </tr>
                        <tr class="item">
                            <td>
                                <asp:Label ID="Label1" runat="server">ID Number</asp:Label>
                            </td>
                            <td></td>
                            <td>
                                <asp:Label ID="lblIdNo" runat="server">$10.00</asp:Label>
                            </td>
                        </tr>
                        <tr class="item">
                            <td>
                                <asp:Label ID="Label6" runat="server">Date Of Birth</asp:Label>
                            </td>
                            <td></td>
                            <td>
                                <asp:Label ID="lblDOB" runat="server">$10.00</asp:Label>
                            </td>
                        </tr>
                        <tr class="item">
                            <td>
                                <asp:Label ID="Label5" runat="server">Location</asp:Label>
                            </td>
                            <td></td>
                            <td>
                                <asp:Label ID="lblLocation" runat="server">$10.00</asp:Label>
                            </td>
                        </tr>
                        <tr class="item">
                            <td>
                                <asp:Label ID="Label4" runat="server">Captured By</asp:Label>
                            </td>
                            <td></td>
                            <td>
                                <asp:Label ID="lblCapturedBy" runat="server">$10.00</asp:Label>
                            </td>
                        </tr>
                        <tr class="item">
                            <td>
                                <asp:Label ID="Label10" runat="server">Captured On</asp:Label>
                            </td>
                            <td></td>
                            <td>
                                <asp:Label ID="lblCapturedOn" runat="server">$10.00</asp:Label>
                            </td>
                        </tr>
                        <tr class="item">
                            <td>
                                <asp:Label ID="Label16" runat="server">Customer Photo</asp:Label>
                            </td>
                            <td></td>
                            <td>
                                <a href="ViewKYCDetails.aspx" runat="server">
                                <asp:image ID="Image7" runat="server"/>
                                    </a>
                                <%--<asp:Label ID="Label8" runat="server">$10.00</asp:Label>--%>
                            </td>
                        </tr>
                        <tr class="item">
                            <td>
                                <asp:Label ID="Label7" runat="server">ID Image(Front)</asp:Label>
                            </td>
                            <td></td>
                            <td>
                                <a ><%--href='<%= Url.Action("WebPageImage", "WPMManagement", new { id = actualId }) %>' target="_blank">--%>
                                <asp:image ID="Image1" runat="server"/>
                                    </a>
                                <%--<asp:Label ID="Label8" runat="server">$10.00</asp:Label>--%>
                            </td>
                        </tr>
                        <tr class="item">
                            <td>
                                <a href="ViewKYCDetails.aspx" runat="server">
                                <asp:Label ID="Label8" runat="server">ID Image(Back)</asp:Label>
                                    </a>
                            </td>
                            <td></td>
                            <td>
                                <asp:ImageMap ID="Image2" runat="server"></asp:ImageMap>
                                <asp:image ID="Image2e" runat="server"/>
                            </td>
                        </tr>
                        <tr class="item">
                            <td>
                                <asp:Label ID="Label11" runat="server">Customer Sign</asp:Label>
                            </td>
                            <td></td>
                            <td>
                                <asp:image ID="Image3" runat="server"/>
                            </td>
                        </tr>
                        <tr class="item">
                            <td>
                                <asp:Label ID="Label12" runat="server">Key Facts</asp:Label>
                            </td>
                            <td></td>
                            <td>
                                <asp:image ID="Image4" runat="server"/>
                            </td>
                        </tr>
                        <tr class="item">
                            <td>
                                <asp:Label ID="Label13" runat="server">Reg Form</asp:Label>
                            </td>
                            <td></td>
                            <td>
                                <asp:image ID="Image5" runat="server"/>
                            </td>
                        </tr>
                        <tr class="item">
                            <td>
                                <asp:Label ID="Label14" runat="server">KYC Status</asp:Label>
                            </td>
                            <td></td>
                            <td>
                                <asp:Label ID="lblStatus" runat="server">$10.00</asp:Label>
                            </td>
                        </tr>
                        <tr class="item" id="Reason" runat="server">
                            <td>
                                <asp:Label ID="Label17" runat="server">Rejection Reason</asp:Label>
                            </td>
                            <td></td>
                            <td>
                                <asp:textbox ID="txtRejectionReason" runat="server" TextMode="MultiLine" CssClass="form-control"></asp:textbox>
                            </td>
                        </tr>
                        <tr class="item">
                            <td>
                                <asp:Button ID="btnVerify" runat="server" Width="200px" CssClass="btn btn-success btn-lg" Text="Verify" OnClick="btnVerify_Click" />
                                <%--<asp:Label ID="Label14" runat="server">Reg Form</asp:Label>--%>
                            </td>
                            <td>
                                <asp:Button ID="btnApprove" runat="server" Width="200px" CssClass="btn btn-success btn-lg" Text="Approve" OnClick="btnApprove_Click" />
                                
                            </td>
                            <td>
                                <asp:Button ID="btnReject" runat="server" Width="200px" CssClass="btn btn-danger btn-lg" Text="Reject" OnClick="btnReject_Click" />
                                <asp:Button ID="btnRejectWithReason" runat="server" Width="200px" CssClass="btn btn-danger btn-lg" Text="Reject" OnClick="btnRejectWithReason_Click" />
                                <asp:image ID="Image6" runat="server"/>
                            </td>
                        </tr>
                        <%--<tr class="item last">
                            <td>
                                <asp:Label ID="lblItemCharge" runat="server">Charge Amount</asp:Label>
                            </td>
                            <td></td>
                            <td>
                                <asp:Label ID="lblItemChargeAmount" runat="server">$10.00</asp:Label>
                            </td>
                        </tr>
                        <tr class="total">
                            <td>
                                <asp:Label ID="Label7" runat="server">Total Payment Amount</asp:Label>
                            </td>
                            <td></td>
                            <td>
                                <asp:Label ID="lblTotalTranAmount" runat="server">$10.00</asp:Label>
                            </td>
                        </tr>
                        <tr class="total">
                            <td>
                                <asp:Label ID="Label8" runat="server">Amount In Words</asp:Label>
                            </td>
                            <td></td>
                            <td>
                                <b>
                                    <asp:Label ID="lblAmountInWords" runat="server">$10.00</asp:Label>
                                </b>
                            </td>
                        </tr>--%>
                    </table>
                </div>
            </asp:View>
            <asp:View ID="EmptyView" runat="server">
                <div class="jumbotron text-center" style="padding-top: 10px; padding-bottom: 10px">
                    <h3 class="display-3">
                        <asp:Label ID="lblMsg" runat="server" />
                    </h3>
                </div>
            </asp:View>
            <asp:View runat="server" ID="PollingView">
                <script type="text/javascript">
                    $(document).ready(function () {
                        //$("#image1").attr("src", "small.png");
                        $("#Image2").hover(
                            function () { // mouse pointer goes over the image
                                $(this).attr("height", "400px");
                                $(this).attr("width", "600px");
                            },
                            function () { // mouse pointer goes out
                                //$(this).attr("src", "small.png");
                                $(this).attr("height", "200px");
                                $(this).attr("width", "100px");
                            }
                        );
                    });
                    //var images = [];
                    //$(function () {
                    //    $(document).on("mouseover", "tr td", function () {
                    //        var _image = $(this).find('Image1');
                    //        _image.width = '600px';
                    //        _image.height = '500px';
                    //        _image.finish();
                    //        images[$(this).index()] = {
                    //            width: _image.width(),
                    //            height: _image.height()
                    //        };
                    //        _image.ani
                    //        _image.animate({
                    //            width: '600px',
                    //            height: '500px',
                    //            top: 0,
                    //            left: 0,
                    //            opacity: 1.0
                    //        });
                    //    }).on("mouseout", "tr td", function () {
                    //        var _image = $(this).find('Image1');
                    //        _image.finish();
                    //        _image.animate({
                    //            width: images[$(this).index()].width + "px",
                    //            height: images[$(this).index()].height + "px",
                    //            top: '-50%',
                    //            left: '-50%',
                    //            opacity: 0.5
                    //        });
                    //    });
                    //});



                    function printDocument() {
                        document.getElementById('Button3').style.visibility = 'hidden';
                        //document.getElementById('Button4').style.visibility = 'hidden';
                        //document.getElementById('MsgDiv').style.visibility = 'hidden';
                        window.print();
                        document.getElementById('Button3').style.visibility = 'visible';
                        //document.getElementById('Button4').style.visibility = 'visible';
                    }

                    $(document).ready(function () {
                        SetTime();
                        setTimeout(CallHandler, 3000);
                    });

                    function CallHandler() {

                        var obj = {
                            'VendorId': $('.txtVendorId').val()
                        }

                        $.ajax({
                            url: "PollRequestHandler.ashx",
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            data: obj,
                            responseType: "json",
                            success: OnComplete,
                            error: OnFail
                        });

                        return false;
                    }

                    function SetTime() {
                        var dt = new Date();
                        var time = dt.getHours() + ":" + dt.getMinutes() + ":" + dt.getSeconds();
                        $('#lastQueryTime').text(time);
                        $('#lastQueryTime').css('color', '#449D44');
                    }

                    function OnComplete(Status) {
                        debugger;
                        var StatusCode = Status.StatusCode;
                        var StatusDesc = Status.StatusDesc;
                        var UtilityRef = Status.UtilityRef;
                        var VendorID = $('#txtVendorId').val();
                        var Status = $('#txtStatus').val();
                        var Reason = $('#txtReason').val();
                        var DigitalSignature = $('#txtDigitalSignature').val();
                        var TranId = $('#txtTranId').val();
                        var URL = "Reciept.aspx?Status=" + Status + "&Reason=" + Reason + "&VendorID=" + VendorID + "&DigitalSignature=" + DigitalSignature + "&TranID=" + TranId + "&Generate=true";

                        if (StatusCode == "0") {
                            SetTime();
                            $('#StatusDesc').text('Reciept Generation was Successfull');
                            changeClass('alert-info', 'alert-success');
                            window.location = URL;
                        }
                        else if (StatusCode == "1000" || StatusCode == "122") {
                            SetTime();
                            $('#StatusDesc').text(StatusDesc);
                            changeClass('alert-info', 'alert-info');
                            setTimeout(CallHandler, 3000);
                        }
                        else {
                            SetTime();
                            $('#StatusDesc').text("Reciept Generation Failed: " + StatusDesc + ". Retrying...");
                            changeClass('alert-info', 'alert-danger');
                            setTimeout(CallHandler, 3000);
                        }
                    }

                    function OnFail(result) {
                        SetTime();
                        $('#StatusDesc').text('Reciept Generation Failing. Check your Internet Connection.Retrying...');
                        changeClass('alert-info', 'alert-danger');
                        setTimeout(CallHandler, 3000);
                    }

                    function changeClass(oldClassName, newClassName) {
                        $("#StatusDesc." + oldClassName).removeClass(oldClassName).addClass(newClassName);
                    }

                    $("div.table-responsive")
                          .mouseenter(function () {
                              //$(this).width(100); $(this).height(100);
                              //$(this).find("a").width(300);
                              $("#Image3").mouseover.width(1000);
                          })


                    $().ready(function () {
                        $("Image3").mouseover(function () { $(this).width(200); $(this).height(200) });
                        $("image3").mouseleave(function () { $(this).width(100); $(this).height(100) });
                    })

                    $(document).ready(function(){


                        });

                    jQuery(document).ready(function ($) {
                           
                        $("Image3").mouseover(function () { $(this).width(200); $(this).height(200) });

                        $('#Image3').addimagezoom()

                        $('#Image2').addimagezoom({
                            zoomrange: [7, 7],
                            magnifiersize: [550, 550],
                            magnifierpos: 'right',
                            cursorshade: true,
                            cursorshadecolor: 'Green',
                            cursorshadeopacity: 0.3,
                            cursorshadeborder: '1px solid red',
                            largeimage: '2.png'
                        })
                        $('#Image3').addimagezoom({
                            zoomrange: [3, 10],
                            magnifiersize: [300, 300],
                            magnifierpos: 'right',
                            cursorshade: true,
                            largeimage: '1.png'
                        })

                    })
                </script>
                <div class="container">
                    <div class="row text-center">
                        <input type="hidden" id="txtTranId" runat="server" class="form-control txtVendorCode" /><br />
                        <input type="hidden" id="txtVendorId" runat="server" class="form-control txtVendorId" /><br />
                        <input type="hidden" id="txtStatus" runat="server" class="form-control txtVendorId" /><br />
                        <input type="hidden" id="txtReason" runat="server" class="form-control txtVendorId" /><br />
                        <input type="hidden" id="txtDigitalSignature" runat="server" class="form-control txtVendorId" /><br />
                        <h4>Generating Your Reciept Please Wait:
                        <br />
                        </h4>
                        <br />
                        <h6>Time of Last Status Check:
                        <label id="lastQueryTime">
                        </label>
                            Status:
                        <label id="StatusDesc">
                            Generating Reciept
                        </label>
                        </h6>
                        <img src="Images/processing.gif" alt="" />
                        <br />
                        <br />
                    </div>
                </div>
            </asp:View>
        </asp:MultiView>
    </form>
</body>
</html>
<%--</asp:Content>--%>


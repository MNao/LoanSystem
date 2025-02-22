<%@ Page Language="C#" MasterPageFile="~/MasterMain.master" AutoEventWireup="true" CodeFile="SmsSending.aspx.cs" Inherits="SmsSending" Title="SMS SENDING PANEL" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<%@ Register 
 Assembly="AjaxControlToolkit" 
 Namespace="AjaxControlToolkit" 
 TagPrefix="ajaxToolkit" %>
 <ajaxToolkit:ToolkitScriptManager runat="Server" EnableScriptGlobalization="true"
        EnableScriptLocalization="true" ID="ScriptManager1" />
    <asp:MultiView ID="MultiView1" runat="server" ActiveViewIndex="0">
        <asp:View ID="View1" runat="server">
            <table align="center" style="width: 90%">
                <tr>
                    <td style="width: 100%; height: 2px; text-align: center">
                        <table align="center" cellpadding="0" cellspacing="0" style="width: 50%">
                            <tr>
                                <td class="InterfaceHeaderLabel">
                                    SMS SENDING PANEL</td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td style="width: 100%; height: 2px; text-align: center">
                        <asp:Label ID="lblCredit" runat="server" Font-Bold="True" Text="." Font-Names="Arial" ForeColor="Blue"></asp:Label></td>
                </tr>
                <tr>
                    <td style="width: 100%; height: 2px; text-align: center">
                        <table align="center" cellpadding="0" cellspacing="0" class="style12" style="border-right: #617da6 1px solid;
                            border-top: #617da6 1px solid; border-left: #617da6 1px solid; border-bottom: #617da6 1px solid"
                            width="85%">
                            <tr>
                                <td colspan="3" style="vertical-align: top; height: 5px; text-align: left">
                                    <table align="center" cellpadding="0" cellspacing="0" style="width: 98%">
                                        <tbody>
                                            <tr>
                                                <td class="InterfaceHeaderLabel2" style="height: 18px; text-align: center">
                                                    </td>
                                            </tr>
                                        </tbody>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3" style="vertical-align: top; height: 5px; text-align: left">
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3" style="vertical-align: top; height: 4px; text-align: center; width: 100%;"><table align="center" cellpadding="0" cellspacing="0" style="width: 60%">
                                    <tr>
                                        <td class="InterFaceTableLeftRowUp" style="height: 19px">
                                            Enter Number(s)</td>
                                        <td class="InterFaceTableMiddleRowUp" style="width: 2%; height: 19px">
                                        </td>
                                        <td class="InterFaceTableRightRow" style="width: 66%; height: 19px">
                                            <asp:TextBox ID="txtPhones" runat="server" CssClass="InterfaceTextboxLongReadOnly"
                                                Height="19px"></asp:TextBox></td>
                                    </tr>
                                    <tr>
                                        <td class="InterFaceTableLeftRowUp" style="height: 19px">
                                        </td>
                                        <td class="InterFaceTableMiddleRowUp" style="width: 2%; height: 19px">
                                        </td>
                                        <td class="InterFaceTableRightRow" style="width: 66%; height: 19px">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="InterFaceTableLeftRowUp" style="height: 19px">
                                            List To Send To</td>
                                        <td class="InterFaceTableMiddleRowUp" style="width: 2%; height: 19px;">
                                        </td>
                                        <td class="InterFaceTableRightRow" style="height: 19px; width: 66%;">
                                            <asp:DropDownList ID="ddlists" runat="server" Width="98%" OnDataBound="ddlists_DataBound">
                                            </asp:DropDownList></td>
                                    </tr>
                                    <tr>
                                        <td class="InterFaceTableRightRow" colspan="3" style="height: 2px">
                                        </td>
                                    </tr>
                                        <tr>
                                            <td class="InterFaceTableLeftRowUp" style="height: 20px">
                                                Prefix</td>
                                            <td class="InterFaceTableMiddleRowUp" style="width: 2%; height: 20px;">
                                            </td>
                                            <td class="InterFaceTableRightRow" style="width: 66%; height: 20px;">
                                                <asp:DropDownList ID="ddlPrefix" runat="server" Width="40%" OnSelectedIndexChanged="ddlPrefix_SelectedIndexChanged">
                                                    <asp:ListItem Value="0">NONE</asp:ListItem>
                                                    <asp:ListItem Value="1">DEAR</asp:ListItem>
                                                    <asp:ListItem Value="2">HELLO</asp:ListItem>
                                                </asp:DropDownList></td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3" style="vertical-align: top; height: 4px; text-align: left">
                                </td>
                            </tr>
                            <tr>
                                <td style="vertical-align: top; height: 5px; text-align: left" colspan="3">
                                    <table align="center" cellpadding="0" cellspacing="0" style="width: 98%">
                                        <tr>
                                            <td class="InterFaceTableLeftRowUp" colspan="3" style="width: 100%; height: 19px;
                                                text-align: center">
                                                Message To Send</td>
                                        </tr>
                                        <tr>
                                            <td class="InterFaceTableRightRow" colspan="3" style="height: 19px; width: 100%;">
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="InterFaceTableRightRow" colspan="3" style="width: 100%; text-align: center; height: 20px;">
                                                <asp:TextBox ID="txtMessage" runat="server" CssClass="InterfaceTextboxLongReadOnly"
                                                    Width="60%" Height="61px" TextMode="MultiLine" onKeyDown="textCounter(document.txtMessage,document.txtCount,125)"
                                        onKeyUp="textCounter(document.txtMessage,document.txtCount,125)" style="font-family: Arial"></asp:TextBox></td>
                                        </tr>
                                        <tr>
                                            <td class="InterFaceTableRightRow" colspan="3" style="width: 100%; height: 1px; text-align: center">
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="InterFaceTableRightRow" colspan="3" style="width: 100%; text-align: center; height: 20px;">
                                                <asp:Label ID="lblMessageLength" runat="server" Font-Bold="True" Text="SMS MESSAGE LENGTH : 160"></asp:Label></td>
                                        </tr>
                                        <tr>
                                            <td class="InterFaceTableRightRow" colspan="3" style="width: 100%; text-align: center">
                                                <table align="center" cellpadding="0" cellspacing="0" style="width: 60%">
                                                    <tr>
                                                       Count: <span id="spanDisplay"> </span>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                    &nbsp;</td>
                            </tr>
                            <tr>
                                <td colspan="3" style="vertical-align: top; height: 2px; text-align: center">
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3" style="vertical-align: top; height: 2px; text-align: center">
                                    <asp:Button ID="btnOK" runat="server" Font-Bold="True" Font-Size="9pt" Height="23px"
                                        OnClick="btnOK_Click" Style="font: menu" Text="SEND MESSAGE" Width="150px" /></td>
                            </tr>
                            <tr>
                                <td colspan="3" style="vertical-align: top; width: 100%; height: 5px; text-align: center">
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3" style="vertical-align: top; width: 100%; height: 5px; text-align: center">
                                </td>
                            </tr>
                        </table>

    <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server"
        TargetControlID="txtPhones" ValidChars="0123456789,+" Enabled="True">
    </ajaxToolkit:FilteredTextBoxExtender>
                    </td>
                </tr>
            </table>
        </asp:View>
        &nbsp;&nbsp;
        <asp:View ID="View2" runat="server">
            <table align="center" cellpadding="0" cellspacing="0" class="style12" style="border-right: #617da6 1px solid;
                border-top: #617da6 1px solid; border-left: #617da6 1px solid; border-bottom: #617da6 1px solid"
                width="92%">
                <tr>
                    <td colspan="3" style="vertical-align: top; height: 5px; text-align: left">
                        <table align="center" cellpadding="0" cellspacing="0" style="width: 98%">
                            <tbody>
                                <tr>
                                    <td class="InterfaceHeaderLabel2" style="height: 18px; text-align: center">
                                        SMS CREDIT MESSAGE</td>
                                </tr>
                            </tbody>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td colspan="3" style="vertical-align: top; height: 4px; text-align: left">
                    </td>
                </tr>
                <tr>
                    <td colspan="3" style="vertical-align: top; width: 100%; height: 5px; text-align: center" class="InterFaceTableLeftRowUp">
                        &nbsp;<asp:Label ID="lblerror" runat="server" Font-Bold="True" ForeColor="#C00000"
                            Text="."></asp:Label></td>
                </tr>
                <tr>
                    <td colspan="3" style="vertical-align: top; height: 2px; text-align: center">
                    </td>
                </tr>
                <tr>
                    <td colspan="3" style="vertical-align: top; width: 100%; height: 5px; text-align: center">
                    </td>
                </tr>
            </table>
        </asp:View><asp:View ID="View3" runat="server">
            <table align="center" style="width: 90%">
                <tr>
                    <td style="width: 100%; height: 2px; text-align: center">
                        <table align="center" cellpadding="0" cellspacing="0" style="width: 50%">
                            <tr>
                                <td class="InterfaceHeaderLabel">
                                    SMS CONFIRMATION</td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td style="width: 100%; height: 2px; text-align: center">
                    </td>
                </tr>
                <tr>
                    <td style="width: 100%; height: 2px; text-align: center">
                        <table align="center" cellpadding="0" cellspacing="0" class="style12" style="border-right: #617da6 1px solid;
                            border-top: #617da6 1px solid; border-left: #617da6 1px solid; border-bottom: #617da6 1px solid"
                            width="85%">
                            <tr>
                                <td colspan="3" style="vertical-align: top; height: 5px; text-align: left">
                                    <table align="center" cellpadding="0" cellspacing="0" style="width: 98%">
                                        <tbody>
                                            <tr>
                                                <td class="InterfaceHeaderLabel2" style="height: 18px; text-align: center">
                                                    <asp:Label ID="Label1" runat="server" Text="."></asp:Label></td>
                                            </tr>
                                        </tbody>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3" style="vertical-align: top; height: 5px; text-align: left">
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3" style="vertical-align: top; height: 4px; text-align: center; width: 100%;">
                                    <table align="center" cellpadding="0" cellspacing="0" style="width: 60%">
                                        <tr>
                                            <td class="InterFaceTableLeftRowUp" style="height: 19px">
                                                List To Send To</td>
                                            <td class="InterFaceTableMiddleRowUp" style="width: 2%; height: 19px;">
                                            </td>
                                            <td class="InterFaceTableRightRow" style="height: 19px">
                                                <asp:TextBox ID="txtviewlistname" runat="server" Width="88%" Enabled="False"></asp:TextBox></td>
                                        </tr>
                                        <tr>
                                            <td class="InterFaceTableRightRow" colspan="3" style="height: 2px">
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="InterFaceTableLeftRowUp">
                                                Prefix</td>
                                            <td class="InterFaceTableMiddleRowUp" style="width: 2%">
                                            </td>
                                            <td class="InterFaceTableRightRow">
                                                <asp:TextBox ID="txtviewprefix" runat="server" Enabled="False"></asp:TextBox></td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3" style="vertical-align: top; height: 4px; text-align: left">
                                </td>
                            </tr>
                            <tr>
                                <td style="vertical-align: top; height: 5px; text-align: left" colspan="3">
                                    <table align="center" cellpadding="0" cellspacing="0" style="width: 98%">
                                        <tr>
                                            <td class="InterFaceTableLeftRowUp" colspan="3" style="width: 100%; height: 19px;
                                                text-align: center">
                                                Message To Send</td>
                                        </tr>
                                        <tr>
                                            <td class="InterFaceTableRightRow" colspan="3" style="height: 19px">
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="InterFaceTableRightRow" colspan="3" style="width: 100%; text-align: center">
                                                <asp:TextBox ID="txtViewMessage" runat="server" CssClass="InterfaceTextboxLongReadOnly"
                                                    Height="69px" TextMode="MultiLine" Width="60%" Enabled="False" ReadOnly="True"></asp:TextBox></td>
                                        </tr>
                                        <tr>
                                            <td class="InterFaceTableRightRow" colspan="3" style="width: 100%; height: 1px; text-align: center">
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="InterFaceTableRightRow" colspan="3" style="width: 100%; text-align: center">
                                                <table align="center" cellpadding="0" cellspacing="0" style="width: 60%">
                                                    <tr>
                                                        <td class="InterFaceTableLeftRowUp" style="height: 19px">
                                                            Count</td>
                                                        <td class="InterFaceTableMiddleRowUp" style="width: 2%; height: 19px;">
                                                        </td>
                                                        <td class="InterFaceTableRightRow" style="height: 19px">
                                                            <asp:TextBox ID="TextBox3" runat="server"></asp:TextBox></td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                    &nbsp;</td>
                            </tr>
                            <tr>
                                <td colspan="3" style="vertical-align: top; height: 2px; text-align: center">
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3" style="vertical-align: top; height: 2px; text-align: center">
                                    <asp:Button ID="Button1" runat="server" Font-Bold="True" Font-Size="9pt" Height="23px"
                                        OnClick="Button1_Click" Style="font: menu" Text="CONTINUE TO SEND" Width="150px" />&nbsp;
                                    <asp:Button ID="Button2" runat="server" Font-Bold="True" Font-Size="9pt" Height="23px"
                                        OnClick="Button2_Click" Style="font: menu" Text="CANCEL" Width="150px" /></td>
                            </tr>
                            <tr>
                                <td colspan="3" style="vertical-align: top; width: 100%; height: 5px; text-align: center">
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3" style="vertical-align: top; width: 100%; height: 5px; text-align: center">
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </asp:View>       
    </asp:MultiView>
<script type="text/javascript">
  function count(clientId) {
    var txtMessage = document.getElementById(clientId);
    var spanDisplay = document.getElementById('spanDisplay');
    spanDisplay.innerHTML = txtMessage.value.length;
  }
</script>

</asp:Content>


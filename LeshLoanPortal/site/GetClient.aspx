<%@ Page Language="C#" AutoEventWireup="true" CodeFile="GetClient.aspx.cs" Inherits="GetClient" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
        <link href="css/jquery-ui.css" type="text/css" rel="stylesheet"/>
       <script src="js/jquery-1.11.js" type="text/javascript"></script>
       <script src="js/jquery-ui.js" type="text/javascript"></script>
        <script>
            $(document).ready(function () {
                $("#txtSearch").autocomplete({
                    minLength: 3,
                //source:  [ "c++", "java", "php", "coldfusion", "javascript", "asp", "ruby" ] 
                source: 'GetClientName.ashx'
                });
                $("#txtSearch").autocomplete("option", "position",
                                {my: "right bottom", at: "left bottom", collision: "flip" });
        });
                    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        Search
           <asp:TextBox ID="txtSearch" runat="server"></asp:TextBox>
    </div>
    </form>
</body>
</html>

<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ServerText.aspx.cs" Inherits="IT2163_ApplicationSecurityAssignment.ServerText" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <meta http-equiv="refresh" content="3, url=Login.aspx" />
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:Label Text=" " runat="server" ID="lbl_head" style="font-size:50px;"/>
            <br />
            <asp:Label Text=" " runat="server" ID="lbl_body" style="font-size:25px;"/>
        </div>
    </form>
    <br />
    <br />
    <br />
    <asp:Label Text="This default page will return to login after 3 seconds" runat="server" />
</body>
</html>

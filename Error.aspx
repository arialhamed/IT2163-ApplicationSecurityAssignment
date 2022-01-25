<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Error.aspx.cs" Inherits="IT2163_ApplicationSecurityAssignment.Error" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <h1>An error has occured in the previous page</h1>
        <asp:Button Text="Go back" runat="server" ID="btn_goback" Width="300px" OnClick="btn_goback_Click"/>
    </form>
</body>
</html>

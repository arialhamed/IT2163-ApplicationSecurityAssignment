<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Home.aspx.cs" Inherits="IT2163_ApplicationSecurityAssignment.Home" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <h1>Home</h1>
            <asp:Label Text=" " runat="server" ID="lbl_body1" />
        </div>
        Your profile picture: 
        <asp:Image ID="showPhoto" ImageUrl="" runat="server" alt="Profile picture display is not available right now, we're working on it!" Width="200px"/>
        <fieldset>
            <asp:Button Text="Logout" runat="server" ID="btn_logout" Visible="false" OnClick="btn_logout_Click" />
        </fieldset>
    </form>
</body>
</html>

<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Registration.aspx.cs" Inherits="IT2163_ApplicationSecurityAssignment.Registration" ValidateRequest="true" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script src="https://cdn.jsdelivr.net/npm/@popperjs/core@2.9.2/dist/umd/popper.min.js" integrity="sha384-IQsoLXl5PILFhosVNubq5LC7Qb9DXgDA9i+tQ8Zj3iwWAwPtgFTxbJ8NT4GN1R8p" crossorigin="anonymous"></script>
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.0.2/dist/js/bootstrap.min.js" integrity="sha384-cVKIPhGWiC2Al4u+LWgxfKTRIcfu0JTxR+EQDz/bgldoEyl4H0zUF0QKbrJ0EcQF" crossorigin="anonymous"></script>
    <script type="text/javascript" >
        function validatePassword() {
            var str = document.getElementById('<%=tb_password.ClientID %>').value;
            var newstr = "";
            var maxScoreFalse = false;

            if (str.length < 8) { newstr += "Password must be at least 12 characters long.<br/>"; maxScoreFalse = true; }
            if (str.search(/[a-z]/) == -1) { newstr += "Password require lowercase characters.<br/>"; maxScoreFalse = true; }
            if (str.search(/[A-Z]/) == -1) { newstr += "Password require uppercase characters.<br/>"; maxScoreFalse = true; }
            if (str.search(/[0-9]/) == -1) { newstr += "Password require at least 1 number.<br/>"; maxScoreFalse = true; }
            if (str.search(/[\x21-\x2F\x3A-\x40\x5B-\x60\x7B-\x7E]/) == -1) { newstr += "Password require special characters.<br/>"; maxScoreFalse = true; }
            if (maxScoreFalse) {
                document.getElementById("lbl_pwdchecker").innerHTML = newstr;
                document.getElementById("lbl_pwdchecker").style.color = "Red";
            }
            else {
                document.getElementById("lbl_pwdchecker").innerHTML = "All set! Password is strong.";
                document.getElementById("lbl_pwdchecker").style.color = "Green";
            }
        }
    </script>
</head>
<body>
    <form id="form1" runat="server" class="form-horizontal">
        <fieldset>
            <legend>Registration</legend>
            <div class="form-group">
                <div class="col-sm-10">
                    <asp:Label Text="Name: " runat="server" Width="150px" />
                    <asp:TextBox ID="tb_firstName" runat="server" placeholder="First" Width="96px"/>
                    <asp:TextBox ID="tb_lastName" runat="server" placeholder="Last" Width="96px"/>
                </div>

            </div>
            <div class="form-group">
                <div class="col-sm-10">
                    <asp:Label Text="Email: " runat="server" Width="150px"/>
                    <asp:TextBox ID="tb_email" runat="server" Width="200px"/>
                </div>
            </div>
            <div class="form-group">
                <div class="col-sm-10">
                    <asp:Label Text="Password: " runat="server" for="tb_password" Width="150px"/>
                    <asp:TextBox ID="tb_password" runat="server" TextMode="Password" onkeyup="javascript:validatePassword()" Width="200px"></asp:TextBox>
                </div>
            </div>
        
            <asp:Label Text=" " ID="lbl_pwdchecker" runat="server" />

            <div class="form-group">
                <div class="col-sm-10">
                    <asp:Label Text="Date of Birth:" runat="server" for="tb_dob" Width="150px"/>
                    <asp:TextBox ID="tb_dob" runat="server" TextMode="Date" Width="200px"/>
                </div>
            </div>
        </fieldset>

        <fieldset>
            <legend>Credit Card Information</legend>
            <div class="form-group">
                <div class="col-sm-10">
                    <asp:Label Text="Card Number: " runat="server" for="tb_cardnumber" Width="150px"/>
                    <asp:TextBox ID="tb_cardnumber" runat="server" Width="200px"/>
                </div>
            </div>
            <div class="form-group">
                <div class="col-sm-10">
                    <asp:Label Text="Card CV & Expiry" runat="server" Width="150px"/>
                    <asp:TextBox ID="tb_cardcv" runat="server" Width="80px" placeholder="000"/>
                    <asp:TextBox ID="tb_cardexpiry" runat="server" Width="112px" TextMode="Date" placeholder="MM/YY"/>
                </div>
            </div>
        </fieldset>
        <div class="form-group">
            <asp:Button Text="Submit" runat="server" ID="btn_checkPassword" OnClick="btn_checkPassword_Click" Width="360px"/>
        </div>
    </form>
</body>
</html>

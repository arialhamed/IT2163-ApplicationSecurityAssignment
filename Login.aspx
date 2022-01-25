<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="IT2163_ApplicationSecurityAssignment.Login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Application Security - Login</title>
    <meta name="description" content="This is for Student 201922D's IT2163 Application Security Practical Assignment" />
    <script src="https://cdn.jsdelivr.net/npm/@popperjs/core@2.9.2/dist/umd/popper.min.js" integrity="sha384-IQsoLXl5PILFhosVNubq5LC7Qb9DXgDA9i+tQ8Zj3iwWAwPtgFTxbJ8NT4GN1R8p" crossorigin="anonymous"></script>
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.0.2/dist/js/bootstrap.min.js" integrity="sha384-cVKIPhGWiC2Al4u+LWgxfKTRIcfu0JTxR+EQDz/bgldoEyl4H0zUF0QKbrJ0EcQF" crossorigin="anonymous"></script>
    <script src ="https://www.google.com/recaptcha/api.js?render=6LfGcDIeAAAAAFkstlP8uamypJkwzuGczX7jvHkP"></script>

</head>
<body>
    <form id="form1" runat="server">
        <fieldset>
            <legend style="font-size:40px">Login</legend>
            <div class="form-group">
                <div class="col-sm-10">
                    <asp:Label Text="Email: " runat="server" Width="150px"/>
                    <asp:TextBox runat="server" ID="tb_email" placeholder="someone@example.com" Width="200px"/>
                </div>
            </div>
            <div class="form-group">
                <div class="col-sm-10">
                    <asp:Label Text="Password: " runat="server" Width="150px"/>
                    <asp:TextBox runat="server" ID="tb_password" placeholder="Enter password here" Width="200px" TextMode="Password"/>
                </div>
            </div>
        </fieldset>
        <asp:Label Text=" " runat="server" ID="lbl_error" />
        <input type="hidden" id="g-recaptcha-response" name="g-recaptcha-response" />
        
        <div class="form-group">
            <asp:Button Text="Login" runat="server" ID="btn_login" OnClick="btn_login_Click" Width="180px"/>
            <asp:Button Text="Register" runat="server" ID="btn_register" Width="180px" OnClick="btn_register_Click"/>
        </div>
    </form>
    <script>
        grecaptcha.ready(function () {
            grecaptcha.execute("6LfGcDIeAAAAAFkstlP8uamypJkwzuGczX7jvHkP", { action: 'Login' }).then(function (token) {
                document.getElementById("g-recaptcha-response").value = token;
            });
        });
    </script>
</body>
</html>

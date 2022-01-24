<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Registration.aspx.cs" Inherits="IT2163_ApplicationSecurityAssignment.Registration" ValidateRequest="true" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Application Security - Registration</title>
    <meta name="description" content="This is for Student 201922D's IT2163 Application Security Practical Assignment" />
    <script src="https://cdn.jsdelivr.net/npm/@popperjs/core@2.9.2/dist/umd/popper.min.js" integrity="sha384-IQsoLXl5PILFhosVNubq5LC7Qb9DXgDA9i+tQ8Zj3iwWAwPtgFTxbJ8NT4GN1R8p" crossorigin="anonymous"></script>
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.0.2/dist/js/bootstrap.min.js" integrity="sha384-cVKIPhGWiC2Al4u+LWgxfKTRIcfu0JTxR+EQDz/bgldoEyl4H0zUF0QKbrJ0EcQF" crossorigin="anonymous"></script>
    <script type="text/javascript" >
        function validatePassword() {
            var str = document.getElementById('<%=tb_password.ClientID %>').value;
            var newstr = "";
            var maxScoreFalse = false;

            if (str.length < 12) { newstr += "Password must be at least 12 characters long.<br/>"; maxScoreFalse = true; }
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
    <script src ="https://www.google.com/recaptcha/api.js?render=6LfGcDIeAAAAAFkstlP8uamypJkwzuGczX7jvHkP"></script>
</head>
<body>
    <form id="form1" runat="server" class="form-horizontal">
        <fieldset>
            <legend style="font-size:40px">Registration</legend>
            <table class="table table-borderless">
                <tr>
                    <td><asp:Label Text="Name: " runat="server"/></td>
                    <td><asp:TextBox ID="tb_firstName" runat="server" placeholder="First"/></td>
                    <td><asp:TextBox ID="tb_lastName" runat="server" placeholder="Last"/></td>
                </tr>
                <tr>
                    <td><asp:Label Text="Email: " runat="server"/></td>
                    <td colspan="2"><asp:TextBox ID="tb_email" runat="server"/></td>
                </tr>
                <tr>
                    <td colspan="3"><small>Your email will be used as your ID, hence you will not be able to change it in the future</small></td>
                </tr>
                <tr>
                    <td><asp:Label Text="Password: " runat="server" for="tb_password"/></td>
                    <td><asp:TextBox ID="tb_password" runat="server" TextMode="Password" onkeyup="javascript:validatePassword()"></asp:TextBox></td>
                    <td><asp:Label Text=" " ID="lbl_pwdchecker" runat="server" /></td>
                </tr>
                <tr>
                    <td><asp:Label Text="NRIC:" runat="server" for="tb_nric"/></td>
                    <td><asp:TextBox ID="tb_nric" runat="server"/></td>
                    <td><asp:Label Text=" " ID="lbl_nricchecker" runat="server" /></td>
                </tr>
                <tr>
                    <td><asp:Label Text="Date of Birth:" runat="server" for="tb_dob"/></td>
                    <td colspan="2"><asp:TextBox ID="tb_dob" runat="server" TextMode="Date"/></td>
                </tr>
                <tr>
                    <td><asp:Label Text="Mobile no.:" runat="server" for="tb_mobile"/></td>
                    <td colspan="2"><asp:TextBox ID="tb_mobile" runat="server"/></td>
                </tr>
            </table>
        </fieldset>

        <fieldset>
            <legend style="font-size:25px;">Credit Card Information</legend>
            <table class="table table-borderless">
                <tr>
                    <td><asp:Label Text="Card Number: " runat="server" for="tb_cardnumber"/></td>
                    <td><asp:TextBox ID="tb_cardnumber" runat="server"/></td>
                </tr>
                <tr>
                    <td><asp:Label Text="Card CV & Expiry: " runat="server"/></td>
                    <td><asp:TextBox ID="tb_cardcv" runat="server" TextMode="Number" placeholder="000"/></td>
                    <td><asp:TextBox ID="tb_cardexpiry" runat="server" TextMode="Date" placeholder="MM/YY"/></td>
                </tr>
            </table>
        </fieldset>
        <fieldset>
            <legend style="font-size:25px;">Profile picture</legend>
            <div class="form-group">
                <div class="col-sm-10">
                    <asp:Label Text="Photo: " runat="server" for="tb_photo"/>
                    <input type="file" name="tb_photo" id="tb_photo" value="" />
                </div>
            </div>
        </fieldset>
        <!--fieldset>
            <legend style="font-size:25px;">Captcha</legend>
            <div class="g-recaptcha" data-sitekey="6LfGcDIeAAAAAFkstlP8uamypJkwzuGczX7jvHkP"></div>
        </fieldset-->
            <input type="hidden" id="g-recaptcha-response" name="g-recaptcha-response" />
        <div class="form-group">
            <asp:Button Text="Register" runat="server" ID="btn_register" OnClick="btn_register_Click" Width="360px"/>
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

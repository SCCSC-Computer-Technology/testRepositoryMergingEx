<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ForgotPassword.aspx.cs" Inherits="CPT_275_Capstone.Login.ForgotPassword" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Your Page</title>
    <script src="https://cdn.jsdelivr.net/npm/sweetalert2@11"></script>
    <style>
        body {
            margin: 0;
            padding: 0;
            background-image: url('../ImageFile/background%20image.jpg');
            background-size: cover;
            background-repeat: no-repeat;
            height: 100vh;
            display: flex;
            align-items: center;
            justify-content: center;
            font-family: Arial, sans-serif;
        }

        .forgot-password-box {
            text-align: center;
            padding: 20px;
            background-color: rgba(255, 255, 255, 0.8);
            border-radius: 5px;
            box-shadow: 0 0 10px rgba(0, 0, 0, 0.3);
            max-width: 400px;
            height: 590px;
        }

        .forgot-password-box h2 {
            font-size: 24px;
            margin-bottom: 20px;
        }

        .forgot-password-box p {
            font-size: 16px;
            margin-bottom: 30px;
        }

        .forgot-password-box label {
            display: block;
            margin-bottom: 10px;
            font-weight: bold;
        }

        .forgot-password-box input[type="email"],
        .forgot-password-box input[type="text"],
        .forgot-password-box input[type="password"] {
            padding: 10px;
            border: 1px solid #ccc;
            border-radius: 5px;
            font-size: 16px;
            margin-bottom: 20px;
            box-sizing: border-box; 
            width: 100%; 
        }

        .forgot-password-box input[type="submit"] {
            background-color: #4CAF50;
            color: white;
            padding: 12px 20px;
            border: none;
            border-radius: 5px;
            font-size: 16px;
            cursor: pointer;
            width: 100%; 
        }

        .forgot-password-box input[type="submit"]:hover {
            background-color: #45a049;
        }

        .forgot-password-box a {
            color: #333;
            text-decoration: none;
        }
    </style>
     <script>
         function ShowEmailSentMessage() {
             Swal.fire({
                 icon: 'success',
                 title: 'Email Sent',
                 text: 'The password has been changed and email has been sent successfully.',
             });
         }
     </script>
</head>
<body>

    <div class="container">
        <div class="forgot-password-box">
            <h2>Forgot Password</h2>
            <p>Enter your email and driver's license number to reset your password.</p>
            <form id="form1" runat="server">
                <div>
                    <label for="txtemail">Email:</label>
                    <asp:TextBox ID="txtemail" runat="server" Width="200px" TextMode="Email"></asp:TextBox>
                </div>
                <div>
    <label for="txtdl">Driver's License Number:</label>
    <asp:TextBox ID="txtdl" runat="server" Width="200px"></asp:TextBox>
                    <div>
                    <asp:Label ID="Label1" runat="server" Text="New Password:" Font-Bold="True" Font-Size="Large"></asp:Label>
                        <div>
                        <asp:TextBox ID="txtnewpassword" runat="server" Width="204px" Height="32px" style="margin-left: 0px" TextMode="Password"></asp:TextBox>
                            <div>
                            <asp:Label ID="Label2" runat="server" Text="Confirm Password:" Font-Bold="True" Font-Size="Large"></asp:Label>
                                <div style="height: 49px">
                                <asp:TextBox ID="txtconfirmpassword" runat="server" Width="208px" Height="34px" TextMode="Password"></asp:TextBox>
                                </div>
                                </div>
                            </div>
                        </div>
                    <div style="height: 45px">
        <asp:Label ID="lblerrormessage" runat="server" CssClass="error-message" ForeColor="Red"></asp:Label>
                        </div>
</div>
                <div style="height: 52px; margin-top: 2px;">
                    <asp:Button ID="btnreset" runat="server" OnClick="btnreset_Click" Text="Reset Password" Width="150px" />
                </div>
            </form>
            <br />
            <a href="Login.aspx">Back to Login</a>
        </div>
    </div>
</body>
</html>
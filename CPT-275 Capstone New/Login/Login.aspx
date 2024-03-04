<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="CPT_275_Capstone.Login.Login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
   <style>
       *{
              margin: 0;
              padding: 0;
              box-sizing: border-box;
         }
       body{
                height: 100vh;
                display: flex;
                align-items: center;
                justify-content: center;
                
            }
       .container {
           display: flex;
           flex-direction: column;
           width: 337px;
           ;
           padding: 15px;
           border-radius: 5px;
       }
       input{
           margin: 5px 0;
           padding: 7px;
       }
       button{
             border-style: none;
           border-color: inherit;
           border-width: medium;
           height: 34px;
              margin: 5px 0;
              background: skyblue;
              border-radius: 5px;
              color : #333;
           width: 176px;
       }
       button:hover{
              background: #333;
              color: skyblue;
       }
       .forgot-password {
           text-align:center;
           margin: 5px 0;
       }
       .forgot-password{
            color: #333;
           width: 149px;
       }
       }
    </style>
    <script>
        function showErrorMessage(message) {
        var errorMessage = document.getElementById('errorMessage');
        errorMessage.textContent = message;
        errorMessage.style.display = 'block';
    }
    </script>
<body style="background-image: url('../ImageFile/background image.jpg')">
    <form id="form1" runat="server">
        <body style="background-image: url('../ImageFile/background image.jpg')">
            <div class ="center">
                <p style="height: 175px">
                    <asp:Image ID="Image1" runat="server" Height="167px" ImageUrl="~/ImageFile/scclogo1.png" Width="189px" />
                </p>
                <h1 style="background-image: url('../ImageFile/background%20image.jpg'); height: 0px;">&nbsp;</h1>
                    <div class ="container" dir="ltr" style="background-image: url('../ImageFile/background%20image.jpg'); height: 301px;">
                        &nbsp;<asp:Label ID="Label1" runat="server" Text="Email:"></asp:Label>
                        <asp:TextBox ID="txtEmail" runat="server" TextMode="Email" Width="295px"></asp:TextBox>
                        <asp:Label ID="Label2" runat="server" Text="Password:"></asp:Label>
                        <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" Width="295px">Enter Password</asp:TextBox>
                        <asp:Button ID="btnlogin" runat="server" BackColor="SkyBlue" BorderStyle="Ridge" OnClick="btnlogin_Click" Text="Login" Width="295px" />
                        <%--<div class="forgot-password">
                            <a href="ForgotPassword.aspx">Forgot Password?</a>--%>
                        <asp:Button ID="btnForgotPassword" runat="server" Text="Forgot Password?" Width="124px" BackColor="SkyBlue" OnClick="btnForgotPassword_Click" />
                        <asp:Button ID="btnRegister" runat="server" Text="Register" Width="124px" BackColor="SkyBlue" OnClick="btnRegister_Click" />
                    </div>
                <div>
                       <p id="errorMessage" style="color: red; display: none;"></p>
                 </div>
                </body>
                </form>
            </body>
 
</html>


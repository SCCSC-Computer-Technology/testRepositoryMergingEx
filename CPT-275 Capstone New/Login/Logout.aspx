<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Logout.aspx.cs" Inherits="CPT_275_Capstone.Login.Logout" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Logout</title>
</head>
<style>
    * {
        margin: 0;
        padding: 0;
        box-sizing: border-box;
    }
    body {
        height: 100vh;
        display: flex;
        align-items: center;
        justify-content: center;
    }
    .container {
        display: flex;
        flex-direction: column;
        align-items: center;
        padding: 15px;
        border-radius: 5px;
    }
    .custom-button {
        border-style: none;
        border-color: inherit;
        border-width: medium;
        height: 34px;
        margin: 10px 0;
        background: blue;
        border-radius: 5px;
        color: white;
        width: 150px;
    }
    .custom-button:hover {
        background: #333;
        color: skyblue;
    }
</style>
   <script type ="text/javascript">  

       window.onload = window.history.forward(0);  //calling function on window onload

   </script> 
<body style="background-image: url('../ImageFile/background image.jpg')">
    <form id="form1" runat="server">
        <div class="container">
            <h1>You're now logged out.</h1>
            <h1>Thank you for using SCC Fleet Services</h1>
            <asp:Button ID="btnBackToLogin" runat="server" CssClass="custom-button" Text="Back to Login" OnClick="btnBackToLogin_Click" />
        </div>
    </form>
</body>
</html>
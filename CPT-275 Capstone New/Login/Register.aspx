<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Register.aspx.cs" Inherits="CPT_275_Capstone.Login.Register" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<script src="https://cdn.jsdelivr.net/npm/sweetalert2@11"></script>
<head runat="server">
    <title>Registration</title>
    <style>
    body {
        background-image: url('../ImageFile/background%20image.jpg');
            background-size: cover;
            background-repeat: no-repeat;
        font-family: Arial, sans-serif;
        margin: 0;
        padding: 0;
    }

    .container {
        max-width: 500px;
        margin: 0 auto;
        padding: 20px;
        background-color: #fff;
        border-radius: 5px;
        box-shadow: 0 2px 5px rgba(0, 0, 0, 0.1);
        text-align: center;
    }

    h2 {
        text-align: center;
        margin-bottom: 20px;
        color: #333;
    }

    label {
        display: block;
        margin-bottom: 5px;
        color: #555;
        text-align: left;
    }

    input[type="text"],
    input[type="password"] {
        width: 100%;
        padding: 10px;
        border: 1px solid #ccc;
        border-radius: 3px;
        font-size: 16px;
        color: #555;
        text-align: center; 
        margin: 0 auto; 
        box-sizing: border-box; 
    }

    .error-message {
        color: red;
        margin-top: 10px;
        text-align: center;
    }

    .btn-container {
        text-align: center;
        margin-top: 20px;
    }

    .btn {
        display: inline-block;
        padding: 10px 20px;
        border: none;
        border-radius: 3px;
        font-size: 16px;
        color: #fff;
        background-color: #4caf50;
        cursor: pointer;
        transition: background-color 0.3s ease;
    }

    .btn:hover {
        background-color: #45a049;
    }

    .btn-secondary {
        background-color: #777;
    }

    .btn-secondary:hover {
        background-color: #555;
    }
    <script>
        function ShowAccountCreated() {
            Swal.fire({
                icon: 'success',
                title: 'Account has been created',
                text: 'The account has been created successfully.',
            });
        }
    </script>
</style>
</head>
<body>
    <div class="container">
        <h2>Sign Up for Your New Account</h2>
        <form id="form1" runat="server">
            <label for="FirstName">First Name:</label>
            <asp:TextBox ID="FirstName" runat="server"></asp:TextBox>
            <asp:RequiredFieldValidator ID="FirstNameRequired" runat="server" ControlToValidate="FirstName" ErrorMessage="First Name is required." ValidationGroup="RegistrationValidation">*</asp:RequiredFieldValidator>

            <label for="LastName">Last Name:</label>
            <asp:TextBox ID="LastName" runat="server"></asp:TextBox>
            <asp:RequiredFieldValidator ID="LastNameRequired" runat="server" ControlToValidate="LastName" ErrorMessage="Last Name is required." ValidationGroup="RegistrationValidation">*</asp:RequiredFieldValidator>

            <label for="DLNumber">Driver's License Number:</label>
            <asp:TextBox ID="DLNumber" runat="server"></asp:TextBox>
            <asp:RequiredFieldValidator ID="DLNumberRequired" runat="server" ControlToValidate="DLNumber" ErrorMessage="Driver's License Number is required." ValidationGroup="RegistrationValidation">*</asp:RequiredFieldValidator>

            <label for="DLState">Driver's License State:</label>
            <asp:TextBox ID="DLState" runat="server"></asp:TextBox>
            <asp:RequiredFieldValidator ID="DLStateRequired" runat="server" ControlToValidate="DLState" ErrorMessage="Driver's License State is required." ValidationGroup="RegistrationValidation">*</asp:RequiredFieldValidator>

            <label for="Email">E-mail:</label>
            <asp:TextBox ID="Email" runat="server"></asp:TextBox>
            <asp:RequiredFieldValidator ID="EmailRequired" runat="server" ControlToValidate="Email" ErrorMessage="E-mail is required." ValidationGroup="RegistrationValidation">*</asp:RequiredFieldValidator>

            <label for="Phone">Phone Number:</label>
            <asp:TextBox ID="Phone" runat="server"></asp:TextBox>
            <asp:RequiredFieldValidator ID="PhoneRequired" runat="server" ControlToValidate="Phone" ErrorMessage="Phone Number is required." ValidationGroup="RegistrationValidation">*</asp:RequiredFieldValidator>

            <label for="Password">Password:</label>
            <asp:TextBox ID="Password" runat="server" TextMode="Password"></asp:TextBox>
            <asp:RequiredFieldValidator ID="PasswordRequired" runat="server" ControlToValidate="Password" ErrorMessage="Password is required." ValidationGroup="RegistrationValidation">*</asp:RequiredFieldValidator>

            <label for="ConfirmPassword">Confirm Password:</label>
            <asp:TextBox ID="ConfirmPassword" runat="server" TextMode="Password"></asp:TextBox>
            <asp:RequiredFieldValidator ID="ConfirmPasswordRequired" runat="server" ControlToValidate="ConfirmPassword" ErrorMessage="Confirm Password is required." ValidationGroup="RegistrationValidation">*</asp:RequiredFieldValidator>

            <asp:Label ID="lblErrorMessage" runat="server" CssClass="error-message"></asp:Label>

            <div class="btn-container">
                <asp:Button ID="RegisterButton" runat="server" Text="Register" OnClick="RegisterButton_Click" CssClass="btn" />
                <asp:Button ID="btnLoginButton" runat="server" Text="Back to Login" OnClick="LoginButton_Click" CssClass="btn btn-secondary" />
            </div>
        </form>
    </div>
</body>
</html>
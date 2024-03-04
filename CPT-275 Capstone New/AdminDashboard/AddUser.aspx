<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AddUser.aspx.cs" Inherits="CPT_275_Capstone.AdminDashboard.AddDriver" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
<!DOCTYPE html>
<script src="https://cdn.jsdelivr.net/npm/sweetalert2@11"></script>
<html>
<head>
    <title>Add User</title>
    <style>
        body {
            background-image: url('../ImageFile/background image.jpg');
        }
    </style>
    <style>
        label {
            display: block;
            margin-bottom: 5px;
            font-weight: bold;
        }

        input[type="text"],
        input[type="password"],
        select {
            width: 300px;
            height: 30px;
            
        }

        input[type="submit"],
        input[type="button"] {
            width: 100px;
            text-align: center;
        }

        .vehicle-list {
            height: 150px;
        }

        .form-row {
            margin-bottom: 20px;
        }

        .form-actions {
            margin-top: 20px;
        }
        .button {
            display: inline-block;
            padding: 10px 20px;
            font-size: 16px;
            background-color: #4CAF50;
            color: white;
            border: none;
            cursor: pointer;
        }
        
        .button:hover {
            background-color: #45a049;

        }
        .button-container {
            text-align: center;
        }
        .listbox-style {
    font-weight: bold;
    font-size: larger;
    height: 150px; 
    width: 300px;
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
    <h1>Add User</h1>
    <form id="driverForm">
        <div class="form-row">
            <label for="firstName">First Name:</label>           
            <asp:TextBox ID="txtfirstname" runat="server"></asp:TextBox>
            <asp:RequiredFieldValidator ID ="rfvyear" runat="server" ControlToValidate="txtfirstname" ErrorMessage="First Name is required" ForeColor="Red"></asp:RequiredFieldValidator>
        </div>

        <div class="form-row">
            <label for="lastName">Last Name:</label>
            <asp:TextBox ID="txtlastname" runat="server"></asp:TextBox>
             <asp:RequiredFieldValidator ID ="RequiredFieldValidator1" runat="server" ControlToValidate="txtlastname" ErrorMessage="Last Name is required" ForeColor="Red"></asp:RequiredFieldValidator>
        </div>

        <div class="form-row">
            <label for="email">Email:</label>
            <asp:TextBox ID="txtemail" runat="server"></asp:TextBox>
             <asp:RequiredFieldValidator ID ="RequiredFieldValidator2" runat="server" ControlToValidate="txtemail" ErrorMessage="Email is required" ForeColor="Red"></asp:RequiredFieldValidator>
        </div>

        <div class="form-row">
            <label for="password">Password:</label>
            <asp:TextBox ID="txtpassword" runat="server" TextMode="Password"></asp:TextBox>
             <asp:RequiredFieldValidator ID ="RequiredFieldValidator3" runat="server" ControlToValidate="txtpassword" ErrorMessage="Password is required" ForeColor="Red"></asp:RequiredFieldValidator>
        </div>

        <div class="form-row">
            <label for="dlNumber">DL#:</label>
            <asp:TextBox ID="txtdlNumber" runat="server"></asp:TextBox>
             <asp:RequiredFieldValidator ID ="RequiredFieldValidator4" runat="server" ControlToValidate="txtdlNumber" ErrorMessage="DL is required" ForeColor="Red"></asp:RequiredFieldValidator>
        </div>

        <div class="form-row">
            <label for="dlState">DL State:</label>
            <asp:TextBox ID="txtdlState" runat="server"></asp:TextBox>
             <asp:RequiredFieldValidator ID ="RequiredFieldValidator5" runat="server" ControlToValidate="txtdlState" ErrorMessage="DL State is required" ForeColor="Red"></asp:RequiredFieldValidator>
        </div>

        <div class="form-row">
            <label for="phone">Phone Number:</label>
            <asp:TextBox ID="txtphone" runat="server"></asp:TextBox>
             <asp:RequiredFieldValidator ID ="RequiredFieldValidator6" runat="server" ControlToValidate="txtphone" ErrorMessage="Phone number is required" ForeColor="Red"></asp:RequiredFieldValidator>
        </div>

        <div class="form-row">
            <label for="accountType">Account Type:</label>
            <select id="accountType" name="accountType" runat="server">
    <option value="Admin">Administrator</option>
    <option value="Basic">Basic User</option>
</select>
        </div>
        <script>
            // Get the selected value
            const selectElement = document.getElementById("accountType");
            const selectedValue = selectElement.value;

            // Send the selected value to the server-side code for database insertion

            fetch("/your-server-endpoint", {
                method: "POST",
                body: JSON.stringify({ users_type: selectedValue }),
                headers: {
                    "Content-Type": "application/json",
                },
            })
                .then((response) => response.json())
                .then((data) => {
                    // Handle the server response if needed
                    console.log(data);
                })
                .catch((error) => {
                    // Handle any errors that occurred during the request
                    console.error(error);
                });
        </script>

        <div class="form-row">
            <label for="defaultVehicle">Default Vehicle:</label>
            <asp:ListBox ID="lstvehicle" runat="server" CssClass="listbox-style" ></asp:ListBox>
            <div>
        <asp:Label ID="lblerror" runat="server" Font-Size="Large" Font-Bold="True" ForeColor="Red"></asp:Label>
                </div>
        </div>

        <div class="form-actions">
                <asp:Button ID="btnSubmit" runat="server" Text="Submit" OnClick="SubmitButton_Click"  class="button" />
      <input type="button" class="button" value="Cancel" onclick="goBack()">
        </div>
    </form>

    <script>
        function goBack() {
            // Go back to the previous screen
            history.back();
        }
    </script>
</body>
</html>
    </asp:Content>

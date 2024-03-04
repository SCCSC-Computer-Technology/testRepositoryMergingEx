<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="UpdateUser.aspx.cs" Inherits="CPT_275_Capstone.AdminDashboard.UpdateUser" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <!DOCTYPE html>
    <html>
    <head>
        <title>Edit User</title>
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
                width: 300px;
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
        </style>
    </head>
    <body>
        <h1>EDIT USER</h1>
        <form id="driverForm">
            <div class="form-row">
                <label for="firstName">First Name:</label>
                <asp:TextBox ID="txtfirstname" runat="server"></asp:TextBox>
            </div>

            <div class="form-row">
                <label for="lastName">Last Name:</label>
                <asp:TextBox ID="txtlastname" runat="server"></asp:TextBox>
            </div>

            <div class="form-row">
                <label for="email">Email:</label>
                <asp:TextBox ID="txtemail" runat="server"></asp:TextBox>
            </div>

            <div class="form-row">
                <label for="password">Password:</label>
                <asp:TextBox ID="txtpassword" runat="server"></asp:TextBox>
            </div>

            <div class="form-row">
                <label for="dlNumber">DL#:</label>
                <asp:TextBox ID="txtdlNumber" runat="server"></asp:TextBox>
            </div>

            <div class="form-row">
                <label for="dlState">DL State:</label>
                <asp:TextBox ID="txtdlState" runat="server"></asp:TextBox>
            </div>

            <div class="form-row">
                <label for="phone">Phone Number:</label>
                <asp:TextBox ID="txtphone" runat="server"></asp:TextBox>
            </div>

            <div class="form-row">
                <label for="accountType">Account Type:</label>
                <asp:DropDownList ID="accountType" runat="server">
                    <asp:ListItem Text="Admin" Value="Admin               "></asp:ListItem>              
                    <asp:ListItem Text="Basic" Value="Basic               "></asp:ListItem>
                </asp:DropDownList>
            </div>

            <div class="form-row">
                <label for="defaultVehicle">Default Vehicle:</label>
                <asp:ListBox ID="lstvehicle" runat="server" CssClass="listbox-style"></asp:ListBox>
                <div>
                    <asp:Label ID="lblerror" runat="server" Font-Size="Large" Font-Bold="True"></asp:Label>
                </div>
            </div>

            <div class="form-actions">
                <asp:Button ID="btnSubmit" runat="server" CssClass="button" Text="Submit" OnClick="btnSubmit_Click" />
                <input type="button" class="button" value="Cancel" onclick="goBack()" />
            </div>
        </form>

        <script>
            function goBack() {
                history.back();
            }

            document.getElementById("<%= accountType.ClientID %>").addEventListener("change", function () {
                const selectedValue = this.value;

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
            });
        </script>
    </body>
    </html>
    <asp:HiddenField runat="server" ID="userIdHiddenField" />
</asp:Content>
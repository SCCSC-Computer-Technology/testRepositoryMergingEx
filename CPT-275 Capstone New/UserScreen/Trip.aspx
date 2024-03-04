<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Trip.aspx.cs" Inherits="CPT_275_Capstone.UserScreen.WebForm2" %>

  <!DOCTYPE html>
<html>
<head>
    <title>Trip Information</title>
     <style>
    body {
        background-image: url('../ImageFile/background image.jpg');
    }

    .form-row {
        display: flex;
        align-items: center;
    }

    .form-label {
        width: 200px;
        text-align: left;
        padding-right: 10px;
        font-weight: bold;
    }

    .form-field {
        width: 300px; 
        font-size: 20px;
        text-align: left;
    }

    .button {
        padding: 10px 20px;
        font-size: 16px;
        background-color: #4CAF50;
        color: white;
        border: none;
    }

    .button:hover {
        background-color: #45a049;
    }

    .auto-style1 {
        display: flex;
        align-items: center;
        text-align: left;
    }
</style>
    <script>
        window.onload = function () {
            var dateInput = document.getElementById("<%= txtDate.ClientID %>");
            var today = new Date().toISOString().split("T")[0];
            dateInput.value = today;
        };
    </script>
</head>
<body>
     <form id="form1" runat="server">
     <h1>Trip Information</h1>
            <div class="text-start">
            <div class="auto-style1">
                <label for="txtDate" class="form-label">
                    <div class="text-start">
                        Date:
                    </div>
                </label>&nbsp;
                <asp:TextBox ID="txtDate" runat="server" TextMode="Date" Width="176px"></asp:TextBox>
                <asp:RequiredFieldValidator ID ="rfvDate" runat="server" ControlToValidate="txtDate" ErrorMessage="Date is required" ForeColor="Red"></asp:RequiredFieldValidator>
            </div>
            <div class="text-start">
                <br />
            </div>
            <div class="auto-style1">
                <label for="txtBeginMileage" class="form-label">Beginning Mileage:</label>&nbsp;
                <asp:TextBox ID="txtBeginningMileage" runat="server" AutoPostBack="true" OnTextChanged="CalculateTotalMiles" Width="294px" TextMode="Number"></asp:TextBox>
                <asp:RequiredFieldValidator ID ="rfvBeginningMileage" runat="server" ControlToValidate="txtBeginningMileage" ErrorMessage="Beginning Mileage is required" ForeColor="Red"></asp:RequiredFieldValidator>
            </div>
            <div class="text-start">
                <br />
            </div>
            <div class="auto-style1">
                <label for="txtDestination" class="form-label">Destination:</label>&nbsp;
                <asp:TextBox ID="txtDestination" runat="server" Width="293px"></asp:TextBox>
                <asp:RequiredFieldValidator ID ="rfvDestination" runat="server" ControlToValidate="txtDestination" ErrorMessage="Destination is required" ForeColor="Red"></asp:RequiredFieldValidator>
            </div>
            <div class="text-start">
                <br />
            </div>
            <div class="auto-style1">
                <label for="txtPurpose" class="form-label">Purpose of Trip:</label>&nbsp;
                <asp:TextBox ID="txtPurposeoftrip" runat="server" MaxLength="50" TextMode="MultiLine" Width="295px"></asp:TextBox>
                <asp:RequiredFieldValidator ID ="rfvPurpose" runat="server" ControlToValidate="txtPurposeoftrip" ErrorMessage="Purpose is required" ForeColor="Red"></asp:RequiredFieldValidator>
            </div>
            <div class="text-start">
                <br />
            </div>
            <div class="auto-style1">
                <label for="txtEndingMileage" class="form-label">Ending Mileage:</label>&nbsp;
                <asp:TextBox ID="txtEndingMileage" runat="server" AutoPostBack="true" OnTextChanged="CalculateTotalMiles" Width="293px" TextMode="Number"></asp:TextBox>
                <asp:RequiredFieldValidator ID ="rfvEndingMileage" runat="server" ControlToValidate="txtEndingMileage" ErrorMessage="Ending Mileage is required" ForeColor="Red"></asp:RequiredFieldValidator>
            </div>
            <div class="text-start">
                <br />
            </div>
            <div class="form-row">
                <label for="txtTotal" class="form-label">Total Miles:</label>&nbsp;
                <asp:Label ID="lbltotalmilese" runat="server" Font-Bold="True" Font-Italic="False" Font-Size="Large"></asp:Label>
            </div>
            <div class="text-start">
                <asp:Label ID="lblselectedcar" runat="server" Font-Bold="True" Font-Size="X-Large"></asp:Label>
                <br />
            </div>
                <asp:Label ID="lblmessage" runat="server" Font-Bold="True" Font-Size="X-Large"></asp:Label>
            <br />
                <asp:Button ID="btnsubmit" runat="server" Height="39px" OnClick="btnsubmit_Click" Text="Submit" Width="103px" />
                <asp:Button ID="btncancel" runat="server" Height="39px" OnClick="btncancel_Click" CausesValidation ="false" Text="Cancel" Width="103px" />
        </div>
    </form>
     </form>
</body>
</html>

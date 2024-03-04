<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AddVehicle.aspx.cs" Inherits="CPT_275_Capstone.AdminDashboard.AddVehicle" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <!DOCTYPE html>
<html>
<head>
  <title>ADD VEHICLE</title>
    <style>
        body {
            background-image: url('../ImageFile/background image.jpg');
         
        }
    </style>
  <style>
    label {
      display: block;
      margin-top: 10px;
      font-weight: bold;
    }
    
    input[type="text"] {
      width: 200px;
    }
    
    .form-row {
      margin-bottom: 20px;
    }
    
    .form-actions {
      margin-top: 20px;
    }
    .button {
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
  </style>
</head>
<body>
  <h1>ADD VEHICLE</h1>
  
  <form>
    <div class="form-row">
      <label for="year">Year:</label>&nbsp;
        <asp:TextBox ID="txtyear" runat="server" TextMode="Number" Width="198px"></asp:TextBox>
        <asp:RequiredFieldValidator ID ="rfvyear" runat="server" ControlToValidate="txtyear" ErrorMessage="Year is required" ForeColor="Red"></asp:RequiredFieldValidator>
    </div>
      
    <div class="form-row">
      <label for="make">Make:</label>&nbsp;
        <asp:TextBox ID="txtmake" runat="server"></asp:TextBox>
       <asp:RequiredFieldValidator ID ="rfvmake" runat="server" ControlToValidate="txtmake" ErrorMessage="Make is required" ForeColor="Red"></asp:RequiredFieldValidator>
    </div>
    
    <div class="form-row">
      <label for="model">Model:</label>&nbsp;
        <asp:TextBox ID="txtmodel" runat="server"></asp:TextBox>
      <asp:RequiredFieldValidator ID ="rfvmodel" runat="server" ControlToValidate="txtmodel" ErrorMessage="Model is required" ForeColor="Red"></asp:RequiredFieldValidator>
    </div>

    <div class="form-row">
      <label for="vin">VIN#:</label>&nbsp;
        <asp:TextBox ID="txtvin" runat="server"></asp:TextBox>
<asp:RequiredFieldValidator ID ="rfvvin" runat="server" ControlToValidate="txtvin" ErrorMessage="VIN is required" ForeColor="Red"></asp:RequiredFieldValidator>
    </div>
    
    <div class="form-row">
      <label for="license">License Plate:</label>&nbsp;
        <asp:TextBox ID="txtlicenseplate" runat="server"></asp:TextBox>
       <asp:RequiredFieldValidator ID ="rfvlicenseplate" runat="server" ControlToValidate="txtlicenseplate" ErrorMessage="License Plate is required" ForeColor="Red"></asp:RequiredFieldValidator>
    </div>
    
    <div class="form-row">
      <label for="odometer">Odometer:</label>&nbsp;
        <asp:TextBox ID="txtodometer" runat="server" TextMode="Number"></asp:TextBox>
<asp:RequiredFieldValidator ID ="rfvodometer" runat="server" ControlToValidate="txtodometer" ErrorMessage="Odometer is required" ForeColor="Red"></asp:RequiredFieldValidator>
      <div>
        <asp:Label ID="lblerror" runat="server" Font-Size="Large" ForeColor="Red"></asp:Label>
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
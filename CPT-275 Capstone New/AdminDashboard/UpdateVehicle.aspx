<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="UpdateVehicle.aspx.cs" Inherits="CPT_275_Capstone.AdminDashboard.UpdateVehicle" OnLoad="Page_Load" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <!DOCTYPE html>
<html>
<head>
  <title>Edit Vehicle</title>
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
  <h1>EDIT VEHICLE</h1>
  
  <form>
    <div class="form-row">
      <label for="year">Year:</label>&nbsp;
        <asp:TextBox ID="txtyear" runat="server"></asp:TextBox>
    </div>
      
    <div class="form-row">
      <label for="make">Make:</label>&nbsp;
        <asp:TextBox ID="txtmake" runat="server"></asp:TextBox>
    </div>
    
    <div class="form-row">
      <label for="model">Model:</label>&nbsp;
        <asp:TextBox ID="txtmodel" runat="server"></asp:TextBox>
    </div>

    <div class="form-row">
      <label for="vin">VIN#:</label>&nbsp;
        <asp:TextBox ID="txtvin" runat="server"></asp:TextBox>
    </div>
    
    <div class="form-row">
      <label for="license">License Plate:</label>&nbsp;
        <asp:TextBox ID="txtlicenseplate" runat="server"></asp:TextBox>
    </div>
    
    <div class="form-row">
      <label for="odometer">Odometer:</label>&nbsp;
        <asp:TextBox ID="txtodometer" runat="server" ></asp:TextBox>
    </div>
    
    <div class="form-actions">
                <asp:Button ID="btnSubmit" runat="server" class="button" Text="Submit" OnClick="btnSubmit_Click" />
      <asp:Button ID="btnCancel" runat="server" class="button" Text="Cancel" OnClick="btnCancel_Click" />
        <asp:Label ID="lblerror" runat="server" Font-Size="Large"></asp:Label>
    </div>
      
  </form>
  
  <script>
      function goBack() {
          history.back();
      }
  </script>
</body>
</html>
    <asp:HiddenField ID="vehicleIdHiddenField" runat="server" />
</asp:Content>
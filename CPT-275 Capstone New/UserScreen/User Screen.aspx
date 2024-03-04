<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="User_Screen.aspx.cs" Inherits="CPT_275_Capstone.UserScreen.User_Screen" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>User Name</title>
    <style>
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
        body {
            background-image: url('../ImageFile/background image.jpg');
            display: flex;
            justify-content: center;
            align-items: center;
            height: 100vh;
        }

        #container {
            text-align: center;
            
        }
        .listbox-style {
    font-weight: bold;
    font-size: larger;
    height: 200px; 
    width: 500px;
}
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div id="container">
            <asp:ListBox ID="lstvehicle" runat="server" CssClass="listbox-style" ></asp:ListBox>
            <br />
            <asp:Label ID="ErrorMessageLabel" runat="server" Text="" Font-Size="20px" ForeColor="Red"></asp:Label>
            <br />
            
            <asp:Button ID="btnInspection" runat="server" class="button" Text="Inspection" Font-Size="20px" OnClick ="NewInspection_Click"  />
            
            <asp:Button ID="btnTrip" runat="server" class="button" Text="New Trip" Font-Size="20px" OnClick="NewTripButton_Click" />
            <asp:Button ID="btnExit" runat="server" class="button" Text="Log Out" Font-Size="20px" OnClick="ExitButton_Click" />
        </div>
    </form>
</body>
</html>
<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="UserScreenT.aspx.cs" Inherits="CPT_275_Capstone.WebForm1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <!DOCTYPE html>
<html>
<head>
    <title>User Name</title>
    <style>
        body {
            background-image: url('../ImageFile/background image.jpg');        
        }

    </style>
</head>
<body>
    <h1>User Screen</h1>
        <div id="container">
            &nbsp;<asp:DropDownList ID="drpcarsvehicle" runat="server" Height="50px" Width="621px">
            </asp:DropDownList>
            <br />
            <label id="ErrorMessageLabel" class="form-label" style="font-size: 20px; color: red;"></label>
            <br />
        
        </div>  
</body>
</html>
</asp:Content>

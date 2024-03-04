<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="TripT.aspx.cs" Inherits="CPT_275_Capstone.WebForm3" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <!DOCTYPE html>
    <html>
    <head>
        <title>Trip Information</title>
        <br>
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
                width: 295px; 
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
                align-items: self-start;
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
        <h1>Trip Information</h1>
        <br>
        <div class="text-start">
            <div class="auto-style1">
                <label for="txtDate" class="form-label">
                    <div class="text-start">
                        Date:
                    </div>
                </label>&nbsp;
                <asp:TextBox ID="txtDate" runat="server" TextMode="Date" Width="176px"></asp:TextBox>
                
            </div>
            <div class="text-start">
                <br />
            </div>
            <div class="auto-style1">
                <label for="txtBeginningMileage" class="form-label">Beginning Mileage:</label>&nbsp;
                <asp:TextBox ID="txtBeginningMileage" runat="server" AutoPostBack="false" Width="295px" TextMode="Number"></asp:TextBox>
            </div>
            <div class="text-start">
                <br />
            </div>
            <div class="auto-style1">
                <label for="txtDestination" class="form-label">Destination:</label>&nbsp;
                <asp:TextBox ID="txtDestination" runat="server" Width="295px"></asp:TextBox>
            </div>
            <div class="text-start">
                <br />
            </div>
            <div class="auto-style1">
                <label for="txtPurpose" class="form-label">Purpose of Trip:</label>&nbsp;
                <asp:TextBox ID="txtPurposeoftrip" runat="server" MaxLength="50" TextMode="MultiLine" Width="295px"></asp:TextBox>
            </div>
            <div class="text-start">
                <br />
            </div>
            <div class="auto-style1">
                <label for="txtEndingMileage" class="form-label">Ending Mileage:</label>&nbsp;
                <asp:TextBox ID="txtEndingMileage" runat="server" AutoPostBack="false" Width="295px" TextMode="Number"></asp:TextBox>
            </div>
            <div class="text-start">
                <br />
            </div>
            <div class="form-row">
                <label for="txtTotal" class="form-label">Total Miles:</label>&nbsp;
                <asp:Label ID="lbltotalmilese" runat="server" Font-Bold="True" Font-Italic="False" Font-Size="Large"></asp:Label>
            </div>
            <div class="text-start">
                <br />
            </div>
            <div class="auto-style1">
                <label for="Vehicle" class="form-label">Vehicle:</label>&nbsp;
                
                    &nbsp;<asp:DropDownList ID="drpcars" CssClass="custom-listbox" class="form-label" runat="server" Height="40px" Width="295px">

                    </asp:DropDownList>
                    <br><br>
                    <br>
                
            </div>
            <div>
                <div class="auto-style1">
                    <br>
                    <br>
                    <label for="User" class="form-label">User:</label>&nbsp;
                   
                        <br>
                
                <asp:ListBox ID="lstUsers" CssClass="custom-listbox" class="form-label" runat="server"  AutoPostBack="false"></asp:ListBox>
            </div>
                    </div>
            <div class="text-start">
                <asp:Label ID="lblmessage" runat="server"  Font-Size="Large"></asp:Label>
                <br />
            </div>
            <asp:Button ID="btnsubmit" class="button" runat="server" Height="38px" OnClick="btnsubmit_Click" Text="Submit" Width="97px" />
            <asp:Button ID="btncancel" class="button" runat="server" Height="38px" Text="Cancel" Width="104px" OnClick="btncancel_Click" />
            <br />
        </div>
    </body>
    </html>
    <script>
        function CalculateMilage() {
            var beginningMileage = $("#<%=txtBeginningMileage.ClientID%>").val()
            var endingMileage = $("#<%=txtEndingMileage.ClientID%>").val();

            if (beginningMileage != "" && endingMileage != "") {
                var sum = endingMileage - beginningMileage;
                $("#<%=lbltotalmilese.ClientID%>").text(sum);
            }

        }

        $("#<%=txtBeginningMileage.ClientID%>").on("input", function () {
            CalculateMilage()
        });

        $("#<%=txtEndingMileage.ClientID%>").on("input", function () {
            CalculateMilage()
        });
    </script>
</asp:Content>
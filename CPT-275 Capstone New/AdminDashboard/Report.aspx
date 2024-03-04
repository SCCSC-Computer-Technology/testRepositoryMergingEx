<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Report.aspx.cs" Inherits="CPT_275_Capstone.AdminDashboard.WebForm1" %>
<%--<%@ Register assembly="Microsoft.ReportViewer.WebForms" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>--%>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <!DOCTYPE html>
<html>
<head>
    <title>Reports</title>
    <link rel="stylesheet" type="text/css" href="https://cdn.jsdelivr.net/npm/flatpickr/dist/flatpickr.min.css">
    <style>
        body {
            background-image: url('../ImageFile/background image.jpg');
        }

        .form-row {
            display: flex;
            flex-direction: column;
            align-items: flex-start;
        }

        .form-label {
            text-align: left;
            padding-bottom: 5px;
            font-weight: bold;
        }

        .form-field {
            font-size: 20px;
            text-align: left;
            width: 271px;
        }

        .button {
            margin-top: 10px;
            padding: 10px 20px;
            font-size: 16px;
            background-color: #4CAF50;
            color: white;
            border: none;
        }

        .button:hover {
            background-color: #45a049;
        }

        .modal {
          display: none;
          position: fixed;
          z-index: 1;
          left: 0;
          top: 0;
          width: 100%;
          height: 100%;
          overflow: auto;
          background-color: rgb(0,0,0);
          background-color: rgba(0,0,0,0.4);
        }

        .modal-content {
          background-color: #fefefe;
          margin: 15% auto;
          padding: 20px;
          border: 1px solid #888;
          width: 300px;
        }

        .close {
          color: #aaa;
          float: right;
          font-size: 28px;
          font-weight: bold;
        }

        .close:hover,
        .close:focus {
          color: black;
          text-decoration: none;
          cursor: pointer;
        }
    </style>
    <script src="https://cdn.jsdelivr.net/npm/flatpickr"></script>
    <%--                <asp:Button ID="btnemail" runat="server" OnClick="btnemail_Click" Text="Email" Visible="False" />--%>
    <script>
        document.addEventListener("DOMContentLoaded", function () {
            flatpickr("#dateRangePicker", {
                mode: "range"
            });
        });

        function submitForm() {
            var dateRange = document.getElementById("dateRangePicker").value;
            console.log("Selected Date Range:", dateRange);
            // Perform further processing with the selected date range
        }

        function goBack() {
 
        }
        // Send email button click event
        btnEmail.onclick = function () {
            var email = document.getElementById("txtEmail").value;

            // Perform validation on the email address if needed
            if (email.trim() === "") {
                alert("Please enter a valid email address.");
                return;
            }

            // Send the email
            console.log("Sending email to: " + email);

            // Close the modal dialog
            modal.style.display = "none";
        }
    </script>
 <style>
        .calendar-container {
            display: flex;
            justify-content: space-between;
            margin-bottom: 5px; 
        }

        .calendar-container .form-field {
            flex-basis: 30%; 
            margin-right: 5px; 
        }

        .form-field .ajax__calendar_container {
            width: 60% !important;
            max-width: 100px; 
        }
    </style>
</head>
<body>
     <h1>Trip Report</h1>
    <div class="calendar-container">
        <div class="calendar-container .form-field">
            <div class="form-field">
                <label for="startDatePicker" class="form-label">Start Date:</label>
                <asp:Calendar ID="startDatePicker" runat="server"></asp:Calendar>
            </div>

            <div class="calendar-container .form-field">
                <div class="form-field">
                <label for="endDatePicker" class="form-label">End Date:</label>
                <asp:Calendar ID="endDatePicker" runat="server"></asp:Calendar>
                    </div>
            </div>
        </div>

        
        <div class="form-row">
            <label for="vehicleList" class="form-label">Vehicle:</label>            
            <asp:DropDownList ID="drpvehicles" runat="server" Width="265px" Height="40px">
            </asp:DropDownList>
            <div>
                <br />
                <asp:CheckBox ID="chkDriver" runat="server" />
                <asp:Label ID="Label3" runat="server" Text="No Vehicle"></asp:Label>
            </div>
        </div>
        <div class="form-row">
            
            <label for="driverList" class="form-label">Driver:</label>
            <asp:DropDownList ID="drpdriver" runat="server" Width="265px" Height="40px">
            </asp:DropDownList>           
            <div>
<br />
              
                <asp:CheckBox ID="chkvehicle" runat="server" />
                <asp:Label ID="Label1" runat="server" Text="No Driver"></asp:Label>
                

            </div>
            <div>
                <br />
                <br />
                <%--                <asp:Button ID="btnemail" runat="server" OnClick="btnemail_Click" Text="Email" Visible="False" />--%>
                <asp:Button ID="btnexportaspdf" runat="server" OnClick="btnexportaspdf_Click" Text="Export as PDF" Visible="False" />
                <asp:Button ID="btnexportasexcel" runat="server" OnClick="btnexportasexcel_Click" Text="Export as Excel" Visible="False" />
<%--                <asp:Button ID="btnemail" runat="server" OnClick="btnemail_Click" Text="Email" Visible="False" />--%>
                <br />
                <br />
                <br />
                <asp:Button ID="btnCancel" runat="server" Height="37px" Text="Cancel" Width="105px" OnClick="Button1_Click" />
                <asp:Button ID="btnSubmit" runat="server" Height="37px" OnClick="btnSubmit_Click" Text="Submit" Width="105px" />
            </div>
        </div>
         <div id="Modal" class="modal" runat ="server">
          <div class="modal-content">
            <span class="close">&times;</span>
            <p>Enter Email Address:</p>
            <asp:TextBox ID="txtEmail" runat="server"></asp:TextBox>
            <button id="btnEmail">Send Email</button>
          </div>
        </div>
        </div>
        <div>
            <asp:Panel ID ="pnlGriedView" runat ="server">
            <asp:GridView ID="reportgried" runat="server" CellPadding="4" ForeColor="#333333" GridLines="None" Height="196px" Width="1186px">
                <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                <EditRowStyle BackColor="#999999" />
                <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                <SortedAscendingCellStyle BackColor="#E9E7E2" />
                <SortedAscendingHeaderStyle BackColor="#506C8C" />
                <SortedDescendingCellStyle BackColor="#FFFDF8" />
                <SortedDescendingHeaderStyle BackColor="#6F8DAE" />
            </asp:GridView>
                </asp:Panel>
            <br />
            <br />
            <div>
                <asp:Label ID="lblerrormessage" runat="server" Font-Bold="True" Font-Size="X-Large" ForeColor="Red"></asp:Label>

            </div>
        </div>
         
    <div class="text-start">
        <br />
    </div>

</body>
</html>
    </asp:Content>
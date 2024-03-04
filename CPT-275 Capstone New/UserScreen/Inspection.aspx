<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Inspection.aspx.cs" Inherits="CPT_275_Capstone.UserScreen.WebForm1" %>

<!DOCTYPE html>
<html>
<head>
    <title>Inspection Form</title>
    <style>
        body {
            background-image: url('../ImageFile/background image.jpg');
         
        }
    </style>
    <style>

        body {
            font-family: Arial, sans-serif;
            margin: 20px;
        }
        
        .form-group {
            margin-bottom: 20px;
        }
        
        label {
            font-weight: bold;
            display: block;
        }
        
        input[type="text"] {
            width: 200px;
            padding: 5px;
            font-size: 14px;
        }
        
        textarea {
            width: 400px;
            height: 100px;
            font-size: 14px;
            padding: 5px;
        }
        
        .checklist {
            list-style-type: none;
            margin: 0;
            padding: 0;
        }
        
        .checklist li {
            margin-bottom: 5px;
        }
        
        .title {
            font-size: 18px;
            font-weight: bold;
            margin-bottom: 10px;
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
        .form-group.inline {
            display: inline-block;
            margin-right: 20px;
        }
        .button-container {
            text-align: center;
        }
    *,::after,::before{box-sizing:border-box}
        #lblselectedcar0 {
            height: 31px;
        }
    </style>
    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/moment.js/2.29.1/moment.min.js"></script>
    <script>
        function submitForm() {

        }

        function goBack() {
            // Go back to the previous screen
            history.back();
        }
        window.onload = function () {
            var dateInput = document.getElementById("<%= txtDate.ClientID %>");
            var today = new Date().toISOString().split("T")[0];
            dateInput.value = today;
        };
        $(document).ready(function () {
            // Attach event handlers to the mileage input fields
            $('#txtbeginningmilleage, #txtendingmileage').on('input', function () {
                calculateTotalMilesDriven();
            });

            // Handler for date input event on Last Oil Change Date and Oil Change Due fields
            $("#txtlastoilchangedate, #txtoilchangedue").on("input", function () {
                calculateInterval();
            });

            // Function to calculate and update the total miles driven
            function calculateTotalMilesDriven() {
                var beginningMileage = parseInt($('#txtbeginningmilleage').val()) || 0;
                var endingMileage = parseInt($('#txtendingmileage').val()) || 0;
                var totalMilesDriven = endingMileage - beginningMileage;
                $('#lbltotalmilesdriventotal').text(totalMilesDriven);
            }

            // Function to calculate and update the interval
            function calculateInterval() {
                var lastOilChangeDate = $('#txtlastoilchangedate').val();
                var oilChangeDue = $('#txtoilchangedue').val();

                if (lastOilChangeDate && oilChangeDue) {
                    var startDate = moment(lastOilChangeDate, "YYYY-MM-DD");
                    var endDate = moment(oilChangeDue, "YYYY-MM-DD");
                    var interval = endDate.diff(startDate, "days"); // Calculate interval in months

                    $('#txtinterval').val(interval); // Update the interval input field without decimals
                }
            }

            // Calculate total miles driven and interval on page load
            calculateTotalMilesDriven();
            calculateInterval();
        });
    </script>
</head>
<body>
    <h1>Inspection Form</h1>
    <form id="form1" runat="server">

            
              <div>
                    <div class="form-group inline">
                        
                   <label for="txtDate" >Date: </ label>
                &nbsp;
                <asp:TextBox ID="txtDate" runat="server" TextMode="Date" Width="200px" Height="30px" ></asp:TextBox>&nbsp;</div>
                <asp:RequiredFieldValidator ID ="rfvDate" runat="server" ControlToValidate="txtDate" ErrorMessage="Date is required" ForeColor="Red"></asp:RequiredFieldValidator>
            </div>
        

        <div class="form-group inline">
            <label for="beginningMileage">Beginning Mileage:</label>
            <asp:TextBox ID="txtbeginningmilleage" runat="server"></asp:TextBox>
&nbsp;</div>
        <div class="form-group inline">
            <label for="endingMileage">Ending Mileage:</label>
            <asp:TextBox ID="txtendingmileage" runat="server"></asp:TextBox>
&nbsp;</div>
        <div class="form-group inline">
            <label for="totalMilesDriven">Total Miles Driven:</label>
            <asp:Label ID="lbltotalmilesdriventotal" runat="server"></asp:Label>
&nbsp;</div>
        
        <div class="title">Oil Change</div>
        <div class="form-group inline">
            <label for="lastOilChangeDate">Last Oil Change Date:</label>
            <asp:TextBox ID="txtlastoilchangedate" runat="server" TextMode="Date"></asp:TextBox>
&nbsp;</div>
        <div class="form-group inline">
            <label for="oilChangeDue">Oil Change Due:</label>
            <asp:TextBox ID="txtoilchangedue" runat="server" TextMode="Date"></asp:TextBox>
&nbsp;</div>
        <div class="form-group inline">
            <label for="oilChangeInterval">Interval (Days):</label>
            <asp:TextBox ID="txtinterval" runat="server" Enabled="False"></asp:TextBox>
&nbsp;</div>
        
        <div class="title">Tires</div>
        <div class="form-group inline">
            <label for="lastTireRotation">Last Tire Rotation:</label>
            <asp:TextBox ID="txtlasttirerotation" runat="server" TextMode="Date"></asp:TextBox>
&nbsp;</div>
        <div class="form-group inline">
            <label for="rotationDue">Rotation Due:</label>
            <asp:TextBox ID="txtrotationdue" runat="server" TextMode="Date"></asp:TextBox>
&nbsp;</div>
        <div class="form-group inline">
            <label for="tirePressure">Tire Pressure (PSI):</label>
            <asp:TextBox ID="txttirepressure" runat="server"></asp:TextBox>
&nbsp;</div>
        
       <div class="title">Checklist</div>
        <div class="form-group inline-checklist">
            <ul class="checklist">
                <li>
                    <label>
                   <asp:CheckBox ID ="chkfluidchecked" runat="server" />&nbsp;Fluid Levels Checked </label>
                </li>
                <li>
                    <label>
                   <asp:CheckBox ID ="chkBattery" runat="server" />&nbsp;Battery Checked </label>
                </li>
                <li>
                    <label>
                   <asp:CheckBox ID ="chkGaugesWorking" runat="server" />&nbsp;Gauges Working </label>
                </li>
                <li>
                    <label>
                   <asp:CheckBox ID ="chkCleanedInteriorExterior" runat="server" />&nbsp;Cleaned interior and exterior</label>
                </li>
                <li>
                    <label>
                   <asp:CheckBox ID ="chkBedCabCleanOrganized" runat="server" />&nbsp;Bed / Cab clean & organized</label>
                </li>
            </ul>
            <div class="form-group inline">
                <label for="additionalNotes">Additional Notes:</label>
                <asp:TextBox ID="txtnotes" runat="server" Height="92px" TextMode="MultiLine" Width="366px"></asp:TextBox>
                <div>
                &nbsp;<asp:Label ID="lblselectedcar" runat="server" Font-Bold="True" Font-Size="X-Large"></asp:Label>
                   </div>
                    <div>
                    <asp:Label ID="lblmessage" runat="server" Font-Bold="True" Font-Size="X-Large"></asp:Label>
                     </div>
                </div>
            </div>
        </div>     
            <asp:Button ID="btnSubmit" runat="server" Height="41px" Text="Submit" Width="99px" OnClick="btnSubmit_Click" />
  
            <asp:Button ID="btnCancel" runat="server" Height="41px" OnClick="btnCancel_Click"  CausesValidation ="false" Text="Cancel" Width="93px" />
  
    </form>
</body>
</html>
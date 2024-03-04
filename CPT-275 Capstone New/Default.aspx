<%@ Page Title="Default" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="CPT_275_Capstone.Default" %>
<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <!-- Include SweetAlert2 CSS and JavaScript -->
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/sweetalert2@11.1.6/dist/sweetalert2.min.css">
    <script src="https://cdn.jsdelivr.net/npm/sweetalert2@11.1.6/dist/sweetalert2.all.min.js"></script>
    <style>
        body {
            background-image: url('../ImageFile/background image.jpg');
        }

        .jumbotron {
            text-align: center;
        }

        .listbox-container {
            margin-bottom: 20px;
            margin-top: auto;
        }

        .custom-listbox {
            width: 70%;
            height: 200px; 
            border: 1px solid #ccc;
            padding: 10px;
            font-size: 16px;
            background-color: #fff;
        }

        .custom-button {
            margin: 10px;
            padding: 8px 16px;
            font-size: 16px;
            border: none;
            border-radius: 4px;
            cursor: pointer;
        }

        .custom-button.edit {
            background-color: #007bff; 
            color: #fff;
        }

        .custom-button.edit:hover {
            background-color: #0056b3;
        }

        .custom-button.delete {
            background-color: #ff0000; 
            color: #fff;
        }

        .custom-button.delete:hover {
            background-color: #b30000;
        }

        .flex-container {
            display: flex;
            align-items: flex-start;
        }

        .flex-container > div {
            flex: 2;
        }
    </style>
   <script>
        function confirmDelete1() {
            Swal.fire({
                title: 'Confirmation',
                text: 'Are you sure you want to delete this record?',
                icon: 'warning',
                showCancelButton: true,
                confirmButtonColor: '#3085d6',
                cancelButtonColor: '#d33',
                confirmButtonText: 'Delete',
                cancelButtonText: 'Cancel'
            }).then((result) => {
                if (result.isConfirmed) {
                    // User confirmed the deletion
<%--                    __doPostBack("<%= btnDelete.UniqueID %>", "");--%>
                <%= Page.ClientScript.GetPostBackEventReference(btnDelete, "") %>;
                }
            });
            return false;
        }

        function deleteRecord() {
     
            Swal.fire('Deleted!', 'The record has been deleted.', 'success');
        }

   </script>

    <div class="jumbotron">
        <h1>SPARTANBURG COMMUNITY COLLEGE FLEET TRACKER</h1>
    </div>

    <div class="row">
        <div class="flex-container">
            <div>
                <div class="listbox-container">
                    <h3>VEHICLES</h3>
                </div>
                <asp:ListBox ID="lstVehicles" CssClass="custom-listbox" runat="server" AutoPostBack="false"></asp:ListBox>
            </div>

            <div>
                <div class="listbox-container">
                    <h3>USERS</h3>
                </div>
                <asp:ListBox ID="lstUsers" CssClass="custom-listbox" runat="server" AutoPostBack="false"></asp:ListBox>
            </div>
        </div>
    </div>

    <div class="row">
        <div class="col-md-10 text-center">
            <asp:Button ID="btnEdit" runat="server" class="custom-button edit" Text="Edit" OnClick="btnEdit_Click"  />
            
        <asp:Button ID="btnDelete" runat="server" class="custom-button delete" Text="Delete Record" OnClick="btnDelete_Click" OnClientClick="return confirmDelete1();" />
        </div>
    </div>
    <script>
        $("#<%=lstVehicles.ClientID%>").on("change", function () {
            $("#<%=lstUsers.ClientID%>").val("");
        });

        $("#<%=lstUsers.ClientID%>").on("change", function () {
            $("#<%=lstVehicles.ClientID%>").val("");
        });
    </script>
</asp:Content>
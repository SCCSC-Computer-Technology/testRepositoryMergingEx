<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Email Sender.aspx.cs" Inherits="CPT_275_Capstone.Sender_Email.Email_Sender" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script src="https://cdn.jsdelivr.net/npm/sweetalert2@11"></script>
    <script>
        function ShowEmailSentMessage() {
            Swal.fire({
                icon: 'success',
                title: 'Email Sent',
                text: 'The email has been sent successfully.',
            });
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div>
        </div>
        <asp:Label ID="Label1" runat="server" Text="Enter Email:"></asp:Label>
        <asp:TextBox ID="txtemail" runat="server" Width="262px"></asp:TextBox>
        <p>
            <asp:Label ID="Label2" runat="server" Text="Message:"></asp:Label>
            <asp:TextBox ID="txtmessage" runat="server" EnableTheming="True" Height="68px" TextMode="MultiLine" Width="209px"></asp:TextBox>
        </p>
        <asp:Button ID="btnemail" runat="server" Height="38px" OnClick="btnemail_Click" Text="Email" Width="123px" />
        <p>
            <asp:Label ID="lblconfirmemail" runat="server" Font-Size="X-Large" ForeColor="Aqua"></asp:Label>
        </p>
    </form>
</body>
</html>

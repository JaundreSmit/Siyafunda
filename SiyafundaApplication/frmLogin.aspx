<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmLogin.aspx.cs" Inherits="SiyafundaApplication.frmLogin" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Login</title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <h2>Login</h2>

            <!-- Username Label and TextBox -->
            <asp:Label ID="UsernameLabel" runat="server" Text="Username"></asp:Label><br />
            <asp:TextBox ID="UsernameTextBox" runat="server"></asp:TextBox><br />
            <asp:RequiredFieldValidator ID="UsernameRequired" runat="server" ControlToValidate="UsernameTextBox" 
                ErrorMessage="Username is required" ForeColor="Red"></asp:RequiredFieldValidator><br />

            <!-- Password Label and TextBox -->
            <asp:Label ID="PasswordLabel" runat="server" Text="Password"></asp:Label><br />
            <asp:TextBox ID="PasswordTextBox" runat="server" TextMode="Password"></asp:TextBox><br />
            <asp:RequiredFieldValidator ID="PasswordRequired" runat="server" ControlToValidate="PasswordTextBox" 
                ErrorMessage="Password is required" ForeColor="Red"></asp:RequiredFieldValidator><br />

            <!-- Login Button -->
            <asp:Button ID="LoginButton" runat="server" Text="Login" OnClick="LoginButton_Click" /><br />

            <!-- Error Label -->
            <asp:Label ID="lblError" runat="server" Text="" ForeColor="Red" Visible="false"></asp:Label>
        </div>
    </form>
</body>
</html>

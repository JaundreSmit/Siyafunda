<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmLogin.aspx.cs" Inherits="SiyafundaApplication.frmLogin" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Login</title>
    <style type="text/css">
        .auto-style1 {
            width: 134px;
        }
        .auto-style2 {
            width: 363px;
        }
        .auto-style3 {
            width: 134px;
            height: 30px;
        }
        .auto-style4 {
            width: 363px;
            height: 30px;
        }
        .auto-style5 {
            height: 30px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <h2>Login</h2>

            <!-- Username Label and TextBox -->
            <table style="width:100%;">
                <tr>
                    <td class="auto-style3">
            <asp:Label ID="UsernameLabel" runat="server" Text="Username/Email"></asp:Label></td>
                    <td class="auto-style4">
            <asp:TextBox ID="UsernameTextBox" runat="server"></asp:TextBox>&nbsp;<asp:RequiredFieldValidator ID="UsernameRequired" runat="server" ControlToValidate="UsernameTextBox" 
                ErrorMessage="Username is required" ForeColor="Red"></asp:RequiredFieldValidator></td>
                    <td class="auto-style5"></td>
                </tr>
                <tr>
                    <td class="auto-style1">
            <asp:Label ID="PasswordLabel" runat="server" Text="Password"></asp:Label></td>
                    <td class="auto-style2">
            <asp:TextBox ID="PasswordTextBox" runat="server" TextMode="Password"></asp:TextBox>&nbsp;<asp:RequiredFieldValidator ID="PasswordRequired" runat="server" ControlToValidate="PasswordTextBox" 
                ErrorMessage="Password is required" ForeColor="Red"></asp:RequiredFieldValidator></td>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td class="auto-style1">
            <asp:Label ID="lblError" runat="server" Text="" ForeColor="Red" Visible="false"></asp:Label>
                    </td>
                    <td class="auto-style2">&nbsp;</td>
                    <td>&nbsp;</td>
                </tr>
                <tr>
    <td class="auto-style1">
            <asp:Button ID="LoginButton" runat="server" Text="Login" OnClick="LoginButton_Click" /></td>
    <td class="auto-style2">
            <asp:Button ID="btnBack" runat="server" Text="Back" OnClick="btnBack_Click" style="height: 26px" /></td>
    <td>&nbsp;</td>
</tr>
            </table>
            <br />
            <br />
            <br />

            <!-- Password Label and TextBox -->
            <br />
            <br />
            <br />

            <!-- Login Button -->
            <br />

            <!-- Error Label -->
        </div>
    </form>
</body>
</html>

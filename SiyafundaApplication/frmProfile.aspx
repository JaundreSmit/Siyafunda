<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmProfile.aspx.cs" Inherits="SiyafundaApplication.Profile" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>User Profile</title>
    <style type="text/css">
        .auto-style1 {
            margin-bottom: 0px;
        }
        .auto-style3 {
            width: 88px;
        }
        .auto-style4 {
            height: 30px;
        }
        .auto-style5 {
            width: 86px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <h2>User Profile</h2>
            <!-- User Information -->
            <h3>Edit Profile</h3>
            <table style="width:100%;">
                <tr>
                    <td>Role:</td>
                    <td>
            <asp:Label ID="RoleLabel" runat="server" Text="Role: "></asp:Label></td>
                    <td>&nbsp;</td>
                </tr>
                    <tr>
        <td class="auto-style3">
<asp:Label ID="Label1" runat="server" Text="Name: "></asp:Label>
        </td>
        <td>
<asp:TextBox ID="NameTextBox" runat="server"></asp:TextBox></td>
        <td>&nbsp;</td>
    </tr>
                <tr>
                    <td class="auto-style3">
            <asp:Label ID="SurnameTextLabel" runat="server" Text="Surname: "></asp:Label>
                    </td>
                    <td>
            <asp:TextBox ID="SurnameTextBox" runat="server" CssClass="auto-style1"></asp:TextBox></td>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td class="auto-style3">
            <asp:Label ID="EmailTextLabel" runat="server" Text="Email: "></asp:Label>
                    </td>
                    <td>
            <asp:TextBox ID="EmailTextBox" runat="server"></asp:TextBox></td>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td class="auto-style5">
            <asp:Label ID="PasswordLabel" runat="server" Text="Password: "></asp:Label>
                    </td>
                    <td>
            <asp:TextBox ID="PasswordTextBox" runat="server"></asp:TextBox></td>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td class="auto-style4">

            <asp:Button ID="SaveButton" runat="server" Text="Save" OnClick="SaveButton_Click" Width="83px" />

                    </td>
                    <td class="auto-style4">
                        <asp:Label ID="lblErrors" runat="server" Text="[Edit Errors]"></asp:Label>
                    </td>
                    <td class="auto-style4"></td>
                </tr>
                    <tr>
        <td class="auto-style4">

            <asp:Button ID="btnBack" runat="server" Text="Back" OnClick="btnBack_Click" Width="83px" />

        </td>
        <td class="auto-style4">&nbsp;</td>
        <td class="auto-style4"></td>
    </tr>
            </table>
            <br />
            <br />
            <br />
            <table style="width:100%;">
                <tr>
                    <td>
                        <h3>Enrolled Modules</h3>
                    </td>
                    <td>&nbsp;</td>
                    <td>&nbsp;</td>
                </tr>
            </table>
            <table style="width:100%;">
                <tr>
                    <td>
            <asp:GridView ID="ModulesGridView" runat="server"></asp:GridView>
                    </td>
                    <td>&nbsp;</td>
                    <td>&nbsp;</td>
                </tr>
            </table>
            <table style="width:100%;">
                <tr>
                    <td>&nbsp;</td>
                    <td>&nbsp;</td>
                    <td>&nbsp;</td>
                </tr>
            </table>
            <br />

            <!-- User's Modules -->
            <h3>&nbsp;</h3>
        </div>
    </form>
</body>
</html>

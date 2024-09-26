<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Profile.aspx.cs" Inherits="SiyafundaApplication.Profile" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>User Profile</title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <h2>User Profile</h2>
            <!-- User Information -->
            <asp:Label ID="RoleLabel" runat="server" Text="Role: "></asp:Label><br />
            <asp:Label ID="NameLabel" runat="server" Text="Name: "></asp:Label><br />
            <asp:Label ID="SurnameLabel" runat="server" Text="Surname: "></asp:Label><br />
            <asp:Label ID="EmailLabel" runat="server" Text="Email: "></asp:Label><br />

            <!-- User Editable Fields -->
            <h3>Edit Profile</h3>
            <asp:Label ID="NameTextLabel" runat="server" Text="Name: "></asp:Label>
            <asp:TextBox ID="NameTextBox" runat="server"></asp:TextBox><br />
            <asp:Label ID="SurnameTextLabel" runat="server" Text="Surname: "></asp:Label>
            <asp:TextBox ID="SurnameTextBox" runat="server"></asp:TextBox><br />
            <asp:Label ID="EmailTextLabel" runat="server" Text="Email: "></asp:Label>
            <asp:TextBox ID="EmailTextBox" runat="server"></asp:TextBox><br />
            <asp:Label ID="PasswordLabel" runat="server" Text="Password: "></asp:Label>
            <asp:TextBox ID="PasswordTextBox" TextMode="Password" runat="server"></asp:TextBox><br />

            <asp:Button ID="SaveButton" runat="server" Text="Save" OnClick="SaveButton_Click" />

            <!-- User's Modules -->
            <h3>Enrolled Modules</h3>
            <asp:GridView ID="ModulesGridView" runat="server"></asp:GridView>
        </div>
    </form>
</body>
</html>

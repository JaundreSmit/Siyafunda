<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Admin.aspx.cs" Inherits="SiyafundaApplication.Admin" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Admin Panel</title>
</head>
<body>
    <form id="form1" runat="server">
        <h2>Admin Panel - Manage Users</h2>

        <asp:GridView ID="UsersGridView" runat="server" AutoGenerateColumns="false" OnRowCommand="UsersGridView_RowCommand">
            <Columns>
                <asp:BoundField DataField="UserId" HeaderText="User ID" />
                <asp:BoundField DataField="Name" HeaderText="Name" />
                <asp:BoundField DataField="Surname" HeaderText="Surname" />
                <asp:BoundField DataField="Email" HeaderText="Email" />
                <asp:BoundField DataField="RoleName" HeaderText="Role" />
                
                <!-- Dropdown for changing roles -->
                <asp:TemplateField HeaderText="Change Role">
                    <ItemTemplate>
                        <asp:DropDownList ID="RoleDropDownList" runat="server" AutoPostBack="true" OnSelectedIndexChanged="RoleDropDownList_SelectedIndexChanged" DataSourceID="RolesDataSource" DataTextField="role_name" DataValueField="role_id" />
                        <asp:SqlDataSource ID="RolesDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:yourConnectionString %>" SelectCommand="SELECT * FROM Roles"></asp:SqlDataSource>
                    </ItemTemplate>
                </asp:TemplateField>

                <!-- Button to delete users -->
                <asp:ButtonField ButtonType="Button" CommandName="DeleteUser" Text="Delete" />
            </Columns>
        </asp:GridView>
    </form>
</body>
</html>
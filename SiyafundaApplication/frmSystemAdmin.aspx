<%@ Page Language="C#" MasterPageFile="~/site.master" AutoEventWireup="true" CodeBehind="frmSystemAdmin.aspx.cs" Inherits="SiyafundaApplication.Admin" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <title>Admin Page</title>
    <style>
        body {
            font-family: Arial, sans-serif;
            margin: 20px;
        }
        h1 {
            color: #2c3e50;
        }
        .grid-container {
            width: 100%;
            margin-top: 20px;
        }
        .button {
            background-color: #3498db;
            color: white;
            border: none;
            padding: 10px 15px;
            cursor: pointer;
            border-radius: 5px;
        }
        .button:hover {
            background-color: #2980b9;
        }
    </style>
    
    <div>
        <h1>Admin User Management</h1>
        <asp:GridView ID="UsersGridView" runat="server" AutoGenerateColumns="False" OnRowCommand="UsersGridView_RowCommand" CssClass="grid-container">
            <Columns>
                <asp:BoundField DataField="user_id" HeaderText="User ID" Visible="false" />
                <asp:BoundField DataField="Name" HeaderText="Name" />
                <asp:BoundField DataField="Surname" HeaderText="Surname" />
                <asp:BoundField DataField="Email" HeaderText="Email" />
                <asp:BoundField DataField="role_name" HeaderText="Role" />
                <asp:TemplateField HeaderText="Actions">
                    <ItemTemplate>
                        <asp:Button ID="DeleteButton" runat="server" Text="Delete" CommandArgument='<%# Eval("user_id") %>' CommandName="DeleteUser" CssClass="button" />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>
</asp:Content>

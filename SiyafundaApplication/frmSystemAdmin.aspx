<%@ Page Language="C#" MasterPageFile="~/site.master" AutoEventWireup="true" CodeBehind="frmSystemAdmin.aspx.cs" Inherits="SiyafundaApplication.Admin" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <title>Admin Page</title>

    <div class="container mt-2">
        <div class="card bg-purple text-white mb-4" style="margin-top: 50px;">
            <div class="card-header text-center">
                <h5 class="mb-0">Admin User Management</h5>
            </div>
        </div>

        <div class="card-body">
            <asp:GridView ID="UsersGridView" runat="server" AutoGenerateColumns="False" OnRowCommand="UsersGridView_RowCommand" CssClass="table table-striped">
                <Columns>
                    <asp:BoundField DataField="user_id" HeaderText="User ID" Visible="false" />
                    <asp:BoundField DataField="Name" HeaderText="Name" />
                    <asp:BoundField DataField="Surname" HeaderText="Surname" />
                    <asp:BoundField DataField="Email" HeaderText="Email" />
                    <asp:BoundField DataField="role_name" HeaderText="Role" />
                    <asp:TemplateField HeaderText="Actions">
                        <ItemTemplate>
                            <asp:Button ID="DeleteButton" runat="server" Text="Delete" CommandArgument='<%# Eval("user_id") %>' CommandName="DeleteUser" CssClass="btn btn-primary btn-purple mx-2" />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
    </div>

    <style>
        .bg-purple {
            background-color: rgb(108, 61, 145);
        }

        .table {
            color: white; 
        }

        .table-striped tbody tr:nth-of-type(odd) {
            background-color: rgba(255, 255, 255, 0.1);
        }

        .table-striped tbody tr:nth-of-type(even) {
            background-color: rgba(255, 255, 255, 0.2); 
        }

        .card-header {
            border-bottom: none; 
        }

        .btn-purple {
            background-color: whitesmoke;
            color: black;
            border: none;
            font-weight: bold;
            font-size: 12px;
            padding: 12px 24px;
            text-transform: uppercase;
            transition: background-color 0.3s ease, transform 0.2s ease;
            box-shadow: 0 4px 8px rgba(0, 0, 0, 0.2);
            border-radius: 0;
            width: 100px;
        }

        .btn-purple:hover {
            background-color: rgb(108, 61, 145);
            color: black;
            transform: translateY(-2px);
            box-shadow: 0 6px 12px rgba(0, 0, 0, 0.3);
            border-radius: 0;
        }

    </style>
</asp:Content>

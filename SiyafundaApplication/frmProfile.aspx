<%@ Page Language="C#" MasterPageFile="~/site.master" AutoEventWireup="true" CodeBehind="frmProfile.aspx.cs" Inherits="SiyafundaApplication.Profile" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <title>User Profile</title>

    <div class="container custom-container"> <!-- Main container for the profile page -->
        <!-- Purple Card for User Profile Title -->
        <div class="card text-white bg-purple mb-4 mx-auto" style="max-width: 600px;">
            <div class="card-header text-center no-border">
                <h2>User Profile</h2>
            </div>
        </div>

        <!-- User Information -->
        <h3 style="color: whitesmoke;">Edit Profile</h3>
        <table class="table table-bordered">
            <tr>
                <td>Role:</td>
                <td>
                    <asp:Label ID="RoleLabel" runat="server" Text="Role: "></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="Label1" runat="server" Text="Name: "></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="NameTextBox" runat="server" CssClass="form-control"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="SurnameTextLabel" runat="server" Text="Surname: "></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="SurnameTextBox" runat="server" CssClass="form-control"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="EmailTextLabel" runat="server" Text="Email: "></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="EmailTextBox" runat="server" CssClass="form-control"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="PasswordLabel" runat="server" Text="Password: "></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="PasswordTextBox" runat="server" CssClass="form-control"></asp:TextBox>
                </td>
            </tr>
        </table>

        <!-- Buttons placed outside the grid, aligned below the form -->
        <div class="text-center mt-3">
            <asp:Button ID="SaveButton" runat="server" Text="Save" OnClick="SaveButton_Click" CssClass="btn btn-primary w-50 btn-purple mx-2" />
            <asp:Button ID="btnBack" runat="server" Text="Back" OnClick="btnBack_Click" CssClass="btn btn-primary w-50 btn-purple mx-2" />
        </div>

        <!-- Error label -->
        <div class="text-center mt-2">
            <asp:Label ID="lblErrors" runat="server" CssClass="text-danger"></asp:Label>
        </div>

        <br />

        <h3 style="color: whitesmoke;">Enrolled Modules</h3>
        <asp:GridView ID="ModulesGridView" runat="server" CssClass="table table-striped"></asp:GridView>

        <br />
    </div>

    <style type="text/css">
        
        .bg-purple {
            background-color: rgb(108, 61, 145);
        }
       
        .form-control {
            width: 100%;
        }
       
        .mx-2 {
            margin-left: 0.5rem;
            margin-right: 0.5rem;
        }
        
        .custom-container {
            margin-top: -140px; 
            padding-top: 0; 
        }
        .btn-purple {
            background-color: whitesmoke; 
            color: black; 
            border: none; 
            font-weight: bold; 
            padding: 12px 24px; 
            text-transform: uppercase;
            transition: background-color 0.3s ease, transform 0.2s ease; 
            box-shadow: 0 4px 8px rgba(0, 0, 0, 0.2); 
            border-radius: 0;
        }

        .btn-purple:hover {
            background-color: rgb(108, 61, 145); 
            color: black; 
            transform: translateY(-2px); 
            box-shadow: 0 6px 12px rgba(0, 0, 0, 0.3);
            border-radius: 0;
        }

        .no-border {
            border-bottom: none !important;
        }

    </style>
</asp:Content>

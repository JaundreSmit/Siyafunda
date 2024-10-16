<%@ Page Language="C#" MasterPageFile="~/site.master" AutoEventWireup="true" CodeBehind="frmSystemDev.aspx.cs" Inherits="SiyafundaApplication.frmSystemDev" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <title>System Developer Management</title>

    <div class="container mt-2">
        <div class="card bg-purple text-white mb-4" style="margin-top: 50px;">
            <div class="card-body">
                <div class="mb-4"></div> <!-- Add extra space if needed -->
            </div>
            <div class="card-header text-center">
                <h5 class="mb-0">System Developer Management</h5>
            </div>
            <div class="card-body">
                <div class="mb-4"></div> <!-- Add extra space if needed -->
            </div>
        </div>

        <!-- Back Button Centered Below the Card -->
        <div class="text-center mb-4">
            <asp:Button ID="btnBack" runat="server" OnClick="btnBack_Click" Text="Back" CssClass="btn btn-primary btn-purple mx-2" />
        </div>

        <!-- Add Module Section -->
        <div class="mb-4">
            <div class="card bg-purple text-white mb-2" style="margin-top: 50px;">
                <div class="card-header">
                    <h5>Add Module</h5>
                </div>
            </div>
            <div class="mb-3">
                <label for="txtAddModName" style="color: whitesmoke;">Title:</label>
                <asp:TextBox ID="txtAddModName" runat="server" MaxLength="100" CssClass="form-control" />
            </div>
            <div class="mb-3">
                <label for="txtAddModDesc" style="color: whitesmoke;">Description:</label>
                <asp:TextBox ID="txtAddModDesc" runat="server" MaxLength="200" TextMode="MultiLine" CssClass="form-control" />
            </div>
            <div class="mb-3">
                <label for="ddlAddEducator" style="color: whitesmoke;">Educator:</label>
                <asp:DropDownList ID="ddlAddEducator" runat="server" CssClass="form-control"></asp:DropDownList>
            </div>
            <asp:Label ID="lblAddModErrors" runat="server" Text="[Adding Errors]" CssClass="text-danger mb-3" />
            <asp:Button ID="btnAddMod" runat="server" Text="Add Module" OnClick="btnAddMod_Click" CssClass="btn btn-primary btn-purple mx-2" />
        </div>

        <!-- Edit Module Section -->
        <div class="mb-4">
            <div class="card bg-purple text-white mb-2" style="margin-top: 50px;">
                <div class="card-header">
                    <h5>Edit Module</h5>
                </div>
            </div>
            <div class="mb-3">
                <label for="ddlEditModSelect" style="color: whitesmoke;">Module:</label>
                <asp:DropDownList ID="ddlEditModSelect" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlEditModSelect_SelectedIndexChanged" CssClass="form-control"></asp:DropDownList>
            </div>
            <div class="mb-3">
                <label for="txtEditModTitle" style="color: whitesmoke;">Title:</label>
                <asp:TextBox ID="txtEditModTitle" runat="server" MaxLength="100" CssClass="form-control" />
            </div>
            <div class="mb-3">
                <label for="txtEditModDesc" style="color: whitesmoke;">Description:</label>
                <asp:TextBox ID="txtEditModDesc" runat="server" MaxLength="200" TextMode="MultiLine" CssClass="form-control" />
            </div>
            <div class="mb-3">
                <label for="ddlEditEducator" style="color: whitesmoke;">Educator:</label>
                <asp:DropDownList ID="ddlEditEducator" runat="server" CssClass="form-control"></asp:DropDownList>
            </div>
            <asp:Label ID="lblEditModErrors" runat="server" Text="[Editing Errors]" CssClass="text-danger mb-3" />
            <div class="d-flex justify-content-between">
                <div>
                    <asp:Button ID="btnEditMod" runat="server" Text="Edit Module" OnClick="btnEditMod_Click" CssClass="btn btn-primary btn-purple mx-2" />
                    <asp:Button ID="btnDeleteModule" runat="server" Text="Delete Module" OnClick="btnDeleteModule_Click" CssClass="btn btn-primary btn-purple mx-2" />
                </div>
            </div>
        </div>
    </div>

    <style type="text/css">
        .bg-purple {
            background-color: rgb(108, 61, 145);
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
            width: 150px;
        }

        .btn-purple:hover {
            background-color: rgb(108, 61, 145);
            color: black;
            transform: translateY(-2px);
            box-shadow: 0 6px 12px rgba(0, 0, 0, 0.3);
            border-radius: 0;
        }

       
        .btn:disabled {
            background-color: darkgrey; 
            color: white; 
            border-color: darkgrey; 
            opacity: 1; 
            cursor: not-allowed; 
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

        .error-label {
            color: black; 
        }

        .card-header {
            border-bottom: none; 
        }

        .no-educators-label {
            display: block; 
            font-weight: bold; 
            font-size: 16px; 
            color: #FF0000; 
            text-align: center; 
            margin: 20px 0; 
            padding: 10px; 
            background-color: rgba(255, 0, 0, 0.1); 
            border-radius: 5px; 
        }


    </style>
</asp:Content>

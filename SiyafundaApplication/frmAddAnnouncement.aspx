<%@ Page Language="C#" MasterPageFile="~/site.master" AutoEventWireup="true" CodeBehind="frmAddAnnouncement.aspx.cs" Inherits="SiyafundaApplication.frmAddAnnouncement" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <title>Add Announcement</title>

    <div class="container mt-4">
        <div class="card bg-purple text-white mb-4" style="margin-top: 50px;">
            <div class="card-header text-center">
                <h5 class="mb-0">Add Announcement</h5>
            </div>
        </div>

        <div class="form-group">
            <label for="lblModuleName" style="color: whitesmoke;">Module:</label>
            <asp:Label ID="lblModuleName" runat="server" Text="[Module Name]" CssClass="form-control" />
        </div>
        <div class="form-group">
            <label for="txtTitle" style="color: whitesmoke;">Title:</label>
            <asp:TextBox ID="txtTitle" runat="server" MaxLength="600" CssClass="form-control" />
        </div>
        <div class="form-group">
            <label for="txtContent" style="color: whitesmoke;">Content:</label>
            <asp:TextBox ID="txtContent" runat="server" MaxLength="600" TextMode="MultiLine" CssClass="form-control" Rows="5" />
        </div>
        <div class="form-group">
            <asp:Label ID="lblErrors" runat="server" Text="[Errors]" CssClass="text-danger" />
        </div>
        <div class="form-group text-center">
            <asp:Button ID="btnAddAnnouncement" runat="server" OnClick="btnAddAnnouncement_Click" Text="Add Announcement" CssClass="btn btn-primary btn-purple mx-2" />
            <asp:Button ID="Back" runat="server" OnClick="Back_Click" Text="Back" CssClass="btn btn-primary btn-purple mx-2" />
        </div>
    </div>

    <style>
        .bg-purple {
            background-color: rgb(108, 61, 145);
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
            width: 200px;
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

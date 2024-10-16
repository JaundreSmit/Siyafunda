<%@ Page Language="C#" MasterPageFile="~/site.master" AutoEventWireup="true" CodeBehind="frmAddFAQ.aspx.cs" Inherits="SiyafundaApplication.frmAddFAQ" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <title>Add FAQ</title>

    <div class="container mt-4">
        <div class="card bg-purple text-white mb-4" style="margin-top: 50px;">
            <div class="card-header text-center">
                <h5 class="mb-0">Add FAQ</h5>
            </div>
        </div>

        <div class="form-group">
            <label for="lblModuleName">Module:</label>
            <asp:Label ID="lblModuleName" runat="server" Text="[Module Name]" CssClass="form-control" />
        </div>
        <div class="form-group">
            <label for="txtQuestion">Question:</label>
            <asp:TextBox ID="txtQuestion" runat="server" MaxLength="600" TextMode="MultiLine" CssClass="form-control" Rows="4" />
        </div>
        <div class="form-group">
            <label for="txtAnswer">Answer:</label>
            <asp:TextBox ID="txtAnswer" runat="server" MaxLength="600" TextMode="MultiLine" CssClass="form-control" Rows="4" />
        </div>
        <div class="form-group">
            <asp:Label ID="lblErrors" runat="server" Text="[Errors]" CssClass="text-danger" />
        </div>
        <div class="form-group text-center">
            <asp:Button ID="Back" runat="server" OnClick="Back_Click" Text="Back" CssClass="btn btn-primary btn-purple mx-2" />
            <asp:Button ID="btnAddFAQ" runat="server" OnClick="btnAddFAQ_Click" Text="Add FAQ" CssClass="btn btn-primary btn-purple mx-2" />
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

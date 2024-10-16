<%@ Page Language="C#" MasterPageFile="~/site.master" AutoEventWireup="true" CodeBehind="frmModerator.aspx.cs" Inherits="SiyafundaApplication.frmModerator" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <title>File Moderation</title>

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

        .error-label {
            color: red; 
        }
    </style>

    <div class="container mt-4">
        <div class="row">
            <div class="col text-center mb-3">
                <h2 class="bg-purple text-white p-3"><strong>File Moderation</strong></h2>
            </div>
        </div>

        <div class="row mb-3">
            <div class="col">
                <asp:Button ID="btnBack" runat="server" OnClick="btnBack_Click" Text="Back" CssClass="btn btn-primary btn-purple" />
            </div>
        </div>

        <div class="row mb-3">
            <div class="col">
                <label style="color: whitesmoke;"><strong>In Progress files:</strong></label>
                <div class="input-group">
                    <asp:TextBox ID="txtSearchProgress" runat="server" AutoPostBack="True" OnTextChanged="txtSearchProgress_TextChanged" CssClass="form-control" />
                    <asp:Label ID="lblProgressErrors" runat="server" Text="[Progress Errors]" CssClass="error-label" />
                </div>
            </div>
        </div>

        <div class="row mb-3">
            <div class="col">
                <asp:GridView ID="dgvInProgress" runat="server" CssClass="table table-striped" AllowSorting="True" OnSelectedIndexChanged="dgvInProgress_SelectedIndexChanged">
                </asp:GridView>
            </div>
        </div>

        <div class="row mb-3">
            <div class="col text-center">
                <asp:Button ID="btnReject" runat="server" Text="Reject" OnClick="btnReject_Click" CssClass="btn btn-danger btn-purple mx-2" />
                <asp:Button ID="btnApprove" runat="server" Text="Approve" OnClick="btnApprove_Click" CssClass="btn btn-success btn-purple mx-2" />
            </div>
        </div>

        <div class="row mb-3">
            <div class="col">
                <asp:TextBox ID="txtFeedback" runat="server" Height="68px" MaxLength="200" TextMode="MultiLine" CssClass="form-control" />
            </div>
        </div>

        <div class="row">
            <div class="col">
                <asp:Button ID="btnInProgressSubmit" runat="server" OnClick="btnInProgressSubmit_Click" Text="Submit" CssClass="btn btn-primary btn-purple" />
            </div>
        </div>
    </div>
</asp:Content>

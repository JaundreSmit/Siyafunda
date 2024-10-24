<%@ Page Async="true" Language="C#" MasterPageFile="~/site.master" AutoEventWireup="true" CodeBehind="frmUploadFiles.aspx.cs" Inherits="SiyafundaApplication.frmUploadFiles" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <title>Upload Files</title>

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

        /* New styles for disabled buttons */
        .btn:disabled {
            background-color: darkgrey; /* Dark grey background */
            color: white; /* White text for contrast */
            border-color: darkgrey; /* Match border color with background */
            opacity: 1; /* Ensure the button is fully opaque */
            cursor: not-allowed; /* Change cursor to indicate disabled */
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
            color: red; /* Red color for error labels */
        }
    </style>

    <div class="container mt-4">
        <div class="card bg-purple text-white mx-auto mb-3" style="max-width: 600px; margin-top: 10px;">
            <div class="card-body text-center">
                <h2 class="card-title"><strong>Upload Files</strong></h2>
            </div>
        </div>

        <div class="card mb-3" style="max-width: 600px; margin: auto;">
            <div class="card-body">
                <div class="mb-3">
                    <asp:Label ID="ModuleLabel" runat="server" Text="Select Module:" CssClass="form-label"></asp:Label>
                    <asp:DropDownList ID="ModuleDropDown" runat="server" DataTextField="title" DataValueField="module_id" CssClass="form-select" />
                </div>

                <div class="mb-3">
                    <asp:Label ID="FileLabel" runat="server" Text="Select File:" CssClass="form-label"></asp:Label>
                    <asp:FileUpload ID="FileUploadControl" runat="server" CssClass="form-control" />
                </div>

                <div class="mb-3">
                    <asp:Label ID="TitleLabel" runat="server" Text="Title:" CssClass="form-label"></asp:Label>
                    <asp:TextBox ID="txtTitle" runat="server" CssClass="form-control" />
                </div>

                <div class="mb-3">
                    <asp:Label ID="DescLabel" runat="server" Text="Description:" CssClass="form-label"></asp:Label>
                    <asp:TextBox ID="txtDesc" runat="server" Height="54px" TextMode="MultiLine" CssClass="form-control" />
                </div>

                <div class="text-center">
                    <asp:Button ID="UploadButton" runat="server" OnClick="UploadButton_Click" Text="Upload" CssClass="btn btn-primary w-50 btn-purple" />
                </div>
                <br />

                <div class="text-center">
                    <asp:Button ID="BackButton" runat="server" OnClick="BackButton_Click" Text="Back" CssClass="btn btn-primary w-25 btn-purple" />
                </div>

                <div class="mt-3">
                    <asp:Label ID="lblError" runat="server" Text="[Error Label]" CssClass="error-label"></asp:Label>
                </div>
            </div>
        </div>
    </div>
</asp:Content>

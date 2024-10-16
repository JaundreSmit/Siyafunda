<%@ Page Language="C#" MasterPageFile="~/site.master" AutoEventWireup="true" CodeBehind="frmTimeTableEdit.aspx.cs" Inherits="SiyafundaApplication.frmTimeTableEdit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <title>Edit Timetable</title>
    <div class="container">
        <div class="row">
            <div class="col-md-6">
                <div class="card bg-purple text-white mb-4" style="margin-top: 50px;">
                    <div class="card-header">
                        <h5 class="card-title">Add Class</h5>
                    </div>
                </div>
                <asp:Panel ID="pnlAddClass" runat="server">
                    <div class="form-group">
                        <label for="ddlModule">Module:</label>
                        <asp:DropDownList ID="ddlModule" runat="server" CssClass="form-control">
                        </asp:DropDownList>
                    </div>
                    <div class="form-group">
                        <label for="ddlDayOfWeek">Day of The Week:</label>
                        <asp:DropDownList ID="ddlDayOfWeek" runat="server" CssClass="form-control">
                        </asp:DropDownList>
                    </div>
                    <div class="form-group">
                        <label for="txtStartTime">Start Time:</label>
                        <asp:TextBox ID="txtStartTime" runat="server" CssClass="form-control"></asp:TextBox>
                    </div>
                    <div class="form-group">
                        <label for="txtEndTime">End Time:</label>
                        <asp:TextBox ID="txtEndTime" runat="server" CssClass="form-control"></asp:TextBox>
                    </div>
                    <div class="form-group">
                        <asp:Button ID="btnAddClass" runat="server" OnClick="btnAddClass_Click" Text="Add Class" CssClass="btn btn-primary btn-purple mx-2" style="margin-top: 20px;" />
                    </div>
                    <div>
                        <asp:Label ID="lblResult" runat="server" Text="[Results]" CssClass="text-success"></asp:Label>
                    </div>
                </asp:Panel>
            </div>

            <div class="col-md-6">
                <div class="card bg-purple text-white mb-4" style="margin-top: 50px;">
                    <div class="card-header">
                        <h5 class="card-title">Edit Timetable</h5>
                    </div>
                </div>
                <asp:Panel ID="pnlEditDeleteClass" runat="server">
                    <div class="form-group">
                        <label for="ddlClass">Select Class:</label>
                        <asp:DropDownList ID="ddlClass" runat="server" CssClass="form-control" OnSelectedIndexChanged="ddlClass_SelectedIndexChanged">
                        </asp:DropDownList>
                    </div>
                    <div class="form-group">
                        <asp:Label ID="lblEditDay" runat="server" Text="Day:" CssClass="control-label"></asp:Label>
                        <asp:DropDownList ID="ddlEditDayOfWeek" runat="server" CssClass="form-control">
                        </asp:DropDownList>
                    </div>
                    <div class="form-group">
                        <asp:Label ID="lblEditStartTime" runat="server" Text="Start Time:" CssClass="control-label"></asp:Label>
                        <asp:TextBox ID="txtEditStartTime" runat="server" CssClass="form-control"></asp:TextBox>
                    </div>
                    <div class="form-group">
                        <asp:Label ID="lblEditEndTime" runat="server" Text="End Time:" CssClass="control-label"></asp:Label>
                        <asp:TextBox ID="txtEditEndTime" runat="server" CssClass="form-control"></asp:TextBox>
                    </div>
                    <div class="form-group">
                        <asp:Button ID="btnConfirmEdit" runat="server" OnClick="btnConfirmEdit_Click" Text="Update Class" CssClass="btn btn-primary btn-purple mx-2" />
                        <asp:Button ID="btnDeleteClass" runat="server" OnClick="btnDeleteClass_Click" Text="Delete Class" CssClass="btn btn-primary btn-purple mx-2" />
                    </div>
                    <div>
                        <asp:Label ID="lblEditResults" runat="server" Text="[Results]" CssClass="text-success"></asp:Label>
                    </div>
                </asp:Panel>
            </div>
        </div>
        <div class="mt-3">
            <asp:Button ID="btnBack" runat="server" OnClick="btnBack_Click" Text="Done!" CssClass="btn btn-primary btn-purple mx-2" />
        </div>
    </div>
    <style>

        .label-margin {
            margin-top: 10px; /* Adjust the value as needed */
        }

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

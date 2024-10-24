﻿<%@ Page Language="C#" MasterPageFile="~/site.master" AutoEventWireup="true" CodeBehind="frmDashboard.aspx.cs" Inherits="SiyafundaApplication.Dashboard" Async="true"%>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <title>Dashboard</title>
    <div class="container mt-4"> <!-- Main container for the dashboard -->

        <!-- Welcome Section -->
        <div class="row mb-4"> <!-- Row for welcome message -->
            <div class="col-md-6">
                <asp:Label ID="lblWelcome" runat="server" Text="[Welcome Label]" CssClass="h4 custom-label"></asp:Label>
            </div>
            <div class="col-md-6 text-end">
                <asp:Label ID="lblRole" runat="server" Text="[Role Label]" CssClass="custom-label"></asp:Label>
                <asp:Button ID="btnProfile" runat="server" OnClick="btnProfile_Click" Text="Profile" CssClass="btn btn-primary w-5 btn-purple mx-2" />
                <asp:Button ID="btnLogOut" runat="server" OnClick="btnLogOut_Click" Text="Log Out" CssClass="btn btn-primary w-5 btn-purple" />
            </div>
        </div>

        <!-- Navigation Buttons -->
        <div class="row mb-4 text-center">
            <div class="col-md-3">
                <asp:Button ID="btnEducators" runat="server" Text="Educator Page" OnClick="btnEducators_Click" CssClass="btn btn-primary w-100 btn-purple" />
            </div>
            <div class="col-md-3">
                <asp:Button ID="btnModerators" runat="server" Text="Moderators Page" OnClick="btnModerators_Click" CssClass="btn btn-primary w-100 btn-purple" />
            </div>
            <div class="col-md-3">
                <asp:Button ID="btnSystemDevs" runat="server" Text="System Dev Page" OnClick="btnSystemDevs_Click" CssClass="btn btn-primary w-100 btn-purple" />
            </div>
            <div class="col-md-3">
                <asp:Button ID="btnSystemAdmins" runat="server" Text="System Admins" OnClick="btnSystemAdmins_Click" CssClass="btn btn-primary w-100 btn-purple" />
            </div>
        </div>

        <!-- Available Resources Section -->
        <h5 style="color: whitesmoke;">Available Resources:</h5>
        <asp:Label ID="lblError" runat="server" Text="[Error Label]" CssClass="text-danger "></asp:Label>
        <asp:GridView ID="dgvAvailableFiles" runat="server" AutoGenerateColumns="False" OnRowCommand="dgvAvailableFiles_RowCommand" CssClass="table table-striped" Width="100%">
    <Columns>
        <asp:BoundField DataField="ModuleName" HeaderText="Module Name" />
        
        <asp:TemplateField HeaderText="Resource Title">
            <ItemTemplate>
                <asp:LinkButton ID="lnkResourceTitle" runat="server" 
                                CommandName="Select" 
                                CommandArgument='<%# Eval("ResourceID") %>' 
                                Text='<%# Eval("ResourceTitle") %>' />
            </ItemTemplate>
        </asp:TemplateField>

        <asp:BoundField DataField="ResourceDescription" HeaderText="Description" />
        <asp:BoundField DataField="UploadDate" HeaderText="Upload Date" />
        <asp:BoundField DataField="FileSize" HeaderText="File Size" />
        
        <asp:TemplateField HeaderText="Actions">
            <ItemTemplate>
                <asp:LinkButton ID="btnDownload" runat="server" 
                                CommandName="Download" 
                                CommandArgument='<%# Eval("ResourceID") %>' 
                                Text="Download" CssClass="btn btn-success" />
            </ItemTemplate>
        </asp:TemplateField>
    </Columns>
</asp:GridView>

        <hr /> <!-- Horizontal line for separation -->

        <!-- Announcements Section -->
        <h5 style="color: whitesmoke;">Announcements</h5>
        <asp:GridView ID="dgvAnnouncements" runat="server" CssClass="table table-striped" Width="100%">
        </asp:GridView>
        <asp:Label ID="lblAnnoucementsError" runat="server" Text="[Announcements Errors]" CssClass="text-danger"></asp:Label>

        <hr /> <!-- Horizontal line for separation -->

        <!-- Time Table Section -->
        <h5 style="color: whitesmoke;">Time Table</h5>
        <asp:Label ID="lblDate" runat="server" Text="[Date]" CssClass="custom-label"></asp:Label>
        <asp:GridView ID="dgvTimeTable" runat="server" CssClass="table table-striped" Width="100%">
        </asp:GridView>
        <asp:Label ID="lblTimeTableError" runat="server" Text="[TT Error]" CssClass="text-danger"></asp:Label>
        <asp:Button ID="btnEditTimeTable" runat="server" OnClick="btnEditTimeTable_Click" Text="Edit Time Table" CssClass="btn btn-warning mt-2 w-100 btn-purple" />
    </div>

    <style type="text/css">
        
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

    </style>
</asp:Content>

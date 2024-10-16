<%@ Page Language="C#" MasterPageFile="~/site.master" AutoEventWireup="true" CodeBehind="frmDashboard.aspx.cs" Inherits="SiyafundaApplication.Dashboard" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <title>Dashboard</title>
    <div class="container mt-4"> <!-- Main container for the dashboard -->

        <!-- Welcome Section -->
        <div class="row mb-4"> <!-- Row for welcome message -->
            <div class="col-md-6">
                <asp:Label ID="lblWelcome" runat="server" Text="[Welcome Label]" CssClass="h4"></asp:Label>
            </div>
            <div class="col-md-6 text-end">
                <asp:Label ID="lblRole" runat="server" Text="[Role Label]"></asp:Label>
                <asp:Button ID="btnProfile" runat="server" OnClick="btnProfile_Click" Text="Profile" CssClass="btn btn-secondary mx-2" />
                <asp:Button ID="btnLogOut" runat="server" OnClick="btnLogOut_Click" Text="Log Out" CssClass="btn btn-danger" />
            </div>
        </div>

        <!-- Navigation Buttons -->
        <div class="row mb-4 text-center">
            <div class="col-md-3">
                <asp:Button ID="btnEducators" runat="server" Text="Educator Page" OnClick="btnEducators_Click" CssClass="btn btn-primary w-100" />
            </div>
            <div class="col-md-3">
                <asp:Button ID="btnModerators" runat="server" Text="Moderators Page" OnClick="btnModerators_Click" CssClass="btn btn-primary w-100" />
            </div>
            <div class="col-md-3">
                <asp:Button ID="btnSystemDevs" runat="server" Text="System Dev Page" OnClick="btnSystemDevs_Click" CssClass="btn btn-primary w-100" />
            </div>
            <div class="col-md-3">
                <asp:Button ID="btnSystemAdmins" runat="server" Text="System Admins" OnClick="btnSystemAdmins_Click" CssClass="btn btn-primary w-100" />
            </div>
        </div>

        <!-- Available Resources Section -->
        <h5>Available Resources:</h5>
        <asp:Label ID="lblError" runat="server" Text="[Error Label]" CssClass="text-danger"></asp:Label>
        <asp:GridView ID="dgvAvailableFiles" runat="server" CssClass="table table-striped" Width="100%">
        </asp:GridView>

        <hr /> <!-- Horizontal line for separation -->

        <!-- Announcements Section -->
        <h5>Announcements</h5>
        <asp:GridView ID="dgvAnnouncements" runat="server" CssClass="table table-striped" Width="100%">
        </asp:GridView>
        <asp:Label ID="lblAnnoucementsError" runat="server" Text="[Announcements Errors]" CssClass="text-danger"></asp:Label>

        <hr /> <!-- Horizontal line for separation -->

        <!-- Time Table Section -->
        <h5>Time Table</h5>
        <asp:Label ID="lblDate" runat="server" Text="[Date]"></asp:Label>
        <asp:GridView ID="dgvTimeTable" runat="server" CssClass="table table-striped" Width="100%">
        </asp:GridView>
        <asp:Label ID="lblTimeTableError" runat="server" Text="[TT Error]" CssClass="text-danger"></asp:Label>
        <asp:Button ID="btnEditTimeTable" runat="server" OnClick="btnEditTimeTable_Click" Text="Edit Time Table" CssClass="btn btn-warning mt-2 w-100" />
    </div>
</asp:Content>

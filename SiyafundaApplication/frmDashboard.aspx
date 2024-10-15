<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmDashboard.aspx.cs" Inherits="SiyafundaApplication.Dashboard" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        .auto-style2 {
            width: 112px;
        }
        .auto-style4 {
            width: 120px;
            height: 30px;
        }
        .auto-style5 {
            width: 146px;
        }
        .auto-style8 {
            width: 4px;
        }
        .auto-style9 {
            width: 1006px;
        }
        .auto-style10 {
            width: 1006px;
            height: 23px;
        }
        .auto-style11 {
            width: 4px;
            height: 30px;
        }
        .auto-style12 {
            width: 112px;
            height: 30px;
        }
        .auto-style13 {
            width: 146px;
            height: 30px;
        }
        .auto-style14 {
            width: 392px;
        }
        .auto-style16 {
            width: 42%;
            margin-right: 0px;
        }
        .auto-style17 {
            width: 299px;
        }
        .auto-style18 {
            width: 299px;
            height: 24px;
        }
        </style>
</head>
<body>
    <form id="form1" runat="server">
        <div>
        </div>
        <table style="width:100%;">
            <tr>
                <td class="auto-style8">
                    <asp:Label ID="lblWelcome" runat="server" Text="[Welcome Label]"></asp:Label>
                </td>
                <td class="auto-style2">
                    <asp:Label ID="lblRole" runat="server" Text="[Role Label]"></asp:Label>
                </td>
                <td class="auto-style5">
                    <asp:Button ID="btnProfile" runat="server" OnClick="btnProfile_Click" Text="Profile" />
                &nbsp;&nbsp;
        <asp:Button ID="btnLogOut" runat="server" OnClick="btnLogOut_Click" Text="Log Out" />
                </td>
            </tr>
            <tr>
                <td class="auto-style8">&nbsp;</td>
                <td class="auto-style2">&nbsp;</td>
                <td class="auto-style5">&nbsp;</td>
            </tr>
            <tr>
                <td class="auto-style11">
                    <asp:Button ID="btnEducators" runat="server" Text="Educator Page" OnClick="btnEducators_Click" />
                </td>
                <td class="auto-style12">
                    <asp:Button ID="btnModerators" runat="server" Text="Moderators Page" OnClick="btnModerators_Click" />
                </td>
                <td class="auto-style13">
                    <asp:Button ID="btnSystemDevs" runat="server" Text="System Dev Page" OnClick="btnSystemDevs_Click" />
                </td>
                <td class="auto-style4">
                    <asp:Button ID="btnSystemAdmins" runat="server" Text="SystemAdmins" OnClick="btnSystemAdmins_Click" />
                </td>
            </tr>
            <tr>
    <td class="auto-style8">&nbsp;</td>
    <td class="auto-style2">&nbsp;</td>
    <td class="auto-style5">&nbsp;</td>
</tr>
            <tr>
    <td class="auto-style8">
        &nbsp;</td>
    <td class="auto-style2">&nbsp;</td>
    <td class="auto-style5">&nbsp;</td>
</tr>
        </table>
        <table style="width:100%;">
            <tr>
                <td class="auto-style10">Available Resources:</td>
            </tr>
            <tr>
                <td class="auto-style9">
                    <asp:Label ID="lblError" runat="server" Text="[Error Label]"></asp:Label>
                </td>
            </tr>
            <tr>
    <td class="auto-style10">
                    <asp:GridView ID="dgvAvailableFiles" runat="server" Width="552px">
                    </asp:GridView>
                </td>
</tr>
            </table>
    <p>
        &nbsp;</p>
    <table class="auto-style16">
        <tr>
            <td class="auto-style14">Announcements</td>
        </tr>
        <tr>
            <td class="auto-style14">
                <asp:GridView ID="dgvAnnouncements" runat="server" Width="350px">
                </asp:GridView>
            </td>

        </tr>
        <tr>
            <td class="auto-style14">
                <asp:Label ID="lblAnnoucementsError" runat="server" Text="[Annoucements Errors]"></asp:Label>
            </td>
            
        </tr>
    </table>
        <br />
    <table style="width:100%;">
        <tr>
            <td class="auto-style17">Time Table</td>
        </tr>
        <tr>
            <td class="auto-style17">
                <asp:Label ID="lblDate" runat="server" Text="[Date]"></asp:Label>
                <asp:GridView ID="dgvTimeTable" runat="server" Width="392px">
                </asp:GridView>
                <asp:Label ID="lblTimeTableError" runat="server" Text="[TT Error]"></asp:Label>
            </td>
        </tr>
        <tr>
            <td class="auto-style18">
                &nbsp;
                <asp:Button ID="btnEditTimeTable" runat="server" OnClick="btnEditTimeTable_Click" Text="Edit Time Table" />
&nbsp;
            </td>
        </tr>
    </table>
    </form>
    </body>
</html>

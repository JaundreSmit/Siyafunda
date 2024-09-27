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
                <td class="auto-style8">
                    <asp:Button ID="btnEducators" runat="server" Text="Educator Page" OnClick="btnEducators_Click" />
                </td>
                <td class="auto-style2">
                    <asp:Button ID="btnModerators" runat="server" Text="Moderators Page" OnClick="btnModerators_Click" />
                </td>
                <td class="auto-style5">
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
                    <asp:GridView ID="dgvAvailableFiles" runat="server" Width="1479px">
                    </asp:GridView>
                </td>
</tr>
            </table>
    </form>
</body>
</html>

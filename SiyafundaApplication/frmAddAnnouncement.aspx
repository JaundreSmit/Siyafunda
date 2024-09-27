﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmAddAnnouncement.aspx.cs" Inherits="SiyafundaApplication.frmAddAnnouncement" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        .auto-style1 {
            height: 23px;
        }
        .auto-style2 {
            width: 98px;
        }
        .auto-style3 {
            height: 23px;
            width: 98px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <table style="width:100%;">
                <tr>
                    <td colspan="3">Add Announcements</td>
                </tr>
                <tr>
                    <td class="auto-style2">&nbsp;</td>
                    <td>&nbsp;</td>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td class="auto-style2">Module:</td>
                    <td>
                        <asp:Label ID="lblModuleName" runat="server" Text="[Module Name]"></asp:Label>
                    </td>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td class="auto-style2">Title:</td>
                    <td>
                        <asp:TextBox ID="txtTitle" runat="server" MaxLength="99"></asp:TextBox>
                    </td>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td class="auto-style3">Content:</td>
                    <td class="auto-style1">
                        <asp:TextBox ID="txtContent" runat="server" Height="121px" MaxLength="600" TextMode="MultiLine"></asp:TextBox>
                    </td>
                    <td class="auto-style1"></td>
                </tr>
                <tr>
                    <td class="auto-style2">&nbsp;</td>
                    <td>
                        <asp:Label ID="lblErrors" runat="server" Text="[Errors]"></asp:Label>
                    </td>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td class="auto-style2">
                        <asp:Button ID="Back" runat="server" OnClick="Back_Click" Text="Back" />
                    </td>
                    <td>
                        <asp:Button ID="btnAddAnnouncement" runat="server" OnClick="btnAddAnnouncement_Click" Text="Add Announcement" />
                    </td>
                    <td>&nbsp;</td>
                </tr>
            </table>
        </div>
    </form>
</body>
</html>

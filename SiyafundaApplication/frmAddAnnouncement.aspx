<%@ Page Language="C#" MasterPageFile="~/site.master" AutoEventWireup="true" CodeBehind="frmAddAnnouncement.aspx.cs" Inherits="SiyafundaApplication.frmAddAnnouncement" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <title>Add Announcement</title>
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
        .auto-style4 {
            width: 406px;
        }
        .auto-style5 {
            height: 23px;
            width: 406px;
        }
    </style>

    <div>
        <table style="width:100%;">
            <tr>
                <td colspan="3">Add Announcement</td>
            </tr>
            <tr>
                <td class="auto-style2">&nbsp;</td>
                <td class="auto-style4">&nbsp;</td>
                <td>&nbsp;</td>
            </tr>
            <tr>
                <td class="auto-style2">Module:</td>
                <td class="auto-style4">
                    <asp:Label ID="lblModuleName" runat="server" Text="[Module Name]"></asp:Label>
                </td>
                <td>&nbsp;</td>
            </tr>
            <tr>
                <td class="auto-style2">Title:</td>
                <td class="auto-style4">
                    <asp:TextBox ID="txtTitle" runat="server" Height="21px" MaxLength="600" Width="448px"></asp:TextBox>
                </td>
                <td>&nbsp;</td>
            </tr>
            <tr>
                <td class="auto-style3">Content:</td>
                <td class="auto-style5">
                    <asp:TextBox ID="txtContent" runat="server" Height="121px" MaxLength="600" TextMode="MultiLine" Width="448px"></asp:TextBox>
                </td>
                <td class="auto-style1"></td>
            </tr>
            <tr>
                <td class="auto-style2">&nbsp;</td>
                <td class="auto-style4">
                    <asp:Label ID="lblErrors" runat="server" Text="[Errors]"></asp:Label>
                </td>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
            </tr>
            <tr>
                <td class="auto-style2">
                    <asp:Button ID="Back" runat="server" OnClick="Back_Click" Text="Back" />
                </td>
                <td class="auto-style4">
                    <asp:Button ID="btnAddAnnouncement" runat="server" OnClick="btnAddAnnouncement_Click" Text="Add Announcement" />
                </td>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
            </tr>
        </table>
    </div>
</asp:Content>

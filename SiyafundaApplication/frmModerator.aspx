<%@ Page Language="C#" MasterPageFile="~/site.master" AutoEventWireup="true" CodeBehind="frmModerator.aspx.cs" Inherits="SiyafundaApplication.frmModerator" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <title>File Moderation</title>
    <style type="text/css">
        .auto-style1 {
            width: 218px;
        }
        .auto-style2 {
            width: 218px;
            height: 23px;
        }
        .auto-style3 {
            height: 23px;
        }
        .auto-style4 {
            width: 657px;
        }
        .auto-style5 {
            width: 243px;
        }
        .auto-style6 {
            width: 113px;
        }
        .auto-style7 {
            width: 707px;
        }
        .auto-style8 {
            height: 23px;
            width: 707px;
        }
    </style>

    <table style="width:100%;">
        <tr>
            <td class="auto-style1"><strong>File Moderation:</strong></td>
            <td class="auto-style7">
                <asp:Button ID="btnBack" runat="server" OnClick="btnBack_Click" Text="Back" />
            </td>
            <td>&nbsp;</td>
        </tr>
        <tr>
            <td class="auto-style2"></td>
            <td class="auto-style8">&nbsp;</td>
            <td class="auto-style3"></td>
        </tr>
        <tr>
            <td class="auto-style1">In Progress files:</td>
            <td class="auto-style7">
                Search title:
                <asp:TextBox ID="txtSearchProgress" runat="server" AutoPostBack="True" OnTextChanged="txtSearchProgress_TextChanged"></asp:TextBox>
                &nbsp;
                <asp:Label ID="lblProgressErrors" runat="server" Text="[Progress Errors]"></asp:Label>
            </td>
            <td>&nbsp;</td>
        </tr>
        <tr>
            <td class="auto-style2">&nbsp;</td>
            <td class="auto-style8">
                <asp:GridView ID="dgvInProgress" runat="server" Width="581px" AllowSorting="True" OnSelectedIndexChanged="dgvInProgress_SelectedIndexChanged">
                </asp:GridView>
            </td>
            <td class="auto-style3"></td>
        </tr>
    </table>

    <table>
        <tr>
            <td class="auto-style6">
                <asp:Button ID="btnReject" runat="server" Text="Reject" OnClick="btnReject_Click" />
            </td>
            <td class="auto-style5">
                <asp:Button ID="btnApprove" runat="server" Text="Approve" OnClick="btnApprove_Click" />
            </td>
            <td class="auto-style4">&nbsp;</td>
        </tr>
        <tr>
            <td colspan="2">
                <asp:TextBox ID="txtFeedback" runat="server" Height="68px" MaxLength="200" TextMode="MultiLine" Width="451px"></asp:TextBox>
            </td>
            <td class="auto-style4">&nbsp;</td>
        </tr>
        <tr>
            <td class="auto-style1">
                <asp:Button ID="btnInProgressSubmit" runat="server" OnClick="btnInProgressSubmit_Click" Text="Submit" />
            </td>
            <td class="auto-style5">&nbsp;</td>
            <td class="auto-style1">&nbsp;</td>
        </tr>
    </table>
</asp:Content>

<%@ Page Language="C#" MasterPageFile="~/site.master" AutoEventWireup="true" CodeBehind="frmModerator.aspx.cs" Inherits="SiyafundaApplication.frmModerator" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <title>File Moderation</title>
    
    <table style="width:100%;">
        <tr>
            <td><strong>File Moderation:</strong></td>
            <td>
                <asp:Button ID="btnBack" runat="server" OnClick="btnBack_Click" Text="Back" />
            </td>
        </tr>
        <tr>
            <td>In Progress files:</td>
            <td>
                Search title:
                <asp:TextBox ID="txtSearchProgress" runat="server" AutoPostBack="True" OnTextChanged="txtSearchProgress_TextChanged"></asp:TextBox>
                <asp:Label ID="lblProgressErrors" runat="server" Text="[Progress Errors]" Visible="false"></asp:Label>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <asp:GridView ID="dgvInProgress" runat="server" AutoGenerateColumns="false" AllowSorting="true" DataKeyNames="resource_id" OnSelectedIndexChanged="dgvInProgress_SelectedIndexChanged">
                    <Columns>
                        <asp:BoundField DataField="ModuleTitle" HeaderText="Module" />
                        <asp:BoundField DataField="ResourceTitle" HeaderText="Title" />
                        <asp:BoundField DataField="description" HeaderText="Description" />
                        <asp:BoundField DataField="upload_date" HeaderText="Upload Date" />
                        <asp:BoundField DataField="file_type" HeaderText="Type" />
                        <asp:BoundField DataField="file_size" HeaderText="Size" />
                        <asp:CommandField ShowSelectButton="True" />
                    </Columns>
                </asp:GridView>
            </td>
        </tr>
    </table>

    <table>
        <tr>
            <td>
                <asp:Button ID="btnReject" runat="server" Text="Reject" OnClick="btnReject_Click" Visible="false" />
            </td>
            <td>
                <asp:Button ID="btnApprove" runat="server" Text="Approve" OnClick="btnApprove_Click" Visible="false" />
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <asp:TextBox ID="txtFeedback" runat="server" TextMode="MultiLine" Height="68px" MaxLength="200" Width="451px" Visible="false"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Button ID="btnInProgressSubmit" runat="server" OnClick="btnInProgressSubmit_Click" Text="Submit" Visible="false" />
            </td>
        </tr>
    </table>
</asp:Content>


<%@ Page Language="C#" MasterPageFile="~/site.master" AutoEventWireup="true" CodeBehind="frmEducator.aspx.cs" Inherits="SiyafundaApplication.frmEducator" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <title>Student Management</title>
    <style type="text/css">
        .auto-style1 {
            width: 218px;
        }
        .auto-style8 {
            width: 218px;
            height: 23px;
        }
        .auto-style9 {
            width: 229px;
        }
        .auto-style10 {
            width: 229px;
            height: 23px;
        }
        .auto-style11 {
            width: 338px;
        }
        .auto-style12 {
            width: 338px;
            height: 23px;
        }
        .auto-style13 {
            width: 284px;
        }
        .auto-style14 {
            width: 284px;
            height: 31px;
        }
        .auto-style15 {
            height: 31px;
        }
    </style>

    <table style="width:100%;">
        <tr>
            <td class="auto-style9"><strong>Student Management:</strong></td>
            <td class="auto-style11">&nbsp;</td>
            <td>
                <asp:Button ID="btnBack" runat="server" OnClick="btnBack_Click" Text="Back" />
            </td>
        </tr>
        <tr>
            <td class="auto-style10">
                &nbsp;
                <asp:RadioButtonList ID="rblStudentFilter" runat="server" AutoPostBack="True" OnSelectedIndexChanged="rblStudentFilter_SelectedIndexChanged">
                </asp:RadioButtonList>
            </td>
            <td class="auto-style12">
                Search: 
                <asp:TextBox ID="txtSearchStudents" runat="server" AutoPostBack="True" OnTextChanged="txtSearchStudents_TextChanged"></asp:TextBox>
                &nbsp;<asp:Label ID="lblStudentErrors" runat="server" Text="[Student Errors]"></asp:Label>
            </td>
            <td class="auto-style8"></td>
        </tr>
        <tr>
            <td colspan="2">
                <asp:GridView ID="dgvStudents" runat="server" OnSelectedIndexChanged="dgvStudents_SelectedIndexChanged" Width="1184px">
                </asp:GridView>
            </td>
            <td class="auto-style1"></td>
        </tr>
        <tr>
            <td class="auto-style9">
                <asp:Button ID="btnRemoveStudent" runat="server" OnClick="btnRemoveStudent_Click" Text="Remove Student" />
                &nbsp;<asp:Button ID="btnAssignStudent" runat="server" OnClick="btnAddStudent_Click" Text="Add Student" />
            </td>
            <td class="auto-style11">&nbsp;</td>
            <td class="auto-style1"></td>
        </tr>
    </table>
    <table style="width:100%;">
        <tr>
            <td class="auto-style13">Other Features:</td>
            <td>&nbsp;</td>
            <td>&nbsp;</td>
        </tr>
        <tr>
            <td class="auto-style13">
                <asp:Button ID="btnUploadResources" runat="server" OnClick="btnUploadResources_Click" Text="Upload Resources" Width="172px" />
            </td>
            <td>&nbsp;</td>
            <td>&nbsp;</td>
        </tr>
        <tr>
            <td class="auto-style14">
                <asp:Button ID="btnAddAnnouncement" runat="server" OnClick="btnAddAnnouncement_Click" Text="Add Announcement" />
            </td>
            <td class="auto-style15"></td>
            <td class="auto-style15"></td>
        </tr>
        <tr>
            <td>
                <asp:Button ID="btnAddFaq" runat="server" OnClick="btnAddFaq_Click" Text="Add FAQ" Width="172px" />
            </td>
            <td>&nbsp;</td>
            <td>&nbsp;</td>
        </tr>
    </table>
</asp:Content>

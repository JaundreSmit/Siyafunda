<%@ Page Language="C#" MasterPageFile="~/site.master" AutoEventWireup="true" CodeBehind="frmTimeTableEdit.aspx.cs" Inherits="SiyafundaApplication.frmTimeTableEdit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <title>Edit Timetable</title>
    <div>
        <asp:Panel ID="pnlAddClass" runat="server" Height="193px" Width="293px">
            <table class="auto-style4">
                <tr>
                    <td class="auto-style5">Module:</td>
                    <td class="auto-style6">
                        <asp:DropDownList ID="ddlModule" runat="server" Height="16px" Width="122px">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td class="auto-style1">Day of The Week:</td>
                    <td>
                        <asp:DropDownList ID="ddlDayOfWeek" runat="server" Height="18px" Width="122px">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td class="auto-style1">Start Time:</td>
                    <td>
                        <asp:TextBox ID="txtStartTime" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="auto-style2">End Time:</td>
                    <td class="auto-style3">
                        <asp:TextBox ID="txtEndTime" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="auto-style2">
                        <asp:Button ID="btnAddClass" runat="server" OnClick="btnAddClass_Click" Text="Add Class" style="height: 26px" />
                    </td>
                    <td class="auto-style3">
                        &nbsp;</td>
                </tr>
                <tr>
                    <td class="auto-style2">
                        <asp:Label ID="lblResult" runat="server" Text="[Results]"></asp:Label>
                    </td>
                </tr>
            </table>
        </asp:Panel>

        <asp:Panel ID="pnlEditDeleteClass" runat="server" Height="211px" Width="292px">
            <table style="width:100%;">
                <tr>
                    <td class="auto-style10">Edit Time table</td>
                    <td class="auto-style11"></td>
                </tr>
                <tr>
                    <td class="auto-style9">Select Class:</td>
                    <td class="auto-style6">
                        <asp:DropDownList ID="ddlClass" runat="server" Height="16px" Width="162px" OnSelectedIndexChanged="ddlClass_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td class="auto-style10">
                        <asp:Label ID="lblEditDay" runat="server" Text="Day:"></asp:Label>
                    </td>
                    <td class="auto-style11">
                        <asp:DropDownList ID="ddlEditDayOfWeek" runat="server" Height="16px" Width="162px">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td class="auto-style10">
                        <asp:Label ID="lblEditStartTime" runat="server" Text="Start Time:"></asp:Label>
                    </td>
                    <td class="auto-style11">
                        <asp:TextBox ID="txtEditStartTime" runat="server" Width="156px"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="auto-style8">
                        <asp:Label ID="lblEditEndTime" runat="server" Text="End Time:"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtEditEndTime" runat="server" Width="156px"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="auto-style12">
                        <asp:Button ID="btnConfirmEdit" runat="server" OnClick="btnConfirmEdit_Click" Text="Update Class" />
                    </td>
                    <td class="auto-style13">
                        <asp:Button ID="btnDeleteClass" runat="server" OnClick="btnDeleteClass_Click" Text="Delete Class" />
                    </td>
                </tr>
                <tr>
                    <td class="auto-style12">
                        <asp:Label ID="lblEditResults" runat="server" Text="[Results]"></asp:Label>
                    </td>
                    <td class="auto-style13">&nbsp;</td>
                </tr>
            </table>
        </asp:Panel>

        <asp:Button ID="btnBack" runat="server" OnClick="btnBack_Click" Text="Done!" />
    </div>
</asp:Content>

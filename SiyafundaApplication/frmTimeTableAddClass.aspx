<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmTimeTableAddClass.aspx.cs" Inherits="SiyafundaApplication.frmTimeTableAddClass" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        .auto-style1 {
            width: 284px;
        }
        .auto-style2 {
            width: 284px;
            height: 22px;
        }
        .auto-style3 {
            height: 22px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <table style="width:100%;">
                <tr>
                    <td class="auto-style1">Module:</td>
                    <td>
                        <asp:DropDownList ID="ddlModule" runat="server">
                        </asp:DropDownList>
                    </td>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td class="auto-style1">Day of The Week:</td>
                    <td>
                        <asp:DropDownList ID="ddlDayOfWeek" runat="server">
                        </asp:DropDownList>
                    </td>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td class="auto-style1">Start Time:</td>
                    <td>
                        <asp:TextBox ID="txtStartTime" runat="server"></asp:TextBox>
                    </td>
                    <td>&nbsp;</td>
                </tr>
            </table>
            <table style="width:100%;">
                <tr>
                    <td class="auto-style2">End Time:</td>
                    <td class="auto-style3">
                        <asp:TextBox ID="txtEndTime" runat="server"></asp:TextBox>
                    </td>
                    <td class="auto-style3"></td>
                </tr>
            </table>
            <table style="width:100%;">
                <tr>
                    <td class="auto-style2">
                        <asp:Button ID="btnAddClass" runat="server" OnClick="btnAddClass_Click" Text="Add Class" style="height: 26px" />
                    </td>
                    <td class="auto-style3">
                        <asp:Button ID="btnBack" runat="server" OnClick="btnBack_Click" Text="Done!" />
                    </td>
                    <td class="auto-style3"></td>
                </tr>
            </table>
        </div>
        <table style="width:100%;">
            <tr>
                <td class="auto-style2">
                    <asp:Label ID="lblResult" runat="server" Text="[Results]"></asp:Label>
                </td>
                <td class="auto-style3">&nbsp;</td>
                <td class="auto-style3"></td>
            </tr>
        </table>
    </form>
</body>
</html>

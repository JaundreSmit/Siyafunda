<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmSystemDev.aspx.cs" Inherits="SiyafundaApplication.frmSystemDev" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        .auto-style1 {
            width: 43%;
        }
        .auto-style2 {
            width: 175px;
        }
        .auto-style3 {
            width: 175px;
            height: 23px;
        }
        .auto-style5 {
            width: 175px;
            height: 79px;
        }
        .auto-style7 {
            height: 23px;
            width: 352px;
        }
        .auto-style8 {
            width: 352px;
        }
        .auto-style9 {
            height: 79px;
            width: 352px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <table style="width:100%;">
                <tr>
                    <td>System Developer management</td>
                    <td>&nbsp;</td>
                    <td>
                        <asp:Button ID="btnBack" runat="server" OnClick="btnBack_Click" Text="Back" />
                    </td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                    <td>&nbsp;</td>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                    <td>&nbsp;</td>
                    <td>&nbsp;</td>
                </tr>
            </table>
        </div>
    <table class="auto-style1">
        <tr>
            <td class="auto-style3">Add Module</td>
            <td class="auto-style7"></td>
        </tr>
        <tr>
            <td class="auto-style2">Title:</td>
            <td class="auto-style8">
                <asp:TextBox ID="txtAddModName" runat="server" MaxLength="100" Width="169px"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="auto-style5">Description</td>
            <td class="auto-style9">
                <asp:TextBox ID="txtAddModDesc" runat="server" Height="49px" MaxLength="200" TextMode="MultiLine" Width="397px"></asp:TextBox>
            </td>
        </tr>
        <tr>
    <td class="auto-style2">Educator:</td>
    <td class="auto-style8">
        <asp:DropDownList ID="ddlAddEducator" runat="server">
        </asp:DropDownList>
            </td>
</tr>
        <tr>
    <td class="auto-style2">
        <asp:Label ID="lblAddModErrors" runat="server" Text="[Adding Errors]"></asp:Label>
            </td>
    <td class="auto-style8">&nbsp;</td>
</tr>
        <tr>
    <td class="auto-style2">
        <asp:Button ID="btnAddMod" runat="server" Text="Add Module" OnClick="btnAddMod_Click" />
            </td>
    <td class="auto-style8">&nbsp;</td>
</tr>
        <tr>
    <td class="auto-style2">&nbsp;</td>
    <td class="auto-style8">&nbsp;</td>
</tr>
    </table>
        <br />
    <table class="auto-style1">
        <tr>
            <td class="auto-style3">Edit Module</td>
            <td class="auto-style7"></td>
        </tr>
                <tr>
    <td class="auto-style2">Module:</td>
    <td class="auto-style8">
        <asp:DropDownList ID="ddlEditModSelect" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlEditModSelect_SelectedIndexChanged">
        </asp:DropDownList>
                    </td>
</tr>
        <tr>
            <td class="auto-style2">Title:</td>
            <td class="auto-style8">
                <asp:TextBox ID="txtEditModTitle" runat="server" MaxLength="100" Width="169px"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="auto-style5">Description</td>
            <td class="auto-style9">
                <asp:TextBox ID="txtEditModDesc" runat="server" Height="49px" MaxLength="200" TextMode="MultiLine" Width="397px"></asp:TextBox>
            </td>
        </tr>
        <tr>
    <td class="auto-style2">Educator:</td>
    <td class="auto-style8">
        <asp:DropDownList ID="ddlEditEducator" runat="server">
        </asp:DropDownList>
            </td>
</tr>
        <tr>
    <td class="auto-style2">
        <asp:Label ID="lblEditModErrors" runat="server" Text="[Editing Errors]"></asp:Label>
            </td>
    <td class="auto-style8">&nbsp;</td>
</tr>
        <tr>
    <td class="auto-style2">
        <asp:Button ID="btnEditMod" runat="server" Text="Edit Module" OnClick="btnEditMod_Click" />
            </td>
    <td class="auto-style8">
        <asp:Button ID="btnDeleteModule" runat="server" Text="Delete Module" OnClick="btnDeleteModule_Click" />
            </td>
</tr>
        <tr>
    <td class="auto-style2">&nbsp;</td>
    <td class="auto-style8">&nbsp;</td>
</tr>
    </table>
    </form>
    </body>
</html>

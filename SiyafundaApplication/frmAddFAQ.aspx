<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmAddFAQ.aspx.cs" Inherits="SiyafundaApplication.frmAddFAQ" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">

        .auto-style2 {
            width: 98px;
        }
        .auto-style3 {
            height: 23px;
            width: 98px;
        }
        .auto-style1 {
            height: 23px;
        }
        .auto-style4 {
            width: 537px;
        }
        .auto-style5 {
            height: 23px;
            width: 537px;
        }
        .auto-style8 {
            width: 97px;
        }
        .auto-style6 {
            width: 68px;
        }
        .auto-style7 {
            width: 107px;
        }
        .auto-style9 {
            height: 23px;
            width: 97px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
            <table style="width:100%;">
                <tr>
                    <td colspan="5" class="auto-style1">Add FAQ<br />
                    </td>
                </tr>
                <tr>
                    <td class="auto-style8">&nbsp;</td>
                    <td class="auto-style4">&nbsp;</td>
                    <td class="auto-style6">&nbsp;</td>
                    <td class="auto-style7">&nbsp;</td>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td class="auto-style8">Module:</td>
                    <td class="auto-style4">
                        <asp:Label ID="lblModuleName" runat="server" Text="[Module Name]"></asp:Label>
                    </td>
                    <td class="auto-style6">&nbsp;</td>
                    <td class="auto-style7">&nbsp;</td>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td class="auto-style8">Question:</td>
                    <td class="auto-style4">
                        <asp:TextBox ID="txtQuestion" runat="server" Height="121px" MaxLength="600" TextMode="MultiLine" Width="382px"></asp:TextBox>
                    </td>
                    <td class="auto-style6">&nbsp;</td>
                    <td class="auto-style7">Answer:</td>
                    <td>
                        <asp:TextBox ID="txtAnwer" runat="server" Height="121px" MaxLength="600" TextMode="MultiLine" Width="382px"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="auto-style9">&nbsp;</td>
                    <td class="auto-style5">
                        &nbsp;</td>
                    <td class="auto-style6">&nbsp;</td>
                    <td class="auto-style7">&nbsp;</td>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td class="auto-style8">&nbsp;</td>
                    <td class="auto-style4">
                        <asp:Label ID="lblErrors" runat="server" Text="[Errors]"></asp:Label>
                    </td>
                    <td class="auto-style6">&nbsp;</td>
                    <td class="auto-style7">&nbsp;</td>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td class="auto-style8">
                        <asp:Button ID="Back" runat="server" OnClick="Back_Click" Text="Back" />
                    </td>
                    <td class="auto-style4">
                        <asp:Button ID="btnAddFAQ" runat="server" OnClick="btnAddFAQ_Click" Text="Add FAQ" />
                    </td>
                    <td class="auto-style6">&nbsp;</td>
                    <td class="auto-style7">&nbsp;</td>
                    <td>&nbsp;</td>
                </tr>
            </table>
        </form>
</body>
</html>

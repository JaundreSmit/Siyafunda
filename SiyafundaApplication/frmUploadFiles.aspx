<%@ Page Async="true" Language="C#" AutoEventWireup="true" CodeBehind="frmUploadFiles.aspx.cs" Inherits="SiyafundaApplication.frmUploadFiles" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        .auto-style1 {
            width: 143px;
        }
        .auto-style2 {
            width: 525px;
        }
    </style>
</head>
<body>
    <form id="form2" runat="server">
        <table style="width:100%;">
            <tr>
                <td class="auto-style1">
                    <asp:Label ID="ModuleLabel" runat="server" Text="Select Module:"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ModuleDropDown" runat="server" DataTextField="title" DataValueField="module_id" Height="17px" Width="273px" />
                </td>
                <td>&nbsp;</td>
            </tr>
            <tr>
                <td class="auto-style1">Select File:</td>
                <td>
                    <asp:FileUpload ID="FileUploadControl" runat="server" Width="273px" />
                </td>
                <td>&nbsp;</td>
            </tr>
            <tr>
                <td class="auto-style1">Title:</td>
                <td>
                    <asp:TextBox ID="txtTitle" runat="server"></asp:TextBox>
                </td>
                <td>&nbsp;</td>
            </tr>
            <tr>
                <td class="auto-style1">Description:</td>
                <td>
                    <asp:TextBox ID="txtDesc" runat="server" Height="54px" TextMode="MultiLine" Width="261px"></asp:TextBox>
                </td>
                <td>&nbsp;</td>
            </tr>
            <tr>
                <td class="auto-style1">
                    <asp:Button ID="UploadButton" runat="server" OnClick="UploadButton_Click" Text="Upload" />
                </td>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
            </tr>
        </table>
        <table style="width:100%;">
            <tr>
                <td class="auto-style2">
                    <asp:Label ID="lblError" runat="server" Text="[Error Label]"></asp:Label>
                </td>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
            </tr>
            <tr>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
            </tr>
        </table>
    </form>
</body>
</html>






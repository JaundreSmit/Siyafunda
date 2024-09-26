<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmUploadFiles.aspx.cs" Inherits="SiyafundaApplication.frmUploadFiles" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Upload Files</title>
    <style type="text/css">
        .auto-style1 {
            width: 143px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
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
                <td class="auto-style1">
            <asp:Button ID="UploadButton" runat="server" OnClick="UploadButton_Click" Text="Upload" />
                </td>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
            </tr>
        </table>
        <table style="width:100%;">
            <tr>
                <td class="auto-style1">
                    <asp:Label ID="lblError" runat="server" Text="[Error Label]"></asp:Label>
                </td>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
            </tr>
        </table>
    </form>
</body>
</html>

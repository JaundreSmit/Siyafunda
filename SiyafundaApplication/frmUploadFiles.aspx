<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmUploadFiles.aspx.cs" Inherits="SiyafundaApplication.frmUploadFiles" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Upload Files</title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:Label ID="ModuleLabel" runat="server" Text="Select Module:"></asp:Label>
            <asp:DropDownList ID="ModuleDropDown" runat="server" DataTextField="title" DataValueField="module_id" />
            <br /><br />
            <asp:FileUpload ID="FileUploadControl" runat="server" />
            <br /><br />
            <asp:Button ID="UploadButton" runat="server" OnClick="UploadButton_Click" Text="Upload" />
        </div>
    </form>
</body>
</html>

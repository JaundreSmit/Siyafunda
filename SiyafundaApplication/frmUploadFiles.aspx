<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmUploadFiles.aspx.cs" Inherits="SiyafundaApplication.frmUploadFiles" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
        </div>
        <asp:Button ID="UploadButton" runat="server" OnClick="UploadButton_Click" Text="Button" />
        <asp:FileUpload ID="FileUploadControl" runat="server" />
    </form>
</body>
</html>

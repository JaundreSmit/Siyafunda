<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmLongForm.aspx.cs" Inherits="SiyafundaApplication.frmLongForm" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Add Long Form Question</title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <h2>Add Long Form Question</h2>

            <label for="txtQuestion">Question Text:</label>
            <asp:TextBox ID="txtQuestion" runat="server"></asp:TextBox>
            <br />
            <br />

            <label for="txtPoints">Points (Max 6):</label>
            <asp:TextBox ID="txtPoints" runat="server"></asp:TextBox>
            <br />
            <br />

            <asp:Button ID="btnSubmitLongForm" runat="server" Text="Add Question" OnClick="btnSubmitLongForm_Click" />

            <br />

            <asp:Label ID="lblMessage" runat="server" ForeColor="Red" Visible="false"></asp:Label>
        </div>
    </form>
</body>
</html>

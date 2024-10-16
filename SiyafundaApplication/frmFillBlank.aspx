<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmFillBlank.aspx.cs" Inherits="SiyafundaApplication.frmFillBlank" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Add Fill in the Blank Question</title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <h2>Add Fill in the Blank Question</h2>

            <label for="txtQuestion">Question Text:</label>
            <asp:TextBox ID="txtQuestion" runat="server"></asp:TextBox>
            <br />
            <br />

            <label for="txtCorrectAnswer">Correct Answer:</label>
            <asp:TextBox ID="txtCorrectAnswer" runat="server"></asp:TextBox>
            <br />
            <br />

            <asp:Button ID="btnSubmitFillBlank" runat="server" Text="Add Question" OnClick="btnSubmitFillBlank_Click" />

            <br />

            <asp:Label ID="lblMessage" runat="server" ForeColor="Red" Visible="false"></asp:Label>
        </div>
    </form>
</body>
</html>

<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmMCQ.aspx.cs" Inherits="SiyafundaApplication.frmMCQ" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
        </div>
    </form>
</body>
</html>
<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmMCQ.aspx.cs" Inherits="SiyafundaApplication.frmMCQ" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Add Multiple Choice Question</title>
</head>
<body>
    <form id="form2" runat="server">
        <div>
            <h2>Add Multiple Choice Question</h2>

            <label for="txtQuestion">Question Text:</label>
            <asp:TextBox ID="txtQuestion" runat="server"></asp:TextBox>
            <br />

            <label for="txtOption1">Option 1:</label>
            <asp:TextBox ID="txtOption1" runat="server"></asp:TextBox>
            <br />

            <label for="txtOption2">Option 2:</label>
            <asp:TextBox ID="txtOption2" runat="server"></asp:TextBox>
            <br />

            <label for="txtOption3">Option 3:</label>
            <asp:TextBox ID="txtOption3" runat="server"></asp:TextBox>
            <br />

            <label for="txtOption4">Option 4:</label>
            <asp:TextBox ID="txtOption4" runat="server"></asp:TextBox>
            <br />

            <label for="ddlCorrectOption">Correct Option:</label>
            <asp:DropDownList ID="ddlCorrectOption" runat="server">
                <asp:ListItem Text="Option 1" Value="1"></asp:ListItem>
                <asp:ListItem Text="Option 2" Value="2"></asp:ListItem>
                <asp:ListItem Text="Option 3" Value="3"></asp:ListItem>
                <asp:ListItem Text="Option 4" Value="4"></asp:ListItem>
            </asp:DropDownList>
            <br />

            <asp:Button ID="btnSubmitMCQ" runat="server" Text="Add Question" OnClick="btnSubmitMCQ_Click" />

            <asp:Label ID="lblMessage" runat="server" ForeColor="Red" Visible="false"></asp:Label>
        </div>
    </form>
</body>
</html>

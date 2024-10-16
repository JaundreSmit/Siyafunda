<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmMCQ.aspx.cs" Inherits="SiyafundaApplication.frmMCQ" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Add Multiple Choice Question</title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <h2>Add Multiple Choice Question</h2>

            <label for="txtQuestion">Question Text:</label>
            <asp:TextBox ID="txtQuestion" runat="server"></asp:TextBox>
            <br />
            <br />

            <label for="txtOptionA">Option A:</label>
            <asp:TextBox ID="txtOptionA" runat="server"></asp:TextBox>
            <br />

            <label for="txtOptionB">Option B:</label>
            <asp:TextBox ID="txtOptionB" runat="server"></asp:TextBox>
            <br />

            <label for="txtOptionC">Option C:</label>
            <asp:TextBox ID="txtOptionC" runat="server"></asp:TextBox>
            <br />

            <label for="txtOptionD">Option D:</label>
            <asp:TextBox ID="txtOptionD" runat="server"></asp:TextBox>
            <br />
            <br />

            <label for="ddlCorrectAnswer">Correct Answer:</label>
            <asp:DropDownList ID="ddlCorrectAnswer" runat="server">
                <asp:ListItem Text="Option A" Value="1"></asp:ListItem>
                <asp:ListItem Text="Option B" Value="2"></asp:ListItem>
                <asp:ListItem Text="Option C" Value="3"></asp:ListItem>
                <asp:ListItem Text="Option D" Value="4"></asp:ListItem>
            </asp:DropDownList>
            <br />
            <br />

            <asp:Button ID="btnSubmitMCQ" runat="server" Text="Add Question" OnClick="btnSubmitMCQ_Click" />

            <br />

            <asp:Label ID="lblMessage" runat="server" ForeColor="Red" Visible="false"></asp:Label>
        </div>
    </form>
</body>
</html>

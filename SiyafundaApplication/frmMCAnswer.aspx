<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmMCAnswer.aspx.cs" Inherits="SiyafundaApplication.frmMCAnswer" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Multiple Choice Question</title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <!-- Display the question number -->
            <h2>Question <asp:Label ID="lblQuestionNumber" runat="server" Text=""></asp:Label>:</h2>

            <!-- Display the question text -->
            <h3>Question:</h3>
            <asp:Label ID="lblQuestionText" runat="server" Text=""></asp:Label>
            <br /><br />

            <!-- Display the 4 answer options -->
            <asp:RadioButtonList ID="rblOptions" runat="server">
                <asp:ListItem Value="A"></asp:ListItem>
                <asp:ListItem Value="B"></asp:ListItem>
                <asp:ListItem Value="C"></asp:ListItem>
                <asp:ListItem Value="D"></asp:ListItem>
            </asp:RadioButtonList>
            <br /><br />

            <!-- Timer for remaining time -->
            <asp:Label ID="lblTimer" runat="server" Text="Time Remaining:"></asp:Label>
            <br />

            <!-- Error message -->
            <asp:Label ID="lblMessage" runat="server" Text="" ForeColor="Red" Visible="false"></asp:Label>
            <br />

            <!-- Next Question button -->
            <asp:Button ID="btnNext" runat="server" Text="Next Question" OnClick="btnNext_Click" />
        </div>
    </form>
</body>
</html>

<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmFBAnswer.aspx.cs" Inherits="SiyafundaApplication.frmFBAnswer" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Fill in the Blank Question</title>
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

            <asp:TextBox ID="txtAnswer" runat="server" Width="150px"/>
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

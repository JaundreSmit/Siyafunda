<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmCreateQuiz.aspx.cs" Inherits="SiyafundaApplication.frmCreateQuiz" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Create a Quiz</title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <h2>Create New Quiz</h2>

            <!-- Quiz Title -->
            <label for="txtQuizTitle">Quiz Title:</label>
            <asp:TextBox ID="txtQuizTitle" runat="server" Placeholder="Enter Quiz Title"></asp:TextBox>
            <br />
            <br />

            <!-- Module Selection Dropdown -->
            <label for="ddlModules">Select Module:</label>
            <asp:DropDownList ID="ddlModules" runat="server">
                <asp:ListItem Value="0">Select Module</asp:ListItem>
            </asp:DropDownList>
            <br />
            <br />

            <!-- Time Limit (in Minutes) -->
            <label for="txtDuration">Quiz Time Limit (in minutes):</label>
            <asp:TextBox ID="txtDuration" runat="server" Placeholder="Enter time limit in minutes"></asp:TextBox>
            <br />
            <br />

            <!-- Due Date Input -->
            <label for="txtDueDate">Due Date:</label>
            <asp:TextBox ID="txtDueDate" runat="server" Placeholder="MM/dd/yyyy hh:mm tt"></asp:TextBox>
            <br />


            <!-- Add Question Types -->
            <h3>Add Questions:</h3>
            <asp:Button ID="btnAddMCQ" runat="server" Text="Add Multiple Choice Question" OnClick="btnAddMCQ_Click" />
            <asp:Button ID="btnAddFillBlank" runat="server" Text="Add Fill in the Blank Question" OnClick="btnAddFillBlank_Click" />
            <asp:Button ID="btnAddLongForm" runat="server" Text="Add Long Form Question" OnClick="btnAddLongForm_Click" />
            <br />
            <br />

            <!-- Label to Show Number of Submitted Questions -->
            <asp:Label ID="lblQuestionsSubmitted" runat="server" Text="0 questions submitted." Visible="true"></asp:Label>
            <br /><br />

            <!-- Submit Button moved to the bottom -->
            <asp:Button ID="btnSubmitQuiz" runat="server" Text="Create Quiz" OnClick="btnSubmitQuiz_Click" />
            <br />

            <!-- Error/Success Message -->
            <asp:Label ID="lblMessage" runat="server" Visible="false" ForeColor="Red"></asp:Label>
        </div>
    </form>
</body>
</html>

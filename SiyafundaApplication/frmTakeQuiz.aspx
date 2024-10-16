<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmTakeQuiz.aspx.cs" Inherits="SiyafundaApplication.frmTakeQuiz" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Take Quiz</title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <h2>Take a Quiz</h2>

            <!-- Module Selection -->
            <label for="ddlModules">Select Module:</label>
            <asp:DropDownList ID="ddlModules" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlModules_SelectedIndexChanged">
                <asp:ListItem Text="Select a Module" Value="" />
            </asp:DropDownList>
            <br />
            <asp:Label ID="lblModuleError" runat="server" Text="Error loading modules. Please try again later." ForeColor="Red" Visible="false" />
            <br />
            <br />

            <!-- Quiz Selection -->
            <label for="ddlQuizzes">Select Quiz:</label>
            <asp:DropDownList ID="ddlQuizzes" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlQuizzes_SelectedIndexChanged" Enabled="false">
                <asp:ListItem Text="Select a Quiz" Value="" />
            </asp:DropDownList>
            <br />
            <asp:Label ID="lblQuizError" runat="server" Text="Error loading quizzes. Please try again later." ForeColor="Red" Visible="false" />

            <!-- Quiz Details Display -->
            <h3>Quiz Information</h3>
            <label for="lblQuizTitle">Title:</label>
            <asp:Label ID="lblQuizTitle" runat="server" Text="N/A"></asp:Label>
            <br />

            <label for="lblDuration">Duration (minutes):</label>
            <asp:Label ID="lblDuration" runat="server" Text="N/A"></asp:Label>
            <br />

            <label for="lblCreateDate">Created on:</label>
            <asp:Label ID="lblCreateDate" runat="server" Text="N/A"></asp:Label>
            <br />

            <label for="lblDueDate">Due by:</label>
            <asp:Label ID="lblDueDate" runat="server" Text="N/A"></asp:Label>
            <br />
            <br />

            <!-- Start Quiz Button -->
            <asp:Button ID="btnStartQuiz" runat="server" Text="Start Quiz" Enabled="false" OnClick="btnStartQuiz_Click" />
            <br />

            <asp:Label ID="lblWarning" runat="server" Text="Note: The questions are linear and you can't go back to a previous question." ForeColor="Red" />

        </div>
    </form>
</body>
</html>

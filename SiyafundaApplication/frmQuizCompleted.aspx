<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmQuizCompleted.aspx.cs" Inherits="SiyafundaApplication.frmQuizCompleted" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Quiz Completed</title>
    <style>
        .completed-container {
            margin: 50px auto;
            padding: 20px;
            text-align: center;
            font-family: Arial, sans-serif;
        }

        .message-header {
            font-size: 24px;
            font-weight: bold;
            margin-bottom: 20px;
        }

        .score-summary {
            font-size: 20px;
            margin-bottom: 10px;
        }

        .preliminary-text {
            font-size: 16px;
            color: #777;
            margin-bottom: 20px;
        }

        .btn-return {
            padding: 10px 20px;
            background-color: #5cb85c;
            color: white;
            border: none;
            border-radius: 5px;
            cursor: pointer;
            font-size: 16px;
        }

        .btn-return:hover {
            background-color: #4cae4c;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="completed-container">
            <!-- Display the main message -->
            <div class="message-header">
                <asp:Label ID="lblCompletionMessage" runat="server" Text="Quiz Submitted"></asp:Label>
            </div>

            <!-- Display the user's score -->
            <div class="score-summary">
                <asp:Label ID="lblScore" runat="server" Text="Your score: "></asp:Label>
            </div>

            <!-- Preliminary score message (only show if long-form questions were present) -->
            <div class="preliminary-text">
                <asp:Label ID="lblPreliminaryMessage" runat="server" Text="Preliminary Score. Long Form Question(s) to be marked." Visible="false"></asp:Label>
            </div>

            <!-- Return button -->
            <asp:Button ID="btnReturn" runat="server" CssClass="btn-return" Text="Return" OnClick="btnReturn_Click" />
        </div>
    </form>
</body>
</html>

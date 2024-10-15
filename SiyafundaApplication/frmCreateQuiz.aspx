<%@ Page Language="C#" MasterPageFile="~/site.master" AutoEventWireup="true" CodeBehind="frmCreateQuiz.aspx.cs" Inherits="SiyafundaApplication.frmCreateQuiz" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <title>Create Quiz</title>
    
    <div>
        <h2>Create a New Quiz</h2>
        
        <label for="txtQuizTitle">Quiz Title:</label>
        <asp:TextBox ID="txtQuizTitle" runat="server"></asp:TextBox>
        <br />

        <label for="txtQuizDescription">Description:</label>
        <asp:TextBox ID="txtQuizDescription" runat="server" TextMode="MultiLine"></asp:TextBox>
        <br />

        <h3>Add Questions</h3>

        <!-- Buttons to navigate to specific question creation forms -->
        <asp:Button ID="btnAddMCQ" runat="server" Text="Add Multiple Choice Question" OnClick="btnAddMCQ_Click" />
        <asp:Button ID="btnAddFillBlank" runat="server" Text="Add Fill in the Blank Question" OnClick="btnAddFillBlank_Click" />
        <asp:Button ID="btnAddLongForm" runat="server" Text="Add Long Form Question" OnClick="btnAddLongForm_Click" />
        
        <br />

        <!-- Submit Quiz -->
        <asp:Button ID="btnSubmitQuiz" runat="server" Text="Create Quiz" OnClick="btnSubmitQuiz_Click" />

        <asp:Label ID="lblMessage" runat="server" ForeColor="Red" Visible="false"></asp:Label>
    </div>
</asp:Content>

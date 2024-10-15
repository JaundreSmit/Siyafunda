<%@ Page Language="C#" MasterPageFile="~/site.master" AutoEventWireup="true" CodeBehind="frmFillBlank.aspx.cs" Inherits="SiyafundaApplication.frmFillBlank" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <title>Add Fill in the Blank Question</title>

    <div>
        <h2>Add Fill in the Blank Question</h2>

        <label for="txtQuestion">Question Text:</label>
        <asp:TextBox ID="txtQuestion" runat="server"></asp:TextBox>
        <br />

        <label for="txtCorrectAnswer">Correct Answer:</label>
        <asp:TextBox ID="txtCorrectAnswer" runat="server"></asp:TextBox>
        <br />

        <asp:Button ID="btnSubmitFillBlank" runat="server" Text="Add Question" OnClick="btnSubmitFillBlank_Click" />

        <asp:Label ID="lblMessage" runat="server" ForeColor="Red" Visible="false"></asp:Label>
    </div>
</asp:Content>

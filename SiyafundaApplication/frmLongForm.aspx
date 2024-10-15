<%@ Page Language="C#" MasterPageFile="~/site.master" AutoEventWireup="true" CodeBehind="frmLongForm.aspx.cs" Inherits="SiyafundaApplication.frmLongForm" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <title>Add Long Form Question</title>

    <div>
        <h2>Add Long Form Question</h2>

        <label for="txtQuestion">Question Text:</label>
        <asp:TextBox ID="txtQuestion" runat="server" TextMode="MultiLine"></asp:TextBox>
        <br />

        <label for="txtPoints">Points (Max 6):</label>
        <asp:TextBox ID="txtPoints" runat="server"></asp:TextBox>
        <br />

        <asp:Button ID="btnSubmitLongForm" runat="server" Text="Add Question" OnClick="btnSubmitLongForm_Click" />

        <asp:Label ID="lblMessage" runat="server" ForeColor="Red" Visible="false"></asp:Label>
    </div>
</asp:Content>

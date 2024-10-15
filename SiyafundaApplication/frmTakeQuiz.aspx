<%@ Page Language="C#" MasterPageFile="~/site.master" AutoEventWireup="true" CodeBehind="frmTakeQuiz.aspx.cs" Inherits="SiyafundaApplication.frmTakeQuiz" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <title>Take Quiz</title>
    <div>
        <h2><asp:Label ID="lblQuizTitle" runat="server"></asp:Label></h2>
        <p><asp:Label ID="lblQuizDescription" runat="server"></asp:Label></p>

        <asp:Repeater ID="rptQuestions" runat="server">
            <ItemTemplate>
                <div class="question-container">
                    <p>Question <%# Container.ItemIndex + 1 %>: <%# Eval("question_text") %></p>

                    <%-- Multiple Choice Questions --%>
                    <asp:Panel ID="pnlMCQ" runat="server" Visible='<%# Eval("question_type").ToString() == "MCQ" %>'>
                        <asp:RadioButtonList ID="rblOptions" runat="server" DataSource='<%# Eval("options") %>' DataTextField="Text" DataValueField="Value"></asp:RadioButtonList>
                    </asp:Panel>

                    <%-- Fill in the Blank Question --%>
                    <asp:Panel ID="pnlFillBlank" runat="server" Visible='<%# Eval("question_type").ToString() == "FillBlank" %>'>
                        <asp:TextBox ID="txtFillBlank" runat="server"></asp:TextBox>
                    </asp:Panel>

                    <%-- Long Form Answer --%>
                    <asp:Panel ID="pnlLongForm" runat="server" Visible='<%# Eval("question_type").ToString() == "LongForm" %>'>
                        <asp:TextBox ID="txtLongFormAnswer" runat="server" TextMode="MultiLine" Rows="5" Columns="50"></asp:TextBox>
                    </asp:Panel>

                    <asp:HiddenField ID="hdnQuestionID" runat="server" Value='<%# Eval("question_id") %>' />
                </div>
            </ItemTemplate>
        </asp:Repeater>

        <asp:Button ID="btnSubmitQuiz" runat="server" Text="Submit Quiz" OnClick="btnSubmitQuiz_Click" />
        <asp:Label ID="lblMessage" runat="server" ForeColor="Red" Visible="false"></asp:Label>
    </div>
</asp:Content>

<%@ Page Language="C#" MasterPageFile="~/site.master" AutoEventWireup="true" CodeBehind="frmLandingPage.aspx.cs" Inherits="SiyafundaApplication.LandingPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div>
        <asp:Button ID="btnLogIn" runat="server" OnClick="btnLogIn_Click" Text="Log In" />
        <p>
            <asp:Button ID="btnSignUp" runat="server" OnClick="btnSignUp_Click" Text="Sign Up" />
        </p>
    </div>
</asp:Content>

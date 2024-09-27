<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmLandingPage.aspx.cs" Inherits="SiyafundaApplication.LandingPage" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
        </div>
        <asp:Button ID="btnLogIn" runat="server" OnClick="btnLogIn_Click" Text="Log In" />
        <p>
            <asp:Button ID="btnSignUp" runat="server" OnClick="btnSignUp_Click" Text="Sign Up" />
        </p>
    </form>
</body>
</html>

<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmSignUp.aspx.cs" Inherits="SiyafundaApplication.frmSignUp" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Sign Up</title>
</head>
<body>
    <form id="form1" runat="server">
        <div style="text-align:center; margin-top: 50px;">
            <h2>Sign Up</h2>

            <!-- Username Label and TextBox -->
            <asp:Label ID="UsernameLabel" runat="server" Text="Username"></asp:Label><br />
            <asp:TextBox ID="UsernameTextBox" runat="server"></asp:TextBox><br />
            <asp:RequiredFieldValidator ID="UsernameRequired" runat="server" ControlToValidate="UsernameTextBox" 
                ErrorMessage="Username is required" ForeColor="Red"></asp:RequiredFieldValidator><br />

            <!-- Email Label and TextBox -->
            <asp:Label ID="EmailLabel" runat="server" Text="Email"></asp:Label><br />
            <asp:TextBox ID="EmailTextBox" runat="server"></asp:TextBox><br />
            <asp:RequiredFieldValidator ID="EmailRequired" runat="server" ControlToValidate="EmailTextBox" 
                ErrorMessage="Email is required" ForeColor="Red"></asp:RequiredFieldValidator><br />
            <asp:RegularExpressionValidator ID="EmailValidator" runat="server" ControlToValidate="EmailTextBox" 
                ErrorMessage="Invalid email format" ValidationExpression="\w+@[a-zA-Z_]+?\.[a-zA-Z]{2,3}" 
                ForeColor="Red"></asp:RegularExpressionValidator><br />

            <!-- Password Label and TextBox -->
            <asp:Label ID="PasswordLabel" runat="server" Text="Password"></asp:Label><br />
            <asp:TextBox ID="PasswordTextBox" runat="server" TextMode="Password"></asp:TextBox><br />
            <asp:RequiredFieldValidator ID="PasswordRequired" runat="server" ControlToValidate="PasswordTextBox" 
                ErrorMessage="Password is required" ForeColor="Red"></asp:RequiredFieldValidator><br />

            <!-- Confirm Password Label and TextBox -->
            <asp:Label ID="ConfirmPasswordLabel" runat="server" Text="Confirm Password"></asp:Label><br />
            <asp:TextBox ID="ConfirmPasswordTextBox" runat="server" TextMode="Password"></asp:TextBox><br />
            <asp:RequiredFieldValidator ID="ConfirmPasswordRequired" runat="server" ControlToValidate="ConfirmPasswordTextBox" 
                ErrorMessage="Confirm Password is required" ForeColor="Red"></asp:RequiredFieldValidator><br />
            <asp:CompareValidator ID="PasswordCompareValidator" runat="server" ControlToValidate="ConfirmPasswordTextBox" 
                ControlToCompare="PasswordTextBox" ErrorMessage="Passwords do not match" ForeColor="Red"></asp:CompareValidator><br />

            <!-- Sign Up Button -->
            <asp:Button ID="SignUpButton" runat="server" Text="Sign Up" OnClick="SignUpButton_Click" /><br /><br />

            <!-- Error Label -->
            <asp:Label ID="lblError" runat="server" Text="" ForeColor="Red" Visible="false"></asp:Label>
        </div>
    </form>
</body>
</html>

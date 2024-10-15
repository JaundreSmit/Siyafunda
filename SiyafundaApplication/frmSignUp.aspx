<%@ Page Language="C#" MasterPageFile="~/site.master" AutoEventWireup="true" CodeBehind="frmSignUp.aspx.cs" Inherits="SiyafundaApplication.frmSignUp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <title>Sign Up</title>
    <style type="text/css">
        .auto-style1 {
            height: 30px;
        }
        .auto-style2 {
            width: 362px;
        }
        .auto-style3 {
            height: 30px;
            width: 362px;
        }
        .auto-style4 {
            width: 58%;
        }
    </style>

    <p>
        <table class="auto-style4">
            <tr>
                <td>Sign Up</td>
                <td class="auto-style2">&nbsp;</td>
                <td>&nbsp;</td>
            </tr>
            <tr>
                <td class="auto-style1">Username:</td>
                <td class="auto-style3">
                    <asp:TextBox ID="UsernameTextBox" runat="server"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="UsernameRequired" runat="server" ControlToValidate="UsernameTextBox" 
                        ErrorMessage="Username is required" ForeColor="Red"></asp:RequiredFieldValidator>
                </td>
                <td class="auto-style1"></td>
            </tr>
            <tr>
                <td>Name:</td>
                <td class="auto-style2">
                    <asp:TextBox ID="txtName" runat="server" TabIndex="1"></asp:TextBox>
                </td>
                <td>&nbsp;</td>
            </tr>
            <tr>
                <td>Surname:</td>
                <td class="auto-style2">
                    <asp:TextBox ID="txtSurname" runat="server" TabIndex="2"></asp:TextBox>
                </td>
                <td>&nbsp;</td>
            </tr>
            <tr>
                <td class="auto-style1">Email:</td>
                <td class="auto-style3">
                    <asp:TextBox ID="EmailTextBox" runat="server" TabIndex="3"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="EmailRequired" runat="server" ControlToValidate="EmailTextBox" 
                        ErrorMessage="Email is required" ForeColor="Red"></asp:RequiredFieldValidator>
                </td>
                <td class="auto-style1">
                    <asp:RegularExpressionValidator ID="EmailValidator" runat="server" ControlToValidate="EmailTextBox" 
                        ErrorMessage="Invalid email format" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" 
                        ForeColor="Red"></asp:RegularExpressionValidator>
                </td>
            </tr>
            <tr>
                <td>Password:</td>
                <td class="auto-style2">
                    <asp:TextBox ID="PasswordTextBox" runat="server" TextMode="Password" TabIndex="4"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="PasswordRequired" runat="server" ControlToValidate="PasswordTextBox" 
                        ErrorMessage="Password is required" ForeColor="Red"></asp:RequiredFieldValidator>
                </td>
                <td>&nbsp;</td>
            </tr>
            <tr>
                <td>Confirm Password:</td>
                <td class="auto-style2">
                    <asp:TextBox ID="ConfirmPasswordTextBox" runat="server" TextMode="Password" TabIndex="5"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="ConfirmPasswordRequired" runat="server" ControlToValidate="ConfirmPasswordTextBox" 
                        ErrorMessage="Confirm Password is required" ForeColor="Red"></asp:RequiredFieldValidator>
                </td>
                <td>
                    <asp:CompareValidator ID="PasswordCompareValidator" runat="server" ControlToValidate="ConfirmPasswordTextBox" 
                        ControlToCompare="PasswordTextBox" ErrorMessage="Passwords do not match" ForeColor="Red"></asp:CompareValidator>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblError" runat="server" Text="" ForeColor="Red" Visible="false"></asp:Label>
                </td>
                <td class="auto-style2">&nbsp;</td>
                <td>&nbsp;</td>
            </tr>
            <tr>
                <td>
                    <asp:Button ID="SignUpButton" runat="server" Text="Sign Up" OnClick="SignUpButton_Click" style="height: 26px" />
                </td>
                <td class="auto-style2">
                    <asp:Button ID="btnBack" runat="server" Text="Back" OnClick="btnBack_Click" style="height: 26px" />
                </td>
                <td>&nbsp;</td>
            </tr>
            <tr>
                <td>&nbsp;</td>
                <td class="auto-style2">&nbsp;</td>
                <td>&nbsp;</td>
            </tr>
        </table>
    </p>
    <p>&nbsp;</p>
</asp:Content>

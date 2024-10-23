<%@ Page Language="C#" MasterPageFile="~/site.master" AutoEventWireup="true" CodeBehind="frmSignUp.aspx.cs" Inherits="SiyafundaApplication.frmSignUp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <title>Sign Up</title>

    <div class="container d-flex align-items-center justify-content-center"> 
        <div class="row w-100"> 
            <div class="col-md-4 mx-auto"> 
                <!-- Bootstrap card for styling the sign-up section -->
                <div class="card" style="border: none; background-color: rgb(108, 61, 145); border-radius: 15px; margin-top: 20px;">
                    <div class="card-body text-white"> 
                        <h4 class="text-center">Sign Up</h4> <!-- Reduced font size -->

                        <!-- Uniform Input Group -->
                        <div class="mb-2 d-flex flex-column align-items-center"> <!-- Centering the text boxes -->
                            <asp:Label ID="UsernameLabel" runat="server" Text="Username:" CssClass="form-label"></asp:Label>
                            <asp:TextBox ID="UsernameTextBox" runat="server" CssClass="form-control" style="width: 80%;"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="UsernameRequired" runat="server" ControlToValidate="UsernameTextBox" 
                                ErrorMessage="Username is required" ForeColor="Red"></asp:RequiredFieldValidator>
                        </div>

                        <div class="mb-2 d-flex flex-column align-items-center"> <!-- Centering the text boxes -->
                            <asp:Label ID="NameLabel" runat="server" Text="Name:" CssClass="form-label"></asp:Label>
                            <asp:TextBox ID="txtName" runat="server" CssClass="form-control" style="width: 80%;"></asp:TextBox>
                        </div>

                        <div class="mb-2 d-flex flex-column align-items-center"> <!-- Centering the text boxes -->
                            <asp:Label ID="SurnameLabel" runat="server" Text="Surname:" CssClass="form-label"></asp:Label>
                            <asp:TextBox ID="txtSurname" runat="server" CssClass="form-control" style="width: 80%;"></asp:TextBox>
                        </div>

                        <div class="mb-2 d-flex flex-column align-items-center"> <!-- Centering the text boxes -->
                            <asp:Label ID="EmailLabel" runat="server" Text="Email:" CssClass="form-label"></asp:Label>
                            <asp:TextBox ID="EmailTextBox" runat="server" CssClass="form-control" style="width: 80%;"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="EmailRequired" runat="server" ControlToValidate="EmailTextBox" 
                                ErrorMessage="Email is required" ForeColor="Red"></asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="EmailValidator" runat="server" ControlToValidate="EmailTextBox" 
                                ErrorMessage="Invalid email format" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" 
                                ForeColor="Red"></asp:RegularExpressionValidator>
                        </div>

                        <div class="mb-2 d-flex flex-column align-items-center"> <!-- Centering the text boxes -->
                            <asp:Label ID="PasswordLabel" runat="server" Text="Password:" CssClass="form-label"></asp:Label>
                            <asp:TextBox ID="PasswordTextBox" runat="server" TextMode="Password" CssClass="form-control" style="width: 80%;"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="PasswordRequired" runat="server" ControlToValidate="PasswordTextBox" 
                                ErrorMessage="Password is required" ForeColor="Red"></asp:RequiredFieldValidator>
                        </div>

                        <div class="mb-2 d-flex flex-column align-items-center"> <!-- Centering the text boxes -->
                            <asp:Label ID="ConfirmPasswordLabel" runat="server" Text="Confirm Password:" CssClass="form-label"></asp:Label>
                            <asp:TextBox ID="ConfirmPasswordTextBox" runat="server" TextMode="Password" CssClass="form-control" style="width: 80%;"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="ConfirmPasswordRequired" runat="server" ControlToValidate="ConfirmPasswordTextBox" 
                                ErrorMessage="Confirm Password is required" ForeColor="Red"></asp:RequiredFieldValidator>
                            <asp:CompareValidator ID="PasswordCompareValidator" runat="server" ControlToValidate="ConfirmPasswordTextBox" 
                                ControlToCompare="PasswordTextBox" ErrorMessage="Passwords do not match" ForeColor="Red"></asp:CompareValidator>
                        </div>

                        <asp:Label ID="lblError" runat="server" Text="" ForeColor="Red" Visible="false" class="text-center mb-2"></asp:Label>

                        <div class="text-center mt-3"> <!-- Center the buttons -->
                            <asp:Button ID="SignUpButton" runat="server" Text="Sign Up" CssClass="btn btn-light mx-2" OnClick="SignUpButton_Click" />
                            <asp:Button ID="btnBack" runat="server" Text="Back" CssClass="btn btn-light mx-2" OnClick="btnBack_Click" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>

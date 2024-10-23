<%@ Page Language="C#" MasterPageFile="~/site.master" AutoEventWireup="true" CodeBehind="frmLogin.aspx.cs" Inherits="SiyafundaApplication.frmLogin" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <title>Login</title>

    <div class="container d-flex align-items-center justify-content-center vh-100">
        <div class="row w-100">
            <div class="col-md-4 mx-auto"> 
                <!-- Bootstrap card for styling the login section -->
                <div class="card" style="border: none; background-color: rgb(108, 61, 145); border-radius: 15px; margin-top: -50px;"> <!-- Adjust margin-top -->
                    <div class="card-body text-white"> <!-- Text color to match the sidebar and navbar -->
                        <h2 class="text-center">Login</h2>

                        <!-- Centered Username Label and TextBox -->
                        <div class="mb-3 text-center">
                            <asp:Label ID="UsernameLabel" runat="server" Text="Username/Email" CssClass="form-label"></asp:Label>
                            <asp:TextBox ID="UsernameTextBox" runat="server" CssClass="form-control mx-auto" style="width: 80%;"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="UsernameRequired" runat="server" ControlToValidate="UsernameTextBox" 
                                ErrorMessage="Username is required" ForeColor="Red"></asp:RequiredFieldValidator>
                        </div>

                        <!-- Centered Password Label and TextBox -->
                        <div class="mb-3 text-center">
                            <asp:Label ID="PasswordLabel" runat="server" Text="Password" CssClass="form-label"></asp:Label>
                            <asp:TextBox ID="PasswordTextBox" runat="server" TextMode="Password" CssClass="form-control mx-auto" style="width: 80%;"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="PasswordRequired" runat="server" ControlToValidate="PasswordTextBox" 
                                ErrorMessage="Password is required" ForeColor="Red"></asp:RequiredFieldValidator>
                        </div>

                        <asp:Label ID="lblError" runat="server" Text="" ForeColor="Red" Visible="false" class="text-center"></asp:Label>

                        <div class="text-center mt-4"> <!-- Center the buttons -->
                            <asp:Button ID="LoginButton" runat="server" Text="Login" CssClass="btn btn-light mx-2" OnClick="LoginButton_Click" />
                            <asp:Button ID="btnBack" runat="server" Text="Back" CssClass="btn btn-light mx-2" OnClick="btnBack_Click" />
                        </div>

                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>

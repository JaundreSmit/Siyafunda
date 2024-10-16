<%@ Page Language="C#" MasterPageFile="~/site.master" AutoEventWireup="true" CodeBehind="frmLandingPage.aspx.cs" Inherits="SiyafundaApplication.LandingPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <!-- Bootstrap container to align the content -->
    <div class="container d-flex align-items-center justify-content-center"> <!-- Added py-5 for padding -->
        <div class="row w-100"> <!-- Set the row to full width -->
            <div class="col-md-4 mx-auto"> <!-- Center the column -->
                <!-- Bootstrap card for styling the login/signup section -->
                <div class="card" style="border: none; background-color: rgb(108, 61, 145);"> <!-- Card background color -->
                    <div class="card-body text-white"> <!-- Text color to match the sidebar and navbar -->
                        <h3 class="card-title text-center custom-font">Welcome to Siyafunda</h3> <!-- Apply the chosen font -->
                        
                        <!-- Bootstrap button classes for 'Log In' and 'Sign Up' buttons -->
                        <div class="mb-3 text-center">
                            <asp:Button ID="btnLogIn" runat="server" OnClick="btnLogIn_Click" Text="Log In" CssClass="btn btn-light btn-block custom-font" />
                        </div>
                        <div class="text-center">
                            <asp:Button ID="btnSignUp" runat="server" OnClick="btnSignUp_Click" Text="Sign Up" CssClass="btn btn-light btn-block custom-font" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>

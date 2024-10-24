<%@ Page Language="C#" MasterPageFile="~/site.master" AutoEventWireup="true" CodeBehind="frmEducator.aspx.cs" Inherits="SiyafundaApplication.frmEducator" Async="true"%>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <title>Student Management</title>

    <div class="container mt-4">
        <div class="card bg-purple text-white mx-auto mb-3" style="max-width: 600px; margin-top: 10px;">
            <div class="card-body text-center">
                <h2 class="card-title"><strong>Student Management:</strong></h2>
            </div>
        </div>

        <div class="row">
            <div class="col-md-4">
                <!-- Improved Radio Button List -->
                <asp:RadioButtonList ID="rblStudentFilter" runat="server" AutoPostBack="True"
                                        OnSelectedIndexChanged="rblStudentFilter_SelectedIndexChanged" 
                                        CssClass="form-check form-check-inline" style="color: whitesmoke;">
                    <asp:ListItem Text="Show unassigned students" Value="Unassigned"></asp:ListItem>
                    <asp:ListItem Text="Show students for Module A" Value="Assigned"></asp:ListItem>
                </asp:RadioButtonList>


                <div class="input-group mb-3 mt-2">
                    <asp:TextBox ID="txtSearchStudents" runat="server" AutoPostBack="True" 
                                  OnTextChanged="txtSearchStudents_TextChanged" CssClass="form-control" placeholder="Search Students"></asp:TextBox>
                    <div class="input-group-append">
                        <asp:Label ID="lblStudentErrors" runat="server" Text="[Student Errors]" CssClass="alert alert-danger d-none"></asp:Label>
                    </div>
                </div>

                <div>
                    <asp:Button ID="btnRemoveStudent" runat="server" OnClick="btnRemoveStudent_Click" Text="Remove Student" CssClass="btn btn-primary w-50 btn-purple mx-2" />
                    <asp:Button ID="btnAssignStudent" runat="server" OnClick="btnAddStudent_Click" Text="Add Student" CssClass="btn btn-primary w-50 btn-purple mx-2" />
                </div>
            </div>

            <div class="col-md-8">
                <div class="d-flex align-items-center mb-3">
                    <h3 class="me-2" style="color: whitesmoke;">Other Features:</h3>
                    <div class="d-flex">
                        <asp:Button ID="btnBack" runat="server" OnClick="btnBack_Click" Text="Back" CssClass="btn btn-primary btn-purple mx-2" />
                        <asp:Button ID="btnUploadResources" runat="server" OnClick="btnUploadResources_Click" Text="Upload Resources" CssClass="btn btn-primary w-50 btn-purple mx-2" />
                        <asp:Button ID="btnAddAnnouncement" runat="server" OnClick="btnAddAnnouncement_Click" Text="Add Announcement" CssClass="btn btn-primary w-50 btn-purple mx-2" />
                        <asp:Button ID="btnAddFaq" runat="server" OnClick="btnAddFaq_Click" Text="Add FAQ" CssClass="btn btn-primary w-50 btn-purple mx-2" />
                    </div>
                </div>
                <asp:GridView ID="dgvStudents" runat="server" OnSelectedIndexChanged="dgvStudents_SelectedIndexChanged" CssClass="table table-striped mt-3" Width="100%"></asp:GridView>
            </div>
        </div>
    </div>

    <style type="text/css">

        .alert {
            font-size: 14px; 
            padding: 10px;   
            margin-top: 10px; 
            border-radius: 5px; 
        }

        .btn-purple {
            background-color: whitesmoke; 
            color: black; 
            border: none; 
            font-weight: bold; 
            font-size: 12px;
            padding: 12px 24px; 
            text-transform: uppercase;
            transition: background-color 0.3s ease, transform 0.2s ease; 
            box-shadow: 0 4px 8px rgba(0, 0, 0, 0.2); 
            border-radius: 0;
        }

        .btn-purple:hover {
            background-color: rgb(108, 61, 145); 
            color: black; 
            transform: translateY(-2px); 
            box-shadow: 0 6px 12px rgba(0, 0, 0, 0.3);
            border-radius: 0;
        }

        .bg-purple {
            background-color: rgb(108, 61, 145);
        }
       
        .form-control {
            width: 100%;
        }
       
        .mx-2 {
            margin-left: 0.5rem;
            margin-right: 0.5rem;
        }

        .custom-container {
            margin-top: -140px; 
            padding-top: 0; 
        }

        .btn:disabled {
            background-color: darkgrey; 
            color: white; 
            border-color: darkgrey;
            opacity: 1;
            cursor: not-allowed; 
        }

    </style>
</asp:Content>

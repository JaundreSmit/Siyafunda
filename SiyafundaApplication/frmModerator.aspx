<%@ Page Language="C#" MasterPageFile="~/site.master" AutoEventWireup="true" CodeBehind="frmModerator.aspx.cs" Inherits="SiyafundaApplication.frmModerator" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <title>File Moderation</title>
    <style type="text/css">
        .auto-style2 {
            width: 218px;
            height: 23px;
        }
        .auto-style8 {
            height: 23px;
            width: 707px;
        }
        .auto-style10 {
            width: 81px;
        }
        .auto-style11 {
            width: 215px;
        }
        .auto-style12 {
            width: 304px;
        }
        .auto-style13 {
            width: 218px;
            height: 31px;
        }
        .auto-style14 {
            width: 707px;
            height: 31px;
        }
    </style>
    <asp:Panel ID="pnlInProgress" runat="server" Width="744px">

         <table style="width:100%;">
     <tr>
         <td class="auto-style13">In Progress files:</td>
         <td class="auto-style14">
             Search title:
             <asp:TextBox ID="txtSearchProgress" runat="server" AutoPostBack="True" OnTextChanged="txtSearchProgress_TextChanged"></asp:TextBox>
             &nbsp;
             <asp:Label ID="lblProgressErrors" runat="server" Text="[Progress Errors]"></asp:Label>
         </td>
        
     </tr>
     <tr>
         <td class="auto-style2">
             
         </td>
         <td class="auto-style8">
             <asp:GridView ID="dgvInProgress" runat="server" Width="607px" AllowSorting="True" AutoGenerateColumns="False" OnSelectedIndexChanged="dgvInProgress_SelectedIndexChanged" DataKeyNames="resource_id">
                 <Columns>
                     <asp:BoundField DataField="ModuleTitle" HeaderText="Module Title" />
                     <asp:BoundField DataField="ResourceTitle" HeaderText="Resource Title" />
                     <asp:BoundField DataField="upload_date" HeaderText="Upload Date" />
                     <asp:BoundField DataField="file_type" HeaderText="File Type" />
                     <asp:BoundField DataField="file_size" HeaderText="File Size" />
                     <asp:TemplateField HeaderText="Select">
                         <ItemTemplate>
                             <asp:Button ID="btnSelect" runat="server" Text="Select" CommandName="Select" />
                         </ItemTemplate>
                     </asp:TemplateField>
                 </Columns>
             </asp:GridView>
         </td>
         
     </tr>
 </table>
</asp:Panel>

   
   <p></p>
    <asp:Panel ID="pnlRejectApprove" runat="server" Height="215px" Width="942px">
        <table>
    <tr>
        <td class="auto-style11">Select decision:</td>
        <td colspan="3">
            <asp:RadioButtonList ID="rbDecision" runat="server" Width="86px" OnSelectedIndexChanged="rbDecision_SelectedIndexChanged">
                <asp:ListItem Value="1">Reject</asp:ListItem>
                <asp:ListItem Value="2">Approve</asp:ListItem>
            </asp:RadioButtonList>
        </td>
    </tr>
    <tr>
        <td class="auto-style11">Feedback:</td>
        <td colspan="3">
            <asp:TextBox ID="txtFeedback" runat="server" Height="68px" MaxLength="200" TextMode="MultiLine" Width="824px"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td class="auto-style11">
            <asp:Button ID="btnInProgressSubmit" runat="server" OnClick="btnInProgressSubmit_Click" Text="Submit" />
        </td>
        <td class="auto-style10">
            <asp:Button ID="btnCancel" runat="server" Text="Cancel" OnClick="btnCancel_Click" />
        </td>
        <td class="auto-style12">&nbsp;</td>
    </tr>
</table>

    </asp:Panel>
    
</asp:Content>

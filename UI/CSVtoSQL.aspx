<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="CSVtoSQL.aspx.cs" Inherits="UI.CSVtoSQL" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">


    <form id="CSVtoSQL" runat="server">
        
            <asp:fileupload id="FileUpload1" runat="server" />
       
        <asp:button text="Upload" onclick="Upload" runat="server" />
    </form>

</asp:Content>

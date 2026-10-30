<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AdminMarcas.aspx.cs" Inherits="TPIWebForm_equipo_K.AdminMarcas" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container mt-4">
        <h1>Marcas</h1>

        <asp:GridView
            ID="dgvMarcas"
            runat="server"
            AutoGenerateColumns="true" CssClass="table table-striped">
        </asp:GridView>
    </div>
</asp:Content>

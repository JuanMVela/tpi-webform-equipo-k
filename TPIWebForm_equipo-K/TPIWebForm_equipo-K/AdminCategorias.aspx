<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AdminCategorias.aspx.cs" Inherits="TPIWebForm_equipo_K.AdminCategorias" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container mt-4">
        <h1>Categorias</h1>
                <div class="mb-3 d-flex align-items-center">
                    <label class="me-2 fw-bold">Buscar Por Nombre:</label>
                    <asp:TextBox ID="txtFiltro" runat="server" CssClass="form-control w-25"
                        AutoPostBack="true" OnTextChanged="txtFiltro_TextChanged"></asp:TextBox>
                </div>
                <asp:GridView ID="dgvCategorias" runat="server"
                    AutoGenerateColumns="false"
                    CssClass="table table-striped"
                    DataKeyNames="Id"
                    OnSelectedIndexChanged="dgvCategorias_SelectedIndexChanged"
                    OnRowDeleting="dgvCategorias_RowDeleting">
                    <Columns>
                        <asp:BoundField DataField="Id" HeaderText="ID" />
                        <asp:BoundField DataField="Nombre" HeaderText="Nombre" />
                        <asp:CheckBoxField DataField="Activo" HeaderText="Activo" ReadOnly="true" />
                        <asp:CommandField ShowSelectButton="true" SelectText="Modificar" ControlStyle-CssClass="btn btn-warning btn-sm" />
                        <asp:CommandField ShowDeleteButton="true" DeleteText="Desactivar" ControlStyle-CssClass="btn btn-danger btn-sm" />
                    </Columns>
                </asp:GridView>
    </div>

    <div class="mt-4">
        <asp:Button ID="BtnAgregar" runat="server" Text="Agregar" CssClass="btn btn-primary" OnClick="BtnAgregar_Click" />
    </div>
</asp:Content>

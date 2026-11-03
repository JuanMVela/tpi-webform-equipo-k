<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AdminProductos.aspx.cs" Inherits="TPIWebForm_equipo_K.AdminProductos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container mt-4">
        <h2 class="mb-4">Administración de Productos</h2>

        <div class="d-flex justify-content-between align-items-center mb-3">
            <div class="input-group w-50">
                <span class="input-group-text">🔍 Buscar</span>
                <asp:TextBox ID="txtFiltro" runat="server" CssClass="form-control"
                    placeholder="Nombre del producto..." AutoPostBack="true"
                    OnTextChanged="txtFiltro_TextChanged"></asp:TextBox>
            </div>
            <asp:Button ID="btnAgregar" runat="server" Text="+ Nuevo Producto"
                CssClass="btn btn-success" OnClick="btnAgregar_Click" />
        </div>

        <asp:GridView ID="dgvProductos" runat="server"
            AutoGenerateColumns="false"
            CssClass="table table-striped"
            DataKeyNames="Id"
            OnRowCommand="dgvProductos_RowCommand">
            <Columns>
                <asp:BoundField DataField="Codigo" HeaderText="Código" />
                <asp:BoundField DataField="Nombre" HeaderText="Nombre" />
                <asp:BoundField DataField="Categoria.Nombre" HeaderText="Categoría" />
                <asp:BoundField DataField="Marca.Nombre" HeaderText="Marca" />
                <asp:BoundField DataField="Precio" HeaderText="Precio" DataFormatString="{0:C}" />
                <asp:BoundField DataField="Stock" HeaderText="Stock" />
                <asp:CheckBoxField DataField="Activo" HeaderText="Activo" ReadOnly="true" />

                <asp:TemplateField HeaderText="Acciones">
                    <ItemTemplate>

                        <asp:Button ID="btnModificar" runat="server" Text="Modificar"
                            CommandName="Modificar" CommandArgument='<%# Eval("Id") %>'
                            CssClass="btn btn-warning btn-sm" />

                        <asp:Button ID="btnEstado" runat="server"
                            CommandName="CambiarEstado" CommandArgument='<%# Eval("Id") %>'
                            Text='<%# Convert.ToBoolean(Eval("Activo")) ? "Desactivar" : "Reactivar" %>'
                            CssClass='<%# Convert.ToBoolean(Eval("Activo")) ? "btn btn-danger btn-sm" : "btn btn-success btn-sm" %>' />

                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>
</asp:Content>

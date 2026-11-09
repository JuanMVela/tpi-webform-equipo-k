<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AdminFormasEntrega.aspx.cs" Inherits="TPIWebForm_equipo_K.AdminFormasEntrega" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container mt-4">
        <h2>Administración de Formas de Entrega</h2>
        <div class="d-flex justify-content-between align-items-center mb-3">
            <div class="input-group w-50">
                <span class="input-group-text">🔍 Buscar</span>
                <asp:TextBox ID="txtFiltro" runat="server" CssClass="form-control" placeholder="Nombre de la forma de entrega..." AutoPostBack="true" OnTextChanged="txtFiltro_TextChanged"></asp:TextBox>
            </div>
            <a href="FormularioFormaEntrega.aspx" class="btn btn-success">+ Nueva Forma de Entrega</a>
        </div>
        <asp:GridView ID="dgvFormasEntrega" runat="server" AutoGenerateColumns="false" CssClass="table table-bordered table-striped" EmptyDataText="No hay formas de entrega cargadas." OnRowCommand="dgvFormasEntrega_RowCommand">
            <Columns>
                <asp:BoundField DataField="Id" HeaderText="ID" />
                <asp:BoundField DataField="Nombre" HeaderText="Nombre" />
                <asp:CheckBoxField DataField="RequiereDireccion" HeaderText="Requiere dirección" ReadOnly="true" />
                <asp:CheckBoxField DataField="Activo" HeaderText="Activo" ReadOnly="true" />
                <asp:TemplateField HeaderText="Acciones">
                    <ItemTemplate>
                        <asp:Button ID="btnModificar" runat="server" Text="Modificar" CommandName="Modificar" CommandArgument='<%# Eval("Id") %>' CssClass="btn btn-warning btn-sm" />
                        <asp:Button ID="btnEstado" runat="server" CommandName="CambiarEstado" CommandArgument='<%# Eval("Id") %>' Text='<%# Convert.ToBoolean(Eval("Activo")) ? "Desactivar" : "Reactivar" %>' CssClass='<%# Convert.ToBoolean(Eval("Activo")) ? "btn btn-danger btn-sm" : "btn btn-success btn-sm" %>' />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>
</asp:Content>

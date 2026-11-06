<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AdminFormasPago.aspx.cs" Inherits="TPIWebForm_equipo_K.AdminFormasPago" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container mt-4">
        <h2>Administración de Formas de Pago</h2>
        <div class="d-flex justify-content-end mb-3">
            <a href="FormularioFormaPago.aspx" class="btn btn-success">+ Nueva Forma de Pago
            </a>
        </div>
        <asp:GridView ID="dgvFormasPago" runat="server" AutoGenerateColumns="false" CssClass="table table-bordered table-striped" EmptyDataText="No hay formas de pago cargadas." OnRowCommand="dgvFormasPago_RowCommand">

            <Columns>
                <asp:BoundField DataField="Id" HeaderText="ID" />
                <asp:BoundField DataField="Nombre" HeaderText="Nombre" />
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

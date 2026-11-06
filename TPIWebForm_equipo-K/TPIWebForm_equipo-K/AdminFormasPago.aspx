<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AdminFormasPago.aspx.cs" Inherits="TPIWebForm_equipo_K.AdminFormasPago" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container mt-4">
        <h2>Administración de Formas de Pago</h2>
        <div class="mb-3">
            <a href="FormularioFormaPago.aspx" class="btn btn-success">+ Nueva Forma de Pago
            </a>
        </div>
        <asp:GridView ID="dgvFormasPago" runat="server"
            AutoGenerateColumns="false"
            CssClass="table table-bordered table-striped"
            EmptyDataText="No hay formas de pago cargadas.">

            <Columns>
                <asp:BoundField DataField="Id" HeaderText="ID" />
                <asp:BoundField DataField="Nombre" HeaderText="Nombre" />
                <asp:CheckBoxField DataField="Activo" HeaderText="Activo"
                    ReadOnly="true" />
                <asp:HyperLinkField
                    HeaderText="Acciones"
                    Text="Modificar"
                    DataNavigateUrlFields="Id"
                    DataNavigateUrlFormatString="FormularioFormaPago.aspx?id={0}"
                    ControlStyle-CssClass="btn btn-warning" />
            </Columns>
        </asp:GridView>
    </div>
</asp:Content>

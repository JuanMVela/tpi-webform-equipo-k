<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AdminMarcas.aspx.cs" Inherits="TPIWebForm_equipo_K.AdminMarcas" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        /* Estilo para el paginador del GridView */
        .pagination a, .pagination span {
            display: inline-block;
            padding: 6px 12px;
            margin: 0 2px;
            border: 1px solid #dee2e6;
            border-radius: 4px;
            color: #0d6efd;
            text-decoration: none;
            cursor: pointer;
        }

            .pagination a:hover {
                background-color: #e9ecef;
            }

        .pagination span {
            background-color: #0d6efd;
            color: white;
            border-color: #0d6efd;
            font-weight: bold;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container mt-4">

        <h1>Marcas</h1>

        <div class="d-flex justify-content-between align-items-center mb-3">
            <div class="input-group w-50">
                <span class="input-group-text">🔍 Buscar</span>
                <asp:TextBox ID="txtFiltro" runat="server" CssClass="form-control"
                    placeholder="Nombre de la marca..." AutoPostBack="true"
                    OnTextChanged="txtFiltro_TextChanged"></asp:TextBox>
            </div>
            <asp:Button ID="Button1" runat="server" Text="+ Nueva Marca"
                CssClass="btn btn-success" OnClick="BtnAgregar_Click" />
        </div>

        <asp:GridView
            ID="dgvMarcas"
            runat="server"
            AutoGenerateColumns="false"
            CssClass="table table-striped"
            DataKeyNames="Id"
            OnRowCommand="dgvMarcas_RowCommand"
            PageSize="6"
            OnPageIndexChanging="dgvCategorias_PageIndexChanging"
            PagerStyle-CssClass="pagination">

            <Columns>

                <asp:BoundField
                    DataField="Id"
                    HeaderText="ID" />

                <asp:BoundField
                    DataField="Nombre"
                    HeaderText="Nombre" />

                <asp:CheckBoxField
                    DataField="Activo"
                    HeaderText="Activo"
                    ReadOnly="true" />

                <asp:TemplateField HeaderText="Acciones">

                    <ItemTemplate>

                        <asp:Button
                            ID="btnModificar"
                            runat="server"
                            Text="Modificar"
                            CommandName="Modificar"
                            CommandArgument='<%# Eval("Id") %>'
                            CssClass="btn btn-warning btn-sm" />

                        <asp:Button
                            ID="btnEstado"
                            runat="server"
                            CommandName="CambiarEstado"
                            CommandArgument='<%# Eval("Id") %>'
                            Text='<%# Convert.ToBoolean(Eval("Activo")) ? "Desactivar" : "Reactivar" %>'
                            CssClass='<%# Convert.ToBoolean(Eval("Activo")) ? "btn btn-danger btn-sm" : "btn btn-success btn-sm" %>' />

                    </ItemTemplate>

                </asp:TemplateField>

            </Columns>

        </asp:GridView>

    </div>
</asp:Content>

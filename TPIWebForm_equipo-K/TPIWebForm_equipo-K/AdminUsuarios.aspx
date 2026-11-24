<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AdminUsuarios.aspx.cs" Inherits="TPIWebForm_equipo_K.AdminUsuarios" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
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
        <h2 class="mb-4">Administración de Usuarios</h2>
        <asp:Label ID="lblAviso" runat="server"
            CssClass="d-block mb-3" Visible="false" />

        <div class="d-flex justify-content-between align-items-center mb-3">
            <div class="input-group w-50">
                <span class="input-group-text">🔍 Buscar</span>
                <asp:TextBox ID="txtFiltro" runat="server" CssClass="form-control"
                    placeholder="Nombre o email..." AutoPostBack="true"
                    OnTextChanged="txtFiltro_TextChanged"></asp:TextBox>
            </div>

        </div>

        <asp:GridView ID="dgvUsuarios" runat="server" AutoGenerateColumns="false" OnRowDataBound="dgvUsuarios_RowDataBound"
            CssClass="table table-striped" DataKeyNames="Id"
            OnRowCommand="dgvUsuarios_RowCommand"
            AllowPaging="true" PageSize="10"
            OnPageIndexChanging="dgvUsuarios_PageIndexChanging"
            PagerStyle-CssClass="pagination"
            EmptyDataText="No se encontraron usuarios.">
            <Columns>
                <asp:BoundField DataField="Id" HeaderText="ID" />
                <asp:BoundField DataField="Nombre" HeaderText="Nombre" />
                <asp:BoundField DataField="Apellido" HeaderText="Apellido" />
                <asp:TemplateField HeaderText="Perfil">
                    <ItemTemplate>
                        <asp:DropDownList ID="ddlPerfil" runat="server"
                            CssClass="form-select form-select-sm"
                            SelectedValue='<%# Eval("Perfil.Id") %>'>
                            <asp:ListItem Text="Cliente" Value="1" />
                            <asp:ListItem Text="Administrador" Value="2" />
                        </asp:DropDownList>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="Perfil.Nombre" HeaderText="Perfil" />
                <asp:CheckBoxField DataField="Activo" HeaderText="Activo" ReadOnly="true" />
                <asp:TemplateField HeaderText="Acciones">
                    <ItemTemplate>
                        <asp:Button ID="btnGuardarRol" runat="server"
                            Text="Guardar rol"
                            CommandName="GuardarRol"
                            CssClass="btn btn-primary btn-sm me-2"
                            CausesValidation="false" />
                        <asp:Button ID="btnEstado" runat="server"
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

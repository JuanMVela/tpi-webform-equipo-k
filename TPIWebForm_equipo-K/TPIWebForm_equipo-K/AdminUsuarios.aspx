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

        <div class="d-flex justify-content-between align-items-center mb-3">
            <div class="input-group w-50">
                <span class="input-group-text">🔍 Buscar</span>
                <asp:TextBox ID="txtFiltro" runat="server" CssClass="form-control"
                    placeholder="Nombre o email..." AutoPostBack="true"
                    OnTextChanged="txtFiltro_TextChanged"></asp:TextBox>
            </div>
            
        </div>

        <asp:GridView ID="dgvUsuarios" runat="server" AutoGenerateColumns="false" 
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
                <asp:BoundField DataField="Email" HeaderText="Email" />
                <asp:BoundField DataField="Perfil.Nombre" HeaderText="Perfil" />
                <asp:CheckBoxField DataField="Activo" HeaderText="Activo" ReadOnly="true" />
                <asp:TemplateField HeaderText="Acciones">
                    <ItemTemplate>
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

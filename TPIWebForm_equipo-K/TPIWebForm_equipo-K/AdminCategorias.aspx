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
        <asp:GridView ID="dgvCategorias" runat="server" AutoGenerateColumns="false" 
              CssClass="table table-striped" DataKeyNames="Id" 
              OnRowCommand="dgvCategorias_RowCommand">
            <Columns>
        <asp:BoundField DataField="Id" HeaderText="ID" />
        <asp:BoundField DataField="Nombre" HeaderText="Nombre" />
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
        <div class="mt-4">
            <asp:Button ID="BtnAgregar" runat="server" Text="Agregar" CssClass="btn btn-primary" OnClick="BtnAgregar_Click" />
        </div>

    </div>

</asp:Content>

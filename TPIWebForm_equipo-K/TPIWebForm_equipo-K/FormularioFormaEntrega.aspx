<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="FormularioFormaEntrega.aspx.cs" Inherits="TPIWebForm_equipo_K.FormularioFormaEntrega" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container mt-4">
    <div class="row justify-content-center">
        <div class="col-md-6">
            <h2 id="tituloFormulario" runat="server" class="mb-4">Agregar Forma de Entrega</h2>

            <div class="card shadow-sm">
                <div class="card-body">
                    <div class="mb-3">
                        <label class="form-label">Nombre de la forma de entrega</label>
                        <asp:TextBox ID="txtNombre" runat="server" CssClass="form-control" MaxLength="100"></asp:TextBox>
                        <asp:Label ID="lblErrorNombre" runat="server" CssClass="text-danger" Visible="false"></asp:Label>
                    </div>

                    <div class="mb-3">
                        <asp:CheckBox ID="chkRequiereDireccion" runat="server" Text=" Requiere dirección" />
                    </div>

                    <div class="mt-4">
                        <asp:Button ID="btnAceptar" runat="server" Text="Aceptar" CssClass="btn btn-primary" OnClick="btnAceptar_Click" />
                        <a href="AdminFormasEntrega.aspx" class="btn btn-secondary">Cancelar</a>
                    </div>
                </div>
            </div>
        </div>
    </div>
</div>
</asp:Content>

<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="FormularioFormaPago.aspx.cs" Inherits="TPIWebForm_equipo_K.FormularioFormaPago" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container mt-4">
        <div class="row justify-content-center">
            <div class="col-md-6">

                <h2 id="tituloFormulario" runat="server" class="mb-4">Agregar Forma de Pago
                </h2>

                <div class="card shadow-sm">
                    <div class="card-body">

                        <div class="mb-3">
                            <label for="<%= txtNombre.ClientID %>" class="form-label">
                                Nombre de la forma de pago
                            </label>

                           <asp:TextBox ID="txtNombre" runat="server" CssClass="form-control"></asp:TextBox>

                            <asp:Label ID="lblErrorNombre" runat="server" CssClass="text-danger" Visible="false"></asp:Label>
                        </div>

                        <div class="mt-4">
                            <asp:Button ID="Button1" runat="server" Text="Aceptar" CssClass="btn btn-primary" OnClick="btnAceptar_Click" />
                            <a href="AdminFormasPago.aspx" class="btn btn-secondary">Cancelar
                            </a>
                        </div>

                    </div>
                </div>

            </div>
        </div>
    </div>
</asp:Content>

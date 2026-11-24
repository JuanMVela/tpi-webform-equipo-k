<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ConfiguracionInicial.aspx.cs" Inherits="TPIWebForm_equipo_K.ConfiguracionInicial" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container py-4">
        <h1 class="mb-4">Configuración inicial</h1>

        <div class="card shadow-sm" style="max-width: 550px;">
            <div class="card-body">
                <h2 class="h5 mb-3">Configurar primer administrador</h2>

                <p class="text-muted">
                    Tu cuenta recibirá permisos para administrar la tienda.
           
                </p>

                <asp:Label runat="server"
                    AssociatedControlID="txtClaveInstalacion"
                    Text="Clave de instalación"
                    CssClass="form-label" />

                <asp:TextBox ID="txtClaveInstalacion" runat="server"
                    TextMode="Password"
                    CssClass="form-control" />

                <asp:RequiredFieldValidator ID="rfvClave" runat="server"
                    ControlToValidate="txtClaveInstalacion"
                    ErrorMessage="Ingresá la clave de instalación."
                    CssClass="text-danger"
                    Display="Dynamic"
                    ValidationGroup="Configuracion"
                    EnableClientScript="false" />

                <asp:Label ID="lblAviso" runat="server"
                    CssClass="d-block text-danger mt-3"
                    Visible="false" />

                <div class="d-flex gap-2 mt-3">
                    <asp:Button ID="btnConfigurar" runat="server"
                        Text="Configurar administrador"
                        CssClass="btn btn-primary"
                        ValidationGroup="Configuracion"
                        Enabled="false"
                        OnClick="btnConfigurar_Click" />

                    <a href="Inicio.aspx" class="btn btn-outline-secondary">Volver al inicio
                </a>
                </div>
            </div>
        </div>
    </div>
</asp:Content>

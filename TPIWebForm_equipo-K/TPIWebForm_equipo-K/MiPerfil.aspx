<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MiPerfil.aspx.cs" Inherits="TPIWebForm_equipo_K.MiPerfil" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container py-4">
        <h1 class="mb-4">Mi perfil</h1>

        <div class="row">
            <div class="col-12 col-lg-8">
                <div class="card shadow-sm">
                    <div class="card-body">
                        <h2 class="h5 mb-3">Datos personales</h2>

                        <asp:Label ID="lblAviso" runat="server"
                            CssClass="d-block text-danger mb-3"
                            Visible="false" />

                        <div class="row g-3">
                            <div class="col-md-6">
                                <asp:Label runat="server"
                                    AssociatedControlID="txtNombre"
                                    Text="Nombre" CssClass="form-label" />
                                <asp:TextBox ID="txtNombre" runat="server"
                                    CssClass="form-control" ReadOnly="true" />
                                <asp:RequiredFieldValidator ID="rfvNombre" runat="server"
                                    ControlToValidate="txtNombre"
                                    ErrorMessage="Ingresá tu nombre."
                                    CssClass="text-danger"
                                    Display="Dynamic"
                                    ValidationGroup="MiPerfil"
                                    EnableClientScript="false" />
                            </div>

                            <div class="col-md-6">
                                <asp:Label runat="server"
                                    AssociatedControlID="txtApellido"
                                    Text="Apellido" CssClass="form-label" />
                                <asp:TextBox ID="txtApellido" runat="server"
                                    CssClass="form-control" ReadOnly="true" />
                                <asp:RequiredFieldValidator ID="rfvApellido" runat="server"
                                    ControlToValidate="txtApellido"
                                    ErrorMessage="Ingresá tu apellido."
                                    CssClass="text-danger"
                                    Display="Dynamic"
                                    ValidationGroup="MiPerfil"
                                    EnableClientScript="false" />
                            </div>

                            <div class="col-md-6">
                                <asp:Label runat="server"
                                    AssociatedControlID="txtEmail"
                                    Text="Email" CssClass="form-label" />
                                <asp:TextBox ID="txtEmail" runat="server"
                                    CssClass="form-control" ReadOnly="true" />
                                <asp:RequiredFieldValidator ID="rfvEmail" runat="server"
                                    ControlToValidate="txtEmail"
                                    ErrorMessage="Ingresá tu email."
                                    CssClass="text-danger"
                                    Display="Dynamic"
                                    ValidationGroup="MiPerfil"
                                    EnableClientScript="false" />
                            </div>

                            <div class="col-md-6">
                                <asp:Label runat="server"
                                    AssociatedControlID="txtTelefono"
                                    Text="Teléfono" CssClass="form-label" />
                                <asp:TextBox ID="txtTelefono" runat="server"
                                    CssClass="form-control" ReadOnly="true" />
                                <asp:RequiredFieldValidator ID="rfvTelefono" runat="server"
                                    ControlToValidate="txtTelefono"
                                    ErrorMessage="Ingresá tu teléfono."
                                    CssClass="text-danger"
                                    Display="Dynamic"
                                    ValidationGroup="MiPerfil"
                                    EnableClientScript="false" />
                            </div>
                        </div>

                        <div class="d-flex flex-wrap gap-2 mt-4">
                            <asp:Button ID="btnModificar" runat="server"
                                Text="Modificar datos"
                                CssClass="btn btn-primary"
                                CausesValidation="false"
                                OnClick="btnModificar_Click" />

                            <asp:Button ID="btnGuardar" runat="server"
                                Text="Guardar cambios"
                                CssClass="btn btn-success"
                                Visible="false"
                                ValidationGroup="MiPerfil"
                                OnClick="btnGuardar_Click" />

                            <asp:Button ID="btnCancelar" runat="server"
                                Text="Cancelar"
                                CssClass="btn btn-outline-secondary"
                                Visible="false"
                                CausesValidation="false"
                                OnClick="btnCancelar_Click" />

                            <a href="Inicio.aspx" class="btn btn-outline-secondary">Volver al inicio
    </a>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>

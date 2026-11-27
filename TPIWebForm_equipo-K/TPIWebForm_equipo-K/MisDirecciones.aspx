<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MisDirecciones.aspx.cs" Inherits="TPIWebForm_equipo_K.MisDirecciones" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container py-4">
        <h1 class="mb-4">Mis direcciones</h1>

        <asp:Label ID="lblAviso" runat="server"
            CssClass="d-block text-danger mb-3" Visible="false" />

        <asp:Label ID="lblSinDirecciones" runat="server"
            Text="Todavía no tenés direcciones guardadas."
            CssClass="d-block text-muted mb-3" Visible="false" />

        <%-- Mostramos una tarjeta por dirección. --%>
        <asp:Repeater ID="repDirecciones" runat="server">
            <ItemTemplate>
                <div class="card shadow-sm mb-3">
                    <div class="card-body">
                        <h2 class="h5">
                            <%#: Eval("Calle") %> <%#: Eval("Numero") %>
                        </h2>
                        <p class="mb-1">
                            <%#: Eval("Localidad") %> ·
                        CP <%#: Eval("CodigoPostal") %>
                        </p>
                        <p class="text-muted mb-0">
                            <%#: Eval("Observaciones") %>
                        </p>
                        <asp:HyperLink ID="lnkEditar" runat="server"
                            Text="Editar"
                            CssClass="btn btn-outline-primary btn-sm mt-2"
                            NavigateUrl='<%# "~/MisDirecciones.aspx?editar=" + Eval("Id") %>' />
                    </div>
                </div>
            </ItemTemplate>
        </asp:Repeater>

        <asp:Button ID="btnNuevaDireccion" runat="server"
            Text="+ Agregar nueva dirección"
            CssClass="btn btn-outline-primary"
            CausesValidation="false"
            OnClick="btnNuevaDireccion_Click" />

        <%-- El formulario aparece al presionar Agregar nueva dirección. --%>
        <asp:Panel ID="pnlFormulario" runat="server"
            CssClass="card shadow-sm mt-3" Visible="false">
            <div class="card-body">
                <h2 class="h5 mb-3">
                    <asp:Literal ID="litTituloFormulario" runat="server"
                        Text="Nueva dirección" />
                </h2>

                <div class="row g-3">
                    <div class="col-md-8">
                        <asp:Label runat="server" AssociatedControlID="txtCalle"
                            Text="Calle" CssClass="form-label" />
                        <asp:TextBox ID="txtCalle" runat="server"
                            CssClass="form-control" MaxLength="150" />
                        <asp:RequiredFieldValidator ID="rfvCalle" runat="server"
                            ControlToValidate="txtCalle"
                            ErrorMessage="Ingresá la calle."
                            CssClass="text-danger" Display="Dynamic"
                            ValidationGroup="Direccion" EnableClientScript="false" />
                    </div>

                    <div class="col-md-4">
                        <asp:Label runat="server" AssociatedControlID="txtNumero"
                            Text="Número" CssClass="form-label" />
                        <asp:TextBox ID="txtNumero" runat="server"
                            CssClass="form-control" MaxLength="20" />
                        <asp:RequiredFieldValidator ID="rfvNumero" runat="server"
                            ControlToValidate="txtNumero"
                            ErrorMessage="Ingresá el número."
                            CssClass="text-danger" Display="Dynamic"
                            ValidationGroup="Direccion" EnableClientScript="false" />
                    </div>

                    <div class="col-md-8">
                        <asp:Label runat="server" AssociatedControlID="txtLocalidad"
                            Text="Localidad" CssClass="form-label" />
                        <asp:TextBox ID="txtLocalidad" runat="server"
                            CssClass="form-control" MaxLength="100" />
                        <asp:RequiredFieldValidator ID="rfvLocalidad" runat="server"
                            ControlToValidate="txtLocalidad"
                            ErrorMessage="Ingresá la localidad."
                            CssClass="text-danger" Display="Dynamic"
                            ValidationGroup="Direccion" EnableClientScript="false" />
                    </div>

                    <div class="col-md-4">
                        <asp:Label runat="server" AssociatedControlID="txtCodigoPostal"
                            Text="Código postal" CssClass="form-label" />
                        <asp:TextBox ID="txtCodigoPostal" runat="server"
                            CssClass="form-control" MaxLength="20" />
                        <asp:RequiredFieldValidator ID="rfvCodigoPostal" runat="server"
                            ControlToValidate="txtCodigoPostal"
                            ErrorMessage="Ingresá el código postal."
                            CssClass="text-danger" Display="Dynamic"
                            ValidationGroup="Direccion" EnableClientScript="false" />
                    </div>

                    <div class="col-12">
                        <asp:Label runat="server" AssociatedControlID="txtObservaciones"
                            Text="Observaciones (opcional)" CssClass="form-label" />
                        <asp:TextBox ID="txtObservaciones" runat="server"
                            CssClass="form-control" MaxLength="300" />
                    </div>
                </div>

                <div class="d-flex gap-2 mt-3">
                    <asp:Button ID="btnGuardarDireccion" runat="server"
                        Text="Guardar dirección" CssClass="btn btn-success"
                        ValidationGroup="Direccion"
                        OnClick="btnGuardarDireccion_Click" />

                    <asp:HyperLink ID="lnkCancelar" runat="server"
                        NavigateUrl="~/MisDirecciones.aspx"
                        CssClass="btn btn-outline-secondary"
                        Text="Cancelar" />
                </div>
            </div>
        </asp:Panel>
    </div>
</asp:Content>

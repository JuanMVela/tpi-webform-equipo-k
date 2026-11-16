<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="FormularioProducto.aspx.cs" Inherits="TPIWebForm_equipo_K.FormularioProducto" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container mt-4">
        <div class="row justify-content-center">
            <div class="col-md-10">
                <h2 id="tituloFormulario" runat="server" class="mb-4">Agregar Producto</h2>

                <div class="card shadow-sm">
                    <div class="card-body">
                        <div class="row">

                            <div class="col-md-6">
                                <div class="mb-3">
                                    <label class="form-label">Código:</label>
                                    <asp:TextBox ID="txtCodigo" runat="server" CssClass="form-control"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="rfvCodigo" runat="server"
                                        ControlToValidate="txtCodigo"
                                        ErrorMessage="Ingresá el código del producto."
                                        CssClass="text-danger"
                                        Display="Dynamic"
                                        ValidationGroup="Producto"
                                        EnableClientScript="false" />
                                    <asp:Label ID="lblErrorCodigo" runat="server"
                                        CssClass="text-danger" Visible="false"
                                        Text="Ya existe un producto con ese código. Ingresá otro." />
                                </div>
                                <div class="mb-3">
                                    <label class="form-label">Nombre:</label>
                                    <asp:TextBox ID="txtNombre" runat="server" CssClass="form-control"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="rfvNombre" runat="server"
                                        ControlToValidate="txtNombre"
                                        ErrorMessage="Ingresá el nombre del producto."
                                        CssClass="text-danger"
                                        Display="Dynamic"
                                        ValidationGroup="Producto"
                                        EnableClientScript="false" />
                                    <asp:Label ID="lblErrorNombre" runat="server"
                                        CssClass="text-danger" Visible="false"
                                        Text="Ingresá el nombre del producto." />
                                </div>
                                <div class="mb-3">
                                    <label class="form-label">Descripción:</label>
                                    <asp:TextBox ID="txtDescripcion" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="3"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="rfvDescripcion" runat="server"
                                        ControlToValidate="txtDescripcion"
                                        ErrorMessage="Ingresá la descripción del producto."
                                        CssClass="text-danger"
                                        Display="Dynamic"
                                        ValidationGroup="Producto"
                                        EnableClientScript="false" />
                                    <asp:Label ID="lblErrorDescripcion" runat="server"
                                        CssClass="text-danger" Visible="false"
                                        Text="Ingresá la descripción del producto." />
                                </div>
                                <div class="row">
                                    <div class="col-md-6 mb-3">
                                        <label class="form-label">Precio:</label>
                                        <asp:TextBox ID="txtPrecio" runat="server" CssClass="form-control"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="rfvPrecio" runat="server"
                                            ControlToValidate="txtPrecio"
                                            ErrorMessage="Ingresá el precio del producto."
                                            CssClass="text-danger"
                                            Display="Dynamic"
                                            ValidationGroup="Producto"
                                            EnableClientScript="false" />
                                        <asp:Label ID="lblErrorPrecio" runat="server"
                                            CssClass="text-danger" Visible="false"
                                            Text="Ingresá un precio válido." />
                                    </div>
                                    <div class="col-md-6 mb-3">
                                        <label class="form-label">Stock:</label>
                                        <asp:TextBox ID="txtStock" runat="server" CssClass="form-control"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="rfvStock" runat="server"
                                            ControlToValidate="txtStock"
                                            ErrorMessage="Ingresá el stock del producto."
                                            CssClass="text-danger"
                                            Display="Dynamic"
                                            ValidationGroup="Producto"
                                            EnableClientScript="false" />
                                        <asp:Label ID="lblErrorStock" runat="server"
                                            CssClass="text-danger" Visible="false"
                                            Text="Ingresá el stock del producto." />
                                    </div>
                                </div>
                            </div>

                            <div class="col-md-6">
                                <div class="mb-3">
                                    <label class="form-label">Categoría:</label>
                                    <asp:DropDownList ID="ddlCategoria" runat="server" CssClass="form-select" />
                                    <asp:RequiredFieldValidator ID="rfvCategoria" runat="server"
                                        ControlToValidate="ddlCategoria"
                                        InitialValue=""
                                        ErrorMessage="Seleccioná una categoría."
                                        CssClass="text-danger"
                                        Display="Dynamic"
                                        ValidationGroup="Producto"
                                        EnableClientScript="false" />
                                    <asp:Label ID="lblErrorCategoria" runat="server" CssClass="text-danger" Visible="false" Text="Seleccioná una categoría." />
                                </div>
                                <div class="mb-3">
                                    <label class="form-label">Marca:</label>
                                    <asp:DropDownList ID="ddlMarca" runat="server" CssClass="form-select" />

                                    <asp:RequiredFieldValidator ID="rfvMarca" runat="server"
                                        ControlToValidate="ddlMarca"
                                        InitialValue=""
                                        ErrorMessage="Seleccioná una marca."
                                        CssClass="text-danger"
                                        Display="Dynamic"
                                        ValidationGroup="Producto"
                                        EnableClientScript="false" />
                                    <asp:Label ID="lblErrorMarca" runat="server" CssClass="text-danger" Visible="false" Text="Seleccioná una marca." />
                                </div>

                                <div class="mb-3">
                                    <label class="form-label">Imágenes:</label>
                                    <div class="input-group mb-2">
                                        <asp:TextBox ID="txtUrlImagen" runat="server" CssClass="form-control" placeholder="URL de la imagen..."></asp:TextBox>
                                        <asp:Button ID="btnAgregarImagen" runat="server" Text="Agregar" OnClick="btnAgregarImagen_Click" CssClass="btn btn-outline-secondary" CausesValidation="false" />
                                    </div>
                                    <asp:ListBox ID="lbImagenes" runat="server" CssClass="form-control" Rows="3"></asp:ListBox>
                                </div>

                                <div class="text-center mt-3">
                                    <asp:Image ID="imgProducto" runat="server" CssClass="img-thumbnail" Width="200px" />
                                </div>
                            </div>
                        </div>

                        <div class="mt-4 border-top pt-3">
                            <asp:Button ID="btnAceptar" runat="server" Text="Aceptar" CssClass="btn btn-primary" OnClick="btnAceptar_Click" ValidationGroup="Producto" />
                            <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" CssClass="btn btn-secondary" OnClick="btnCancelar_Click" CausesValidation="false" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>

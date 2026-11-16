<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="DetalleProducto.aspx.cs" Inherits="TPIWebForm_equipo_K.DetalleProducto" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container py-4">
        <a href="Catalogo.aspx" class="btn btn-outline-secondary mb-3">Volver al catálogo
    </a>

        <asp:Label ID="lblAviso" runat="server" CssClass="text-danger" />

        <asp:Panel ID="pnlDetalle" runat="server" Visible="false">
            <div class="row g-4">
                <div class="col-md-5">
                    <asp:Image ID="imgProducto" runat="server"
                        CssClass="img-fluid rounded" Visible="false" />
                </div>
                <div class="col-md-7">
                    <p class="text-muted mb-1">
                        <asp:Literal ID="litCategoria" runat="server" Mode="Encode" />
                        |
   
                        <asp:Literal ID="litMarca" runat="server" Mode="Encode" />
                    </p>
                    <h1>
                        <asp:Literal ID="litNombre" runat="server" Mode="Encode" /></h1>
                    <p>
                        <asp:Literal ID="litDescripcion" runat="server" Mode="Encode" />
                    </p>
                    <p class="fs-4 text-primary">
                        <asp:Literal ID="litPrecio" runat="server" />
                    </p>
                    <p>
                        Stock disponible:
                        <asp:Literal ID="litStock" runat="server" />
                    </p>
                    <asp:Label runat="server" AssociatedControlID="txtCantidad"
                        Text="Cantidad" CssClass="form-label" />

                    <div class="d-flex flex-wrap gap-2 align-items-center">
                        <asp:TextBox ID="txtCantidad" runat="server"
                            TextMode="Number" Text="1" min="1" step="1"
                            CssClass="form-control" Style="width: 90px;" />

                        <asp:Button ID="btnAgregarCarrito" runat="server"
                            Text="Agregar al carrito" CssClass="btn btn-success"
                            OnClick="btnAgregarCarrito_Click" />
                    </div>

                    <asp:Label ID="lblCarrito" runat="server" CssClass="d-block mt-2" />
                </div>
            </div>
        </asp:Panel>
    </div>
</asp:Content>

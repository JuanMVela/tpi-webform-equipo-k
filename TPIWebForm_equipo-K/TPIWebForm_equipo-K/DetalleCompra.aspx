<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="DetalleCompra.aspx.cs" Inherits="TPIWebForm_equipo_K.DetalleCompra" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .seguimiento {
            display: flex;
            list-style: none;
            padding: 0;
            margin: 0;
        }

            .seguimiento li {
                flex: 1;
                position: relative;
                text-align: center;
                color: #6c757d;
            }

                .seguimiento li:not(:last-child)::after {
                    content: "";
                    position: absolute;
                    top: 16px;
                    left: 50%;
                    width: 100%;
                    height: 3px;
                    background: #dee2e6;
                }

            .seguimiento .circulo {
                position: relative;
                z-index: 1;
                display: flex;
                align-items: center;
                justify-content: center;
                width: 34px;
                height: 34px;
                margin: 0 auto 8px;
                border-radius: 50%;
                background: #dee2e6;
                color: #495057;
            }

            .seguimiento .actual {
                color: #0d6efd;
                font-weight: bold;
            }

                .seguimiento .actual .circulo {
                    background: #0d6efd;
                    color: white;
                }

            .seguimiento .completado .circulo,
            .seguimiento .completado::after {
                background: #198754;
                color: white;
            }

            .seguimiento .texto {
                display: block;
                padding: 0 4px;
                font-size: 0.85rem;
                overflow-wrap: anywhere;
            }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container py-4">
        <h1 class="mb-4">Detalle de compra</h1>

        <asp:Label ID="lblAviso" runat="server"
            CssClass="d-block text-danger mb-3"
            Visible="false" />

        <asp:Panel ID="pnlPedido" runat="server"
            CssClass="card shadow-sm mb-3"
            Visible="false">
            <div class="card-body">
                <asp:Panel ID="pnlSeguimiento" runat="server"
                    CssClass="mb-4">

                    <ol class="seguimiento" aria-label="Seguimiento del pedido">
                        <li id="pasoPendiente" runat="server">
                            <span class="circulo">1</span>
                            <span class="texto">Pendiente de pago</span>
                        </li>

                        <li id="pasoPagado" runat="server">
                            <span class="circulo">2</span>
                            <span class="texto">Pagado</span>
                        </li>

                        <li id="pasoPreparacion" runat="server">
                            <span class="circulo">3</span>
                            <span class="texto">En preparación</span>
                        </li>

                        <li id="pasoEnviado" runat="server">
                            <span class="circulo">4</span>
                            <span class="texto">Enviado</span>
                        </li>

                        <li id="pasoEntregado" runat="server">
                            <span class="circulo">5</span>
                            <span class="texto">Entregado</span>
                        </li>
                    </ol>
                </asp:Panel>
                <asp:Label ID="lblPedido" runat="server"
                    CssClass="d-block h5 mb-3" />

                <p>
                    Fecha:
   
        <asp:Label ID="lblFecha" runat="server" />
                </p>

                <p>
                    Estado:
   
        <asp:Label ID="lblEstado" runat="server"
            CssClass="fw-bold" />
                </p>

                <p class="mb-0">
                    Total:
   
        <asp:Label ID="lblTotal" runat="server"
            CssClass="fw-bold" />
                </p>
                <hr class="my-4" />

                <h2 class="h5 mb-3">Productos comprados</h2>

                <asp:Repeater ID="repProductos" runat="server">
                    <ItemTemplate>
                        <div class="border-bottom pb-3 mb-3">
                            <div class="fw-semibold mb-1">
                                <%#: Eval("Producto.Nombre") %>
                            </div>

                            <div class="d-flex justify-content-between gap-3">
                                <span>
                                    <%#: Eval("Cantidad") %> ×
       
                        <%#: Eval("PrecioUnitario", "{0:C}") %>
    </span>

                                <strong>
                                    <%#: Eval("Subtotal", "{0:C}") %>
    </strong>
                            </div>
                        </div>
                    </ItemTemplate>
                </asp:Repeater>
                <asp:Panel ID="pnlCargaComprobante" runat="server"
                    CssClass="mt-4" Visible="false">

                    <hr class="my-4" />

                    <h2 class="h5 mb-3">Comprobante de pago</h2>
                    <asp:Label ID="lblComprobanteActual" runat="server"
                        CssClass="d-block mb-3"
                        Visible="false" />

                    <p class="text-muted">
                        Adjuntá un comprobante de prueba en formato PDF, JPG o PNG.
        Tamaño máximo: 2 MB.
   
                    </p>

                    <asp:Label runat="server"
                        AssociatedControlID="fuComprobante"
                        Text="Seleccioná el archivo"
                        CssClass="form-label" />

                    <asp:FileUpload ID="fuComprobante" runat="server"
                        CssClass="form-control"
                        accept=".pdf,.jpg,.jpeg,.png" />

                    <asp:Label ID="lblAvisoComprobante" runat="server"
                        CssClass="d-block mt-2"
                        Visible="false" />

                    <asp:Button ID="btnSubirComprobante" runat="server"
                        Text="Cargar comprobante"
                        CssClass="btn btn-primary mt-3"
                        CausesValidation="false"
                        OnClick="btnSubirComprobante_Click" />

                    <p class="small text-muted mt-3 mb-0">
                        La carga del comprobante no confirma el pago.
        Será revisado por el administrador.
   
                    </p>
                </asp:Panel>
            </div>



        </asp:Panel>

        <a href="MisCompras.aspx" class="btn btn-outline-secondary">Volver a mis compras
    </a>
    </div>
</asp:Content>

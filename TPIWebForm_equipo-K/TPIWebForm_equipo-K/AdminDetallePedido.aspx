<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AdminDetallePedido.aspx.cs" Inherits="TPIWebForm_equipo_K.AdminDetallePedido" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container py-4">
        <h1 class="mb-4">Detalle del pedido</h1>

        <asp:Label ID="lblAviso" runat="server"
            CssClass="d-block text-danger mb-3"
            Visible="false" />

        <asp:Panel ID="pnlPedido" runat="server"
            CssClass="card shadow-sm mb-3" Visible="false">

            <div class="card-body">
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

                <p>
                    Pago:
                    <asp:Label ID="lblPago" runat="server" />
                </p>

                <p>
                    Entrega:
                   
                    <asp:Label ID="lblEntrega" runat="server" />
                </p>

                <asp:Panel ID="pnlDireccion" runat="server"
                    Visible="false">
                    <p>
                        Dirección:
                       
                        <asp:Label ID="lblDireccion" runat="server" />
                    </p>
                </asp:Panel>

                <hr />
                <h2 class="h5 mb-3">Observaciones internas</h2>

                <asp:Label ID="lblObservaciones" runat="server"
                    CssClass="d-block mb-3"
                    Style="white-space: pre-wrap; overflow-wrap: anywhere;" />

                <hr />

                <h2 class="h5 mb-3">Productos</h2>

                <asp:Repeater ID="repProductos" runat="server">
                    <ItemTemplate>
                        <div class="border-bottom pb-3 mb-3">
                            <strong><%#: Eval("Producto.Nombre") %></strong>

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

                <p class="mb-0 text-end">
                    Total:
                   
                    <asp:Label ID="lblTotal" runat="server"
                        CssClass="fw-bold" />
                </p>
            </div>
        </asp:Panel>

        <a href="AdminPedidos.aspx" class="btn btn-outline-secondary">Volver a pedidos
        </a>
    </div>

</asp:Content>

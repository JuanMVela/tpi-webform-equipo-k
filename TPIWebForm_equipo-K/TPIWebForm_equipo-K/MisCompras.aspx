<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MisCompras.aspx.cs" Inherits="TPIWebForm_equipo_K.MisCompras" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container py-4">
        <h1 class="mb-4">Mis compras</h1>

        <asp:Label ID="lblAviso" runat="server"
            CssClass="d-block text-muted mb-3"
            Visible="false" />

        <asp:Repeater ID="repPedidos" runat="server">
            <ItemTemplate>
                <div class="card shadow-sm mb-3">
                    <div class="card-body">
                        <h2 class="h5">Pedido #<%#: Eval("Id") %>
                    </h2>

                        <p class="mb-2">
                            Fecha: <%#: Eval("FechaCreacion", "{0:dd/MM/yyyy HH:mm}") %>
                        </p>

                        <p class="mb-2">
                            Estado: <strong><%#: Eval("NombreEstado") %></strong>
                        </p>

                        <p class="mb-0">
                            Total: <strong><%#: Eval("Total", "{0:C}") %></strong>
                        </p>
                        <asp:HyperLink runat="server"
                            Text="Ver pedido"
                            CssClass="btn btn-outline-primary mt-3"
                            NavigateUrl='<%# "DetalleCompra.aspx?id=" + Eval("Id") %>' />
                    </div>
                </div>
            </ItemTemplate>
        </asp:Repeater>
    </div>
</asp:Content>

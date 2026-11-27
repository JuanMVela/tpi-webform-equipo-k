<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Catalogo.aspx.cs" Inherits="TPIWebForm_equipo_K.Catalogo" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .galeria-producto {
            position: relative;
            height: 240px;
        }

            .galeria-producto img {
                width: 100%;
                height: 100%;
                object-fit: contain;
                padding: 12px;
            }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container mt-4">
        <h2 class="mb-4">Catálogo</h2>

        <div class="row row-cols-1 row-cols-md-3 g-4">
            <asp:Repeater ID="repProductos" runat="server">
                <ItemTemplate>
                    <div class="col">
                        <div class="card h-100 shadow-sm position-relative">
                            <!-- Mostramos las imágenes del producto. -->
                            <!-- Mostramos la primera imagen del producto. -->
                            <div class="galeria-producto">
                                <asp:Repeater ID="repImagenPrincipal" runat="server"
                                    DataSource='<%# Eval("Imagenes") %>'>
                                    <ItemTemplate>
                                        <asp:PlaceHolder runat="server"
                                            Visible='<%# Container.ItemIndex == 0 %>'>
                                            <img src='<%#: Eval("ImagenUrl") %>'
                                                alt="Imagen del producto" />
                                        </asp:PlaceHolder>
                                    </ItemTemplate>
                                </asp:Repeater>

                                <asp:PlaceHolder runat="server"
                                    Visible='<%# (int)Eval("Imagenes.Count") == 0 %>'>
                                    <div class="h-100 d-flex align-items-center justify-content-center text-muted">
                                        Sin imagen
       
                                    </div>
                                </asp:PlaceHolder>
                            </div>

                            <div class="card-body d-flex flex-column">
                                <h5 class="card-title fw-bold">
                                    <a href='DetalleProducto.aspx?id=<%# Eval("Id") %>'
                                        class="stretched-link text-decoration-none text-reset">
                                        <%#: Eval("Nombre") %>
                                    </a>
                                </h5>
                                <p class="text-muted mb-1">
                                    <%# Eval("Categoria.Nombre") %> | <%# Eval("Marca.Nombre") %>
                                </p>
                                <h4 class="mt-auto text-primary">$<%# Eval("Precio", "{0:N2}") %></h4>

                                <!-- Botón para ver el detalle -->
                                <a href='DetalleProducto.aspx?id=<%# Eval("Id") %>' class="btn btn-primary mt-2 w-100">Ver Detalle
                                </a>
                            </div>
                        </div>
                    </div>
                </ItemTemplate>
            </asp:Repeater>
        </div>
    </div>
</asp:Content>

<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Catalogo.aspx.cs" Inherits="TPIWebForm_equipo_K.Catalogo" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container mt-4">
    <h2 class="mb-4">Nuestro Catálogo</h2>
    
    <div class="row row-cols-1 row-cols-md-3 g-4">
        <asp:Repeater ID="repProductos" runat="server">
            <ItemTemplate>
                <div class="col">
                    <div class="card h-100 shadow-sm">
                        <!-- Mostramos la imagen -->
                        <img src=<%# Eval("Imagenes.Count") != null && (int)Eval("Imagenes.Count") > 0 ? Eval("Imagenes[0].ImagenUrl") : "https://media.istockphoto.com/id/1147544807/vector/thumbnail-image-vector-graphic.jpg?s=612x612&w=0&k=20&c=rnCKVbdxqkjlcs3xH87-9gocETqpspHFXu5dIGB4wuM=" %> />
                        
                        <div class="card-body d-flex flex-column">
                            <h5 class="card-title fw-bold"><%# Eval("Nombre") %></h5>
                            <p class="text-muted mb-1">
                                <%# Eval("Categoria.Nombre") %> | <%# Eval("Marca.Nombre") %>
                            </p>
                            <h4 class="mt-auto text-primary">$<%# Eval("Precio", "{0:N2}") %></h4>
                            
                            <!-- Botón para ver el detalle -->
                            <a href='DetalleProducto.aspx?id=<%# Eval("Id") %>' class="btn btn-primary mt-2 w-100">
                                Ver Detalle
                            </a>
                        </div>
                    </div>
                </div>
            </ItemTemplate>
        </asp:Repeater>
    </div>
</div>
</asp:Content>

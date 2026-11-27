<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="DetalleProducto.aspx.cs" Inherits="TPIWebForm_equipo_K.DetalleProducto" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .controles-galeria {
            display: flex;
            align-items: center;
            gap: 8px;
            margin-top: 16px;
        }

        .lista-miniaturas {
            display: flex;
            gap: 8px;
            overflow-x: auto;
            min-width: 0;
            padding: 4px;
        }

        .boton-miniatura {
            flex-shrink: 0;
            padding: 4px;
            border: 2px solid #dee2e6;
            border-radius: 6px;
            background: white;
        }

            .boton-miniatura[aria-pressed="true"] {
                border-color: #0d6efd;
            }

        .miniatura-producto {
            width: 65px;
            height: 65px;
            object-fit: contain;
        }

        .flecha-detalle {
            flex-shrink: 0;
            width: 34px;
            height: 38px;
            border: 1px solid #dee2e6;
            border-radius: 6px;
            background: white;
            color: #0d6efd;
            cursor: pointer;
        }

        .imagen-detalle {
            display: block;
            width: 100%;
            height: 620px;
            object-fit: contain;
        }

        @media (max-width: 767px) {
            .imagen-detalle {
                height: 400px;
            }
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container py-4">
        <a href="Catalogo.aspx" class="btn btn-outline-secondary mb-3">Volver al catálogo
        </a>

        <asp:Label ID="lblAviso" runat="server" CssClass="text-danger" />

        <asp:Panel ID="pnlDetalle" runat="server" Visible="false">
            <div class="row g-4">
                <div class="col-md-5">

                    <!-- Conservamos el tamaño de la imagen grande. -->
                    <asp:Image ID="imgProducto" runat="server"
                        CssClass="imagen-detalle rounded"
                        Visible="false" />

                    <!-- Mostramos los controles cuando hay varias imágenes. -->
                    <asp:Panel ID="pnlGaleria" runat="server"
                        CssClass="controles-galeria" Visible="false">

                        <button type="button"
                            class="flecha-detalle"
                            aria-label="Imagen anterior"
                            onclick="moverImagenDetalle(-1);">
                            &#10094;
       
                        </button>

                        <div id="listaMiniaturas" class="lista-miniaturas">
                            <asp:Repeater ID="repMiniaturas" runat="server">
                                <ItemTemplate>
                                    <button type="button"
                                        class="boton-miniatura"
                                        aria-label='<%# "Ver imagen " + (Container.ItemIndex + 1) %>'
                                        aria-pressed='<%# Container.ItemIndex == 0 ? "true" : "false" %>'
                                        onclick="seleccionarImagen(this);">

                                        <img src='<%#: Eval("ImagenUrl") %>'
                                            alt="Vista del producto"
                                            class="miniatura-producto" />
                                    </button>
                                </ItemTemplate>
                            </asp:Repeater>
                        </div>

                        <button type="button"
                            class="flecha-detalle"
                            aria-label="Imagen siguiente"
                            onclick="moverImagenDetalle(1);">
                            &#10095;
       
                        </button>

                    </asp:Panel>
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
    <script>
    // Mostramos la imagen de la miniatura elegida.
    /** @param {Element} boton */
function seleccionarImagen(boton = document.createElement("button")) {
        if (!(boton instanceof HTMLButtonElement)) {
            return;
        }

        var imagenGrande =
            document.getElementById('<%= imgProducto.ClientID %>');

        var miniatura = boton.querySelector("img");

        if (!(imagenGrande instanceof HTMLImageElement) ||
            !(miniatura instanceof HTMLImageElement)) {
            return;
        }

        imagenGrande.src = miniatura.src;

        var botones = document.querySelectorAll(
            "#listaMiniaturas button.boton-miniatura");

        botones.forEach(function (elemento) {
            elemento.setAttribute("aria-pressed", "false");
        });

        boton.setAttribute("aria-pressed", "true");

        var lista = document.getElementById("listaMiniaturas");

        if (lista instanceof HTMLElement) {
            lista.scrollLeft =
                boton.offsetLeft - lista.offsetLeft
                - (lista.clientWidth - boton.offsetWidth) / 2;
        }
    }

    // Cambiamos de imagen con las flechas.
    /** @param {number} desplazamiento */
function moverImagenDetalle(desplazamiento = 0) {
        var botones = document.querySelectorAll(
            "#listaMiniaturas button.boton-miniatura");

        if (botones.length == 0) {
            return;
        }

        var posicionActual = 0;

        botones.forEach(function (elemento, indice) {
            if (elemento.getAttribute("aria-pressed") == "true") {
                posicionActual = indice;
            }
        });

        var nuevaPosicion = posicionActual + desplazamiento;

        if (nuevaPosicion >= botones.length) {
            nuevaPosicion = 0;
        }

        if (nuevaPosicion < 0) {
            nuevaPosicion = botones.length - 1;
        }

        var siguienteBoton = botones.item(nuevaPosicion);

        if (siguienteBoton instanceof HTMLButtonElement) {
            seleccionarImagen(siguienteBoton);
        }
    }
</script>
</asp:Content>

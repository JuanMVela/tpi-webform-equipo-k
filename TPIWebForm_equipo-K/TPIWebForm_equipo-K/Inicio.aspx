<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Inicio.aspx.cs" Inherits="TPIWebForm_equipo_K.Inicio" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container mt-5">
        <div class="row align-items-center">

            <!-- COLUMNA IZQUIERDA: Textos y Botón -->
            <div class="col-md-6">
                <asp:Label ID="lblBienvenida" runat="server" CssClass="text-primary fw-bold fs-5 mb-3 d-block"></asp:Label>

                <h1 class="display-3 fw-bold mb-3">Las mejores productos en <span class="text-primary">tecnología</span></h1>

                <p class="lead text-muted mb-4">Descubrí los últimos lanzamientos</p>

                <a href="Catalogo.aspx" class="btn btn-primary btn-lg rounded-pill px-4 py-3 fw-bold">Ver Catálogo &rarr;</a>
            </div>

            <!-- COLUMNA DERECHA: Imagen Hero -->
            <div class="col-md-6 text-center mt-4 mt-md-0">

                <img src="img/Ofertas tecnológicas de vanguardia.png"
                    alt="Ofertas en tecnología"
                    class="img-fluid rounded-4 shadow-lg" />
            </div>

        </div>
    </div>
</asp:Content>

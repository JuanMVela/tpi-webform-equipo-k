<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AdminDashboard.aspx.cs" Inherits="TPIWebForm_equipo_K.AdminDashboard" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        /* Esto es para el efecto re pro al seleccionar */
        .list-group-item-action {
            transition: all 0.2s ease-in-out;
        }
        .list-group-item-action:hover {
            transform: translateX(5px);
            background-color: #f8f9fa;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container mt-5">
        <div class="row justify-content-center">
            <div class="col-md-8 col-lg-6">
                
                <div class="text-center mb-4">
                    <h2 class="fw-bold">Panel de Control</h2>
                    <p class="text-muted">Seleccioná una opción para administrar la tienda.</p>
                </div>

                <div class="list-group shadow-sm rounded-3">
                    
                    <!-- Productos -->
                    <a href="AdminProductos.aspx" class="list-group-item list-group-item-action d-flex justify-content-between align-items-center p-3">
                        <div class="d-flex align-items-center">
                            <span class="fs-4 me-3">📦</span>
                            <div>
                                <h6 class="mb-0 fw-bold">Productos</h6>
                                <small class="text-muted">Gestionar catálogo, stock y precios.</small>
                            </div>
                        </div>
                        <span class="text-primary fs-5">&rarr;</span>
                    </a>

                    <!-- Categorías -->
                    <a href="AdminCategorias.aspx" class="list-group-item list-group-item-action d-flex justify-content-between align-items-center p-3">
                        <div class="d-flex align-items-center">
                            <span class="fs-4 me-3">📁</span>
                            <div>
                                <h6 class="mb-0 fw-bold">Categorías</h6>
                                <small class="text-muted">Organizar los productos por tipo.</small>
                            </div>
                        </div>
                        <span class="text-primary fs-5">&rarr;</span>
                    </a>

                    <!-- Marcas -->
                    <a href="AdminMarcas.aspx" class="list-group-item list-group-item-action d-flex justify-content-between align-items-center p-3">
                        <div class="d-flex align-items-center">
                            <span class="fs-4 me-3">🏷️</span>
                            <div>
                                <h6 class="mb-0 fw-bold">Marcas</h6>
                                <small class="text-muted">Administrar las marcas disponibles.</small>
                            </div>
                        </div>
                        <span class="text-primary fs-5">&rarr;</span>
                    </a>

                    <!-- Formas de Pago -->
                    <a href="AdminFormasPago.aspx" class="list-group-item list-group-item-action d-flex justify-content-between align-items-center p-3">
                        <div class="d-flex align-items-center">
                            <span class="fs-4 me-3">💳</span>
                            <div>
                                <h6 class="mb-0 fw-bold">Formas de Pago</h6>
                                <small class="text-muted">Configurar métodos de pago.</small>
                            </div>
                        </div>
                        <span class="text-primary fs-5">&rarr;</span>
                    </a>

                    <!-- Formas de Entrega -->
                    <a href="AdminFormasEntrega.aspx" class="list-group-item list-group-item-action d-flex justify-content-between align-items-center p-3">
                        <div class="d-flex align-items-center">
                            <span class="fs-4 me-3">🚚</span>
                            <div>
                                <h6 class="mb-0 fw-bold">Formas de Entrega</h6>
                                <small class="text-muted">Gestionar envíos y retiros.</small>
                            </div>
                        </div>
                        <span class="text-primary fs-5">&rarr;</span>
                    </a>

                    <!-- Pedidos -->
                    <a href="AdminPedidos.aspx" class="list-group-item list-group-item-action d-flex justify-content-between align-items-center p-3">
                        <div class="d-flex align-items-center">
                            <span class="fs-4 me-3">🛒</span>
                            <div>
                                <h6 class="mb-0 fw-bold">Pedidos</h6>
                                <small class="text-muted">Ver y gestionar las ventas.</small>
                            </div>
                        </div>
                        <span class="text-primary fs-5">&rarr;</span>
                    </a>

                    <!-- Usuarios -->
                    <a href="AdminUsuarios.aspx" class="list-group-item list-group-item-action d-flex justify-content-between align-items-center p-3">
                        <div class="d-flex align-items-center">
                            <span class="fs-4 me-3">👥</span>
                            <div>
                                <h6 class="mb-0 fw-bold">Usuarios</h6>
                                <small class="text-muted">Administrar cuentas y accesos.</small>
                            </div>
                        </div>
                        <span class="text-primary fs-5">&rarr;</span>
                    </a>

                </div>
            </div>
        </div>
    </div>
</asp:Content>
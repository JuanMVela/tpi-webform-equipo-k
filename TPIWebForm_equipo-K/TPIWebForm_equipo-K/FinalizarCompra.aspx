<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="FinalizarCompra.aspx.cs" Inherits="TPIWebForm_equipo_K.FinalizarCompra" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <style>
        .lista-direcciones {
            list-style: none;
            padding: 0;
            margin: 0;
        }

            .lista-direcciones li {
                display: flex;
                align-items: baseline;
                gap: 12px;
                padding: 16px;
                margin-bottom: 12px;
                border: 1px solid #dee2e6;
                border-radius: 6px;
            }

            .lista-direcciones label {
                flex: 1;
                cursor: pointer;
                overflow-wrap: anywhere;
            }

            .lista-direcciones input {
                cursor: pointer;
            }
    </style>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container py-4">
        <h1 class="mb-4">Finalizar compra</h1>


        <div class="row g-3 align-items-start">
            <div class="col-12 col-lg-8">
                <asp:Panel ID="pnlRevision" runat="server"
                    CssClass="card shadow-sm mb-3" Visible="false">

                    <div class="card-body">
                        <h2 class="h5 mb-3">Revisá tu compra</h2>

                        <p class="text-muted">
                            Comprobá los datos antes de confirmar el pedido.
       
                        </p>

                        <div class="mb-3">
                            <strong>Forma de entrega</strong>
                            <asp:Label ID="lblRevisionEntrega" runat="server"
                                CssClass="d-block" />
                        </div>

                        <asp:Panel ID="pnlRevisionDireccion" runat="server"
                            CssClass="mb-3" Visible="false">
                            <strong>Dirección de envío</strong>
                            <asp:Label ID="lblRevisionDireccion" runat="server"
                                CssClass="d-block" />
                        </asp:Panel>

                        <div>
                            <strong>Forma de pago</strong>
                            <asp:Label ID="lblRevisionPago" runat="server"
                                CssClass="d-block" />
                        </div>
                        <asp:Button ID="btnEditarCompra" runat="server"
                            Text="Volver a editar"
                            CssClass="btn btn-outline-secondary mt-3"
                            CausesValidation="false"
                            OnClick="btnEditarCompra_Click" />

                    </div>
                </asp:Panel>
                <asp:Panel ID="pnlDatosCompra" runat="server">
                    <div class="card shadow-sm">
                        <div class="card-body">
                            <h2 class="h5 mb-3">Entrega y pago</h2>

                            <div class="mb-3">
                                <asp:Label runat="server" AssociatedControlID="ddlEntrega"
                                    Text="Forma de entrega" CssClass="form-label" />
                                <asp:DropDownList ID="ddlEntrega" runat="server"
                                    CssClass="form-select" AutoPostBack="true"
                                    OnSelectedIndexChanged="ddlEntrega_SelectedIndexChanged" />
                                <asp:RequiredFieldValidator ID="rfvEntrega" runat="server"
                                    ControlToValidate="ddlEntrega"
                                    InitialValue=""
                                    ErrorMessage="Seleccioná una forma de entrega."
                                    CssClass="text-danger"
                                    Display="Dynamic"
                                    ValidationGroup="Compra"
                                    EnableClientScript="false" />
                            </div>

                            <div class="mb-3">
                                <asp:Label runat="server" AssociatedControlID="ddlPago"
                                    Text="Forma de pago" CssClass="form-label" />
                                <asp:DropDownList ID="ddlPago" runat="server"
                                    CssClass="form-select" />
                                <asp:RequiredFieldValidator ID="rfvPago" runat="server"
                                    ControlToValidate="ddlPago"
                                    InitialValue=""
                                    ErrorMessage="Seleccioná una forma de pago."
                                    CssClass="text-danger"
                                    Display="Dynamic"
                                    ValidationGroup="Compra"
                                    EnableClientScript="false" />
                            </div>

                            <a href="Carrito.aspx" class="btn btn-outline-secondary">Volver al carrito
                    </a>
                        </div>
                    </div>
                    <asp:Panel ID="pnlDireccion" runat="server"
                        CssClass="card shadow-sm mt-3" Visible="false">
                        <div class="card-body">
                            <h2 class="h5 mb-3">Dirección de envío</h2>

                            <asp:RadioButtonList ID="rblDirecciones" runat="server"
                                CssClass="lista-direcciones"
                                RepeatLayout="UnorderedList" />
                            <asp:RequiredFieldValidator ID="rfvDireccion" runat="server"
                                ControlToValidate="rblDirecciones"
                                ErrorMessage="Seleccioná una dirección de envío."
                                CssClass="text-danger"
                                Display="Dynamic"
                                ValidationGroup="Compra"
                                EnableClientScript="false" />

                            <asp:Label ID="lblSinDirecciones" runat="server"
                                CssClass="d-block text-muted mb-3"
                                Visible="false" />

                            <asp:Button ID="btnAgregarDireccion" runat="server"
                                Text="+ Agregar nueva dirección"
                                CssClass="btn btn-outline-primary btn-sm"
                                CausesValidation="false"
                                OnClick="btnAgregarDireccion_Click" />
                        </div>
                    </asp:Panel>
                </asp:Panel>

            </div>

            <div class="col-12 col-lg-4">
                <div class="card shadow-sm">
                    <div class="card-body">
                        <h2 class="h5 mb-3">Resumen de compra</h2>

                        <asp:Repeater ID="repResumen" runat="server">
                            <ItemTemplate>
                                <div class="border-bottom pb-2 mb-2">
                                    <div class="fw-semibold"><%#: Eval("Nombre") %></div>
                                    <div class="d-flex justify-content-between gap-2">
                                        <span><%#: Eval("Cantidad") %> × <%#: Eval("Precio", "{0:C}") %></span>
                                        <span><%#: Eval("Subtotal", "{0:C}") %></span>
                                    </div>
                                </div>
                            </ItemTemplate>
                        </asp:Repeater>

                        <asp:Label ID="lblAvisoResumen" runat="server"
                            CssClass="d-block text-danger mb-2" Visible="false" />

                        <div class="d-flex justify-content-between fw-bold">
                            <span>Subtotal de productos</span>
                            <asp:Label ID="lblSubtotalCompra" runat="server" />
                        </div>
                        <asp:Label ID="lblAvisoCompra" runat="server"
                            CssClass="d-block text-danger mt-3"
                            Visible="false" />

                        <asp:Button ID="btnContinuar" runat="server"
                            Text="Continuar"
                            CssClass="btn btn-primary w-100 mt-3"
                            ValidationGroup="Compra"
                            OnClick="btnContinuar_Click" />
                        <asp:Button ID="btnConfirmarCompra" runat="server"
                            Text="Confirmar compra"
                            CssClass="btn btn-success w-100 mt-3"
                            Visible="false"
                            CausesValidation="false"
                            OnClick="btnConfirmarCompra_Click" />
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>

<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AdminPedidos.aspx.cs" Inherits="TPIWebForm_equipo_K.AdminPedidos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container py-4">
        <h1 class="mb-4">Administración de pedidos</h1>

        <h2 class="h5 mb-3">Comprobantes pendientes de revisión</h2>

        <asp:Label ID="lblAviso" runat="server"
            CssClass="d-block text-muted mb-3"
            Visible="false" />

        <div class="table-responsive">
            <asp:GridView ID="gvComprobantes" runat="server"
                AutoGenerateColumns="false"
                CssClass="table table-bordered align-middle"
                GridLines="None"
                DataKeyNames="Id,IdPedido"
                OnRowCommand="gvComprobantes_RowCommand">

                <Columns>
                    <asp:BoundField DataField="Id"
                        HeaderText="Comprobante" />

                    <asp:BoundField DataField="IdPedido"
                        HeaderText="Pedido" />

                    <asp:BoundField DataField="NombreArchivo"
                        HeaderText="Archivo"
                        HtmlEncode="true" />

                    <asp:BoundField DataField="FechaCarga"
                        HeaderText="Fecha de carga"
                        DataFormatString="{0:dd/MM/yyyy HH:mm}" />

                    <asp:BoundField DataField="Estado"
                        HeaderText="Estado"
                        HtmlEncode="true" />
                    <asp:TemplateField HeaderText="Archivo adjunto">
                        <ItemTemplate>
                            <asp:HyperLink runat="server"
                                Text="Descargar"
                                CssClass="btn btn-outline-primary btn-sm"
                                NavigateUrl='<%# "DescargarComprobante.ashx?id=" + Eval("Id") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Acciones">
                        <ItemTemplate>
                            <asp:Button ID="btnAprobar" runat="server"
                                Text="Aprobar"
                                CssClass="btn btn-success btn-sm me-2"
                                CommandName="AprobarComprobante"
                                CausesValidation="false" />

                            <asp:Button ID="btnRechazar" runat="server"
                                Text="Rechazar"
                                CssClass="btn btn-outline-danger btn-sm"
                                CommandName="RechazarComprobante"
                                CausesValidation="false" />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
        <hr class="my-4" />

        <h2 class="h5 mb-3">Listado de pedidos</h2>
        <div class="row g-3 mb-3">
            <div class="col-md-3">
                <asp:Label runat="server"
                    AssociatedControlID="ddlEstadoFiltro"
                    Text="Estado" CssClass="form-label" />

                <asp:DropDownList ID="ddlEstadoFiltro" runat="server"
                    CssClass="form-select">
                    <asp:ListItem Text="Todos" Value="0" />
                    <asp:ListItem Text="Pendiente" Value="1" />
                    <asp:ListItem Text="Pagado" Value="2" />
                    <asp:ListItem Text="En preparación" Value="3" />
                    <asp:ListItem Text="Enviado" Value="4" />
                    <asp:ListItem Text="Entregado" Value="5" />
                    <asp:ListItem Text="Cancelado" Value="6" />
                </asp:DropDownList>
            </div>

            <div class="col-md-3">
                <asp:Label runat="server"
                    AssociatedControlID="txtClienteFiltro"
                    Text="ID del cliente" CssClass="form-label" />

                <asp:TextBox ID="txtClienteFiltro" runat="server"
                    CssClass="form-control" MaxLength="10" />
            </div>

            <div class="col-md-3">
                <asp:Label runat="server"
                    AssociatedControlID="txtDesdeFiltro"
                    Text="Desde" CssClass="form-label" />

                <asp:TextBox ID="txtDesdeFiltro" runat="server"
                    TextMode="Date" CssClass="form-control" />
            </div>

            <div class="col-md-3">
                <asp:Label runat="server"
                    AssociatedControlID="txtHastaFiltro"
                    Text="Hasta" CssClass="form-label" />

                <asp:TextBox ID="txtHastaFiltro" runat="server"
                    TextMode="Date" CssClass="form-control" />
            </div>

            <div class="col-12">
                <asp:Button ID="btnFiltrarPedidos" runat="server"
                    Text="Filtrar"
                    CssClass="btn btn-primary me-2"
                    CausesValidation="false"
                    OnClick="btnFiltrarPedidos_Click" />

                <asp:Button ID="btnLimpiarFiltros" runat="server"
                    Text="Limpiar"
                    CssClass="btn btn-outline-secondary"
                    CausesValidation="false"
                    OnClick="btnLimpiarFiltros_Click" />
            </div>
        </div>

        <asp:Label ID="lblAvisoPedidos" runat="server"
            CssClass="d-block text-muted mb-3"
            Visible="false" />

        <div class="table-responsive">
            <asp:GridView ID="gvPedidos" runat="server"
                AutoGenerateColumns="false"
                CssClass="table table-bordered align-middle"
                GridLines="None"
                DataKeyNames="Id,IdEstado"
                OnRowCommand="gvPedidos_RowCommand">

                <Columns>
                    <asp:TemplateField HeaderText="Detalle">
                        <ItemTemplate>
                            <asp:HyperLink runat="server"
                                Text="Ver detalle"
                                CssClass="btn btn-outline-primary btn-sm"
                                NavigateUrl='<%# "AdminDetallePedido.aspx?id=" + Eval("Id") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="Id"
                        HeaderText="Pedido" />

                    <asp:TemplateField HeaderText="Cliente">
                        <ItemTemplate>
                            <%#: Eval("Cliente.Id") %>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:BoundField DataField="FechaCreacion"
                        HeaderText="Fecha"
                        DataFormatString="{0:dd/MM/yyyy HH:mm}" />

                    <asp:BoundField DataField="Total"
                        HeaderText="Total"
                        DataFormatString="{0:C}" />

                    <asp:BoundField DataField="NombreEstado"
                        HeaderText="Estado"
                        HtmlEncode="true" />
                    <asp:TemplateField HeaderText="Acciones">
                        <ItemTemplate>
                            <asp:Button ID="btnAvanzarEstado" runat="server"
                                CssClass="btn btn-primary btn-sm"
                                CommandName="AvanzarEstado"
                                CausesValidation="false"
                                Visible='<%# (int)Eval("IdEstado") >= 2
                && (int)Eval("IdEstado") <= 4 %>'
                                Text='<%# TextoSiguienteEstado((int)Eval("IdEstado")) %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Observaciones internas">
                        <ItemTemplate>
                            <asp:TextBox ID="txtObservaciones" runat="server"
                                TextMode="MultiLine"
                                Rows="3"
                                CssClass="form-control mb-2"
                                Text='<%# Eval("ObservacionesInternas") %>' />

                            <span class="small text-muted d-block mb-2">Máximo 1000 caracteres. Solo para administración.
        </span>

                            <asp:Button ID="btnGuardarObservaciones" runat="server"
                                Text="Guardar observaciones"
                                CssClass="btn btn-outline-primary btn-sm"
                                CommandName="GuardarObservaciones"
                                CausesValidation="false" />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>

        <a href="AdminDashboard.aspx" class="btn btn-outline-secondary">Volver al panel
        </a>
    </div>
</asp:Content>

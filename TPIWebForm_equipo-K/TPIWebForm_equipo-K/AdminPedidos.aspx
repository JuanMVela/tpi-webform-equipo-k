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
        
        <a href="AdminDashboard.aspx" class="btn btn-outline-secondary">Volver al panel
        </a>
    </div>
</asp:Content>

<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Carrito.aspx.cs" Inherits="TPIWebForm_equipo_K.Carrito" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container py-4">
        <h1 class="mb-3">Carrito de compras</h1>

        <asp:Label ID="lblAviso" runat="server"
            CssClass="d-block mb-3 text-muted" Visible="false" />
        <%--dgvCarrito. --%>

        <div class="row g-3 align-items-start">
            <div class="col-12 col-lg-8">
                <div class="card shadow-sm">
                    <div class="card-body">
                        <h2 class="h5 mb-3">Productos</h2>

                        <div class="table-responsive">
                            <asp:GridView ID="dgvCarrito" runat="server"
                                AutoGenerateColumns="false"
                                CssClass="table align-middle"
                                GridLines="None"
                                Visible="false"
                                OnRowCommand="dgvCarrito_RowCommand">
                                <Columns>
                                    <asp:TemplateField HeaderText="Producto">
                                        <ItemTemplate>
                                            <div class="d-flex align-items-center gap-2">
                                                <asp:Image runat="server"
                                                    ImageUrl='<%# Eval("ImagenUrl") %>'
                                                    Visible='<%# !string.IsNullOrWhiteSpace((string)Eval("ImagenUrl")) %>'
                                                    AlternateText=""
                                                    Width="56px" Height="56px"
                                                    CssClass="rounded border"
                                                    Style="object-fit: contain;" />

                                                <span><%#: Eval("Nombre") %></span>
                                            </div>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Cantidad">
                                        <ItemTemplate>
                                            <div class="d-flex align-items-center gap-2">
                                                <asp:Button runat="server" Text="−"
                                                    CssClass="btn btn-sm btn-outline-secondary"
                                                    CommandName="RestarUnidad"
                                                    CommandArgument='<%# Eval("Id") %>'
                                                    Enabled='<%# (int)Eval("Cantidad") > 1 %>'
                                                    CausesValidation="false" />

                                                <span><%#: Eval("Cantidad") %></span>

                                                <asp:Button runat="server" Text="+"
                                                    CssClass="btn btn-sm btn-outline-secondary"
                                                    CommandName="SumarUnidad"
                                                    CommandArgument='<%# Eval("Id") %>'
                                                    CausesValidation="false" />
                                            </div>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="Precio" HeaderText="Precio unitario"
                                        DataFormatString="{0:C}" />
                                    <asp:BoundField DataField="Subtotal" HeaderText="Subtotal"
                                        DataFormatString="{0:C}" />
                                    <asp:TemplateField HeaderText="Acciones">
                                        <ItemTemplate>
                                            <asp:Button runat="server" Text="Quitar"
                                                CssClass="btn btn-sm btn-outline-danger"
                                                CommandName="QuitarProducto"
                                                CommandArgument='<%# Eval("Id") %>'
                                                CausesValidation="false" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>

                        <a href="Catalogo.aspx" class="btn btn-outline-secondary">Seguir comprando
                </a>
                    </div>
                </div>
            </div>

            <div class="col-12 col-lg-4">
                <asp:Panel ID="pnlResumen" runat="server"
                    CssClass="card shadow-sm" Visible="false">
                    <div class="card-body">
                        <h2 class="h5 mb-3">Resumen</h2>

                        <div class="d-flex justify-content-between border-top pt-3 fw-bold">
                            <span>Total</span>
                            <asp:Label ID="lblTotal" runat="server" />
                        </div>
                    </div>
                </asp:Panel>
            </div>
        </div>

        <%-- dgvCarrito. --%>
    </div>
</asp:Content>

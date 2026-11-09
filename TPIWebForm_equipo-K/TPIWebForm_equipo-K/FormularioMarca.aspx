<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="FormularioMarca.aspx.cs" Inherits="TPIWebForm_equipo_K.FormularioMarca" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container mt-4">

        <div class="row justify-content-center">

            <div class="col-md-6">

                <h2 id="tituloFormulario"
                    runat="server"
                    class="mb-4">Marca
                </h2>

                <div class="card shadow-sm">
                    <div class="card-body">
                        <div class="mb-3">
                            <label class="form-label">
                                Nombre de la Marca
                           
                            </label>
                            <asp:TextBox ID="txtNombre" runat="server" CssClass="form-control" ></asp:TextBox>
                            <asp:Label ID="lblErrorNombre" runat="server"
                                CssClass="text-danger" Visible="false"
                                Text="Ya existe una marca con ese nombre." />
                        </div>

                        <div class="mt-4">

                            <asp:Button
                                ID="btnAceptar"
                                runat="server"
                                Text="Aceptar"
                                CssClass="btn btn-primary"
                                OnClick="btnAceptar_Click" />

                            <asp:Button
                                ID="btnCancelar"
                                runat="server"
                                Text="Cancelar"
                                CssClass="btn btn-secondary"
                                OnClick="btnCancelar_Click" />

                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>

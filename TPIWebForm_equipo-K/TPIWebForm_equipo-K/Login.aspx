<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="TPIWebForm_equipo_K.Login" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="row justify-content-center">
        <div class="col-md-5">
            <h2 class="mb-4">Iniciar sesión</h2>

            <div class="card shadow-sm">
                <div class="card-body">
                    <div class="mb-3">
                        <label for="txtEmail" class="form-label">Email</label>
                        <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" TextMode="Email" placeholder="Ingresá tu email" MaxLength="150"></asp:TextBox>
                    </div>

                    <div class="mb-3">
                        <label for="txtPassword" class="form-label">Contraseña</label>
                        <asp:TextBox ID="txtPassword" runat="server" CssClass="form-control" TextMode="Password" placeholder="Ingresá tu contraseña"></asp:TextBox>
                    </div>

                    <asp:Button ID="btnIngresar" runat="server" Text="Ingresar" CssClass="btn btn-primary" OnClick="btnIngresar_Click" />
                    <a href="Inicio.aspx" class="btn btn-secondary">Cancelar</a>

                    <hr class="my-3" />
                    <!-- Una línea divisoria linda -->

                    <div class="text-center">
                        <span class="text-muted">¿No tenés una cuenta?</span>
                        <a href="Registro.aspx" class="fw-bold text-decoration-none">Registrate acá</a>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>

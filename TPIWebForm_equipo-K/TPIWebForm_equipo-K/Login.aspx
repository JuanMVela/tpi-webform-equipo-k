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
                        <asp:RequiredFieldValidator ID="rfvEmail" runat="server"
                            ControlToValidate="txtEmail"
                            ErrorMessage="Ingresá tu email."
                            CssClass="text-danger"
                            Display="Dynamic"
                            ValidationGroup="Login"
                            EnableClientScript="false" />
                        <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" TextMode="Email" placeholder="Ingresá tu email" MaxLength="150"></asp:TextBox>
                    </div>

                    <div class="mb-3">
                        <label for="txtPassword" class="form-label">Contraseña</label>
                        <asp:RequiredFieldValidator ID="rfvPassword" runat="server"
                            ControlToValidate="txtPassword"
                            ErrorMessage="Ingresá tu contraseña."
                            CssClass="text-danger"
                            Display="Dynamic"
                            ValidationGroup="Login"
                            EnableClientScript="false" />
                        <asp:TextBox ID="txtPassword" runat="server" CssClass="form-control" TextMode="Password" placeholder="Ingresá tu contraseña"></asp:TextBox>
                    </div>

                    <asp:Button ID="btnIngresar" runat="server"
                        Text="Ingresar"
                        CssClass="btn btn-primary"
                        OnClick="btnIngresar_Click"
                        ValidationGroup="Login" />
                    <a href="Inicio.aspx" class="btn btn-secondary">Cancelar</a>

                    <hr class="my-3" />
                    <!-- Una línea divisoria linda -->

                    <div class="mt-3">
                        ¿Todavía no tenés cuenta?   
                        <asp:HyperLink ID="lnkRegistro" runat="server"
                            NavigateUrl="~/Registro.aspx"
                            Text="Registrate" />
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>

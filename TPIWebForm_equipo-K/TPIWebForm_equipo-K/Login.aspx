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
                        <label for="emailLogin" class="form-label">Email</label>
                        <input id="emailLogin" name="emailLogin" type="email" class="form-control" maxlength="150" autocomplete="username" placeholder="Ingresá tu email" />
                    </div>

                    <div class="mb-3">
                        <label for="passwordLogin" class="form-label">Contraseña</label>
                        <input id="passwordLogin" name="passwordLogin" type="password" class="form-control" autocomplete="current-password" placeholder="Ingresá tu contraseña" />
                    </div>

                    <button type="button" class="btn btn-primary">Ingresar</button>
                    <a href="Inicio.aspx" class="btn btn-secondary">Cancelar</a>
                </div>
            </div>
        </div>
    </div>
</asp:Content>

using dominio;
using negocio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace TPIWebForm_equipo_K
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Conservamos el destino al pasar al registro.
            if (Request.QueryString["origen"] == "compra")
            {
                lnkRegistro.NavigateUrl = "~/Registro.aspx?origen=compra";
            }
            // Si ya ingresó, lo enviamos al destino correspondiente.
            if (!IsPostBack && Session["UsuarioLogueado"] != null)
            {
                if (Request.QueryString["origen"] == "compra")
                {
                    Response.Redirect("FinalizarCompra.aspx");
                    return;
                }

                Response.Redirect("Inicio.aspx");
                return;
            }
        }

        protected void btnIngresar_Click(object sender, EventArgs e)
        {
            try
            {
                // Si falta el email o la contraseña, salimos antes de iniciar sesión.
                if (!Page.IsValid)
                {
                    return;
                }

                UsuarioNegocio negocio = new UsuarioNegocio();
                Usuario usuarioLogueado = negocio.login(txtEmail.Text.Trim(), txtPassword.Text);

                if (usuarioLogueado != null)
                {
                    // si sale bien Guardamos al usuario en la Sesión
                    Session["UsuarioLogueado"] = usuarioLogueado;
                    // Retomamos la compra después de ingresar.
                    if (Request.QueryString["origen"] == "compra")
                    {
                        Response.Redirect("FinalizarCompra.aspx");
                        return;
                    }

                    if (usuarioLogueado.Perfil.Id == 2)
                    {
                        // Es Administrador, lo mandamos al panel de administración
                        Response.Redirect("AdminProductos.aspx");
                    }
                    else
                    {
                        // Es Cliente, lo mandamos al inicio
                        Response.Redirect("Inicio.aspx");
                    }
                }
                else
                {
                    Response.Write("<script>alert('Email o contraseña incorrectos.');</script>");
                }
            }
            catch (Exception ex)
            {
                Response.Write("<script>alert('Error: " + ex.Message + "');</script>");
            }
        }
    }
}
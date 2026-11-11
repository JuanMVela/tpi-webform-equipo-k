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
            // Si el usuario ya está logueado, lo mandamos al inicio para que no vea el login de nuevo
            if (!IsPostBack && Session["UsuarioLogueado"] != null)
            {
                Response.Redirect("Inicio.aspx");
            }
        }

        protected void btnIngresar_Click(object sender, EventArgs e)
        {
            try
            {
                // 1. Validamos que no estén vacíos
                if (string.IsNullOrWhiteSpace(txtEmail.Text) || string.IsNullOrWhiteSpace(txtPassword.Text))
                {
                    Response.Write("<script>alert('Por favor, completá todos los campos.');</script>");
                    return;
                }

                UsuarioNegocio negocio = new UsuarioNegocio();
                Usuario usuarioLogueado = negocio.login(txtEmail.Text.Trim(), txtPassword.Text);

                if (usuarioLogueado != null)
                {
                    // si sale bien Guardamos al usuario en la Sesión
                    Session["UsuarioLogueado"] = usuarioLogueado;

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
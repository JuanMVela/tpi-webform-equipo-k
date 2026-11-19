using dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace TPIWebForm_equipo_K
{
    public partial class Site : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UsuarioLogueado"] != null)
            {
                // Está logueado: mostramos su nombre y el botón Salir
                Usuario usuario = (Usuario)Session["UsuarioLogueado"];
                phInvitado.Visible = false;
                phLogueado.Visible = true;
                // Mostramos los datos del usuario en el menú.
                lblNombreUsuario.Text = Server.HtmlEncode(usuario.Nombre);
                lblEmailUsuario.Text = Server.HtmlEncode(usuario.Email);
            }
            else
            {
                // No está logueado: mostramos Ingresar y Registro
                phInvitado.Visible = true;
                phLogueado.Visible = false;
            }
        }

        protected void btnSalir_Click(object sender, EventArgs e)
        {
            Session.Clear();
            Session.Abandon();
            Response.Redirect("Inicio.aspx");
        }
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            // Leemos el carrito guardado en la sesión.
            var carrito = Session["Carrito"] as Dictionary<int, int>;
            int totalUnidades = 0;

            if (carrito != null)
            {
                foreach (int cantidad in carrito.Values)
                {
                    totalUnidades += cantidad;
                }
            }

            lblCantidadCarrito.Text = totalUnidades.ToString();
            lblCantidadCarrito.Visible = totalUnidades > 0;
        }
    }
}
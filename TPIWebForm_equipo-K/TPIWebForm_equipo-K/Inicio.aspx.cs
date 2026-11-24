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
    public partial class Inicio : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            pnlConfiguracionInicial.Visible = false;

            Usuario usuario = Session["UsuarioLogueado"] as Usuario;

            if (usuario != null)
            {
                // Mostramos el nombre del usuario.
                lblBienvenida.Text = "¡Hola, "
                    + Server.HtmlEncode(usuario.Nombre)
                    + "! Bienvenido de nuevo.";

                lblBienvenida.CssClass =
                    "text-success fw-bold fs-5 mb-3 d-block";

                try
                {
                    // Mostramos el acceso mientras falte el primer administrador.
                    UsuarioNegocio negocio = new UsuarioNegocio();

                    pnlConfiguracionInicial.Visible =
                        !negocio.existeAdministrador();
                }
                catch (Exception ex)
                {
                    // Si falla la consulta, el acceso permanece oculto.
                    System.Diagnostics.Trace.TraceError(
                        "Error al comprobar la configuración inicial: {0}", ex);
                }
            }
            else
            {
                lblBienvenida.Text =
                    "¡Bienvenido! Iniciá sesión para comprar más rápido.";

                lblBienvenida.CssClass =
                    "text-primary fw-bold fs-5 mb-3 d-block";
            }
        }
    }
}
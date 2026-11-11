using dominio;
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
            if (Session["UsuarioLogueado"] != null)
            {
                // Si está logueado, mostramos su nombre
                Usuario usuario = (Usuario)Session["UsuarioLogueado"];
                lblBienvenida.Text = "¡Hola, " + usuario.Nombre + "! Bienvenido de nuevo.";
                lblBienvenida.CssClass = "text-success fw-bold fs-5 mb-3 d-block"; // son colores
            }
            else
            {
                lblBienvenida.Text = "¡Bienvenido! Iniciá sesión para comprar más rápido.";
                lblBienvenida.CssClass = "text-primary fw-bold fs-5 mb-3 d-block"; //son colores
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using dominio;
using negocio;

namespace TPIWebForm_equipo_K
{
    public partial class AdminPedidos : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        // Verificamos el permiso actual antes de cargar la página.
        protected override void OnPreInit(EventArgs e)
        {
            Usuario usuario = Session["UsuarioLogueado"] as Usuario;

            if (usuario == null)
            {
                Response.Redirect("Login.aspx");
                return;
            }

            bool autorizado;

            try
            {
                UsuarioNegocio negocio = new UsuarioNegocio();
                autorizado = negocio.esAdministradorActivo(usuario.Id);
            }
            catch (Exception)
            {
                throw new System.Web.HttpException(
                    503, "No pudimos verificar tus permisos. Intentá nuevamente.");
            }

            if (!autorizado)
            {
                Response.Redirect("Inicio.aspx");
                return;
            }

            base.OnPreInit(e);
        }
    }
}
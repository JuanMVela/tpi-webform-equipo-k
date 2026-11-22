using dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace TPIWebForm_equipo_K
{
    public partial class AdminDashboard : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // asi solo el admin puede ver este panel
            Usuario usuario = Session["UsuarioLogueado"] as Usuario;
            if (usuario == null || usuario.Perfil.Id != 2)
            {
                Response.Redirect("Inicio.aspx");
                return;
            }
        }
    }
}
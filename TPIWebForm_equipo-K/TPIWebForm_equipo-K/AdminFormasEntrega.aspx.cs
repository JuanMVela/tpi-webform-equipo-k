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
    public partial class AdminFormasEntrega : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                cargarGrilla();
            }
        }

        // Muestra las formas de entrega en la grilla
        private void cargarGrilla()
        {
            FormaEntregaNegocio negocio = new FormaEntregaNegocio();
            dgvFormasEntrega.DataSource = negocio.listar();
            dgvFormasEntrega.DataBind();
        }
        // Abre el formulario de la forma de entrega seleccionada
        protected void dgvFormasEntrega_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Modificar")
            {
                int id = int.Parse(e.CommandArgument.ToString());
                Response.Redirect("FormularioFormaEntrega.aspx?id=" + id);
                return;
            }
        }
    }
}
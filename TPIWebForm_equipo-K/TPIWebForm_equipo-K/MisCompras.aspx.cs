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
    public partial class MisCompras : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Usuario usuario = Session["UsuarioLogueado"] as Usuario;

            if (usuario == null)
            {
                Response.Redirect("Login.aspx");
                return;
            }

            if (!IsPostBack)
            {
                CargarPedidos(usuario.Id);
            }
        }

        // Mostramos los pedidos del usuario que inició sesión.
        private void CargarPedidos(int idCliente)
        {
            lblAviso.Visible = false;

            try
            {
                PedidoNegocio negocio = new PedidoNegocio();
                var pedidos = negocio.listarPorCliente(idCliente);

                repPedidos.DataSource = pedidos;
                repPedidos.DataBind();

                if (pedidos.Count == 0)
                {
                    lblAviso.Text = "Todavía no realizaste compras.";
                    lblAviso.Visible = true;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.TraceError(ex.ToString());

                repPedidos.Visible = false;
                lblAviso.CssClass = "d-block text-danger mb-3";
                lblAviso.Text =
                    "No pudimos cargar tus compras. Intentá nuevamente.";
                lblAviso.Visible = true;
            }
        }
    }
}
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
            if (!IsPostBack)
            {
                Usuario usuario = Session["UsuarioLogueado"] as Usuario;

                if (usuario == null)
                {
                    return;
                }

                CargarComprobantes(usuario.Id);
            }
        }

        // Mostramos los comprobantes que esperan revisión.
        private void CargarComprobantes(int idAdministrador)
        {
            lblAviso.Visible = false;
            gvComprobantes.Visible = false;

            try
            {
                ComprobantePagoNegocio negocio = new ComprobantePagoNegocio();

                var comprobantes =
                    negocio.listarPendientesAdministrador(idAdministrador);

                gvComprobantes.DataSource = comprobantes;
                gvComprobantes.DataBind();

                if (comprobantes.Count == 0)
                {
                    lblAviso.CssClass = "d-block text-muted mb-3";
                    lblAviso.Text = "No hay comprobantes pendientes de revisión.";
                    lblAviso.Visible = true;
                }
                else
                {
                    gvComprobantes.Visible = true;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.TraceError(ex.ToString());

                lblAviso.CssClass = "d-block text-danger mb-3";
                lblAviso.Text =
                    "No pudimos cargar los comprobantes. Intentá nuevamente.";
                lblAviso.Visible = true;
            }
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

        // Procesamos la decisión del administrador.
        protected void gvComprobantes_RowCommand(
            object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName != "AprobarComprobante" &&
                e.CommandName != "RechazarComprobante")
            {
                return;
            }

            Usuario usuario = Session["UsuarioLogueado"] as Usuario;

            if (usuario == null)
            {
                Response.Redirect("Login.aspx");
                return;
            }

            lblAviso.Visible = false;

            bool aprobar = e.CommandName == "AprobarComprobante";

            try
            {
                Control boton = (Control)e.CommandSource;
                GridViewRow fila = (GridViewRow)boton.NamingContainer;

                int idComprobante =
                    (int)gvComprobantes.DataKeys[fila.RowIndex].Values["Id"];

                int idPedido =
                    (int)gvComprobantes.DataKeys[fila.RowIndex].Values["IdPedido"];

                ComprobantePagoNegocio negocio = new ComprobantePagoNegocio();

                negocio.revisarComprobante(
                    idComprobante, idPedido, usuario.Id, aprobar);
            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                System.Diagnostics.Trace.TraceError(ex.ToString());

                lblAviso.CssClass = "d-block text-danger mb-3";
                lblAviso.Text =
                    "No pudimos guardar la revisión. Intentá nuevamente.";
                lblAviso.Visible = true;
                return;
            }
            catch (Exception ex)
            {
                lblAviso.CssClass = "d-block text-danger mb-3";
                lblAviso.Text = Server.HtmlEncode(ex.Message);
                lblAviso.Visible = true;
                return;
            }

            CargarComprobantes(usuario.Id);

            lblAviso.CssClass = "d-block text-success mb-3";

            if (aprobar)
            {
                lblAviso.Text = "Comprobante aprobado. El pedido figura como pagado.";
            }
            else
            {
                lblAviso.Text =
                    "Comprobante rechazado. El cliente puede cargar uno nuevo.";
            }

            lblAviso.Visible = true;
        }
    }
}
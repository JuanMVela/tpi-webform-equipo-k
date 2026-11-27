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
                CargarPedidos(usuario.Id);
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

        // Mostramos los pedidos en la tabla del administrador.
        private void CargarPedidos(int idAdministrador)
        {
            lblAvisoPedidos.Visible = false;
            gvPedidos.Visible = false;

            try
            {
                List<Pedido> pedidos =
    ObtenerPedidosFiltrados(idAdministrador);

                gvPedidos.DataSource = pedidos;
                gvPedidos.DataBind();

                if (pedidos.Count == 0)
                {
                    lblAvisoPedidos.CssClass = "d-block text-muted mb-3";
                    lblAvisoPedidos.Text =
    "No hay pedidos que coincidan con los filtros.";
                    lblAvisoPedidos.Visible = true;
                }
                else
                {
                    gvPedidos.Visible = true;
                }
            }
            catch (ArgumentException ex)
            {
                lblAvisoPedidos.CssClass = "d-block text-danger mb-3";
                lblAvisoPedidos.Text = Server.HtmlEncode(ex.Message);
                lblAvisoPedidos.Visible = true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.TraceError(ex.ToString());

                lblAvisoPedidos.CssClass = "d-block text-danger mb-3";
                lblAvisoPedidos.Text =
                    "No pudimos cargar los pedidos. Intentá nuevamente.";
                lblAvisoPedidos.Visible = true;
            }
        }

        // Indicamos qué acción corresponde al estado actual.
        protected string TextoSiguienteEstado(int idEstado)
        {
            switch (idEstado)
            {
                case 2:
                    return "Pasar a preparación";

                case 3:
                    return "Marcar enviado";

                case 4:
                    return "Marcar entregado";

                default:
                    return "";
            }
        }

        // Avanzamos el pedido seleccionado y actualizamos la tabla.
        protected void gvPedidos_RowCommand(
            object sender, GridViewCommandEventArgs e)
        {
            // Procesamos el guardado de observaciones por separado.
            if (e.CommandName == "GuardarObservaciones")
            {
                GuardarObservacionesFila(e);
                return;
            }

            if (e.CommandName != "AvanzarEstado")
            {
                return;
            }

            Usuario usuario = Session["UsuarioLogueado"] as Usuario;

            if (usuario == null)
            {
                Response.Redirect("Login.aspx");
                return;
            }

            try
            {
                Control boton = (Control)e.CommandSource;
                GridViewRow fila = (GridViewRow)boton.NamingContainer;

                int idPedido =
                    (int)gvPedidos.DataKeys[fila.RowIndex].Values["Id"];

                int estadoActual =
                    (int)gvPedidos.DataKeys[fila.RowIndex].Values["IdEstado"];

                PedidoNegocio negocio = new PedidoNegocio();

                negocio.avanzarEstado(
                    idPedido, estadoActual, usuario.Id);
            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                System.Diagnostics.Trace.TraceError(ex.ToString());

                lblAvisoPedidos.CssClass = "d-block text-danger mb-3";
                lblAvisoPedidos.Text =
                    "No pudimos actualizar el pedido. Intentá nuevamente.";
                lblAvisoPedidos.Visible = true;
                return;
            }
            catch (Exception ex)
            {
                lblAvisoPedidos.CssClass = "d-block text-danger mb-3";
                lblAvisoPedidos.Text = Server.HtmlEncode(ex.Message);
                lblAvisoPedidos.Visible = true;
                return;
            }

            CargarPedidos(usuario.Id);

            lblAvisoPedidos.CssClass = "d-block text-success mb-3";
            lblAvisoPedidos.Text = "Estado del pedido actualizado.";
            lblAvisoPedidos.Visible = true;
        }

        // Guardamos la observación escrita en la fila seleccionada.
        private void GuardarObservacionesFila(GridViewCommandEventArgs e)
        {
            Usuario usuario = Session["UsuarioLogueado"] as Usuario;

            if (usuario == null)
            {
                Response.Redirect("Login.aspx");
                return;
            }

            try
            {
                Control boton = (Control)e.CommandSource;
                GridViewRow fila = (GridViewRow)boton.NamingContainer;

                int idPedido =
                    (int)gvPedidos.DataKeys[fila.RowIndex].Values["Id"];

                TextBox texto =
                    (TextBox)fila.FindControl("txtObservaciones");

                PedidoNegocio negocio = new PedidoNegocio();

                negocio.guardarObservaciones(
                    idPedido, texto.Text, usuario.Id);
            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                System.Diagnostics.Trace.TraceError(ex.ToString());

                lblAvisoPedidos.CssClass = "d-block text-danger mb-3";
                lblAvisoPedidos.Text =
                    "No pudimos guardar la observación. Intentá nuevamente.";
                lblAvisoPedidos.Visible = true;
                return;
            }
            catch (Exception ex)
            {
                lblAvisoPedidos.CssClass = "d-block text-danger mb-3";
                lblAvisoPedidos.Text = Server.HtmlEncode(ex.Message);
                lblAvisoPedidos.Visible = true;
                return;
            }

            CargarPedidos(usuario.Id);

            lblAvisoPedidos.CssClass = "d-block text-success mb-3";
            lblAvisoPedidos.Text = "Observación guardada.";
            lblAvisoPedidos.Visible = true;
        }

        // Leemos y validamos los filtros antes de consultar.
        private List<Pedido> ObtenerPedidosFiltrados(int idAdministrador)
        {
            int idEstado = int.Parse(ddlEstadoFiltro.SelectedValue);
            int idCliente = 0;
            DateTime? desde = null;
            DateTime? hasta = null;

            if (idEstado < 0 || idEstado > 6)
            {
                throw new ArgumentException("Seleccioná un estado válido.");
            }

            if (!string.IsNullOrWhiteSpace(txtClienteFiltro.Text))
            {
                try
                {
                    idCliente = int.Parse(txtClienteFiltro.Text.Trim());
                }
                catch (FormatException)
                {
                    throw new ArgumentException(
                        "El ID del cliente debe ser un número entero.");
                }
                catch (OverflowException)
                {
                    throw new ArgumentException(
                        "El ID del cliente es demasiado grande.");
                }

                if (idCliente <= 0)
                {
                    throw new ArgumentException(
                        "El ID del cliente debe ser mayor que cero.");
                }
            }

            try
            {
                if (!string.IsNullOrWhiteSpace(txtDesdeFiltro.Text))
                {
                    desde = DateTime.ParseExact(
                        txtDesdeFiltro.Text,
                        "yyyy-MM-dd",
                        System.Globalization.CultureInfo.InvariantCulture);
                }

                if (!string.IsNullOrWhiteSpace(txtHastaFiltro.Text))
                {
                    hasta = DateTime.ParseExact(
                        txtHastaFiltro.Text,
                        "yyyy-MM-dd",
                        System.Globalization.CultureInfo.InvariantCulture);
                }
            }
            catch (FormatException)
            {
                throw new ArgumentException("Ingresá fechas válidas.");
            }

            // La columna Fecha de SQL utiliza el tipo datetime.
            DateTime fechaMinima = new DateTime(1753, 1, 1);
            DateTime fechaMaxima = new DateTime(9999, 12, 30);

            if ((desde.HasValue &&
                 (desde.Value < fechaMinima || desde.Value > fechaMaxima)) ||
                (hasta.HasValue &&
                 (hasta.Value < fechaMinima || hasta.Value > fechaMaxima)))
            {
                throw new ArgumentException(
                    "Las fechas deben estar entre 01/01/1753 y 30/12/9999.");
            }

            if (desde.HasValue && hasta.HasValue &&
                desde.Value > hasta.Value)
            {
                throw new ArgumentException(
                    "La fecha Desde no puede ser posterior a Hasta.");
            }

            PedidoNegocio negocio = new PedidoNegocio();

            return negocio.listarAdministrador(
                idAdministrador, idEstado, idCliente, desde, hasta);
        }

        // Consultamos usando los filtros seleccionados.
        protected void btnFiltrarPedidos_Click(object sender, EventArgs e)
        {
            Usuario usuario = Session["UsuarioLogueado"] as Usuario;

            if (usuario == null)
            {
                Response.Redirect("Login.aspx");
                return;
            }

            CargarPedidos(usuario.Id);
        }

        // Borramos los filtros y volvemos a mostrar todos los pedidos.
        protected void btnLimpiarFiltros_Click(object sender, EventArgs e)
        {
            ddlEstadoFiltro.SelectedValue = "0";
            txtClienteFiltro.Text = "";
            txtDesdeFiltro.Text = "";
            txtHastaFiltro.Text = "";

            btnFiltrarPedidos_Click(sender, e);
        }
    }
}
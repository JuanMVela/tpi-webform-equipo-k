using dominio;
using negocio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;

namespace TPIWebForm_equipo_K
{
    public partial class DetalleCompra : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Usuario usuario = Session["UsuarioLogueado"] as Usuario;

            if (usuario == null)
            {
                Response.Redirect("Login.aspx");
                return;
            }

            // Consultamos el pedido en cada solicitud.
            pnlPedido.Visible = false;
            lblAviso.Visible = false;

            int idPedido;

            try
            {
                idPedido = int.Parse(Request.QueryString["id"]);

                if (idPedido <= 0)
                {
                    lblAviso.Text = "El pedido indicado no es válido.";
                    lblAviso.Visible = true;
                    return;
                }
            }
            catch (FormatException)
            {
                lblAviso.Text = "El pedido indicado no es válido.";
                lblAviso.Visible = true;
                return;
            }
            catch (OverflowException)
            {
                lblAviso.Text = "El pedido indicado no es válido.";
                lblAviso.Visible = true;
                return;
            }
            catch (ArgumentNullException)
            {
                lblAviso.Text = "No se indicó un pedido.";
                lblAviso.Visible = true;
                return;
            }

            try
            {
                PedidoNegocio negocio = new PedidoNegocio();
                Pedido pedido = negocio.obtenerPorCliente(idPedido, usuario.Id);

                if (pedido == null)
                {
                    lblAviso.Text = "El pedido no está disponible.";
                    lblAviso.Visible = true;
                    return;
                }

                lblPedido.Text = "Pedido #" + pedido.Id;
                lblFecha.Text = pedido.FechaCreacion.ToString("dd/MM/yyyy HH:mm");
                lblEstado.Text = Server.HtmlEncode(pedido.NombreEstado);
                lblTotal.Text = pedido.Total.ToString("C");
                // Mostramos las opciones elegidas al comprar.
                lblFormaPago.Text =
                    Server.HtmlEncode(pedido.FormaPago.Nombre);

                lblFormaEntrega.Text =
                    Server.HtmlEncode(pedido.FormaEntrega.Nombre);

                // Mostramos el domicilio cuando el pedido tiene dirección.
                pnlDireccionPedido.Visible = false;

                if (pedido.Direccion != null)
                {
                    string direccion =
                        pedido.Direccion.Calle + " " +
                        pedido.Direccion.Numero + " — " +
                        pedido.Direccion.Localidad + " · CP " +
                        pedido.Direccion.CodigoPostal;

                    lblDireccionPedido.Text = Server.HtmlEncode(direccion);
                    pnlDireccionPedido.Visible = true;
                }

                // Cargamos los productos y precios de esta compra.
                var detalles = negocio.listarDetallesPorCliente(
                    pedido.Id, usuario.Id);

                repProductos.DataSource = detalles;
                repProductos.DataBind();

                // Permitimos preparar la carga mientras el pedido esté pendiente.
                CargarEstadoComprobante(pedido, usuario.Id);

                // Adaptamos el seguimiento según la forma de entrega.
                lblEtapaDespacho.Text = "Enviado";

                if (!pedido.FormaEntrega.RequiereDireccion)
                {
                    lblEtapaDespacho.Text = "Listo para retirar";

                    if (pedido.IdEstado == 4)
                    {
                        lblEstado.Text = "Listo para retirar";
                    }
                }
                MostrarSeguimiento(pedido.IdEstado);
                pnlPedido.Visible = true;

            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.TraceError(ex.ToString());

                lblAviso.Text =
                    "No pudimos cargar el pedido. Intentá nuevamente.";
                lblAviso.Visible = true;
            }
        }

        // Resaltamos el estado actual y las etapas anteriores.
        private void MostrarSeguimiento(int idEstado)
        {
            var pasos = new System.Web.UI.HtmlControls.HtmlGenericControl[]
            {
        pasoPendiente,
        pasoPagado,
        pasoPreparacion,
        pasoEnviado,
        pasoEntregado
            };

            // Limpiamos los colores y la marca de etapa actual.
            foreach (var paso in pasos)
            {
                paso.Attributes.Remove("class");
                paso.Attributes.Remove("aria-current");
            }

            pnlSeguimiento.Visible = false;
            lblEstado.CssClass = "fw-bold";

            if (idEstado == 6)
            {
                lblEstado.CssClass = "fw-bold text-danger";
                return;
            }

            if (idEstado < 1 || idEstado > 5)
            {
                return;
            }

            pnlSeguimiento.Visible = true;

            for (int i = 0; i < pasos.Length; i++)
            {
                int numeroPaso = i + 1;

                if (numeroPaso < idEstado)
                {
                    pasos[i].Attributes["class"] = "completado";
                }
                else if (numeroPaso == idEstado)
                {
                    pasos[i].Attributes["class"] = "actual";
                    pasos[i].Attributes["aria-current"] = "step";
                }
            }
        }
        // Guardamos el comprobante del usuario para este pedido.
        protected void btnSubirComprobante_Click(object sender, EventArgs e)
        {
            lblAvisoComprobante.Visible = false;
            lblAvisoComprobante.CssClass = "d-block text-danger mt-2";

            Usuario usuario = Session["UsuarioLogueado"] as Usuario;

            if (usuario == null)
            {
                Response.Redirect("Login.aspx");
                return;
            }

            // Page_Load muestra el panel solo para un pedido del usuario.
            if (!pnlPedido.Visible ||
    !pnlCargaComprobante.Visible ||
    !btnSubirComprobante.Enabled)
            {
                lblAviso.Text = "El pedido no está disponible para cargar archivos.";
                lblAviso.Visible = true;
                return;
            }

            if (!fuComprobante.HasFile)
            {
                lblAvisoComprobante.Text = "Seleccioná un archivo.";
                lblAvisoComprobante.Visible = true;
                return;
            }

            if (fuComprobante.PostedFile.ContentLength > 2 * 1024 * 1024)
            {
                lblAvisoComprobante.Text =
                    "El comprobante no puede superar los 2 MB.";
                lblAvisoComprobante.Visible = true;
                return;
            }

            try
            {
                int idPedido = int.Parse(Request.QueryString["id"]);

                ComprobantePago comprobante = new ComprobantePago();

                comprobante.IdPedido = idPedido;
                comprobante.NombreArchivo =
                    Path.GetFileName(fuComprobante.FileName);
                comprobante.Archivo = fuComprobante.FileBytes;

                ComprobantePagoNegocio negocio = new ComprobantePagoNegocio();
                negocio.agregar(comprobante, usuario.Id);
            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                System.Diagnostics.Trace.TraceError(ex.ToString());

                lblAvisoComprobante.Text =
                    "No pudimos guardar el comprobante. Intentá nuevamente.";
                lblAvisoComprobante.Visible = true;
                return;
            }
            catch (Exception ex)
            {
                lblAvisoComprobante.Text = Server.HtmlEncode(ex.Message);
                lblAvisoComprobante.Visible = true;
                return;
            }

            lblAvisoComprobante.CssClass = "d-block text-success mt-2";
            lblAvisoComprobante.Text =
                "Comprobante cargado. Está pendiente de revisión.";

            lblAvisoComprobante.Visible = true;
            fuComprobante.Enabled = false;
            btnSubirComprobante.Enabled = false;
        }

        // Mostramos el comprobante y decidimos si permitimos otra carga.
        private void CargarEstadoComprobante(Pedido pedido, int idCliente)
        {
            ComprobantePagoNegocio negocio = new ComprobantePagoNegocio();

            var comprobante = negocio.obtenerUltimoPorCliente(
                pedido.Id, idCliente);

            bool permiteCarga = pedido.IdEstado == 1;

            lblComprobanteActual.Visible = false;

            if (comprobante != null)
            {
                lblComprobanteActual.Text = Server.HtmlEncode(
                    "Archivo: " + comprobante.NombreArchivo +
                    " — Estado: " + comprobante.Estado);

                lblComprobanteActual.Visible = true;

                // Solo permitimos reemplazar un comprobante rechazado.
                permiteCarga = permiteCarga &&
                    comprobante.Estado == "Rechazado";
            }

            pnlCargaComprobante.Visible =
                pedido.IdEstado == 1 || comprobante != null;

            fuComprobante.Enabled = permiteCarga;
            btnSubirComprobante.Enabled = permiteCarga;
        }
    }
}
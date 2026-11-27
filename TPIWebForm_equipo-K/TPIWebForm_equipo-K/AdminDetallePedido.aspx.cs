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
    public partial class AdminDetallePedido : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Usuario usuario = Session["UsuarioLogueado"] as Usuario;

            if (usuario == null)
            {
                Response.Redirect("Login.aspx");
                return;
            }

            pnlPedido.Visible = false;
            lblAviso.Visible = false;

            try
            {
                // Comprobamos el permiso actual en cada solicitud.
                UsuarioNegocio usuarios = new UsuarioNegocio();

                if (!usuarios.esAdministradorActivo(usuario.Id))
                {
                    Response.Redirect("Inicio.aspx", false);
                    Context.ApplicationInstance.CompleteRequest();
                    return;
                }

                int idPedido;

                try
                {
                    idPedido = int.Parse(Request.QueryString["id"]);
                }
                catch (Exception ex) when (
                    ex is FormatException ||
                    ex is OverflowException ||
                    ex is ArgumentNullException)
                {
                    lblAviso.Text = "El pedido indicado no es válido.";
                    lblAviso.Visible = true;
                    return;
                }

                if (idPedido <= 0)
                {
                    lblAviso.Text = "El pedido indicado no es válido.";
                    lblAviso.Visible = true;
                    return;
                }

                PedidoNegocio negocio = new PedidoNegocio();

                Pedido pedido =
                    negocio.obtenerAdministrador(idPedido, usuario.Id);

                if (pedido == null)
                {
                    lblAviso.Text = "El pedido no está disponible.";
                    lblAviso.Visible = true;
                    return;
                }

                // Mostramos los datos generales.
                lblPedido.Text = "Pedido #" + pedido.Id;
                lblFecha.Text =
                    pedido.FechaCreacion.ToString("dd/MM/yyyy HH:mm");
                lblEstado.Text = Server.HtmlEncode(pedido.NombreEstado);
                if (pedido.IdEstado == 4 &&
    !pedido.FormaEntrega.RequiereDireccion)
                {
                    lblEstado.Text = "Listo para retirar";
                }
                lblPago.Text = Server.HtmlEncode(pedido.FormaPago.Nombre);
                lblEntrega.Text =
                    Server.HtmlEncode(pedido.FormaEntrega.Nombre);
                lblTotal.Text = pedido.Total.ToString("C");
                if (string.IsNullOrWhiteSpace(pedido.ObservacionesInternas))
                {
                    lblObservaciones.Text = "Sin observaciones.";
                }
                else
                {
                    lblObservaciones.Text =
                        Server.HtmlEncode(pedido.ObservacionesInternas);
                }

                pnlDireccion.Visible = false;

                if (pedido.Direccion != null)
                {
                    string direccion =
                        pedido.Direccion.Calle + " " +
                        pedido.Direccion.Numero + " — " +
                        pedido.Direccion.Localidad + " · CP " +
                        pedido.Direccion.CodigoPostal;

                    lblDireccion.Text = Server.HtmlEncode(direccion);
                    pnlDireccion.Visible = true;
                }

                // Los productos ya vienen cargados desde negocio.
                repProductos.DataSource = pedido.Detalles;
                repProductos.DataBind();

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
    }
}
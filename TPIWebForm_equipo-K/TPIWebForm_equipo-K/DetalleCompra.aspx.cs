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

                // Cargamos los productos y precios de esta compra.
                var detalles = negocio.listarDetallesPorCliente(
                    pedido.Id, usuario.Id);

                repProductos.DataSource = detalles;
                repProductos.DataBind();

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
    }
}
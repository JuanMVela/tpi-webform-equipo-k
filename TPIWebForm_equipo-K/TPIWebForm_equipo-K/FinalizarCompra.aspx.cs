using negocio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace TPIWebForm_equipo_K
{
    public partial class FinalizarCompra : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Recuperamos el carrito de la sesión.
            var carrito = Session["Carrito"] as Dictionary<int, int>;

            if (carrito == null || carrito.Count == 0)
            {
                Response.Redirect("Carrito.aspx");
                return;
            }

            if (!IsPostBack)
            {
                // Cargamos las formas de entrega activas.
                FormaEntregaNegocio entregaNegocio = new FormaEntregaNegocio();
                ddlEntrega.DataSource = entregaNegocio.listar().FindAll(forma => forma.Activo);
                ddlEntrega.DataTextField = "Nombre";
                ddlEntrega.DataValueField = "Id";
                ddlEntrega.DataBind();
                ddlEntrega.Items.Insert(0, new ListItem("Seleccioná una forma de entrega", ""));

                // Cargamos las formas de pago activas.
                FormaPagoNegocio pagoNegocio = new FormaPagoNegocio();
                ddlPago.DataSource = pagoNegocio.listar().FindAll(forma => forma.Activo);
                ddlPago.DataTextField = "Nombre";
                ddlPago.DataValueField = "Id";
                ddlPago.DataBind();
                ddlPago.Items.Insert(0, new ListItem("Seleccioná una forma de pago", ""));

                ddlPago.DataBind();
                ddlPago.Items.Insert(0, new ListItem("Seleccioná una forma de pago", ""));

                CargarResumen(carrito);
            }
        }

        protected void ddlEntrega_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Ocultamos el bloque hasta comprobar la opción elegida.
            pnlDireccion.Visible = false;

            int idEntrega;
            if (!int.TryParse(ddlEntrega.SelectedValue, out idEntrega))
            {
                return;
            }

            FormaEntregaNegocio negocio = new FormaEntregaNegocio();
            var entrega = negocio.listar().Find(forma => forma.Id == idEntrega && forma.Activo);

            if (entrega != null)
            {
                pnlDireccion.Visible = entrega.RequiereDireccion;
            }
        }

        private void CargarResumen(Dictionary<int, int> carrito)
        {
            ProductoNegocio negocio = new ProductoNegocio();
            var productos = negocio.listarActivo();
            var filas = new List<object>();
            decimal subtotalCompra = 0;

            // La sesión aporta la cantidad y negocio aporta el precio.
            foreach (var item in carrito)
            {
                var producto = productos.Find(p => p.Id == item.Key);

                if (producto != null)
                {
                    decimal subtotal = producto.Precio * item.Value;

                    filas.Add(new
                    {
                        Nombre = producto.Nombre,
                        Cantidad = item.Value,
                        Precio = producto.Precio,
                        Subtotal = subtotal
                    });

                    subtotalCompra += subtotal;
                }
            }

            repResumen.DataSource = filas;
            repResumen.DataBind();
            lblSubtotalCompra.Text = subtotalCompra.ToString("C");

            if (filas.Count < carrito.Count)
            {
                lblAvisoResumen.Text = "Hay productos que ya no están disponibles. Revisá el carrito.";
                lblAvisoResumen.Visible = true;
            }
        }
    }
}
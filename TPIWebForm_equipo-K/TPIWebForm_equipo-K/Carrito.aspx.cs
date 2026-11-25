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
    public partial class Carrito : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CargarCarrito();
            }
        }

        // Cargamos los productos y recalculamos el total.
        private void CargarCarrito()
        {
            // Ocultamos el aviso, la grilla y el total antes de volver a cargarlos.
            lblAviso.Visible = false;
            dgvCarrito.Visible = false;
            pnlResumen.Visible = false;

            Dictionary<int, int> carrito = Session["Carrito"] as Dictionary<int, int>;

            if (carrito == null || carrito.Count == 0)
            {
                lblAviso.Text = "Tu carrito está vacío.";
                lblAviso.Visible = true;
                return;
            }

            ProductoNegocio negocio = new ProductoNegocio();
            List<Producto> productos = negocio.listarActivo();
            var filas = new List<object>();
            decimal total = 0;
            bool hayProductosNoDisponibles = false;

            foreach (KeyValuePair<int, int> item in carrito)
            {
                Producto producto = productos.Find(p => p.Id == item.Key);

                if (producto == null)
                {
                    hayProductosNoDisponibles = true;
                }
                else
                {
                    decimal subtotal = producto.Precio * item.Value;

                    // Usamos la primera imagen para la miniatura.
                    var imagenes = negocio.listarImagenes(producto.Id);
                    string imagenUrl = imagenes.Count > 0 ? imagenes[0].ImagenUrl : "";

                    filas.Add(new
                    {
                        Id = producto.Id,
                        Nombre = producto.Nombre,
                        ImagenUrl = imagenUrl,
                        Cantidad = item.Value,
                        Precio = producto.Precio,
                        Subtotal = subtotal
                    });

                    total += subtotal;
                }
            }

            if (hayProductosNoDisponibles)
            {
                lblAviso.Text = "Algunos productos del carrito ya no están disponibles.";
                lblAviso.Visible = true;
            }

            dgvCarrito.DataSource = filas;
            dgvCarrito.DataBind();
            dgvCarrito.Visible = filas.Count > 0;

            lblTotal.Text = total.ToString("C");
            pnlResumen.Visible = filas.Count > 0;
        }

        protected void dgvCarrito_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName != "SumarUnidad" &&
                e.CommandName != "RestarUnidad" &&
                e.CommandName != "QuitarProducto")
            {
                return;
            }

            // Recuperamos el ID de la fila y el carrito de la sesión.
            int id = int.Parse(e.CommandArgument.ToString());
            Dictionary<int, int> carrito = Session["Carrito"] as Dictionary<int, int>;

            if (carrito == null || !carrito.ContainsKey(id))
            {
                CargarCarrito();
                return;
            }

            if (e.CommandName == "QuitarProducto")
            {
                carrito.Remove(id);
            }
            else if (e.CommandName == "RestarUnidad")
            {
                if (carrito[id] > 1)
                {
                    carrito[id]--;
                }
            }
            else
            {
                // Consultamos el stock actual antes de sumar.
                ProductoNegocio negocio = new ProductoNegocio();
                Producto producto = negocio.listarActivo().Find(p => p.Id == id);

                if (producto == null || carrito[id] >= producto.Stock)
                {
                    CargarCarrito();
                    lblAviso.Text = producto == null
                        ? "El producto ya no está disponible."
                        : "No hay más stock disponible para este producto.";
                    lblAviso.Visible = true;
                    return;
                }

                carrito[id]++;
            }

            // Guardamos el cambio y recalculamos el total.
            Session["Carrito"] = carrito;
            CargarCarrito();
        }

        // Comprobamos disponibilidad antes de continuar con la compra.
        protected void btnContinuarCompra_Click(object sender, EventArgs e)
        {
            lblAviso.Visible = false;
            lblAviso.CssClass = "d-block mb-3 text-danger";

            var carrito = Session["Carrito"] as Dictionary<int, int>;

            if (carrito == null || carrito.Count == 0)
            {
                CargarCarrito();
                return;
            }

            try
            {
                ProductoNegocio negocio = new ProductoNegocio();
                var productos = negocio.listarActivo();

                foreach (var item in carrito)
                {
                    Producto producto = productos.Find(p => p.Id == item.Key);

                    if (producto == null)
                    {
                        lblAviso.Text =
                            "Un producto ya no está disponible. Revisá el carrito.";
                        lblAviso.Visible = true;
                        return;
                    }

                    if (item.Value <= 0)
                    {
                        lblAviso.Text =
                            "Hay una cantidad inválida. Revisá el carrito.";
                        lblAviso.Visible = true;
                        return;
                    }

                    if (item.Value > producto.Stock)
                    {
                        lblAviso.Text =
                            "No hay stock suficiente de "
                            + Server.HtmlEncode(producto.Nombre)
                            + ". Solicitaste " + item.Value
                            + " y hay " + producto.Stock
                            + " disponibles. Reducí la cantidad para continuar.";

                        lblAviso.Visible = true;
                        return;
                    }
                }
            }
            catch (Exception)
            {
                lblAviso.Text =
                    "No pudimos comprobar el stock. Intentá nuevamente.";
                lblAviso.Visible = true;
                return;
            }

            Response.Redirect("FinalizarCompra.aspx");
        }
    }
}
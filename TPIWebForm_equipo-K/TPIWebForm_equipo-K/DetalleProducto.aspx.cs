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
    public partial class DetalleProducto : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack)
            {
                return;
            }

            // Leemos el ID que llega desde el catálogo.
            int id;
            if (!int.TryParse(Request.QueryString["id"], out id) || id <= 0)
            {
                lblAviso.Text = "El código del producto no es válido.";
                return;
            }

            // Buscamos el producto entre los que están activos.
            ProductoNegocio negocio = new ProductoNegocio();
            Producto producto = negocio.listarActivo().Find(item => item.Id == id);

            if (producto == null)
            {
                lblAviso.Text = "El producto no está disponible.";
                return;
            }

            litNombre.Text = producto.Nombre;
            litCategoria.Text = producto.Categoria.Nombre;
            litMarca.Text = producto.Marca.Nombre;
            litDescripcion.Text = producto.Descripcion;
            litPrecio.Text = producto.Precio.ToString("C");
            bool hayStock = producto.Stock > 0;
            litStock.Text = hayStock ? producto.Stock.ToString() : "Sin stock";
            txtCantidad.Enabled = hayStock;
            btnAgregarCarrito.Enabled = hayStock;

            // Cargamos la imagen principal y las miniaturas.
            var imagenes = negocio.listarImagenes(producto.Id);

            if (imagenes.Count > 0)
            {
                imgProducto.ImageUrl = imagenes[0].ImagenUrl;
                imgProducto.AlternateText = producto.Nombre;
                imgProducto.Visible = true;
            }

            repMiniaturas.DataSource = imagenes;
            repMiniaturas.DataBind();

            pnlGaleria.Visible = imagenes.Count > 1;

            pnlDetalle.Visible = true;
        }
        protected void btnAgregarCarrito_Click(object sender, EventArgs e)
        {
            lblCarrito.CssClass = "d-block mt-2 text-danger";

            // Convertimos la cantidad a un número entero.
            int cantidad;
            try
            {
                cantidad = int.Parse(txtCantidad.Text);
            }
            catch (FormatException)
            {
                lblCarrito.Text = "Ingresá una cantidad entera.";
                return;
            }
            catch (OverflowException)
            {
                lblCarrito.Text = "La cantidad ingresada es demasiado grande.";
                return;
            }

            if (cantidad < 1)
            {
                lblCarrito.Text = "Ingresá una cantidad de 1 o más.";
                return;
            }

            // Leemos el ID del producto desde la URL.
            int id;
            try
            {
                id = int.Parse(Request.QueryString["id"] ?? "");
            }
            catch (FormatException)
            {
                lblCarrito.Text = "El identificador del producto no es válido.";
                return;
            }
            catch (OverflowException)
            {
                lblCarrito.Text = "El identificador del producto no es válido.";
                return;
            }

            if (id <= 0)
            {
                lblCarrito.Text = "El identificador del producto no es válido.";
                return;
            }

            // Consultamos el producto activo y su stock actual.
            ProductoNegocio negocio = new ProductoNegocio();
            Producto producto = negocio.listarActivo().Find(item => item.Id == id);

            if (producto == null)
            {
                lblCarrito.Text = "El producto ya no está disponible.";
                return;
            }

            // Recuperamos el carrito de la sesión o creamos uno vacío.
            Dictionary<int, int> carrito =
                Session["Carrito"] as Dictionary<int, int> ?? new Dictionary<int, int>();

            int cantidadActual;
            carrito.TryGetValue(id, out cantidadActual);

            // Revisamos cuánto stock queda para agregar.
            if (cantidad > producto.Stock - cantidadActual)
            {
                lblCarrito.Text = "La cantidad total supera el stock disponible.";
                return;
            }

            carrito[id] = cantidadActual + cantidad;
            Session["Carrito"] = carrito;

            lblCarrito.CssClass = "d-block mt-2 text-success";
            lblCarrito.Text = "Producto agregado. Unidades en el carrito: " + carrito[id];
        }
    }
}
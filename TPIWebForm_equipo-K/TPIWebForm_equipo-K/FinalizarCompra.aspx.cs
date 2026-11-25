using negocio;
using dominio;
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

            // Pedimos iniciar sesión para continuar con la compra.
            if (Session["UsuarioLogueado"] == null)
            {
                Response.Redirect("Login.aspx?origen=compra");
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

                // Recuperamos las direcciones del usuario que ingresó.
                Usuario usuario = (Usuario)Session["UsuarioLogueado"];
                CargarDirecciones(usuario.Id);

                // Recuperamos las opciones después de administrar direcciones.
                if (Request.QueryString["origen"] == "direcciones")
                {
                    RestaurarOpcionesCompra(usuario.Id);
                }

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

        // Cargamos los domicilios disponibles del usuario.
        // Cargamos las direcciones del usuario para elegir una.
        private void CargarDirecciones(int idUsuario)
        {
            rblDirecciones.Items.Clear();
            lblSinDirecciones.Visible = false;

            try
            {
                DireccionNegocio negocio = new DireccionNegocio();
                var direcciones = negocio.listarPorUsuario(idUsuario);

                foreach (Direccion direccion in direcciones)
                {
                    string descripcion = direccion.Calle + " " + direccion.Numero
                        + " — " + direccion.Localidad
                        + " · CP " + direccion.CodigoPostal;

                    // Codificamos el texto antes de mostrarlo.
                    rblDirecciones.Items.Add(new ListItem(
                        Server.HtmlEncode(descripcion),
                        direccion.Id.ToString()
                    ));
                }

                if (direcciones.Count == 0)
                {
                    lblSinDirecciones.Text =
                        "Todavía no tenés direcciones guardadas.";
                    lblSinDirecciones.Visible = true;
                }
            }
            catch (Exception)
            {
                lblSinDirecciones.Text =
                    "No pudimos cargar tus direcciones. Volvé a intentar.";
                lblSinDirecciones.Visible = true;
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

        // Guardamos las opciones antes de agregar una dirección.
        protected void btnAgregarDireccion_Click(object sender, EventArgs e)
        {
            Usuario usuario = Session["UsuarioLogueado"] as Usuario;

            if (usuario == null)
            {
                Response.Redirect("Login.aspx?origen=compra");
                return;
            }

            var opciones = new Dictionary<string, string>
    {
        { "Entrega", ddlEntrega.SelectedValue },
        { "Pago", ddlPago.SelectedValue },
        { "Direccion", rblDirecciones.SelectedValue }
    };

            // La clave identifica al usuario dueño de estas opciones.
            Session["OpcionesCompra_" + usuario.Id] = opciones;

            Response.Redirect("MisDirecciones.aspx?origen=compra");
        }

        // Recuperamos las opciones al volver a la compra.
        private void RestaurarOpcionesCompra(int idUsuario)
        {
            string clave = "OpcionesCompra_" + idUsuario;

            var opciones = Session[clave] as Dictionary<string, string>;

            if (opciones == null)
            {
                return;
            }

            // Restauramos solo opciones que siguen disponibles.
            ListItem entrega = ddlEntrega.Items.FindByValue(opciones["Entrega"]);
            if (entrega != null)
            {
                ddlEntrega.SelectedValue = entrega.Value;
            }

            ListItem pago = ddlPago.Items.FindByValue(opciones["Pago"]);
            if (pago != null)
            {
                ddlPago.SelectedValue = pago.Value;
            }

            ListItem direccion =
                rblDirecciones.Items.FindByValue(opciones["Direccion"]);

            if (direccion != null)
            {
                rblDirecciones.SelectedValue = direccion.Value;
            }

            // Mostramos las direcciones si la entrega lo requiere.
            ddlEntrega_SelectedIndexChanged(this, EventArgs.Empty);

            Session.Remove(clave);
        }
        // Validamos las opciones antes de preparar la confirmación.
        protected void btnContinuar_Click(object sender, EventArgs e)
        {
            lblAvisoCompra.Visible = false;

            Usuario usuario = Session["UsuarioLogueado"] as Usuario;

            if (usuario == null)
            {
                Response.Redirect("Login.aspx?origen=compra");
                return;
            }

            Page.Validate("Compra");

            if (!Page.IsValid)
            {
                return;
            }

            try
            {
                // Recuperamos el carrito actual.
                var carrito = Session["Carrito"] as Dictionary<int, int>;

                if (carrito == null || carrito.Count == 0)
                {
                    lblAvisoCompra.Text = "El carrito está vacío.";
                    lblAvisoCompra.Visible = true;
                    return;
                }

                // Consultamos los productos actuales de la base.
                ProductoNegocio productoNegocio = new ProductoNegocio();
                var productos = productoNegocio.listarActivo();

                foreach (var item in carrito)
                {
                    var producto = productos.Find(p => p.Id == item.Key);

                    if (producto == null)
                    {
                        lblAvisoCompra.Text =
                            "Hay un producto que ya no está disponible. Revisá el carrito.";
                        lblAvisoCompra.Visible = true;
                        return;
                    }

                    if (item.Value <= 0)
                    {
                        lblAvisoCompra.Text =
                            "Hay una cantidad inválida. Revisá el carrito.";
                        lblAvisoCompra.Visible = true;
                        return;
                    }

                    if (item.Value > producto.Stock)
                    {
                        lblAvisoCompra.Text =
                            "No hay stock suficiente de "
                            + Server.HtmlEncode(producto.Nombre)
                            + ". Stock disponible: " + producto.Stock + ".";

                        lblAvisoCompra.Visible = true;
                        return;
                    }
                }

                // Actualizamos el resumen con los precios actuales.
                CargarResumen(carrito);
                int idEntrega = int.Parse(ddlEntrega.SelectedValue);
                int idPago = int.Parse(ddlPago.SelectedValue);

                // Comprobamos que las opciones sigan activas.
                FormaEntregaNegocio entregaNegocio = new FormaEntregaNegocio();
                var entrega = entregaNegocio.listar().Find(
                    item => item.Id == idEntrega && item.Activo);

                FormaPagoNegocio pagoNegocio = new FormaPagoNegocio();
                var pago = pagoNegocio.listar().Find(
                    item => item.Id == idPago && item.Activo);

                if (entrega == null || pago == null)
                {
                    lblAvisoCompra.Text =
                        "La entrega o el pago ya no están disponibles. Recargá la página.";
                    lblAvisoCompra.Visible = true;
                    return;
                }

                // La base determina si hace falta un domicilio.
                pnlDireccion.Visible = entrega.RequiereDireccion;

                Page.Validate("Compra");

                if (!Page.IsValid)
                {
                    return;
                }

                if (entrega.RequiereDireccion)
                {
                    int idDireccion = int.Parse(rblDirecciones.SelectedValue);

                    DireccionNegocio direccionNegocio = new DireccionNegocio();
                    var direccion = direccionNegocio.listarPorUsuario(usuario.Id)
                        .Find(item => item.Id == idDireccion && item.Activo);

                    if (direccion == null)
                    {
                        lblAvisoCompra.Text =
                            "La dirección seleccionada ya no está disponible.";
                        lblAvisoCompra.Visible = true;
                        return;
                    }
                }
            }
            catch (Exception)
            {
                lblAvisoCompra.CssClass = "d-block text-danger mt-3";
                lblAvisoCompra.Text =
                    "No pudimos validar las opciones. Recargá la página e intentá nuevamente.";
                lblAvisoCompra.Visible = true;
                return;
            }

            // Mensaje provisional mientras armamos la confirmación.
            lblAvisoCompra.CssClass = "d-block text-success mt-3";
            lblAvisoCompra.Text =
    "Los datos de la compra son válidos. Todavía no se generó el pedido.";
            lblAvisoCompra.Visible = true;
        }
    }
}
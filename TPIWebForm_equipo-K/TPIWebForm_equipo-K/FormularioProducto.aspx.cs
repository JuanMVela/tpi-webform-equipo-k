using dominio;
using negocio;
using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;

namespace TPIWebForm_equipo_K
{
    public partial class FormularioProducto : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Session["ProductoModificar"] = null;
                CargarCombos();

                string id = Request.QueryString["id"];

                if (id != null)
                {
                    // MODO MODIFICACIÓN
                    tituloFormulario.InnerText = "Modificar Producto";

                    int idProducto = int.Parse(id);
                    ProductoNegocio negocio = new ProductoNegocio();
                    Producto seleccionado = negocio.listar().Find(x => x.Id == idProducto);

                    if (seleccionado != null)
                    {
                        // Precargamos los TextBox
                        txtCodigo.Text = seleccionado.Codigo;
                        txtNombre.Text = seleccionado.Nombre;
                        txtDescripcion.Text = seleccionado.Descripcion;
                        txtPrecio.Text = seleccionado.Precio.ToString();
                        txtStock.Text = seleccionado.Stock.ToString();

                        // Precargamos los Combos
                        ddlCategoria.SelectedValue = seleccionado.Categoria.Id.ToString();
                        ddlMarca.SelectedValue = seleccionado.Marca.Id.ToString();

                        // Guardamos el producto en sesión para usarlo en el botón Aceptar
                        Session["ProductoModificar"] = seleccionado;

                        List<Imagen> imagenes = negocio.listarImagenes(seleccionado.Id);
                        foreach (Imagen img in imagenes)
                        {
                            lbImagenes.Items.Add(img.ImagenUrl);
                        }
                    }
                }
                else
                {
                    // MODO ALTA
                    tituloFormulario.InnerText = "Agregar Producto";
                }
            }
        }
        private void CargarCombos()
        {
            CategoriaNegocio negCat = new CategoriaNegocio();
            MarcaNegocio negMar = new MarcaNegocio();

            ddlCategoria.DataSource = negCat.listar();
            ddlCategoria.DataTextField = "Nombre";
            ddlCategoria.DataValueField = "Id";
            ddlCategoria.DataBind();

            ddlMarca.DataSource = negMar.listar();
            ddlMarca.DataTextField = "Nombre";
            ddlMarca.DataValueField = "Id";
            ddlMarca.DataBind();
        }
        protected void btnAceptar_Click(object sender, EventArgs e)
        {
            try
            {
                Producto nuevo = new Producto();
                // Si venimos de modificar, traemos el objeto de la sesión
                if (Session["ProductoModificar"] != null)
                {
                    nuevo = (Producto)Session["ProductoModificar"];
                }

                ProductoNegocio negocio = new ProductoNegocio();

                string codigo = txtCodigo.Text.Trim();

                // Verificamos que se haya ingresado el código
                if (codigo == "")
                {
                    lblErrorCodigo.Text = "Ingresá el código del producto.";
                    lblErrorCodigo.Visible = true;
                    return;
                }

                lblErrorCodigo.Text = "Ya existe un producto con ese código. Ingresá otro.";
            

                // Buscamos si otro producto ya usa el código ingresado
                Producto encontrado = negocio.listar().Find(producto =>
                    producto.Id != nuevo.Id && producto.Codigo.ToUpper() == codigo.ToUpper());

                bool codigoRepetido = encontrado != null;

                lblErrorCodigo.Visible = codigoRepetido;

                if (codigoRepetido)
                {
                    return;
                }
                // Verificamos que el nombre del producto esté cargado
                string nombre = txtNombre.Text.Trim();
                lblErrorNombre.Visible = false;

                if (nombre == "")
                {
                    lblErrorNombre.Visible = true;
                    return;
                }

                // Verificamos que la descripción del producto esté cargada
                string descripcion = txtDescripcion.Text.Trim();
                lblErrorDescripcion.Visible = false;

                if (descripcion == "")
                {
                    lblErrorDescripcion.Visible = true;
                    return;
                }
                // Validamos que el precio esté cargado
                lblErrorPrecio.Visible = false;

                if (string.IsNullOrWhiteSpace(txtPrecio.Text))
                {
                    lblErrorPrecio.Text = "Ingresá el precio del producto.";
                    lblErrorPrecio.Visible = true;
                    return;
                }
                // Verificamos que el precio sea un valor numérico
                decimal precio;
                try
                {
                    precio = decimal.Parse(txtPrecio.Text);
                }
                catch (FormatException)
                {
                    lblErrorPrecio.Text = "Ingresá un valor numérico.";
                    lblErrorPrecio.Visible = true;
                    return;
                }

                // Verificamos que se haya ingresado el stock
                lblErrorStock.Visible = false;

                if (string.IsNullOrWhiteSpace(txtStock.Text))
                {
                    lblErrorStock.Visible = true;
                    return;
                }
                //Verificamos que el stock sea un número entero
                int stock;
                try
                {
                    stock = int.Parse(txtStock.Text);
                }
                catch (FormatException)
                {
                    lblErrorStock.Text = "Ingresá un número entero.";
                    lblErrorStock.Visible = true;
                    return;
                }

                // Verificamos que el stock sea cero o mayor
                if (stock < 0)
                {
                    lblErrorStock.Text = "El stock debe ser cero o mayor.";
                    lblErrorStock.Visible = true;
                    return;
                }

                // Verificamos que el precio sea mayor que cero
                if (precio <= 0)
                {
                    lblErrorPrecio.Text = "El precio debe ser mayor que cero.";
                    lblErrorPrecio.Visible = true;
                    return;
                }
                // Mapeamos los datos del formulario
                nuevo.Codigo = codigo;
                nuevo.Nombre = nombre;
                nuevo.Descripcion = txtDescripcion.Text;
                nuevo.Precio = precio;
                nuevo.Stock = int.Parse(txtStock.Text);
                nuevo.Categoria = new Categoria { Id = int.Parse(ddlCategoria.SelectedValue) };
                nuevo.Marca = new Marca { Id = int.Parse(ddlMarca.SelectedValue) };

                if (nuevo.Imagenes == null)
                {
                    nuevo.Imagenes = new List<Imagen>();
                }
                else
                {
                    nuevo.Imagenes.Clear();
                }

                foreach (ListItem item in lbImagenes.Items)
                {
                    Imagen img = new Imagen();
                    img.ImagenUrl = item.Text;
                    nuevo.Imagenes.Add(img);
                }


                // Si es un producto nuevo (Id == 0), lo agregamos
                if (nuevo.Id == 0)
                {
                    if (!string.IsNullOrWhiteSpace(txtUrlImagen.Text))
                    {
                        Imagen img = new Imagen();
                        img.ImagenUrl = txtUrlImagen.Text;
                        nuevo.Imagenes.Add(img);
                    }
                    negocio.agregar(nuevo);
                }
                else
                {
                    negocio.modificar(nuevo);
                }

                Session["ProductoModificar"] = null; // Limpiamos la sesión
                Response.Redirect("AdminProductos.aspx"); // Volvemos al listado
            }
            catch (Exception ex)
            {
                Response.Write("<script>alert('Error: " + ex.Message + "');</script>");
            }
        }
        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            Session["ProductoModificar"] = null;
            Response.Redirect("AdminProductos.aspx");
        }
        protected void btnAgregarImagen_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtUrlImagen.Text))
            {
                // Agregamos la URL al ListBox
                lbImagenes.Items.Add(txtUrlImagen.Text);

                // Limpiamos el TextBox
                txtUrlImagen.Text = "";
            }
        }
    }
}
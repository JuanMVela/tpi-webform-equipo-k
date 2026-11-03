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

                // Mapeamos los datos del formulario
                nuevo.Codigo = txtCodigo.Text;
                nuevo.Nombre = txtNombre.Text;
                nuevo.Descripcion = txtDescripcion.Text;
                nuevo.Precio = decimal.Parse(txtPrecio.Text);
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

                ProductoNegocio negocio = new ProductoNegocio();

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
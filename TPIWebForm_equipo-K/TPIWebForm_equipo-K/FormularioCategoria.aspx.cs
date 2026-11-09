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
    public partial class FormularioCategoria : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Limpiamos una posible modificación anterior
                Session["CategoriaModificar"] = null;
                string id = Request.QueryString["id"];

                if (id != null)
                {
                    // MODO MODIFICACIÓN
                    int idCategoria = int.Parse(id);

                    
                    CategoriaNegocio negocio = new CategoriaNegocio();
                    List<Categoria> lista = negocio.listar();
                    Categoria seleccionada = lista.Find(x => x.Id == idCategoria);
                    // Si la categoría no existe, volvemos al listado
                    if (seleccionada == null)
                    {
                        Response.Redirect("AdminCategorias.aspx");
                        return;
                    }

                    if (seleccionada != null)
                    {
                        tituloFormulario.InnerText = "Modificar Categoria";
                        txtNombre.Text = seleccionada.Nombre;
                        Session["CategoriaModificar"] = seleccionada;
                    }
                }
                else
                {
                    // MODO ALTA NUEVA
                    tituloFormulario.InnerText = "Agregar Categoria";
                }
            }
        }

        protected void btnAceptar_Click(object sender, EventArgs e)
        {
            try
            {
                string nombre = txtNombre.Text.Trim();
                lblErrorNombre.Visible = false;

                // Verificamos que se haya ingresado el nombre
                if (nombre == "")
                {
                    lblErrorNombre.Text = "Ingresá el nombre de la categoría.";
                    lblErrorNombre.Visible = true;
                    return;
                }

                // Controlamos la longitud permitida por la base de datos
                if (nombre.Length > 100)
                {
                    lblErrorNombre.Text = "El nombre admite hasta 100 caracteres.";
                    lblErrorNombre.Visible = true;
                    return;
                }

                CategoriaNegocio negocio = new CategoriaNegocio();
                int idActual = 0;

                // Obtenemos el ID si estamos modificando
                if (Session["CategoriaModificar"] != null)
                {
                    Categoria categoriaActual = (Categoria)Session["CategoriaModificar"];
                    idActual = categoriaActual.Id;
                }

                // Buscamos si otra categoría utiliza el mismo nombre
                Categoria encontrada = negocio.listar().Find(item => item.Id != idActual && item.Nombre.Trim().ToUpper() == nombre.ToUpper());

                if (encontrada != null)
                {
                    lblErrorNombre.Text = "Ya existe una categoría con ese nombre.";
                    lblErrorNombre.Visible = true;
                    return;
                }

                if (Session["CategoriaModificar"] != null)
                {
                    // Es Modificación
                    Categoria cat = (Categoria)Session["CategoriaModificar"];
                    cat.Nombre = nombre;
                    negocio.modificar(cat);
                }
                else
                {
                    // Es Alta Nueva
                    Categoria nueva = new Categoria();
                    nueva.Nombre = nombre;
                    negocio.agregar(nueva);
                }
                Session["CategoriaModificar"] = null;
                Response.Redirect("AdminCategorias.aspx");
            }
            catch (Exception ex)
            {
                Response.Write("<script>alert('Error: " + ex.Message + "');</script>");
            }
        }
    }
}
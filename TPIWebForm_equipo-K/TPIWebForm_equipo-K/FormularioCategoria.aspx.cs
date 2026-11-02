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
                string id = Request.QueryString["id"];

                if (id != null)
                {
                    int idCategoria = int.Parse(id);

                    // Modo Modificación: Buscamos la categoría y cargamos el TextBox

                    CategoriaNegocio negocio = new CategoriaNegocio();
                    
                    List<Categoria> lista = negocio.listar();
                    
                    Categoria seleccionada = lista.Find(x => x.Id == int.Parse(id));

                    if (seleccionada == null)
                    {
                        Response.Redirect("AdminCategorias.aspx");
                        return;
                    }
                    tituloFormulario.InnerText = "Modificar Categoria";

                    // Cargamos el nombre actual de la marca
                    txtNombre.Text = seleccionada.Nombre;

                    // Guardamos la marca para utilizarla al presionar Aceptar
                    Session["CategoriaModificar"] = seleccionada;
                }
            }
        }

        protected void btnAceptar_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtNombre.Text))
                {
                    Response.Write("<script>alert('Por favor, ingresá un nombre para la categoría.');</script>");
                    return; // Corta la ejecución aqui y no guarda
                }
                CategoriaNegocio negocio = new CategoriaNegocio();

                if (Session["CategoriaModificar"] != null)
                {
                    // Es Modificación
                    Categoria cat = (Categoria)Session["CategoriaModificar"];
                    cat.Nombre = txtNombre.Text;
                    negocio.modificar(cat);
                }
                else
                {
                    // Es Alta Nueva
                    Categoria nueva = new Categoria();
                    nueva.Nombre = txtNombre.Text;
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
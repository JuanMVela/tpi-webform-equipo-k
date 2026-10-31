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
                    // Modo Modificación: Buscamos la categoría y cargamos el TextBox
                    CategoriaNegocio negocio = new CategoriaNegocio();
                    List<Categoria> lista = negocio.listar();
                    Categoria seleccionada = lista.Find(x => x.Id == int.Parse(id));

                    if (seleccionada.Activo)
                    {
                        btnEstado.Text = "Desactivar";
                        btnEstado.CssClass = "btn btn-danger"; // Rojo
                    }
                    else
                    {
                        btnEstado.Text = "Reactivar";
                        btnEstado.CssClass = "btn btn-success"; // Verde
                    }

                    if (seleccionada != null)
                    {
                        txtNombre.Text = seleccionada.Nombre;
                        Session["CategoriaModificar"] = seleccionada; // Guardamos para usarla en el botón Aceptar
                    }
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

        protected void btnEstado_Click(object sender, EventArgs e)
        {
            if (Session["CategoriaModificar"] != null)
            {
                Categoria cat = (Categoria)Session["CategoriaModificar"];
                CategoriaNegocio negocio = new CategoriaNegocio();

                // Invertimos el estado y guardamos
                bool nuevoEstado = !cat.Activo;
                negocio.actualizarEstado(cat.Id, nuevoEstado);

                Session["CategoriaModificar"] = null;
                Response.Redirect("AdminCategorias.aspx");
            }
        }
    }
}
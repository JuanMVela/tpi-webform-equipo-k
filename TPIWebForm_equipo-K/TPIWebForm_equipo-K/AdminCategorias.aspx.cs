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
    public partial class AdminCategorias : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                cargarGrilla();
            }
        }
        protected void BtnAgregar_Click(object sender, EventArgs e)
        {
            Session["CategoriaModificar"] = null;
            Response.Redirect("FormularioCategoria.aspx");
        }
        // Gestiona las acciones de la categoría seleccionada
        protected void dgvCategorias_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            bool estadoActualizado = false;

            // Abrimos el formulario si se solicita modificar
            if (e.CommandName == "Modificar")
            {
                int id = int.Parse(e.CommandArgument.ToString());
                Response.Redirect("FormularioCategoria.aspx?id=" + id);
                return;
            }

            if (e.CommandName == "CambiarEstado")
            {
                try
                {
                    int id = int.Parse(e.CommandArgument.ToString());
                    CategoriaNegocio negocio = new CategoriaNegocio();

                    // Buscamos la categoría para conocer su estado
                    Categoria categoria = negocio.listar().Find(item => item.Id == id);

                    if (categoria != null)
                    {
                        bool nuevoEstado = !categoria.Activo;
                        negocio.actualizarEstado(id, nuevoEstado);
                        estadoActualizado = true;
                    }
                }
                catch (Exception)
                {
                    Response.Write("<script>alert('Error al cambiar el estado.');</script>");
                    return;
                }
            }

            // Evitamos repetir el cambio de estado al recargar
            if (estadoActualizado)
            {
                Response.Redirect("AdminCategorias.aspx");
            }
        }
        protected void dgvCategorias_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                int id = Convert.ToInt32(dgvCategorias.DataKeys[e.RowIndex].Value);

                CategoriaNegocio negocio = new CategoriaNegocio();
                negocio.actualizarEstado(id, false);

                cargarGrilla();
            }
            catch (Exception ex)
            {
                Response.Write(
                    "<script>alert('Error al desactivar: " + ex.Message + "');</script>"
                );
            }
        }
        protected void txtFiltro_TextChanged(object sender, EventArgs e)
        {
            CategoriaNegocio negocio = new CategoriaNegocio();
            List<Categoria> lista = negocio.listar();

            string filtro = txtFiltro.Text;

            if (filtro != "")
            {
                lista = lista.FindAll(
                    x => x.Nombre.ToLower().Contains(filtro.ToLower())
                );
            }
            dgvCategorias.PageIndex = 0;
            dgvCategorias.DataSource = lista;
            dgvCategorias.DataBind();
        }
        private void cargarGrilla()
        {
            CategoriaNegocio negocio = new CategoriaNegocio();

            dgvCategorias.DataSource = negocio.listar();
            dgvCategorias.DataBind();
        }
        protected void dgvCategorias_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            // 1. Le decimos al GridView que cambie a la página seleccionada
            dgvCategorias.PageIndex = e.NewPageIndex;

            // 2. Volvemos a cargar la grilla para que muestre los datos de esa página
            cargarGrilla();
        }
    }
}
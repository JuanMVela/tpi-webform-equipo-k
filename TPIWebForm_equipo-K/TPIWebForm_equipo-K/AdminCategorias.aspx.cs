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
        private List<Categoria> listaCategorias;
        protected void Page_Load(object sender, EventArgs e)
        {
            CategoriaNegocio negocio = new CategoriaNegocio();
            listaCategorias = negocio.listar();
            dgvCategorias.DataSource = listaCategorias;
            dgvCategorias.DataBind();
        }
        protected void BtnAgregar_Click(object sender, EventArgs e)
        {
            Session["CategoriaModificar"] = null;
            Response.Redirect("FormularioCategoria.aspx");
        }
        protected void dgvCategorias_SelectedIndexChanged(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(dgvCategorias.SelectedDataKey.Value);

            // Redirigimos al formulario con el ID en la URL
            Response.Redirect("FormularioCategoria.aspx?id=" + id);
        }
        protected void dgvCategorias_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                int id = Convert.ToInt32(dgvCategorias.DataKeys[e.RowIndex].Value);

                CategoriaNegocio negocio = new CategoriaNegocio();
                negocio.eliminarLogico(id);

                dgvCategorias.DataSource = negocio.listar();
                dgvCategorias.DataBind();
            }
            catch (Exception ex)
            {
                Response.Write("<script>alert('Error al eliminar: " + ex.Message + "');</script>");
            }   
        }

        protected void txtFiltro_TextChanged(object sender, EventArgs e)
        {
            if (listaCategorias == null) return;

            List<Categoria> listaFiltrada;
            string filtro = txtFiltro.Text;

            if (filtro != "")
            {
                listaFiltrada = listaCategorias.FindAll(x => x.Nombre.ToLower().Contains(filtro.ToLower()));
            }
            else
            {
                listaFiltrada = listaCategorias;
            }

            dgvCategorias.DataSource = null;
            dgvCategorias.DataSource = listaFiltrada;
            dgvCategorias.DataBind();
        }
    }
}
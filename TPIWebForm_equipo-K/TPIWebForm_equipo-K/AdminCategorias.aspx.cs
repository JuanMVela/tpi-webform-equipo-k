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

            dgvCategorias.DataSource = lista;
            dgvCategorias.DataBind();
        }
        private void cargarGrilla()
        {
            CategoriaNegocio negocio = new CategoriaNegocio();

            dgvCategorias.DataSource = negocio.listar();
            dgvCategorias.DataBind();
        }
    }
}
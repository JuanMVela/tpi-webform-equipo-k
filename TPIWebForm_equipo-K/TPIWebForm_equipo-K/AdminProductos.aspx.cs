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
    public partial class AdminProductos : System.Web.UI.Page
    {
        private void cargarGrilla()
        {
            ProductoNegocio negocio = new ProductoNegocio();
            dgvProductos.DataSource = negocio.listar();
            dgvProductos.DataBind();
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                cargarGrilla();
            }
        }
        protected void txtFiltro_TextChanged(object sender, EventArgs e)
        {
            ProductoNegocio negocio = new ProductoNegocio();
            List<Producto> lista = negocio.listar();

            string filtro = txtFiltro.Text;

            if (filtro != "")
            {
                lista = lista.FindAll(
                    x => x.Nombre.ToLower().Contains(filtro.ToLower())
                );
            }

            dgvProductos.DataSource = lista;
            dgvProductos.DataBind();
        }
        protected void btnAgregar_Click(object sender, EventArgs e)
        {
            Response.Redirect("FormularioProducto.aspx");
        }
        protected void dgvProductos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                int id = int.Parse(e.CommandArgument.ToString());

                if (e.CommandName == "Modificar")
                {
                    Response.Redirect("FormularioProducto.aspx?id=" + id);
                }
                else if (e.CommandName == "CambiarEstado")
                {
                    ProductoNegocio negocio = new ProductoNegocio();
                    // Buscamos el producto para conocer su estado actual
                    Producto producto = negocio.listar().Find(x => x.Id == id);

                    if (producto != null)
                    {
                        // Invertimos el estado actual
                        bool nuevoEstado = !producto.Activo;
                        negocio.actualizarEstado(id, nuevoEstado);

                        // Actualizamos el listado
                        cargarGrilla();
                    }
                }
            }
            catch (Exception ex)
            {
                Response.Write("<script>alert('Error: " + ex.Message + "');</script>");
            }
        }
    }
}
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
            dgvProductos.PageIndex = 0;
            dgvProductos.DataSource = lista;
            dgvProductos.DataBind();
        }
        protected void btnAgregar_Click(object sender, EventArgs e)
        {
            Response.Redirect("FormularioProducto.aspx");
        }
        // Gestiona las acciones del producto seleccionado
        protected void dgvProductos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            bool estadoActualizado = false;

            // Abrimos el formulario si se solicita modificar
            if (e.CommandName == "Modificar")
            {
                int id = int.Parse(e.CommandArgument.ToString());
                Response.Redirect("FormularioProducto.aspx?id=" + id);
                return;
            }

            if (e.CommandName == "CambiarEstado")
            {
                try
                {
                    int id = int.Parse(e.CommandArgument.ToString());
                    ProductoNegocio negocio = new ProductoNegocio();

                    // Buscamos el producto para conocer su estado
                    Producto producto = negocio.listar().Find(item => item.Id == id);

                    if (producto != null)
                    {
                        bool nuevoEstado = !producto.Activo;
                        negocio.actualizarEstado(id, nuevoEstado);
                        estadoActualizado = true;
                    }
                }
                catch (Exception ex)
                {
                    Response.Write("<script>alert('Error al cambiar el estado.');</script>");
                    return;
                }
            }

            // Evitamos repetir el cambio de estado al recargar
            if (estadoActualizado)
            {
                Response.Redirect("AdminProductos.aspx");
            }
        }
        protected void dgvCategorias_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            // 1. Le decimos al GridView que cambie a la página seleccionada
            dgvProductos.PageIndex = e.NewPageIndex;

            // 2. Volvemos a cargar la grilla para que muestre los datos de esa página
            cargarGrilla();
        }
    }
}
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
        // Carga los productos y aplica el filtro por nombre
        private void cargarGrilla()
        {
            ProductoNegocio negocio = new ProductoNegocio();
            List<Producto> lista = negocio.listar();

            string filtro = txtFiltro.Text.Trim();

            if (filtro != "")
            {
                lista = lista.FindAll(item => item.Nombre.ToLower().Contains(filtro.ToLower()));
            }

            dgvProductos.DataSource = lista;
            dgvProductos.DataBind();
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                cargarGrilla();
            }
        }
        // Reinicia la paginación cuando cambia la búsqueda
        protected void txtFiltro_TextChanged(object sender, EventArgs e)
        {
            dgvProductos.PageIndex = 0;
            cargarGrilla();
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
        // Cambia de página conservando el filtro de búsqueda
        protected void dgvProductos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            dgvProductos.PageIndex = e.NewPageIndex;
            cargarGrilla();
        }
    }
}
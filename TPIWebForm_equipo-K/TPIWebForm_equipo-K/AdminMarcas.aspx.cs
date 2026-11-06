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
    public partial class AdminMarcas : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                cargarGrilla();
            }
        }

        private void cargarGrilla()
        {
            // Cargamos todas las marcas en el GridView
            MarcaNegocio negocio = new MarcaNegocio();

            dgvMarcas.DataSource = negocio.listar();
            dgvMarcas.DataBind();
        }

        protected void BtnAgregar_Click(object sender, EventArgs e)
        {
            // Limpiamos una posible modificación anterior
            Session["MarcaModificar"] = null;

            // Redirigimos al formulario en modo Alta
            Response.Redirect("FormularioMarca.aspx");
        }

        // Gestiona las acciones de la marca seleccionada
        protected void dgvMarcas_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            bool estadoActualizado = false;

            // Abrimos el formulario si se solicita modificar
            if (e.CommandName == "Modificar")
            {
                int id = int.Parse(e.CommandArgument.ToString());
                Response.Redirect("FormularioMarca.aspx?id=" + id);
                return;
            }

            if (e.CommandName == "CambiarEstado")
            {
                try
                {
                    int id = int.Parse(e.CommandArgument.ToString());
                    MarcaNegocio negocio = new MarcaNegocio();

                    // Buscamos la marca para conocer su estado
                    Marca marca = negocio.listar().Find(item => item.Id == id);

                    if (marca != null)
                    {
                        bool nuevoEstado = !marca.Activo;
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
                Response.Redirect("AdminMarcas.aspx");
            }
        }

        protected void txtFiltro_TextChanged(object sender, EventArgs e)
        {
            MarcaNegocio negocio = new MarcaNegocio();

            // Obtenemos todas las marcas
            List<Marca> lista = negocio.listar();

            string filtro = txtFiltro.Text;

            if (filtro != "")
            {
                // Filtramos las marcas por nombre
                lista = lista.FindAll(
                    x => x.Nombre.ToLower().Contains(filtro.ToLower())
                );
            }
            dgvMarcas.PageIndex = 0;
            dgvMarcas.DataSource = lista;
            dgvMarcas.DataBind();
        }

        protected void dgvCategorias_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            // 1. Le decimos al GridView que cambie a la página seleccionada
            dgvMarcas.PageIndex = e.NewPageIndex;

            // 2. Volvemos a cargar la grilla para que muestre los datos de esa página
            cargarGrilla();
        }
    }
}
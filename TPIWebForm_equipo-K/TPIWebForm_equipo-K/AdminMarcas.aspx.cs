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

        // Carga las marcas y aplica el filtro por nombre
        private void cargarGrilla()
        {
            MarcaNegocio negocio = new MarcaNegocio();
            List<Marca> lista = negocio.listar();

            string filtro = txtFiltro.Text.Trim();

            if (filtro != "")
            {
                lista = lista.FindAll(item => item.Nombre.ToLower().Contains(filtro.ToLower()));
            }

            dgvMarcas.DataSource = lista;
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

        // Reinicia la paginación cuando cambia la búsqueda
        protected void txtFiltro_TextChanged(object sender, EventArgs e)
        {
            dgvMarcas.PageIndex = 0;
            cargarGrilla();
        }

        // Cambia de página conservando el filtro
        protected void dgvMarcas_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            dgvMarcas.PageIndex = e.NewPageIndex;
            cargarGrilla();
        }
    }
}
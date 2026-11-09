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
    public partial class AdminFormasEntrega : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                cargarGrilla();
            }
        }

        // Muestra las formas de entrega en la grilla
        private void cargarGrilla()
        {
            FormaEntregaNegocio negocio = new FormaEntregaNegocio();
            dgvFormasEntrega.DataSource = negocio.listar();
            dgvFormasEntrega.DataBind();
        }
        
        // Gestiona las acciones de la forma de entrega seleccionada
        protected void dgvFormasEntrega_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            bool estadoActualizado = false;

            // Abrimos el formulario para modificar
            if (e.CommandName == "Modificar")
            {
                int id = int.Parse(e.CommandArgument.ToString());
                Response.Redirect("FormularioFormaEntrega.aspx?id=" + id);
                return;
            }

            if (e.CommandName == "CambiarEstado")
            {
                try
                {
                    int id = int.Parse(e.CommandArgument.ToString());
                    FormaEntregaNegocio negocio = new FormaEntregaNegocio();

                    // Buscamos el registro para conocer su estado
                    FormaEntrega formaEntrega = negocio.listar().Find(item => item.Id == id);

                    if (formaEntrega != null)
                    {
                        bool nuevoEstado = !formaEntrega.Activo;
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

            // Evitamos repetir la acción al recargar
            if (estadoActualizado)
            {
                Response.Redirect("AdminFormasEntrega.aspx");
            }
        }

        // Filtra las formas de entrega por nombre
        protected void txtFiltro_TextChanged(object sender, EventArgs e)
        {
            FormaEntregaNegocio negocio = new FormaEntregaNegocio();
            List<FormaEntrega> lista = negocio.listar();

            string filtro = txtFiltro.Text.Trim();

            if (filtro != "")
            {
                lista = lista.FindAll(item => item.Nombre.ToLower().Contains(filtro.ToLower()));
            }

            dgvFormasEntrega.DataSource = lista;
            dgvFormasEntrega.DataBind();
        }
    }
}
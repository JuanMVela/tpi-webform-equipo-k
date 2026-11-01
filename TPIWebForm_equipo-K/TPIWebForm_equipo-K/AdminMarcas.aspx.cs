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

        protected void dgvMarcas_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                // Obtenemos el ID de la marca seleccionada
                int id = int.Parse(e.CommandArgument.ToString());

                if (e.CommandName == "Modificar")
                {
                    // Enviamos el ID de la marca al formulario
                    Response.Redirect("FormularioMarca.aspx?id=" + id);
                }

                if (e.CommandName == "CambiarEstado")
                {
                    MarcaNegocio negocio = new MarcaNegocio();

                    // Buscamos la marca para conocer su estado actual
                    Marca marca = negocio.listar().Find(x => x.Id == id);

                    if (marca != null)
                    {
                        // Invertimos el estado actual
                        bool nuevoEstado = !marca.Activo;

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

            dgvMarcas.DataSource = lista;
            dgvMarcas.DataBind();
        }
    }
}
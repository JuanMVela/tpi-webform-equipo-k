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
    public partial class AdminFormasPago : System.Web.UI.Page
    {

        // Carga las formas de pago en la grilla administrativa
        private void cargarGrilla()
        {
            FormaPagoNegocio negocio = new FormaPagoNegocio();
            dgvFormasPago.DataSource = negocio.listar();
            dgvFormasPago.DataBind();
        }

        // Filtra las formas de pago por nombre
        protected void txtFiltro_TextChanged(object sender, EventArgs e)
        {
            FormaPagoNegocio negocio = new FormaPagoNegocio();
            List<FormaPago> lista = negocio.listar();

            string filtro = txtFiltro.Text.Trim();

            if (filtro != "")
            {
                lista = lista.FindAll(item => item.Nombre.ToLower().Contains(filtro.ToLower()));
            }

            dgvFormasPago.DataSource = lista;
            dgvFormasPago.DataBind();
        }

        // Gestiona las acciones de la fila seleccionada
        protected void dgvFormasPago_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Modificar")
            {
                int id = int.Parse(e.CommandArgument.ToString());
                Response.Redirect("FormularioFormaPago.aspx?id=" + id);
            }
            else if (e.CommandName == "CambiarEstado")
            {
                int id = int.Parse(e.CommandArgument.ToString());
                FormaPagoNegocio negocio = new FormaPagoNegocio();

                // Buscamos el registro para conocer su estado actual
                FormaPago formaPago = negocio.listar().Find(item => item.Id == id);

                if (formaPago != null)
                {
                    // Invertimos el estado y actualizamos el listado
                    bool nuevoEstado = !formaPago.Activo;
                    negocio.actualizarEstado(id, nuevoEstado);

                    // Abrimos nuevamente el listado para evitar repetir la acción al recargar
                    Response.Redirect("AdminFormasPago.aspx");
                }
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                cargarGrilla();
            }
        }
        // Verificamos el acceso antes de cargar la página y sus eventos.
        protected override void OnPreInit(EventArgs e)
        {
            Usuario usuario = Session["UsuarioLogueado"] as Usuario;

            if (usuario == null)
            {
                Response.Redirect("Login.aspx");
                return;
            }

            bool autorizado;

            try
            {
                UsuarioNegocio negocio = new UsuarioNegocio();
                autorizado = negocio.esAdministradorActivo(usuario.Id);
            }
            catch (Exception)
            {
                throw new System.Web.HttpException(
                    503, "No pudimos verificar tus permisos. Intentá nuevamente.");
            }

            if (!autorizado)
            {
                Response.Redirect("Inicio.aspx");
                return;
            }

            base.OnPreInit(e);
        }
    }
}
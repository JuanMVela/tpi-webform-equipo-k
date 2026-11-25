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
    public partial class AdminUsuarios : System.Web.UI.Page
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
            UsuarioNegocio negocio = new UsuarioNegocio();
            List<Usuario> lista = negocio.listarTodos();

            string filtro = txtFiltro.Text.Trim();

            if (filtro != "")
            {
                lista = lista.FindAll(item =>
                    item.Nombre.ToLower().Contains(filtro.ToLower()) ||
                    item.Email.ToLower().Contains(filtro.ToLower()));
            }

            dgvUsuarios.DataSource = lista;
            dgvUsuarios.DataBind();
        }

        protected void dgvUsuarios_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "GuardarRol")
            {
                GuardarRol(e);
                return;
            }
            if (e.CommandName == "CambiarEstado")
            {
                try
                {
                    int id = int.Parse(e.CommandArgument.ToString());

                    // Validamos que el admin no se desactive a sí mismo
                    Usuario adminLogueado = Session["UsuarioLogueado"] as Usuario;
                    if (adminLogueado != null && adminLogueado.Id == id)
                    {
                        Response.Write("<script>alert('No podés desactivar tu propia cuenta.');</script>");
                        return;
                    }

                    UsuarioNegocio negocio = new UsuarioNegocio();
                    // Buscamos el usuario para saber su estado actual
                    Usuario usuario = negocio.listarTodos().Find(item => item.Id == id);

                    if (usuario != null)
                    {
                        bool nuevoEstado = !usuario.Activo;
                        negocio.cambiarEstado(id, nuevoEstado);
                    }
                }
                catch (Exception)
                {
                    Response.Write("<script>alert('Error al cambiar el estado del usuario.');</script>");
                    return;
                }

                // Recargamos para que se vea el cambio
                Response.Redirect("AdminUsuarios.aspx");
            }
        }

        protected void txtFiltro_TextChanged(object sender, EventArgs e)
        {
            dgvUsuarios.PageIndex = 0;
            cargarGrilla();
        }

        protected void dgvUsuarios_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            dgvUsuarios.PageIndex = e.NewPageIndex;
            cargarGrilla();
        }

        // Bloqueamos las acciones sobre nuestra propia cuenta.
        protected void dgvUsuarios_RowDataBound(
            object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType != DataControlRowType.DataRow)
            {
                return;
            }

            Usuario usuarioFila = (Usuario)e.Row.DataItem;
            Usuario administrador = Session["UsuarioLogueado"] as Usuario;

            bool puedeModificar = administrador != null
                && usuarioFila.Id != administrador.Id;

            DropDownList ddlPerfil =
                (DropDownList)e.Row.FindControl("ddlPerfil");

            Button btnGuardarRol =
                (Button)e.Row.FindControl("btnGuardarRol");

            Button btnEstado =
                (Button)e.Row.FindControl("btnEstado");

            ddlPerfil.Enabled = puedeModificar;
            btnGuardarRol.Enabled = puedeModificar;
            btnEstado.Enabled = puedeModificar;
        }

        // Guardamos el rol seleccionado en la fila.
        private void GuardarRol(GridViewCommandEventArgs e)
        {
            Usuario administrador = Session["UsuarioLogueado"] as Usuario;

            if (administrador == null || administrador.Perfil == null
                || administrador.Perfil.Id != 2)
            {
                Response.Redirect("Inicio.aspx");
                return;
            }

            try
            {
                // Identificamos la fila del botón presionado.
                Control boton = (Control)e.CommandSource;
                GridViewRow fila = (GridViewRow)boton.NamingContainer;

                int idUsuario =
                    Convert.ToInt32(dgvUsuarios.DataKeys[fila.RowIndex].Value);

                DropDownList ddlPerfil =
                    (DropDownList)fila.FindControl("ddlPerfil");

                int idPerfil = int.Parse(ddlPerfil.SelectedValue);

                UsuarioNegocio negocio = new UsuarioNegocio();

                bool actualizado = negocio.cambiarRol(
                    idUsuario, idPerfil, administrador.Id);

                lblAviso.CssClass = actualizado
                    ? "d-block text-success mb-3"
                    : "d-block text-danger mb-3";

                lblAviso.Text = actualizado
                    ? "Rol guardado. El usuario debe volver a iniciar sesión."
                    : "No se pudo cambiar el rol. Verificá tus permisos.";

                lblAviso.Visible = true;

                cargarGrilla();
            }
            catch (ArgumentException ex)
            {
                lblAviso.CssClass = "d-block text-danger mb-3";
                lblAviso.Text = Server.HtmlEncode(ex.Message);
                lblAviso.Visible = true;
            }
            catch (Exception)
            {
                lblAviso.CssClass = "d-block text-danger mb-3";
                lblAviso.Text = "No pudimos guardar el rol. Intentá nuevamente.";
                lblAviso.Visible = true;
            }
        }

        // Verificamos el permiso actual antes de cargar la página.
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
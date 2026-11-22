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
            // Validación de seguridad: solo el admin puede entrar acá
            Usuario usuario = Session["UsuarioLogueado"] as Usuario;
            if (usuario == null || usuario.Perfil.Id != 2)
            {
                Response.Redirect("Inicio.aspx");
                return;
            }

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
    }
}
using dominio;
using negocio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Net.Mail;

namespace TPIWebForm_equipo_K
{
    public partial class MiPerfil : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Usuario usuario = Session["UsuarioLogueado"] as Usuario;

            // El perfil requiere una sesión iniciada.
            if (usuario == null)
            {
                Response.Redirect("Login.aspx");
                return;
            }

            if (!IsPostBack)
            {
                CargarPerfil(usuario.Id);
                if (Request.QueryString["guardado"] == "1" &&
    Session["UsuarioLogueado"] != null &&
    !lblAviso.Visible)
                {
                    lblAviso.Text = "Tus datos se guardaron correctamente.";
                    lblAviso.CssClass = "d-block text-success mb-3";
                    lblAviso.Visible = true;
                }
            }
        }

        // Mostramos los datos guardados en SQL.
        private void CargarPerfil(int idUsuario)
        {
            try
            {
                UsuarioNegocio negocio = new UsuarioNegocio();
                Usuario usuario = negocio.obtenerPorId(idUsuario);

                if (usuario == null)
                {
                    Session["UsuarioLogueado"] = null;
                    Response.Redirect("Login.aspx");
                    return;
                }

                txtNombre.Text = usuario.Nombre;
                txtApellido.Text = usuario.Apellido;
                txtEmail.Text = usuario.Email;
                txtTelefono.Text = usuario.Telefono;
            }
            catch (Exception)
            {
                lblAviso.Text = "No pudimos cargar tus datos. Intentá nuevamente.";
                lblAviso.Visible = true;
            }
        }
        // Cambiamos entre consulta y edición.
        private void HabilitarEdicion(bool editar)
        {
            txtNombre.ReadOnly = !editar;
            txtApellido.ReadOnly = !editar;
            txtEmail.ReadOnly = !editar;
            txtTelefono.ReadOnly = !editar;

            btnModificar.Visible = !editar;
            btnGuardar.Visible = editar;
            btnCancelar.Visible = editar;
        }

        protected void btnModificar_Click(object sender, EventArgs e)
        {
            lblAviso.Visible = false;
            HabilitarEdicion(true);
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            Usuario usuario = Session["UsuarioLogueado"] as Usuario;

            if (usuario == null)
            {
                Response.Redirect("Login.aspx");
                return;
            }

            // Recuperamos los valores que estaban guardados.
            lblAviso.Visible = false;
            CargarPerfil(usuario.Id);
            HabilitarEdicion(false);
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            lblAviso.Visible = false;

            Usuario usuarioSesion = Session["UsuarioLogueado"] as Usuario;
            // Detenemos el guardado si falta un campo obligatorio.
            if (!Page.IsValid)
            {
                return;
            }

            if (usuarioSesion == null)
            {
                Response.Redirect("Login.aspx");
                return;
            }

            string nombre = txtNombre.Text.Trim();
            string apellido = txtApellido.Text.Trim();
            string email = txtEmail.Text.Trim();
            string telefono = txtTelefono.Text.Trim();


            // Respetamos las longitudes de la base de datos.
            if (nombre.Length > 100 || apellido.Length > 100 ||
                email.Length > 150 || telefono.Length > 30)
            {
                MostrarError(
                    "Máximos permitidos: nombre y apellido 100 caracteres, " +
                    "email 150 y teléfono 30.");
                return;
            }

            // Comprobamos el formato del email.
            try
            {
                MailAddress direccionEmail = new MailAddress(email);

                if (direccionEmail.Address != email)
                {
                    MostrarError("Ingresá un email válido.");
                    return;
                }
            }
            catch (FormatException)
            {
                MostrarError("Ingresá un email válido.");
                return;
            }

            try
            {
                // Tomamos el ID desde la sesión.
                Usuario actualizado = new Usuario();
                actualizado.Id = usuarioSesion.Id;
                actualizado.Nombre = nombre;
                actualizado.Apellido = apellido;
                actualizado.Email = email;
                actualizado.Telefono = telefono;

                UsuarioNegocio negocio = new UsuarioNegocio();
                negocio.actualizarDatosPersonales(actualizado);
            }
            catch (SqlException ex)
            {
                // SQL impide repetir un email por su restricción UNIQUE.
                if (ex.Number == 2601 || ex.Number == 2627)
                {
                    MostrarError("Ese email ya está registrado en otra cuenta.");
                }
                else
                {
                    MostrarError("No pudimos guardar los cambios. Intentá nuevamente.");
                }

                return;
            }
            catch (Exception)
            {
                MostrarError("No pudimos guardar los cambios. Intentá nuevamente.");
                return;
            }

            // Actualizamos la sesión después de guardar correctamente.
            usuarioSesion.Nombre = nombre;
            usuarioSesion.Apellido = apellido;
            usuarioSesion.Email = email;
            usuarioSesion.Telefono = telefono;
            Session["UsuarioLogueado"] = usuarioSesion;

            // Volvemos a cargar la página con los datos actualizados.
            Response.Redirect("MiPerfil.aspx?guardado=1");
        }

        // Mostramos los errores dentro de la tarjeta.
        private void MostrarError(string mensaje)
        {
            lblAviso.CssClass = "d-block text-danger mb-3";
            lblAviso.Text = mensaje;
            lblAviso.Visible = true;
        }
    }
}
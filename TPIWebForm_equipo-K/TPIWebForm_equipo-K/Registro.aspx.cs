using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using dominio;
using negocio;

namespace TPIWebForm_equipo_K
{
    public partial class Registro : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Si ya estoy logeado me voy al catalogo directo
            if (!IsPostBack && Session["UsuarioLogueado"] != null)
            {
                Response.Redirect("Catalogo.aspx");
            }
        }
        protected void btnRegistrar_Click(object sender, EventArgs e)
        {
            try
            {
                // 1. Validaciones basicas
                if (string.IsNullOrWhiteSpace(txtNombre.Text) || string.IsNullOrWhiteSpace(txtEmail.Text) || string.IsNullOrWhiteSpace(txtPassword.Text))
                {
                    Response.Write("<script>alert('Por favor, completá los campos obligatorios (Nombre, Email y Contraseña).');</script>");
                    return;
                }

                // 2. Creamos el objeto Usuario
                Usuario nuevo = new Usuario();
                nuevo.Nombre = txtNombre.Text.Trim();
                nuevo.Apellido = txtApellido.Text.Trim();
                nuevo.Email = txtEmail.Text.Trim();
                nuevo.Telefono = txtTelefono.Text.Trim();
                nuevo.PasswordHash = txtPassword.Text;

                // Asignamos el perfil de "Cliente" (suponiendo que el ID 1 es Cliente)
                nuevo.Perfil = new Perfil { Id = 1 };
                nuevo.Activo = true;

                // 3. Guardamos en la base de datos
                UsuarioNegocio negocio = new UsuarioNegocio();
                int idUsuario = negocio.agregar(nuevo);

                if (idUsuario > 0)
                {
                    Response.Write("<script>alert('¡Registro exitoso! Ya podés iniciar sesión.'); window.location='Login.aspx';</script>");
                }
                else
                {
                    Response.Write("<script>alert('Hubo un problema al registrar el usuario.');</script>");
                }
            }
            catch (Exception ex)
            {
                // Si el email ya existe (por la restricción UNIQUE), salta acá
                Response.Write("<script>alert('Error: " + ex.Message.Replace("'", "\\'") + "');</script>");
            }
        }
    }
}
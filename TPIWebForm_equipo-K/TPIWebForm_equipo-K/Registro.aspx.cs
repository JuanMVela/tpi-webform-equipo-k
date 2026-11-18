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
            // Si ya ingresó, lo enviamos al destino correspondiente.
            if (!IsPostBack && Session["UsuarioLogueado"] != null)
            {
                if (Request.QueryString["origen"] == "compra")
                {
                    Response.Redirect("FinalizarCompra.aspx");
                    return;
                }

                Response.Redirect("Catalogo.aspx");
                return;
            }
        }
        protected void btnRegistrar_Click(object sender, EventArgs e)
        {
            try
            {
                // Si falta un campo obligatorio, salimos antes de crear la cuenta.
                if (!Page.IsValid)
                {
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
                    EmailService emailService = new EmailService();
                    string asunto = "¡Bienvenido a Mi eCommerce!";
                    string cuerpo = "<h1>¡Hola " + nuevo.Nombre + "!</h1>" +
                                    "<p>Gracias por registrarte en nuestra tienda. Tu cuenta fue creada con éxito.</p>";

                    emailService.armarCorreo(nuevo.Email, asunto, cuerpo);
                    emailService.enviarEmail();

                    // Conservamos el destino después de crear la cuenta.
                    if (Request.QueryString["origen"] == "compra")
                    {
                        Response.Redirect("Login.aspx?origen=compra");
                        return;
                    }

                    Response.Redirect("Login.aspx");
                    return;
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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using negocio;
using dominio;

namespace TPIWebForm_equipo_K
{
    public partial class ConfiguracionInicial : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Para configurar la tienda primero hay que ingresar.
            Usuario usuario = Session["UsuarioLogueado"] as Usuario;

            if (usuario == null)
            {
                Response.Redirect("Login.aspx");
                return;
            }

            // El botón sigue deshabilitado hasta completar la lógica.
            btnConfigurar.Enabled = false;

            try
            {
                UsuarioNegocio negocio = new UsuarioNegocio();

                if (negocio.existeAdministrador())
                {
                    txtClaveInstalacion.Enabled = false;

                    lblAviso.Text =
                        "La configuración inicial ya fue realizada.";
                    lblAviso.Visible = true;
                    return;
                }
                // Habilitamos el botón si hay una clave configurada.
                string clave = ConfigurationManager.AppSettings["ClaveInstalacionAdmin"];

                if (string.IsNullOrWhiteSpace(clave))
                {
                    lblAviso.Text = "Falta configurar la clave de instalación.";
                    lblAviso.Visible = true;
                    return;
                }

                btnConfigurar.Enabled = true;
            }
            catch (Exception)
            {
                txtClaveInstalacion.Enabled = false;

                lblAviso.Text =
                    "No pudimos comprobar la configuración. Intentá nuevamente.";
                lblAviso.Visible = true;
            }
        }
        // Validamos la clave antes de asignar el primer administrador.
        protected void btnConfigurar_Click(object sender, EventArgs e)
        {
            lblAviso.Visible = false;

            Usuario usuario = Session["UsuarioLogueado"] as Usuario;

            if (usuario == null)
            {
                Response.Redirect("Login.aspx");
                return;
            }

            Page.Validate("Configuracion");

            if (!Page.IsValid)
            {
                return;
            }

            string claveConfigurada =
                ConfigurationManager.AppSettings["ClaveInstalacionAdmin"];

            if (string.IsNullOrWhiteSpace(claveConfigurada))
            {
                lblAviso.Text = "La configuración inicial no está disponible.";
                lblAviso.Visible = true;
                return;
            }

            if (!string.Equals(txtClaveInstalacion.Text,
                claveConfigurada, StringComparison.Ordinal))
            {
                lblAviso.Text = "La clave de instalación es incorrecta.";
                lblAviso.Visible = true;
                return;
            }

            try
            {
                UsuarioNegocio negocio = new UsuarioNegocio();

                bool configurado =
                    negocio.configurarPrimerAdministrador(usuario.Id);

                if (!configurado)
                {
                    lblAviso.Text =
                        "No se pudo asignar el rol. Puede que ya exista un administrador "
                        + "o que tu cuenta no sea un cliente activo.";

                    lblAviso.Visible = true;
                    btnConfigurar.Enabled = false;
                    return;
                }
            }
            catch (Exception)
            {
                lblAviso.Text =
                    "No pudimos completar la configuración. Intentá nuevamente.";
                lblAviso.Visible = true;
                return;
            }

            // Pedimos ingresar nuevamente para cargar los nuevos permisos.
            Session.Remove("UsuarioLogueado");

            Response.Redirect("Login.aspx");
        }
    }
}
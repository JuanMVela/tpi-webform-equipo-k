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
    public partial class MisDirecciones : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Usuario usuario = Session["UsuarioLogueado"] as Usuario;

            if (usuario == null)
            {
                Response.Redirect("Login.aspx");
                return;
            }
            if (!IsPostBack)
            {
                CargarDirecciones(usuario.Id);
                if (Request.QueryString["editar"] != null)
                {
                    CargarDireccionParaEditar(usuario.Id);
                    return;
                }

                if (Request.QueryString["origen"] == "compra")
                {
                    // Abrimos directamente el formulario desde la compra.
                    pnlFormulario.Visible = true;
                    btnNuevaDireccion.Visible = false;

                    lnkCancelar.NavigateUrl =
                        "~/FinalizarCompra.aspx?origen=direcciones";
                }
            }
        }

        // Cargamos las tarjetas del usuario.
        private void CargarDirecciones(int idUsuario)
        {
            try
            {
                DireccionNegocio negocio = new DireccionNegocio();
                var direcciones = negocio.listarPorUsuario(idUsuario);

                repDirecciones.DataSource = direcciones;
                repDirecciones.DataBind();

                lblSinDirecciones.Visible = direcciones.Count == 0;
            }
            catch (Exception)
            {
                lblAviso.Text = "No pudimos cargar tus direcciones.";
                lblAviso.Visible = true;
            }
        }

        protected void btnNuevaDireccion_Click(object sender, EventArgs e)
        {
            lblAviso.Visible = false;
            pnlFormulario.Visible = true;
            btnNuevaDireccion.Visible = false;
        }

        protected void btnGuardarDireccion_Click(object sender, EventArgs e)
        {
            lblAviso.Visible = false;

            Usuario usuario = Session["UsuarioLogueado"] as Usuario;

            if (usuario == null)
            {
                Response.Redirect("Login.aspx");
                return;
            }

            if (!Page.IsValid)
            {
                return;
            }

            try
            {
                // Asociamos la dirección con el usuario de la sesión.
                Direccion direccion = new Direccion();
                direccion.IdUsuario = usuario.Id;
                direccion.Calle = txtCalle.Text.Trim();
                direccion.Numero = txtNumero.Text.Trim();
                direccion.Localidad = txtLocalidad.Text.Trim();
                direccion.CodigoPostal = txtCodigoPostal.Text.Trim();
                direccion.Observaciones = txtObservaciones.Text.Trim();
                DireccionNegocio negocio = new DireccionNegocio();

                if (Request.QueryString["editar"] != null)
                {
                    direccion.Id = int.Parse(Request.QueryString["editar"]);
                    negocio.modificar(direccion);
                }
                else
                {
                    negocio.agregar(direccion);
                }
            }
            catch (ArgumentException ex)
            {
                lblAviso.Text = Server.HtmlEncode(ex.Message);
                lblAviso.Visible = true;
                return;
            }
            catch (Exception)
            {
                lblAviso.Text = "No pudimos guardar la dirección. Intentá nuevamente.";
                lblAviso.Visible = true;
                return;
            }

            // Si venimos de la compra, regresamos después de guardar.
            if (Request.QueryString["origen"] == "compra")
            {
                Response.Redirect("FinalizarCompra.aspx?origen=direcciones");
                return;
            }

            Response.Redirect("MisDirecciones.aspx");
        }
        // Cargamos una dirección del usuario en el formulario.
        private void CargarDireccionParaEditar(int idUsuario)
        {
            try
            {
                int idDireccion = int.Parse(Request.QueryString["editar"]);

                DireccionNegocio negocio = new DireccionNegocio();

                // Buscamos únicamente entre las direcciones del usuario.
                Direccion direccion = negocio.listarPorUsuario(idUsuario)
                    .FirstOrDefault(d => d.Id == idDireccion);

                if (direccion == null)
                {
                    lblAviso.Text = "No encontramos esa dirección.";
                    lblAviso.Visible = true;
                    return;
                }

                txtCalle.Text = direccion.Calle;
                txtNumero.Text = direccion.Numero;
                txtLocalidad.Text = direccion.Localidad;
                txtCodigoPostal.Text = direccion.CodigoPostal;
                txtObservaciones.Text = direccion.Observaciones;

                repDirecciones.Visible = false;
                lblSinDirecciones.Visible = false;

                pnlFormulario.Visible = true;
                btnNuevaDireccion.Visible = false;
                btnGuardarDireccion.Text = "Guardar cambios";
                litTituloFormulario.Text = "Editar dirección";
            }
            catch (FormatException)
            {
                lblAviso.Text = "El identificador de la dirección no es válido.";
                lblAviso.Visible = true;
            }
            catch (OverflowException)
            {
                lblAviso.Text = "El identificador de la dirección no es válido.";
                lblAviso.Visible = true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.TraceError(ex.ToString());

                lblAviso.Text = "No pudimos cargar la dirección.";
                lblAviso.Visible = true;
            }
        }
    }
}
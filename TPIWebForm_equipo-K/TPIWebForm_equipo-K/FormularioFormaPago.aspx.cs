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
    public partial class FormularioFormaPago : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Limpiamos los datos de una modificación anterior
                Session["FormaPagoModificar"] = null;

                string id = Request.QueryString["id"];

                if (id != null)
                {
                    // Buscamos la forma de pago seleccionada
                    int idFormaPago = int.Parse(id);
                    FormaPagoNegocio negocio = new FormaPagoNegocio();

                    FormaPago seleccionada = negocio.listar().Find(
                        formaPago => formaPago.Id == idFormaPago);

                    // Si no existe, volvemos al listado
                    if (seleccionada == null)
                    {
                        Response.Redirect("AdminFormasPago.aspx");
                        return;
                    }

                    // Cargamos sus datos para modificar
                    Session["FormaPagoModificar"] = seleccionada;
                    txtNombre.Text = seleccionada.Nombre;
                    tituloFormulario.InnerText = "Modificar Forma de Pago";
                }
            }
        }
        // Verifica que se haya ingresado el nombre de la forma de pago
        protected void btnAceptar_Click(object sender, EventArgs e)
        {
            string nombre = txtNombre.Text.Trim();
            lblErrorNombre.Visible = false;

            if (nombre == "")
            {
                lblErrorNombre.Text = "Ingresá el nombre de la forma de pago.";
                lblErrorNombre.Visible = true;
                return;
            }
            // Verificamos que no exista otra forma de pago con el mismo nombre
            FormaPagoNegocio negocio = new FormaPagoNegocio();

            FormaPago encontrada = negocio.listar().Find(formaPago =>
                formaPago.Nombre.Trim().ToUpper() == nombre.ToUpper());

            if (encontrada != null)
            {
                lblErrorNombre.Text = "Ya existe una forma de pago con ese nombre.";
                lblErrorNombre.Visible = true;
                return;
            }
            // Guardamos la nueva forma de pago
            FormaPago nueva = new FormaPago();
            nueva.Nombre = nombre;

            negocio.agregar(nueva);

            // Volvemos al listado de formas de pago
            Response.Redirect("AdminFormasPago.aspx");
        }
    }
}
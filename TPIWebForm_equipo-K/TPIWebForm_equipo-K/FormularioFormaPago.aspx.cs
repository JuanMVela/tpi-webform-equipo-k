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

            // Verificamos que se haya ingresado el nombre
            if (nombre == "")
            {
                lblErrorNombre.Text = "Ingresá el nombre de la forma de pago.";
                lblErrorNombre.Visible = true;
                return;
            }

            FormaPago formaPago = new FormaPago();

            // Recuperamos la forma de pago si estamos modificando
            if (Session["FormaPagoModificar"] != null)
            {
                formaPago = (FormaPago)Session["FormaPagoModificar"];
            }

            FormaPagoNegocio negocio = new FormaPagoNegocio();

            // Buscamos si otro registro ya utiliza ese nombre
            FormaPago encontrada = negocio.listar().Find(item =>
                item.Id != formaPago.Id &&
                item.Nombre.Trim().ToUpper() == nombre.ToUpper());

            if (encontrada != null)
            {
                lblErrorNombre.Text = "Ya existe una forma de pago con ese nombre.";
                lblErrorNombre.Visible = true;
                return;
            }

            formaPago.Nombre = nombre;

            // Guardamos el alta o la modificación
            if (formaPago.Id == 0)
            {
                negocio.agregar(formaPago);
            }
            else
            {
                negocio.modificar(formaPago);
            }

            Session["FormaPagoModificar"] = null;
            Response.Redirect("AdminFormasPago.aspx");
        }
    }
}
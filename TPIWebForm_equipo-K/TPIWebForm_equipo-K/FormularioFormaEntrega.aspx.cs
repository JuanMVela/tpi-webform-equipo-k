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
    public partial class FormularioFormaEntrega : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Limpiamos una modificación anterior
                Session["FormaEntregaModificar"] = null;

                string id = Request.QueryString["id"];

                if (id != null)
                {
                    int idFormaEntrega = int.Parse(id);
                    FormaEntregaNegocio negocio = new FormaEntregaNegocio();

                    // Buscamos la forma de entrega seleccionada
                    FormaEntrega seleccionada = negocio.listar().Find(item => item.Id == idFormaEntrega);

                    if (seleccionada == null)
                    {
                        Response.Redirect("AdminFormasEntrega.aspx");
                        return;
                    }

                    // Conservamos el objeto y mostramos sus datos
                    Session["FormaEntregaModificar"] = seleccionada;
                    txtNombre.Text = seleccionada.Nombre;
                    chkRequiereDireccion.Checked = seleccionada.RequiereDireccion;
                    tituloFormulario.InnerText = "Modificar Forma de Entrega";
                }
            }
        }
        // Valida y guarda el alta o la modificación de una forma de entrega
        protected void btnAceptar_Click(object sender, EventArgs e)
        {
            lblErrorNombre.Visible = false;

            // Si la página no pasó la validación, salimos antes de guardar.
            if (!Page.IsValid)
            {
                return;
            }

            string nombre = txtNombre.Text.Trim();

            // Controlamos la longitud permitida por la base de datos
            if (nombre.Length > 100)
            {
                lblErrorNombre.Text = "El nombre admite hasta 100 caracteres.";
                lblErrorNombre.Visible = true;
                return;
            }

            FormaEntrega formaEntrega = new FormaEntrega();

            // Recuperamos el registro si estamos modificando
            if (Session["FormaEntregaModificar"] != null)
            {
                formaEntrega = (FormaEntrega)Session["FormaEntregaModificar"];
            }

            FormaEntregaNegocio negocio = new FormaEntregaNegocio();

            // Verificamos que otro registro no utilice el mismo nombre
            FormaEntrega encontrada = negocio.listar().Find(item =>
                item.Id != formaEntrega.Id &&
                item.Nombre.Trim().ToUpper() == nombre.ToUpper());

            if (encontrada != null)
            {
                lblErrorNombre.Text = "Ya existe una forma de entrega con ese nombre.";
                lblErrorNombre.Visible = true;
                return;
            }

            // Asignamos los datos del formulario
            formaEntrega.Nombre = nombre;
            formaEntrega.RequiereDireccion = chkRequiereDireccion.Checked;

            // Guardamos el alta o la modificación
            if (formaEntrega.Id == 0)
            {
                negocio.agregar(formaEntrega);
            }
            else
            {
                negocio.modificar(formaEntrega);
            }

            Session["FormaEntregaModificar"] = null;
            Response.Redirect("AdminFormasEntrega.aspx");
        }
    }
}
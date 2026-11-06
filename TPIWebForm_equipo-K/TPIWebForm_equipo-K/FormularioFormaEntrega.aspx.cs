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
        // Verifica que se haya ingresado el nombre
        protected void btnAceptar_Click(object sender, EventArgs e)
        {
            string nombre = txtNombre.Text.Trim();
            lblErrorNombre.Visible = false;

            if (nombre == "")
            {
                lblErrorNombre.Text = "Ingresá el nombre de la forma de entrega.";
                lblErrorNombre.Visible = true;
                return;
            }

            // Verificamos que no exista otra forma de entrega con el mismo nombre
            FormaEntregaNegocio negocio = new FormaEntregaNegocio();

            FormaEntrega encontrada = negocio.listar().Find(item => item.Nombre.Trim().ToUpper() == nombre.ToUpper());

            if (encontrada != null)
            {
                lblErrorNombre.Text = "Ya existe una forma de entrega con ese nombre.";
                lblErrorNombre.Visible = true;
                return;
            }
            // Guardamos la nueva forma de entrega
            FormaEntrega nueva = new FormaEntrega();
            nueva.Nombre = nombre;
            nueva.RequiereDireccion = chkRequiereDireccion.Checked;

            negocio.agregar(nueva);

            // Volvemos al listado
            Response.Redirect("AdminFormasEntrega.aspx");
        }
    }
}
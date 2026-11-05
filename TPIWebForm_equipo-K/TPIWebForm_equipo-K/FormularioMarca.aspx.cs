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
    public partial class FormularioMarca : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Limpiamos una posible marca guardada anteriormente
                Session["MarcaModificar"] = null;

                string id = Request.QueryString["id"];

                if (id != null)
                {
                    // Modo Modificación: buscamos la marca seleccionada
                    int idMarca = int.Parse(id);

                    MarcaNegocio negocio = new MarcaNegocio();

                    Marca seleccionada = negocio.listar().Find(x => x.Id == idMarca);

                    // Si la marca no existe, volvemos al listado
                    if (seleccionada == null)
                    {
                        Response.Redirect("AdminMarcas.aspx");
                        return;
                    }

                    tituloFormulario.InnerText = "Modificar Marca";

                    // Cargamos el nombre actual de la marca
                    txtNombre.Text = seleccionada.Nombre;

                    // Guardamos la marca para utilizarla al presionar Aceptar
                    Session["MarcaModificar"] = seleccionada;
                }
                else
                {
                    // Modo Alta Nueva
                    tituloFormulario.InnerText = "Agregar Marca";
                }
            }
        }

        protected void btnAceptar_Click(object sender, EventArgs e)
        {
            try
            {
                // Validamos que el nombre no esté vacío
                if (string.IsNullOrWhiteSpace(txtNombre.Text))
                {
                    Response.Write("<script>alert('Por favor, ingresá un nombre para la marca.');</script>");

                    return; // Corta la ejecución y no guarda
                }

                MarcaNegocio negocio = new MarcaNegocio();
                string nombreIngresado = txtNombre.Text.Trim();
                int idActual = 0;

                if (Session["MarcaModificar"] != null)
                {
                    Marca marcaActual = (Marca)Session["MarcaModificar"];
                    idActual = marcaActual.Id;
                }

                // Buscamos si otra marca ya usa ese nombre
                lblErrorNombre.Visible = false;

                foreach (Marca marcaExistente in negocio.listar())
                {
                    if (marcaExistente.Id != idActual &&
                        marcaExistente.Nombre.Trim().ToUpper() == nombreIngresado.ToUpper())
                    {
                        lblErrorNombre.Visible = true;                        
                        return;
                    }
                }

                if (Session["MarcaModificar"] != null)
                {
                    // Es Modificación
                    Marca marca = (Marca)Session["MarcaModificar"];

                    marca.Nombre = txtNombre.Text.Trim();

                    negocio.modificar(marca);
                }
                else
                {
                    // Es Alta Nueva
                    Marca nueva = new Marca();

                    nueva.Nombre = txtNombre.Text.Trim();

                    negocio.agregar(nueva);
                }

                // Limpiamos la marca guardada en Session
                Session["MarcaModificar"] = null;

                Response.Redirect("AdminMarcas.aspx");
            }
            catch (Exception ex)
            {
                Response.Write("<script>alert('Error: " + ex.Message + "');</script>");
            }
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            // Cancelamos la operación y limpiamos la Session
            Session["MarcaModificar"] = null;

            Response.Redirect("AdminMarcas.aspx");
        }
    }
}
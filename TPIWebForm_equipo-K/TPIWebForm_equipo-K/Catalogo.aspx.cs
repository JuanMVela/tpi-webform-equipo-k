using dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using negocio;

namespace TPIWebForm_equipo_K
{
    public partial class Catalogo : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ProductoNegocio negocio = new ProductoNegocio();

                // 1. Traemos todos los productos
                List<Producto> listaProductos = negocio.listarActivo();

                // 2. Por cada producto, traemos sus imágenes y se las asignamos
                foreach (Producto prod in listaProductos)
                {
                    prod.Imagenes = negocio.listarImagenes(prod.Id);
                }

                // 3. Pasamos la lista completa al Repeater
                repProductos.DataSource = listaProductos;
                repProductos.DataBind();
            }
        }
    }
}
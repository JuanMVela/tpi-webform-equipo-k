using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using negocio;

namespace TPIWebForm_equipo_K
{
    public partial class AdminFormasPago : System.Web.UI.Page
    {
        
        // Carga las formas de pago en la grilla administrativa
        private void cargarGrilla()
        {
            FormaPagoNegocio negocio = new FormaPagoNegocio();
            dgvFormasPago.DataSource = negocio.listar();
            dgvFormasPago.DataBind();
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                cargarGrilla();
            }
        }
    }
}
using dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace negocio
{
    public class FormaPagoNegocio
    {
        // Lista las formas de pago activas e inactivas para la administración
        public List<FormaPago> listar()
        {
            List<FormaPago> lista = new List<FormaPago>();
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.setearConsulta(
                    "SELECT Id, Nombre, Activo FROM FORMAS_PAGO ORDER BY Nombre"
                );
                datos.ejecutarLectura();

                while (datos.Lector.Read())
                {
                    FormaPago formaPago = new FormaPago();
                    formaPago.Id = (int)datos.Lector["Id"];
                    formaPago.Nombre = (string)datos.Lector["Nombre"];
                    formaPago.Activo = (bool)datos.Lector["Activo"];

                    lista.Add(formaPago);
                }

                return lista;
            }
            finally
            {
                datos.cerrarConexion();
            }
        }
    }
}

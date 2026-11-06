using dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace negocio
{
    public class FormaEntregaNegocio
    {
        // Obtiene todas las formas de entrega
        public List<FormaEntrega> listar()
        {
            List<FormaEntrega> lista = new List<FormaEntrega>();
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.setearConsulta("SELECT Id, Nombre, RequiereDireccion, Activo FROM FORMAS_ENTREGA ORDER BY Nombre");
                datos.ejecutarLectura();

                while (datos.Lector.Read())
                {
                    FormaEntrega formaEntrega = new FormaEntrega();
                    formaEntrega.Id = (int)datos.Lector["Id"];
                    formaEntrega.Nombre = (string)datos.Lector["Nombre"];
                    formaEntrega.RequiereDireccion = (bool)datos.Lector["RequiereDireccion"];
                    formaEntrega.Activo = (bool)datos.Lector["Activo"];

                    lista.Add(formaEntrega);
                }

                return lista;
            }
            finally
            {
                datos.cerrarConexion();
            }
        }
        // Agrega una forma de entrega activa
        public void agregar(FormaEntrega nueva)
        {
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.setearConsulta("INSERT INTO FORMAS_ENTREGA (Nombre, RequiereDireccion, Activo) VALUES (@Nombre, @RequiereDireccion, 1)");
                datos.setearParametro("@Nombre", nueva.Nombre);
                datos.setearParametro("@RequiereDireccion", nueva.RequiereDireccion);
                datos.ejecutarAccion();
            }
            finally
            {
                datos.cerrarConexion();
            }
        }
        // Modifica los datos de una forma de entrega
        public void modificar(FormaEntrega formaEntrega)
        {
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.setearConsulta("UPDATE FORMAS_ENTREGA SET Nombre = @Nombre, RequiereDireccion = @RequiereDireccion WHERE Id = @Id");
                datos.setearParametro("@Nombre", formaEntrega.Nombre);
                datos.setearParametro("@RequiereDireccion", formaEntrega.RequiereDireccion);
                datos.setearParametro("@Id", formaEntrega.Id);
                datos.ejecutarAccion();
            }
            finally
            {
                datos.cerrarConexion();
            }
        }
    }
}

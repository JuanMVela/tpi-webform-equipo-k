using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using dominio;

namespace negocio
{
    public class DireccionNegocio
    {
        // Recuperamos las direcciones activas de un usuario.
        public List<Direccion> listarPorUsuario(int idUsuario)
        {
            List<Direccion> lista = new List<Direccion>();
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.setearConsulta(
                    "SELECT Id, IdUsuario, Calle, Numero, Localidad, " +
                    "CodigoPostal, Observaciones, Activo " +
                    "FROM DIRECCIONES " +
                    "WHERE IdUsuario = @IdUsuario AND Activo = 1 " +
                    "ORDER BY Id DESC");

                datos.setearParametro("@IdUsuario", idUsuario);
                datos.ejecutarLectura();

                while (datos.Lector.Read())
                {
                    Direccion direccion = new Direccion();
                    direccion.Id = (int)datos.Lector["Id"];
                    direccion.IdUsuario = (int)datos.Lector["IdUsuario"];
                    direccion.Calle = datos.Lector["Calle"].ToString();
                    direccion.Numero = datos.Lector["Numero"].ToString();
                    direccion.Localidad = datos.Lector["Localidad"].ToString();
                    direccion.CodigoPostal = datos.Lector["CodigoPostal"].ToString();
                    direccion.Observaciones = datos.Lector["Observaciones"].ToString();
                    direccion.Activo = (bool)datos.Lector["Activo"];

                    lista.Add(direccion);
                }

                return lista;
            }
            finally
            {
                datos.cerrarConexion();
            }
        }

        // Guardamos una dirección asociada al usuario.
        public void agregar(Direccion direccion)
        {
            if (string.IsNullOrWhiteSpace(direccion.Calle) ||
                string.IsNullOrWhiteSpace(direccion.Numero) ||
                string.IsNullOrWhiteSpace(direccion.Localidad) ||
                string.IsNullOrWhiteSpace(direccion.CodigoPostal))
            {
                throw new ArgumentException("Completá los campos obligatorios.");
            }

            if (direccion.Calle.Length > 150 ||
                direccion.Numero.Length > 20 ||
                direccion.Localidad.Length > 100 ||
                direccion.CodigoPostal.Length > 20 ||
                (direccion.Observaciones ?? "").Length > 300)
            {
                throw new ArgumentException(
                    "Revisá la longitud de los datos ingresados.");
            }

            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.setearConsulta(
                    "INSERT INTO DIRECCIONES " +
                    "(IdUsuario, Calle, Numero, Localidad, " +
                    "CodigoPostal, Observaciones, Activo) " +
                    "VALUES (@IdUsuario, @Calle, @Numero, @Localidad, " +
                    "@CodigoPostal, @Observaciones, 1)");

                datos.setearParametro("@IdUsuario", direccion.IdUsuario);
                datos.setearParametro("@Calle", direccion.Calle);
                datos.setearParametro("@Numero", direccion.Numero);
                datos.setearParametro("@Localidad", direccion.Localidad);
                datos.setearParametro("@CodigoPostal", direccion.CodigoPostal);
                datos.setearParametro("@Observaciones", direccion.Observaciones ?? "");

                datos.ejecutarAccion();
            }
            finally
            {
                datos.cerrarConexion();
            }
        }
    }
}

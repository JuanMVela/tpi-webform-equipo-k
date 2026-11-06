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
        // Agrega una forma de pago y la deja activa
        public void agregar(FormaPago nueva)
        {
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.setearConsulta(
                    "INSERT INTO FORMAS_PAGO (Nombre, Activo) " +
                    "VALUES (@Nombre, 1)"
                );

                datos.setearParametro("@Nombre", nueva.Nombre);
                datos.ejecutarAccion();
            }
            finally
            {
                datos.cerrarConexion();
            }
        }
        // Modifica el nombre de una forma de pago
        public void modificar(FormaPago formaPago)
        {
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.setearConsulta(
                    "UPDATE FORMAS_PAGO SET Nombre = @Nombre WHERE Id = @Id");

                datos.setearParametro("@Nombre", formaPago.Nombre);
                datos.setearParametro("@Id", formaPago.Id);
                datos.ejecutarAccion();
            }
            finally
            {
                datos.cerrarConexion();
            }
        }
        // Activa o desactiva una forma de pago
        public void actualizarEstado(int id, bool activo)
        {
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.setearConsulta("UPDATE FORMAS_PAGO SET Activo = @Activo WHERE Id = @Id");
                datos.setearParametro("@Activo", activo);
                datos.setearParametro("@Id", id);
                datos.ejecutarAccion();
            }
            finally
            {
                datos.cerrarConexion();
            }
        }
    }
}

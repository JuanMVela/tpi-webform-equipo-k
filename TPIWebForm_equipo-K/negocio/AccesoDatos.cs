using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace negocio
{
    internal class AccesoDatos
    {

        private SqlConnection conexion;
        private SqlCommand comando;
        private SqlDataReader lector;

        public SqlDataReader Lector
        {
            get { return lector; }
        }
        private SqlTransaction transaccion;

        public AccesoDatos()
        {
            conexion = new SqlConnection(
                "server=.\\SQLEXPRESS; database=ECOMMERCE_DB; integrated security=true"
            );

            comando = new SqlCommand();
        }

        public void setearConsulta(string consulta)
        {
            comando.CommandType = CommandType.Text;
            comando.CommandText = consulta;
        }

        // Ejecutamos una consulta para leer sus resultados.
        public void ejecutarLectura()
        {
            comando.Connection = conexion;

            if (conexion.State != ConnectionState.Open)
            {
                conexion.Open();
            }

            lector = comando.ExecuteReader();
        }

        // Ejecutamos una operación y devolvemos las filas afectadas.
        public int ejecutarAccion()
        {
            comando.Connection = conexion;

            if (conexion.State != ConnectionState.Open)
            {
                conexion.Open();
            }

            return comando.ExecuteNonQuery();
        }

        public void setearParametro(string nombre, object valor)
        {
            comando.Parameters.AddWithValue(nombre, valor);
        }

        public void cerrarConexion()
        {
            if (lector != null)
                lector.Close();

            conexion.Close();
        }

        public void setearProcedimiento(string nombreSP)
        {
            comando.CommandType = System.Data.CommandType.StoredProcedure;
            comando.CommandText = nombreSP;
        }

        // Ejecutamos una consulta que devuelve un número entero.
        public int ejecutarEscalar()
        {
            comando.Connection = conexion;

            try
            {
                if (conexion.State != ConnectionState.Open)
                {
                    conexion.Open();
                }

                return Convert.ToInt32(comando.ExecuteScalar());
            }
            finally
            {
                // Sin transacción, conservamos el cierre habitual.
                if (comando.Transaction == null)
                {
                    conexion.Close();
                }
            }
        }

        // Abrimos la conexión e iniciamos la transacción.
        public void iniciarTransaccion()
        {
            if (conexion.State != ConnectionState.Open)
            {
                conexion.Open();
            }

            transaccion = conexion.BeginTransaction();

            comando.Connection = conexion;
            comando.Transaction = transaccion;
        }

        // Confirmamos los cambios realizados.
        public void confirmarTransaccion()
        {
            transaccion.Commit();
        }

        // Deshacemos los cambios realizados.
        public void cancelarTransaccion()
        {
            transaccion.Rollback();
        }

        // Quitamos los parámetros de la consulta anterior.
        public void limpiarParametros()
        {
            comando.Parameters.Clear();
        }
        // Cerramos la lectura sin cerrar la conexión.
        public void cerrarLector()
        {
            if (lector != null)
            {
                lector.Close();
                lector = null;
            }
        }
    }
}

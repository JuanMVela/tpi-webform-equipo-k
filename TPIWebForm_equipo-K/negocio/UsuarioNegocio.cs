using dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace negocio
{
    public class UsuarioNegocio
    {
        public int agregar(Usuario nuevo)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                // 1. Insertamos el usuario
                datos.setearProcedimiento("UsuariosAgregar");
                datos.setearParametro("@Nombre", nuevo.Nombre);
                datos.setearParametro("@Apellido", nuevo.Apellido);
                datos.setearParametro("@Email", nuevo.Email);
                datos.setearParametro("@ClaveHash", nuevo.PasswordHash);
                datos.setearParametro("@Telefono", nuevo.Telefono);
                datos.setearParametro("@IdPerfil", nuevo.Perfil.Id); // Generalmente 1 o 2 (Cliente)

                int idUsuario = datos.ejecutarEscalar();

                return idUsuario;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                datos.cerrarConexion();
            }
        }
        public Usuario login(string email, string password)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                // Buscamos al usuario que coincida con el email y la clave, y que esté activo
                datos.setearConsulta("SELECT U.Id, U.Nombre, U.Apellido, U.Email, U.IdPerfil, P.Nombre AS NombrePerfil " +
                                     "FROM USUARIOS U INNER JOIN PERFILES P ON U.IdPerfil = P.Id " +
                                     "WHERE U.Email = @Email AND U.ClaveHash = @Password AND U.Activo = 1");

                datos.setearParametro("@Email", email);
                datos.setearParametro("@Password", password);
                datos.ejecutarLectura();

                if (datos.Lector.Read())
                {
                    Usuario aux = new Usuario();
                    aux.Id = (int)datos.Lector["Id"];
                    aux.Nombre = (string)datos.Lector["Nombre"];
                    aux.Apellido = (string)datos.Lector["Apellido"];
                    aux.Email = (string)datos.Lector["Email"];

                    // Cargamos el perfil del usuario (para saber si es Admin o Cliente)
                    aux.Perfil = new Perfil();
                    aux.Perfil.Id = (int)datos.Lector["IdPerfil"];
                    aux.Perfil.Nombre = (string)datos.Lector["NombrePerfil"];

                    return aux; // Usuario encontrado
                }

                return null; // No se encontró (email o clave incorrectos)
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                datos.cerrarConexion();
            }
        }
    }
}

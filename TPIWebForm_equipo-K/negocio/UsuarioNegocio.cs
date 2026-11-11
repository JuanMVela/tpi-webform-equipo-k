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
                datos.setearConsulta("INSERT INTO USUARIOS (Nombre, Apellido, Email, ClaveHash, Telefono, IdPerfil, Activo) " +
                                     "VALUES (@Nombre, @Apellido, @Email, @Clave, @Telefono, @IdPerfil, 1)");

                datos.setearParametro("@Nombre", nuevo.Nombre);
                datos.setearParametro("@Apellido", nuevo.Apellido);
                datos.setearParametro("@Email", nuevo.Email);
                datos.setearParametro("@Clave", nuevo.PasswordHash); // Ojo: acá después se encripta
                datos.setearParametro("@Telefono", nuevo.Telefono);
                datos.setearParametro("@IdPerfil", nuevo.Perfil.Id); // Generalmente 1 o 2 (Cliente)

                datos.ejecutarAccion();
                datos.cerrarConexion();

                // 2. Obtenemos el ID del usuario recién insertado
                datos = new AccesoDatos();
                datos.setearConsulta("SELECT MAX(Id) AS UltimoId FROM USUARIOS");
                datos.ejecutarLectura();

                int idUsuario = 0;
                if (datos.Lector.Read())
                {
                    idUsuario = (int)datos.Lector["UltimoId"];
                }
                datos.cerrarConexion();

                return idUsuario;
            }
            catch (Exception ex)
            {
                throw ex;
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

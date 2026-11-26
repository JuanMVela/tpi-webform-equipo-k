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

        // Recuperamos los datos actuales de un usuario.
        public Usuario obtenerPorId(int id)
        {
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.setearConsulta(
                    "SELECT Id, Nombre, Apellido, Email, Telefono " +
                    "FROM USUARIOS WHERE Id = @Id AND Activo = 1");

                datos.setearParametro("@Id", id);
                datos.ejecutarLectura();

                if (datos.Lector.Read())
                {
                    Usuario usuario = new Usuario();
                    usuario.Id = (int)datos.Lector["Id"];
                    usuario.Nombre = datos.Lector["Nombre"].ToString();
                    usuario.Apellido = datos.Lector["Apellido"].ToString();
                    usuario.Email = datos.Lector["Email"].ToString();
                    usuario.Telefono = datos.Lector["Telefono"].ToString();

                    return usuario;
                }

                return null;
            }
            finally
            {
                datos.cerrarConexion();
            }
        }

        // Actualizamos los datos personales del usuario.
        public void actualizarDatosPersonales(Usuario usuario)
        {
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.setearConsulta(
                    "UPDATE USUARIOS " +
                    "SET Nombre = @Nombre, Apellido = @Apellido, " +
                    "Email = @Email, Telefono = @Telefono " +
                    "OUTPUT INSERTED.Id " +
                    "WHERE Id = @Id AND Activo = 1");

                datos.setearParametro("@Nombre", usuario.Nombre);
                datos.setearParametro("@Apellido", usuario.Apellido);
                datos.setearParametro("@Email", usuario.Email);
                datos.setearParametro("@Telefono", usuario.Telefono);
                datos.setearParametro("@Id", usuario.Id);

                datos.ejecutarEscalar();
            }
            finally
            {
                datos.cerrarConexion();
            }
        }
        public List<Usuario> listarTodos()
        {
            List<Usuario> lista = new List<Usuario>();
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.setearConsulta("SELECT U.Id, U.Nombre, U.Apellido, U.Email, U.IdPerfil, P.Nombre AS NombrePerfil, U.Activo " +
                                     "FROM USUARIOS U INNER JOIN PERFILES P ON U.IdPerfil = P.Id " +
                                     "ORDER BY U.Id");
                datos.ejecutarLectura();

                while (datos.Lector.Read())
                {
                    Usuario aux = new Usuario();
                    aux.Id = (int)datos.Lector["Id"];
                    aux.Nombre = (string)datos.Lector["Nombre"];
                    aux.Apellido = (string)datos.Lector["Apellido"];
                    aux.Email = (string)datos.Lector["Email"];
                    aux.Activo = (bool)datos.Lector["Activo"];

                    aux.Perfil = new Perfil();
                    aux.Perfil.Id = (int)datos.Lector["IdPerfil"];
                    aux.Perfil.Nombre = (string)datos.Lector["NombrePerfil"];

                    lista.Add(aux);
                }

                return lista;
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

        // 2. Cambiamos el estado (Activo / Inactivo) de un usuario
        public void cambiarEstado(int idUsuario, bool activo)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearConsulta("UPDATE USUARIOS SET Activo = @Activo WHERE Id = @Id");
                datos.setearParametro("@Activo", activo);
                datos.setearParametro("@Id", idUsuario);
                datos.ejecutarAccion();
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
        // Cambiamos el rol de otra cuenta desde la administración.
        public bool cambiarRol(int idUsuario, int idPerfil, int idAdministrador)
        {
            if (idPerfil != 1 && idPerfil != 2)
            {
                throw new ArgumentException("El perfil seleccionado no es válido.");
            }

            if (idUsuario == idAdministrador)
            {
                throw new ArgumentException("No podés cambiar tu propio rol.");
            }

            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.setearConsulta(@"
            UPDATE USUARIOS
            SET IdPerfil = @IdPerfil
            WHERE Id = @IdUsuario
              AND Id <> @IdAdministrador
              AND EXISTS (
                  SELECT 1
                  FROM USUARIOS
                  WHERE Id = @IdAdministrador
                    AND IdPerfil = 2
                    AND Activo = 1
              );

            SELECT @@ROWCOUNT;
        ");

                datos.setearParametro("@IdUsuario", idUsuario);
                datos.setearParametro("@IdPerfil", idPerfil);
                datos.setearParametro("@IdAdministrador", idAdministrador);

                int actualizados = datos.ejecutarEscalar();

                return actualizados == 1;
            }
            finally
            {
                datos.cerrarConexion();
            }
        }
        // Comprobamos si ya existe una cuenta administradora.
        public bool existeAdministrador()
        {
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.setearConsulta(
                    "SELECT COUNT(*) FROM USUARIOS WHERE IdPerfil = 2");

                int cantidad = datos.ejecutarEscalar();

                return cantidad > 0;
            }
            finally
            {
                datos.cerrarConexion();
            }
        }
        // Asignamos el primer administrador si todavía no existe ninguno.
        public bool configurarPrimerAdministrador(int idUsuario)
        {
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.setearConsulta(@"
            SET XACT_ABORT ON;

            BEGIN TRY
                BEGIN TRANSACTION;

                DECLARE @Cantidad INT;
                DECLARE @Actualizados INT = 0;

                SELECT @Cantidad = COUNT(*)
                FROM USUARIOS WITH (TABLOCKX, HOLDLOCK)
                WHERE IdPerfil = 2;

                IF @Cantidad = 0
                BEGIN
                    UPDATE USUARIOS
                    SET IdPerfil = 2
                    WHERE Id = @IdUsuario
                      AND Activo = 1
                      AND IdPerfil = 1;

                    SET @Actualizados = @@ROWCOUNT;
                END;

                COMMIT TRANSACTION;

                SELECT @Actualizados;
            END TRY
            BEGIN CATCH
                IF @@TRANCOUNT > 0
                    ROLLBACK TRANSACTION;

                THROW;
            END CATCH;
        ");

                datos.setearParametro("@IdUsuario", idUsuario);

                int actualizados = datos.ejecutarEscalar();

                return actualizados == 1;
            }
            finally
            {
                datos.cerrarConexion();
            }
        }

        // Comprobamos el permiso actual del usuario en la base.
        public bool esAdministradorActivo(int idUsuario)
        {
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.setearConsulta(
                    "SELECT COUNT(*) FROM USUARIOS " +
                    "WHERE Id = @Id AND IdPerfil = 2 AND Activo = 1");

                datos.setearParametro("@Id", idUsuario);

                return datos.ejecutarEscalar() == 1;
            }
            finally
            {
                datos.cerrarConexion();
            }
        }
    }
}

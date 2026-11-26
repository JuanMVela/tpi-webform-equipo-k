using dominio;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace negocio
{
    public class ComprobantePagoNegocio
    {
        // Comprobamos los datos básicos del comprobante.
        private void validarComprobante(ComprobantePago comprobante)
        {
            if (comprobante == null)
            {
                throw new Exception("No se recibió un comprobante.");
            }

            if (comprobante.IdPedido <= 0)
            {
                throw new Exception("El pedido indicado no es válido.");
            }

            if (comprobante.Archivo == null ||
                comprobante.Archivo.Length == 0)
            {
                throw new Exception("Seleccioná un archivo.");
            }

            int tamanioMaximo = 2 * 1024 * 1024;

            if (comprobante.Archivo.Length > tamanioMaximo)
            {
                throw new Exception(
                    "El comprobante no puede superar los 2 MB.");
            }

            if (string.IsNullOrWhiteSpace(comprobante.NombreArchivo))
            {
                throw new Exception("El archivo debe tener un nombre.");
            }

            if (comprobante.NombreArchivo.Length > 255)
            {
                throw new Exception(
                    "El nombre del archivo es demasiado largo.");
            }
            validarFormato(comprobante);
        }

        // Comprobamos la extensión y la firma del archivo.
        private void validarFormato(ComprobantePago comprobante)
        {
            string extension = Path.GetExtension(
                comprobante.NombreArchivo).ToLowerInvariant();

            byte[] archivo = comprobante.Archivo;

            if (extension == ".pdf" &&
                archivo.Length >= 5 &&
                archivo[0] == 0x25 &&
                archivo[1] == 0x50 &&
                archivo[2] == 0x44 &&
                archivo[3] == 0x46 &&
                archivo[4] == 0x2D)
            {
                comprobante.TipoContenido = "application/pdf";
                return;
            }

            if ((extension == ".jpg" || extension == ".jpeg") &&
                archivo.Length >= 3 &&
                archivo[0] == 0xFF &&
                archivo[1] == 0xD8 &&
                archivo[2] == 0xFF)
            {
                comprobante.TipoContenido = "image/jpeg";
                return;
            }

            if (extension == ".png" &&
                archivo.Length >= 8 &&
                archivo[0] == 0x89 &&
                archivo[1] == 0x50 &&
                archivo[2] == 0x4E &&
                archivo[3] == 0x47 &&
                archivo[4] == 0x0D &&
                archivo[5] == 0x0A &&
                archivo[6] == 0x1A &&
                archivo[7] == 0x0A)
            {
                comprobante.TipoContenido = "image/png";
                return;
            }

            throw new Exception(
                "Seleccioná un archivo PDF, JPG o PNG válido.");
        }
        // Guardamos un comprobante para un pedido pendiente del cliente.
        public void agregar(ComprobantePago comprobante, int idCliente)
        {
            validarComprobante(comprobante);

            if (idCliente <= 0)
            {
                throw new Exception("El cliente indicado no es válido.");
            }

            AccesoDatos datos = new AccesoDatos();
            bool transaccionIniciada = false;

            try
            {
                datos.iniciarTransaccion();
                transaccionIniciada = true;

                // Bloqueamos el pedido mientras comprobamos y guardamos.
                datos.setearConsulta(
                    "SELECT COUNT(*) FROM PEDIDOS WITH (UPDLOCK, HOLDLOCK) " +
                    "WHERE Id = @IdPedido AND IdCliente = @IdCliente " +
                    "AND IdEstado = 1");

                datos.setearParametro("@IdPedido", comprobante.IdPedido);
                datos.setearParametro("@IdCliente", idCliente);

                int pedidosValidos = datos.ejecutarEscalar();

                if (pedidosValidos != 1)
                {
                    throw new Exception(
                        "El pedido no está disponible para cargar un comprobante.");
                }

                datos.limpiarParametros();

                datos.setearConsulta(
                    "SELECT COUNT(*) FROM COMPROBANTES_PAGO " +
                    "WITH (UPDLOCK, HOLDLOCK) " +
                    "WHERE IdPedido = @IdPedido " +
                    "AND Estado IN ('En revision', 'Aprobado')");

                datos.setearParametro("@IdPedido", comprobante.IdPedido);

                int comprobantesExistentes = datos.ejecutarEscalar();

                if (comprobantesExistentes > 0)
                {
                    throw new Exception(
                        "El pedido ya tiene un comprobante en revisión o aprobado.");
                }

                datos.limpiarParametros();

                datos.setearConsulta(
                    "INSERT INTO COMPROBANTES_PAGO " +
                    "(IdPedido, NombreArchivo, TipoContenido, Archivo) " +
                    "VALUES (@IdPedido, @NombreArchivo, @TipoContenido, @Archivo)");

                datos.setearParametro("@IdPedido", comprobante.IdPedido);
                datos.setearParametro("@NombreArchivo", comprobante.NombreArchivo);
                datos.setearParametro("@TipoContenido", comprobante.TipoContenido);

                datos.setearParametro(
                    "@Archivo", SqlDbType.VarBinary, -1, comprobante.Archivo);

                datos.ejecutarAccion();

                datos.confirmarTransaccion();
                transaccionIniciada = false;
            }
            catch
            {
                if (transaccionIniciada)
                {
                    datos.cancelarTransaccion();
                }

                throw;
            }
            finally
            {
                datos.cerrarConexion();
            }
        }

        // Consultamos el último comprobante de un pedido del cliente.
        public ComprobantePago obtenerUltimoPorCliente(
            int idPedido, int idCliente)
        {
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.setearConsulta(
                    "SELECT TOP 1 C.Id, C.IdPedido, C.NombreArchivo, " +
                    "C.TipoContenido, C.FechaCarga, C.Estado " +
                    "FROM COMPROBANTES_PAGO C " +
                    "INNER JOIN PEDIDOS P ON P.Id = C.IdPedido " +
                    "WHERE C.IdPedido = @IdPedido " +
                    "AND P.IdCliente = @IdCliente " +
                    "ORDER BY C.Id DESC");

                datos.setearParametro("@IdPedido", idPedido);
                datos.setearParametro("@IdCliente", idCliente);

                datos.ejecutarLectura();

                if (datos.Lector.Read())
                {
                    ComprobantePago comprobante = new ComprobantePago();

                    comprobante.Id = (int)datos.Lector["Id"];
                    comprobante.IdPedido = (int)datos.Lector["IdPedido"];
                    comprobante.NombreArchivo =
                        (string)datos.Lector["NombreArchivo"];
                    comprobante.TipoContenido =
                        (string)datos.Lector["TipoContenido"];
                    comprobante.FechaCarga =
                        (DateTime)datos.Lector["FechaCarga"];
                    comprobante.Estado = (string)datos.Lector["Estado"];

                    return comprobante;
                }

                return null;
            }
            finally
            {
                datos.cerrarConexion();
            }
        }

        // Listamos los comprobantes pendientes para un administrador.
        public List<ComprobantePago> listarPendientesAdministrador(
            int idAdministrador)
        {
            UsuarioNegocio usuarios = new UsuarioNegocio();

            if (!usuarios.esAdministradorActivo(idAdministrador))
            {
                throw new Exception(
                    "No tenés permisos para consultar los comprobantes.");
            }

            List<ComprobantePago> comprobantes =
                new List<ComprobantePago>();

            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.setearConsulta(
                    "SELECT C.Id, C.IdPedido, C.NombreArchivo, " +
                    "C.FechaCarga, C.Estado " +
                    "FROM COMPROBANTES_PAGO C " +
                    "INNER JOIN PEDIDOS P ON P.Id = C.IdPedido " +
                    "WHERE C.Estado = 'En revision' AND P.IdEstado = 1 " +
                    "ORDER BY C.FechaCarga, C.Id");

                datos.ejecutarLectura();

                while (datos.Lector.Read())
                {
                    ComprobantePago comprobante = new ComprobantePago();

                    comprobante.Id = (int)datos.Lector["Id"];
                    comprobante.IdPedido = (int)datos.Lector["IdPedido"];
                    comprobante.NombreArchivo =
                        (string)datos.Lector["NombreArchivo"];
                    comprobante.FechaCarga =
                        (DateTime)datos.Lector["FechaCarga"];
                    comprobante.Estado = (string)datos.Lector["Estado"];

                    comprobantes.Add(comprobante);
                }

                return comprobantes;
            }
            finally
            {
                datos.cerrarConexion();
            }
        }

        // Recuperamos el archivo para un administrador activo.
        public ComprobantePago obtenerArchivoAdministrador(
            int idComprobante, int idAdministrador)
        {
            UsuarioNegocio usuarios = new UsuarioNegocio();

            if (!usuarios.esAdministradorActivo(idAdministrador))
            {
                throw new Exception(
                    "No tenés permisos para consultar el comprobante.");
            }

            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.setearConsulta(
                    "SELECT Id, IdPedido, NombreArchivo, TipoContenido, Archivo " +
                    "FROM COMPROBANTES_PAGO " +
                    "WHERE Id = @IdComprobante");

                datos.setearParametro("@IdComprobante", idComprobante);
                datos.ejecutarLectura();

                if (datos.Lector.Read())
                {
                    ComprobantePago comprobante = new ComprobantePago();

                    comprobante.Id = (int)datos.Lector["Id"];
                    comprobante.IdPedido = (int)datos.Lector["IdPedido"];
                    comprobante.NombreArchivo =
                        (string)datos.Lector["NombreArchivo"];
                    comprobante.TipoContenido =
                        (string)datos.Lector["TipoContenido"];
                    comprobante.Archivo = (byte[])datos.Lector["Archivo"];

                    return comprobante;
                }

                return null;
            }
            finally
            {
                datos.cerrarConexion();
            }
        }

        // Revisamos un comprobante y, si corresponde, confirmamos el pago.
        public void revisarComprobante(
            int idComprobante, int idPedido, int idAdministrador, bool aprobar)
        {
            UsuarioNegocio usuarios = new UsuarioNegocio();

            if (!usuarios.esAdministradorActivo(idAdministrador))
            {
                throw new Exception("No tenés permisos para revisar comprobantes.");
            }

            AccesoDatos datos = new AccesoDatos();
            bool transaccionIniciada = false;

            try
            {
                datos.iniciarTransaccion();
                transaccionIniciada = true;

                // Bloqueamos el pedido antes de modificar su comprobante.
                datos.setearConsulta(
                    "SELECT COUNT(*) FROM PEDIDOS WITH (UPDLOCK, HOLDLOCK) " +
                    "WHERE Id = @IdPedido AND IdEstado = 1");

                datos.setearParametro("@IdPedido", idPedido);

                if (datos.ejecutarEscalar() != 1)
                {
                    throw new Exception(
                        "El pedido ya no está pendiente de pago.");
                }

                string nuevoEstado;

                if (aprobar)
                {
                    nuevoEstado = "Aprobado";
                }
                else
                {
                    nuevoEstado = "Rechazado";
                }

                datos.limpiarParametros();

                datos.setearConsulta(
                    "UPDATE COMPROBANTES_PAGO SET Estado = @Estado " +
                    "WHERE Id = @IdComprobante AND IdPedido = @IdPedido " +
                    "AND Estado = 'En revision'");

                datos.setearParametro("@Estado", nuevoEstado);
                datos.setearParametro("@IdComprobante", idComprobante);
                datos.setearParametro("@IdPedido", idPedido);

                if (datos.ejecutarAccion() != 1)
                {
                    throw new Exception(
                        "El comprobante ya fue revisado o no está disponible.");
                }

                if (aprobar)
                {
                    datos.limpiarParametros();

                    datos.setearConsulta(
                        "UPDATE PEDIDOS SET IdEstado = 2 " +
                        "WHERE Id = @IdPedido AND IdEstado = 1");

                    datos.setearParametro("@IdPedido", idPedido);

                    if (datos.ejecutarAccion() != 1)
                    {
                        throw new Exception(
                            "No pudimos confirmar el pago del pedido.");
                    }
                }

                datos.confirmarTransaccion();
                transaccionIniciada = false;
            }
            catch
            {
                if (transaccionIniciada)
                {
                    datos.cancelarTransaccion();
                }

                throw;
            }
            finally
            {
                datos.cerrarConexion();
            }
        }



    }
}

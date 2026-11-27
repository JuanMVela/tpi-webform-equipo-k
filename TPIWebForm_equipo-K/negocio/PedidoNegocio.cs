using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using dominio;

namespace negocio
{
    public class PedidoNegocio
    {
        // Comprobamos los datos básicos del pedido.
        private void validarPedido(Pedido pedido)
        {
            if (pedido == null)
            {
                throw new Exception("No se recibió un pedido.");
            }

            if (pedido.Cliente == null || pedido.Cliente.Id <= 0)
            {
                throw new Exception("El pedido debe tener un cliente válido.");
            }

            if (pedido.FormaPago == null || pedido.FormaPago.Id <= 0)
            {
                throw new Exception("Seleccioná una forma de pago válida.");
            }

            if (pedido.FormaEntrega == null || pedido.FormaEntrega.Id <= 0)
            {
                throw new Exception("Seleccioná una forma de entrega válida.");
            }

            if (pedido.Detalles == null || pedido.Detalles.Count == 0)
            {
                throw new Exception("El pedido debe tener productos.");
            }

            // Comprobamos cada producto del pedido.
            foreach (DetallePedido detalle in pedido.Detalles)
            {
                if (detalle == null)
                {
                    throw new Exception("Hay un detalle de pedido inválido.");
                }

                if (detalle.Producto == null || detalle.Producto.Id <= 0)
                {
                    throw new Exception("Hay un producto inválido en el pedido.");
                }

                if (detalle.Cantidad <= 0)
                {
                    throw new Exception(
                        "La cantidad de cada producto debe ser mayor a cero.");
                }
            }
        }

        // Consultamos precio y stock antes de guardar.
        private void validarProducto(DetallePedido detalle, AccesoDatos datos)
        {
            datos.limpiarParametros();

            datos.setearConsulta(
                "SELECT Precio, Stock FROM PRODUCTOS WITH (UPDLOCK, HOLDLOCK) " +
                "WHERE Id = @IdProducto AND Activo = 1");

            datos.setearParametro("@IdProducto", detalle.Producto.Id);

            try
            {
                datos.ejecutarLectura();

                if (!datos.Lector.Read())
                {
                    throw new Exception(
                        "Un producto ya no está disponible. Revisá el carrito.");
                }

                int stockActual = (int)datos.Lector["Stock"];
                decimal precioActual = (decimal)datos.Lector["Precio"];

                if (detalle.Cantidad > stockActual)
                {
                    throw new Exception(
                        "El stock cambió. Revisá las cantidades del carrito.");
                }

                if (detalle.PrecioUnitario != precioActual)
                {
                    throw new Exception(
                        "El precio cambió. Volvé a revisar la compra.");
                }
            }
            finally
            {
                datos.cerrarLector();
            }
        }

        // Guardamos los datos generales y recuperamos el ID.
        private int guardarCabecera(Pedido pedido, AccesoDatos datos)
        {
            datos.limpiarParametros();

            datos.setearConsulta(
                    "INSERT INTO PEDIDOS " +
                    "(IdCliente, FechaCreacion, Total, IdFormaPago, " +
                    "IdFormaEntrega, IdDireccion, IdEstado) " +
                    "OUTPUT INSERTED.Id " +
                    "VALUES (@IdCliente, GETDATE(), @Total, @IdFormaPago, " +
                    "@IdFormaEntrega, @IdDireccion, 1)");

            datos.setearParametro("@IdCliente", pedido.Cliente.Id);
            datos.setearParametro("@Total", pedido.Total);
            datos.setearParametro("@IdFormaPago", pedido.FormaPago.Id);
            datos.setearParametro("@IdFormaEntrega", pedido.FormaEntrega.Id);

            if (pedido.Direccion == null)
            {
                datos.setearParametro("@IdDireccion", DBNull.Value);
            }
            else
            {
                datos.setearParametro("@IdDireccion", pedido.Direccion.Id);
            }

            return datos.ejecutarEscalar();
        }

        // Descontamos stock y guardamos un producto del pedido.
        private void guardarDetalle(int idPedido, DetallePedido detalle, AccesoDatos datos)
        {
            datos.limpiarParametros();

            datos.setearConsulta(
                "UPDATE PRODUCTOS SET Stock = Stock - @Cantidad " +
                "WHERE Id = @IdProducto AND Activo = 1 " +
                "AND Stock >= @Cantidad");

            datos.setearParametro("@Cantidad", detalle.Cantidad);
            datos.setearParametro("@IdProducto", detalle.Producto.Id);

            int filasAfectadas = datos.ejecutarAccion();

            if (filasAfectadas != 1)
            {
                throw new Exception(
                    "No pudimos actualizar el stock. Revisá el carrito.");
            }

            datos.limpiarParametros();

            datos.setearConsulta(
                "INSERT INTO DETALLE_PEDIDO " +
                "(IdPedido, IdProducto, Cantidad, PrecioUnitario) " +
                "VALUES (@IdPedido, @IdProducto, @Cantidad, @PrecioUnitario)");

            datos.setearParametro("@IdPedido", idPedido);
            datos.setearParametro("@IdProducto", detalle.Producto.Id);
            datos.setearParametro("@Cantidad", detalle.Cantidad);
            datos.setearParametro("@PrecioUnitario", detalle.PrecioUnitario);

            datos.ejecutarAccion();
        }

        // Comprobamos las opciones actuales dentro de la transacción.
        private void validarOpciones(Pedido pedido, AccesoDatos datos)
        {
            datos.limpiarParametros();

            datos.setearConsulta(
                "SELECT COUNT(*) " +
                "FROM USUARIOS U " +
                "INNER JOIN FORMAS_PAGO P ON P.Id = @IdFormaPago " +
                "INNER JOIN FORMAS_ENTREGA E ON E.Id = @IdFormaEntrega " +
                "WHERE U.Id = @IdCliente AND U.Activo = 1 " +
                "AND P.Activo = 1 AND E.Activo = 1 " +
                "AND ((E.RequiereDireccion = 0 AND @IdDireccion IS NULL) " +
                "OR (E.RequiereDireccion = 1 AND EXISTS (" +
                "SELECT 1 FROM DIRECCIONES D " +
                "WHERE D.Id = @IdDireccion AND D.IdUsuario = U.Id " +
                "AND D.Activo = 1)))");

            datos.setearParametro("@IdCliente", pedido.Cliente.Id);
            datos.setearParametro("@IdFormaPago", pedido.FormaPago.Id);
            datos.setearParametro("@IdFormaEntrega", pedido.FormaEntrega.Id);

            if (pedido.Direccion == null)
            {
                datos.setearParametro("@IdDireccion", DBNull.Value);
            }
            else
            {
                datos.setearParametro("@IdDireccion", pedido.Direccion.Id);
            }

            int coincidencias = datos.ejecutarEscalar();

            if (coincidencias != 1)
            {
                throw new Exception(
                    "Los datos de entrega, pago o cliente ya no son válidos. " +
                    "Volvé a revisar la compra.");
            }
        }

        // Guardamos la compra completa dentro de una transacción.
        public int confirmarCompra(Pedido pedido)
        {
            validarPedido(pedido);

            // Evitamos productos repetidos dentro del pedido.
            HashSet<int> idsProductos = new HashSet<int>();

            foreach (DetallePedido detalle in pedido.Detalles)
            {
                if (!idsProductos.Add(detalle.Producto.Id))
                {
                    throw new Exception(
                        "Hay productos repetidos en el pedido.");
                }
            }

            AccesoDatos datos = new AccesoDatos();
            bool transaccionIniciada = false;

            try
            {
                datos.iniciarTransaccion();
                transaccionIniciada = true;

                validarOpciones(pedido, datos);

                // Consultamos los productos siempre en el mismo orden.
                var detallesOrdenados = pedido.Detalles
                    .OrderBy(detalle => detalle.Producto.Id)
                    .ToList();

                decimal total = 0;

                foreach (DetallePedido detalle in detallesOrdenados)
                {
                    validarProducto(detalle, datos);

                    total += detalle.PrecioUnitario * detalle.Cantidad;
                }

                pedido.Total = total;

                int idPedido = guardarCabecera(pedido, datos);

                foreach (DetallePedido detalle in detallesOrdenados)
                {
                    guardarDetalle(idPedido, detalle, datos);
                }

                datos.confirmarTransaccion();
                transaccionIniciada = false;

                return idPedido;
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

        public List<Pedido> listarPorCliente(int idCliente)
        {
            List<Pedido> pedidos = new List<Pedido>();
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.setearConsulta(
                    "SELECT P.Id, P.FechaCreacion, P.Total, P.IdEstado, " +
                    "E.Nombre AS NombreEstado " +
                    "FROM PEDIDOS P " +
                    "INNER JOIN ESTADOS_PEDIDO E ON E.Id = P.IdEstado " +
                    "WHERE P.IdCliente = @IdCliente " +
                    "ORDER BY P.FechaCreacion DESC, P.Id DESC");

                datos.setearParametro("@IdCliente", idCliente);
                datos.ejecutarLectura();

                while (datos.Lector.Read())
                {
                    Pedido pedido = new Pedido();

                    pedido.Id = (int)datos.Lector["Id"];
                    pedido.FechaCreacion = (DateTime)datos.Lector["FechaCreacion"]; // Cambiado acá
                    pedido.Total = (decimal)datos.Lector["Total"];
                    pedido.IdEstado = (int)datos.Lector["IdEstado"];
                    pedido.NombreEstado = (string)datos.Lector["NombreEstado"];

                    pedidos.Add(pedido);
                }

                return pedidos;
            }
            finally
            {
                datos.cerrarConexion();
            }
        }

        // Buscamos un pedido que pertenezca al cliente.
        public Pedido obtenerPorCliente(int idPedido, int idCliente)
        {
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.setearConsulta(
                    "SELECT P.Id, P.FechaCreacion, P.Total, P.IdEstado, " +
                    "E.Nombre AS NombreEstado " +
                    "FROM PEDIDOS P " +
                    "INNER JOIN ESTADOS_PEDIDO E ON E.Id = P.IdEstado " +
                    "WHERE P.Id = @IdPedido AND P.IdCliente = @IdCliente");

                datos.setearParametro("@IdPedido", idPedido);
                datos.setearParametro("@IdCliente", idCliente);

                datos.ejecutarLectura();

                if (datos.Lector.Read())
                {
                    Pedido pedido = new Pedido();

                    pedido.Id = (int)datos.Lector["Id"];
                    pedido.FechaCreacion = (DateTime)datos.Lector["FechaCreacion"]; // Cambiado acá
                    pedido.Total = (decimal)datos.Lector["Total"];
                    pedido.IdEstado = (int)datos.Lector["IdEstado"];
                    pedido.NombreEstado = (string)datos.Lector["NombreEstado"];

                    return pedido;
                }

                return null;
            }
            finally
            {
                datos.cerrarConexion();
            }
        }

        // Listamos los productos de un pedido del cliente.
        public List<DetallePedido> listarDetallesPorCliente(
            int idPedido, int idCliente)
        {
            List<DetallePedido> detalles = new List<DetallePedido>();
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.setearConsulta(
                    "SELECT D.IdProducto, PR.Nombre, D.Cantidad, " +
                    "D.PrecioUnitario " +
                    "FROM DETALLE_PEDIDO D " +
                    "INNER JOIN PEDIDOS P ON P.Id = D.IdPedido " +
                    "INNER JOIN PRODUCTOS PR ON PR.Id = D.IdProducto " +
                    "WHERE D.IdPedido = @IdPedido " +
                    "AND P.IdCliente = @IdCliente " +
                    "ORDER BY PR.Nombre, PR.Id");

                datos.setearParametro("@IdPedido", idPedido);
                datos.setearParametro("@IdCliente", idCliente);

                datos.ejecutarLectura();

                while (datos.Lector.Read())
                {
                    DetallePedido detalle = new DetallePedido();

                    detalle.Producto = new Producto();
                    detalle.Producto.Id = (int)datos.Lector["IdProducto"];
                    detalle.Producto.Nombre = (string)datos.Lector["Nombre"];

                    detalle.Cantidad = (int)datos.Lector["Cantidad"];
                    detalle.PrecioUnitario =
                        (decimal)datos.Lector["PrecioUnitario"];

                    detalles.Add(detalle);
                }

                return detalles;
            }
            finally
            {
                datos.cerrarConexion();
            }
        }
        // Avanzamos el estado del pedido para la logística
        public void avanzarEstado(int idPedido, int idEstadoActual)
        {
            // Solo avanzamos si el pedido ya está pagado (2) y no está entregado (5)
            if (idEstadoActual >= 2 && idEstadoActual < 5)
            {
                int nuevoEstado = idEstadoActual + 1;

                AccesoDatos datos = new AccesoDatos();
                try
                {
                    datos.setearConsulta("UPDATE PEDIDOS SET IdEstado = @NuevoEstado WHERE Id = @IdPedido AND IdEstado = @EstadoActual");
                    datos.setearParametro("@NuevoEstado", nuevoEstado);
                    datos.setearParametro("@IdPedido", idPedido);
                    datos.setearParametro("@EstadoActual", idEstadoActual);
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
        }
    }
}

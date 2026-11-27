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
                "(IdCliente, Fecha, Total, IdFormaPago, " +
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
        private void guardarDetalle(
            int idPedido, DetallePedido detalle, AccesoDatos datos)
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

        // Listamos únicamente los pedidos del cliente indicado.
        public List<Pedido> listarPorCliente(int idCliente)
        {
            List<Pedido> pedidos = new List<Pedido>();
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.setearConsulta(
                    "SELECT P.Id, P.Fecha, P.Total, P.IdEstado, " +
                    "E.Nombre AS NombreEstado " +
                    "FROM PEDIDOS P " +
                    "INNER JOIN ESTADOS_PEDIDO E ON E.Id = P.IdEstado " +
                    "WHERE P.IdCliente = @IdCliente " +
                    "ORDER BY P.Fecha DESC, P.Id DESC");

                datos.setearParametro("@IdCliente", idCliente);
                datos.ejecutarLectura();

                while (datos.Lector.Read())
                {
                    Pedido pedido = new Pedido();

                    pedido.Id = (int)datos.Lector["Id"];
                    pedido.FechaCreacion = (DateTime)datos.Lector["Fecha"];
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
        // Buscamos el pedido del cliente con sus datos de pago y entrega.
        public Pedido obtenerPorCliente(int idPedido, int idCliente)
        {
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.setearConsulta(
                    "SELECT P.Id, P.Fecha, P.Total, P.IdEstado, " +
                    "E.Nombre AS NombreEstado, " +
                    "P.IdFormaPago, FP.Nombre AS NombrePago, " +
                    "P.IdFormaEntrega, FE.Nombre AS NombreEntrega, " +
                    "FE.RequiereDireccion, " +
                    "D.Id AS IdDireccion, D.Calle, D.Numero, " +
                    "D.Localidad, D.CodigoPostal " +
                    "FROM PEDIDOS P " +
                    "INNER JOIN ESTADOS_PEDIDO E ON E.Id = P.IdEstado " +
                    "INNER JOIN FORMAS_PAGO FP ON FP.Id = P.IdFormaPago " +
                    "INNER JOIN FORMAS_ENTREGA FE ON FE.Id = P.IdFormaEntrega " +
                    "LEFT JOIN DIRECCIONES D ON D.Id = P.IdDireccion " +
                    "WHERE P.Id = @IdPedido AND P.IdCliente = @IdCliente");

                datos.setearParametro("@IdPedido", idPedido);
                datos.setearParametro("@IdCliente", idCliente);

                datos.ejecutarLectura();

                if (datos.Lector.Read())
                {
                    Pedido pedido = new Pedido();

                    pedido.Id = (int)datos.Lector["Id"];
                    pedido.FechaCreacion = (DateTime)datos.Lector["Fecha"];
                    pedido.Total = (decimal)datos.Lector["Total"];
                    pedido.IdEstado = (int)datos.Lector["IdEstado"];
                    pedido.NombreEstado = (string)datos.Lector["NombreEstado"];

                    pedido.FormaPago = new FormaPago();
                    pedido.FormaPago.Id = (int)datos.Lector["IdFormaPago"];
                    pedido.FormaPago.Nombre =
                        (string)datos.Lector["NombrePago"];

                    pedido.FormaEntrega = new FormaEntrega();
                    pedido.FormaEntrega.Id = (int)datos.Lector["IdFormaEntrega"];
                    pedido.FormaEntrega.Nombre =
                        (string)datos.Lector["NombreEntrega"];
                    pedido.FormaEntrega.RequiereDireccion =
                        (bool)datos.Lector["RequiereDireccion"];

                    // Los pedidos con retiro pueden no tener dirección.
                    if (datos.Lector["IdDireccion"] != DBNull.Value)
                    {
                        pedido.Direccion = new Direccion();

                        pedido.Direccion.Id =
                            (int)datos.Lector["IdDireccion"];
                        pedido.Direccion.Calle =
                            Convert.ToString(datos.Lector["Calle"]);
                        pedido.Direccion.Numero =
                            Convert.ToString(datos.Lector["Numero"]);
                        pedido.Direccion.Localidad =
                            Convert.ToString(datos.Lector["Localidad"]);
                        pedido.Direccion.CodigoPostal =
                            Convert.ToString(datos.Lector["CodigoPostal"]);
                    }

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

        // Listamos los pedidos para un administrador activo.
        public List<Pedido> listarAdministrador(
    int idAdministrador,
    int idEstado = 0,
    int idCliente = 0,
    DateTime? desde = null,
    DateTime? hasta = null)
        {
            UsuarioNegocio usuarios = new UsuarioNegocio();

            if (!usuarios.esAdministradorActivo(idAdministrador))
            {
                throw new Exception(
                    "No tenés permiso para consultar los pedidos.");
            }

            List<Pedido> pedidos = new List<Pedido>();
            AccesoDatos datos = new AccesoDatos();

            try
            {
                string consulta =
    "SELECT P.Id, P.IdCliente, P.Fecha, P.Total, " +
    "P.IdEstado, E.Nombre AS NombreEstado, " +
    "P.ObservacionesInternas " +
    "FROM PEDIDOS P " +
    "INNER JOIN ESTADOS_PEDIDO E ON E.Id = P.IdEstado " +
    "WHERE 1 = 1 ";

                if (idEstado != 0)
                {
                    consulta += "AND P.IdEstado = @IdEstado ";
                    datos.setearParametro("@IdEstado", idEstado);
                }

                if (idCliente != 0)
                {
                    consulta += "AND P.IdCliente = @IdCliente ";
                    datos.setearParametro("@IdCliente", idCliente);
                }

                if (desde.HasValue)
                {
                    consulta += "AND P.Fecha >= @Desde ";
                    datos.setearParametro("@Desde", desde.Value.Date);
                }

                if (hasta.HasValue)
                {
                    consulta += "AND P.Fecha < @HastaExclusivo ";
                    datos.setearParametro(
                        "@HastaExclusivo", hasta.Value.Date.AddDays(1));
                }

                consulta += "ORDER BY P.Fecha DESC, P.Id DESC";

                datos.setearConsulta(consulta);

                datos.ejecutarLectura();

                while (datos.Lector.Read())
                {
                    Pedido pedido = new Pedido();

                    pedido.Id = (int)datos.Lector["Id"];
                    pedido.FechaCreacion = (DateTime)datos.Lector["Fecha"];
                    pedido.Total = (decimal)datos.Lector["Total"];
                    pedido.IdEstado = (int)datos.Lector["IdEstado"];
                    pedido.NombreEstado = (string)datos.Lector["NombreEstado"];
                    pedido.ObservacionesInternas =
    Convert.ToString(datos.Lector["ObservacionesInternas"]);

                    pedido.Cliente = new Usuario();
                    pedido.Cliente.Id = (int)datos.Lector["IdCliente"];

                    pedidos.Add(pedido);
                }

                return pedidos;
            }
            finally
            {
                datos.cerrarConexion();
            }
        }

        // Avanzamos un pedido a la siguiente etapa.
        public void avanzarEstado(
            int idPedido, int estadoActual, int idAdministrador)
        {
            UsuarioNegocio usuarios = new UsuarioNegocio();

            if (!usuarios.esAdministradorActivo(idAdministrador))
            {
                throw new Exception(
                    "No tenés permiso para modificar los pedidos.");
            }

            if (idPedido <= 0)
            {
                throw new Exception("El pedido no es válido.");
            }

            // Solo avanzamos desde Pagado, En preparación o Enviado.
            if (estadoActual < 2 || estadoActual > 4)
            {
                throw new Exception(
                    "El pedido no puede avanzar desde ese estado.");
            }

            int nuevoEstado = estadoActual + 1;
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.setearConsulta(
                    "UPDATE PEDIDOS SET IdEstado = @NuevoEstado " +
                    "WHERE Id = @IdPedido AND IdEstado = @EstadoActual");

                datos.setearParametro("@NuevoEstado", nuevoEstado);
                datos.setearParametro("@IdPedido", idPedido);
                datos.setearParametro("@EstadoActual", estadoActual);

                int filasAfectadas = datos.ejecutarAccion();

                if (filasAfectadas != 1)
                {
                    throw new Exception(
                        "El pedido cambió de estado o ya no está disponible. " +
                        "Actualizá la página.");
                }
            }
            finally
            {
                datos.cerrarConexion();
            }
        }

        // Guardamos la observación interna de un pedido.
        public void guardarObservaciones(
            int idPedido, string observaciones, int idAdministrador)
        {
            UsuarioNegocio usuarios = new UsuarioNegocio();

            if (!usuarios.esAdministradorActivo(idAdministrador))
            {
                throw new Exception(
                    "No tenés permiso para modificar los pedidos.");
            }

            if (idPedido <= 0)
            {
                throw new Exception("El pedido no es válido.");
            }

            // Si no recibimos texto, usamos una cadena vacía.
            if (observaciones == null)
            {
                observaciones = "";
            }

            observaciones = observaciones.Trim();

            if (observaciones.Length > 1000)
            {
                throw new Exception(
                    "La observación no puede superar los 1000 caracteres.");
            }

            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.setearConsulta(
                    "UPDATE PEDIDOS " +
                    "SET ObservacionesInternas = @Observaciones " +
                    "WHERE Id = @IdPedido");

                datos.setearParametro("@IdPedido", idPedido);
                datos.setearParametro("@Observaciones", observaciones);

                int filasAfectadas = datos.ejecutarAccion();

                if (filasAfectadas != 1)
                {
                    throw new Exception(
                        "El pedido no está disponible.");
                }
            }
            finally
            {
                datos.cerrarConexion();
            }
        }

        // Consultamos un pedido desde administración.
        public Pedido obtenerAdministrador(
            int idPedido, int idAdministrador)
        {
            UsuarioNegocio usuarios = new UsuarioNegocio();

            if (!usuarios.esAdministradorActivo(idAdministrador))
            {
                throw new Exception(
                    "No tenés permiso para consultar los pedidos.");
            }

            AccesoDatos datos = new AccesoDatos();
            int idCliente;
            string observaciones;

            try
            {
                datos.setearConsulta(
    "SELECT IdCliente, ObservacionesInternas " +
    "FROM PEDIDOS WHERE Id = @IdPedido");

                datos.setearParametro("@IdPedido", idPedido);
                datos.ejecutarLectura();

                if (!datos.Lector.Read())
                {
                    return null;
                }

                idCliente = (int)datos.Lector["IdCliente"];
                observaciones =
    Convert.ToString(datos.Lector["ObservacionesInternas"]);
            }
            finally
            {
                datos.cerrarConexion();
            }

            Pedido pedido = obtenerPorCliente(idPedido, idCliente);

            if (pedido != null)
            {
                pedido.Detalles =
                    listarDetallesPorCliente(idPedido, idCliente);
                pedido.ObservacionesInternas = observaciones;
            }

            return pedido;
        }
    }
}

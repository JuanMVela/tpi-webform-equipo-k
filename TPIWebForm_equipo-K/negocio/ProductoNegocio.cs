using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using dominio;

namespace negocio
{
    public class ProductoNegocio
    {
        public List<Producto> listar()
        {
            List<Producto> lista = new List<Producto>();
            AccesoDatos datos = new AccesoDatos();

            try
            {
                // Hacemos JOIN con Categorias y Marcas para traer los nombres
                datos.setearConsulta(
                    "SELECT P.Id, P.Codigo, P.Nombre, P.Descripcion, P.Precio, P.Stock, P.Activo, " +
                    "C.Id AS IdCategoria, C.Nombre AS NombreCategoria, " +
                    "M.Id AS IdMarca, M.Nombre AS NombreMarca " +
                    "FROM PRODUCTOS P " +
                    "INNER JOIN CATEGORIAS C ON P.IdCategoria = C.Id " +
                    "INNER JOIN MARCAS M ON P.IdMarca = M.Id"

                );

                datos.ejecutarLectura();

                while (datos.Lector.Read())
                {
                    Producto aux = new Producto();
                    aux.Id = (int)datos.Lector["Id"];
                    aux.Codigo = (string)datos.Lector["Codigo"];
                    aux.Nombre = (string)datos.Lector["Nombre"];
                    aux.Descripcion = (string)datos.Lector["Descripcion"];
                    aux.Precio = (decimal)datos.Lector["Precio"];
                    aux.Stock = (int)datos.Lector["Stock"];
                    aux.Activo = (bool)datos.Lector["Activo"];

                    // Mapeamos la Categoría
                    aux.Categoria = new Categoria();
                    aux.Categoria.Id = (int)datos.Lector["IdCategoria"];
                    aux.Categoria.Nombre = (string)datos.Lector["NombreCategoria"];

                    // Mapeamos la Marca
                    aux.Marca = new Marca();
                    aux.Marca.Id = (int)datos.Lector["IdMarca"];
                    aux.Marca.Nombre = (string)datos.Lector["NombreMarca"];

                    // NOTA: AUN FALTA LA IMPLEMENTACION DE IMAGENES.

                    lista.Add(aux);
                }
                return lista;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                datos.cerrarConexion();
            }
        }
        public List<Imagen> listarImagenes(int idProducto)
        {
            List<Imagen> lista = new List<Imagen>();
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearConsulta("SELECT Id, IdProducto, ImagenUrl FROM IMAGENES WHERE IdProducto = @IdProducto");
                datos.setearParametro("@IdProducto", idProducto);
                datos.ejecutarLectura();
                while (datos.Lector.Read())
                {
                    Imagen aux = new Imagen();
                    aux.Id = (int)datos.Lector["Id"];
                    aux.ImagenUrl = (string)datos.Lector["ImagenUrl"];
                    lista.Add(aux);
                }
                return lista;
            }
            catch (Exception) { throw; }
            finally { datos.cerrarConexion(); }
        }
        public void actualizarEstado(int id, bool activo)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearConsulta("UPDATE PRODUCTOS SET Activo = @Activo WHERE Id = @Id");
                datos.setearParametro("@Activo", activo);
                datos.setearParametro("@Id", id);
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
        public void agregar(Producto nuevo)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                // 1. Insertamos el producto
                datos.setearConsulta("INSERT INTO PRODUCTOS (Codigo, Nombre, Descripcion, Precio, Stock, IdCategoria, IdMarca, Activo) " +
                                     "VALUES (@Codigo, @Nombre, @Descripcion, @Precio, @Stock, @IdCategoria, @IdMarca, 1)");
                datos.setearParametro("@Codigo", nuevo.Codigo);
                datos.setearParametro("@Nombre", nuevo.Nombre);
                datos.setearParametro("@Descripcion", nuevo.Descripcion);
                datos.setearParametro("@Precio", nuevo.Precio);
                datos.setearParametro("@Stock", nuevo.Stock);
                datos.setearParametro("@IdCategoria", nuevo.Categoria.Id);
                datos.setearParametro("@IdMarca", nuevo.Marca.Id);
                datos.ejecutarAccion();
                datos.cerrarConexion();

                // 2. Obtenemos el ID del producto recién insertado
                datos = new AccesoDatos();
                datos.setearConsulta("SELECT MAX(Id) AS UltimoId FROM PRODUCTOS");
                datos.ejecutarLectura();

                int idProducto = 0;
                if (datos.Lector.Read())
                {
                    idProducto = (int)datos.Lector["UltimoId"];
                }
                datos.cerrarConexion();

                // 3. Insertamos las imágenes (si hay)
                foreach (Imagen img in nuevo.Imagenes)
                {
                    datos = new AccesoDatos();
                    datos.setearConsulta("INSERT INTO IMAGENES (IdProducto, ImagenUrl, EsPrincipal, Orden) VALUES (@IdProducto, @ImagenUrl, 0, 1)");
                    datos.setearParametro("@IdProducto", idProducto);
                    datos.setearParametro("@ImagenUrl", img.ImagenUrl);
                    datos.ejecutarAccion();
                    datos.cerrarConexion();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        public void modificar(Producto modificar)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearConsulta("UPDATE PRODUCTOS SET Codigo = @Codigo, Nombre = @Nombre, Descripcion = @Descripcion, " +
                                     "Precio = @Precio, Stock = @Stock, IdCategoria = @IdCategoria, IdMarca = @IdMarca " +
                                     "WHERE Id = @Id");
                datos.setearParametro("@Codigo", modificar.Codigo);
                datos.setearParametro("@Nombre", modificar.Nombre);
                datos.setearParametro("@Descripcion", modificar.Descripcion);
                datos.setearParametro("@Precio", modificar.Precio);
                datos.setearParametro("@Stock", modificar.Stock);
                datos.setearParametro("@IdCategoria", modificar.Categoria.Id);
                datos.setearParametro("@IdMarca", modificar.Marca.Id);
                datos.setearParametro("@Id", modificar.Id);
                datos.ejecutarAccion();

                datos = new AccesoDatos();
                datos.setearConsulta("DELETE FROM IMAGENES WHERE IdProducto = @IdProducto");
                datos.setearParametro("@IdProducto", modificar.Id);
                datos.ejecutarAccion();
                datos.cerrarConexion();

                foreach (Imagen img in modificar.Imagenes)
                {
                    datos = new AccesoDatos();
                    datos.setearConsulta("INSERT INTO IMAGENES (IdProducto, ImagenUrl, EsPrincipal, Orden) VALUES (@IdProducto, @ImagenUrl, 0, 1)");
                    datos.setearParametro("@IdProducto", modificar.Id);
                    datos.setearParametro("@ImagenUrl", img.ImagenUrl);
                    datos.ejecutarAccion();
                    datos.cerrarConexion();
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                datos.cerrarConexion();
            }
        }
        public List<Producto> listarActivo()
        {
            List<Producto> lista = new List<Producto>();
            AccesoDatos datos = new AccesoDatos();

            try
            {
                // Hacemos JOIN con Categorias y Marcas para traer los nombres
                datos.setearConsulta(
                    "SELECT P.Id, P.Codigo, P.Nombre, P.Descripcion, P.Precio, P.Stock, P.Activo, " +
                    "C.Id AS IdCategoria, C.Nombre AS NombreCategoria, " +
                    "M.Id AS IdMarca, M.Nombre AS NombreMarca " +
                    "FROM PRODUCTOS P " +
                    "INNER JOIN CATEGORIAS C ON P.IdCategoria = C.Id " +
                    "INNER JOIN MARCAS M ON P.IdMarca = M.Id " +
                    "WHERE P.Activo = 1"

                );

                datos.ejecutarLectura();

                while (datos.Lector.Read())
                {
                    Producto aux = new Producto();
                    aux.Id = (int)datos.Lector["Id"];
                    aux.Codigo = (string)datos.Lector["Codigo"];
                    aux.Nombre = (string)datos.Lector["Nombre"];
                    aux.Descripcion = (string)datos.Lector["Descripcion"];
                    aux.Precio = (decimal)datos.Lector["Precio"];
                    aux.Stock = (int)datos.Lector["Stock"];
                    aux.Activo = (bool)datos.Lector["Activo"];

                    // Mapeamos la Categoría
                    aux.Categoria = new Categoria();
                    aux.Categoria.Id = (int)datos.Lector["IdCategoria"];
                    aux.Categoria.Nombre = (string)datos.Lector["NombreCategoria"];

                    // Mapeamos la Marca
                    aux.Marca = new Marca();
                    aux.Marca.Id = (int)datos.Lector["IdMarca"];
                    aux.Marca.Nombre = (string)datos.Lector["NombreMarca"];

                    // NOTA: AUN FALTA LA IMPLEMENTACION DE IMAGENES.

                    lista.Add(aux);
                }
                return lista;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                datos.cerrarConexion();
            }
        }

    }
}

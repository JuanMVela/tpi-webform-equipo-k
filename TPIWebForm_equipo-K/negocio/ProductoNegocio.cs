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
                    "INNER JOIN MARCAS M ON P.IdMarca = M.Id " +
                    "WHERE P.Activo = 1" // Solo traemos los productos activos
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
    }
}

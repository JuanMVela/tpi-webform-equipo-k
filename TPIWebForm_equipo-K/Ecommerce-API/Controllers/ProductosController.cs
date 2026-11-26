using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Ecommerce_API.Models;
using dominio;
using negocio;

namespace Ecommerce_API.Controllers
{
    public class ProductosController : ApiController
    {
        // GET: api/Productos
        public IEnumerable<Producto> Get()
        {
            ProductoNegocio negocio = new ProductoNegocio();
            List<Producto> productos = negocio.listarActivo();

            foreach (var prod in productos)
            {
                prod.Imagenes = negocio.listarImagenes(prod.Id);
            }

            return productos;
        }

        // GET: api/Productos/5
        public Producto Get(int id)
        {
            ProductoNegocio negocio = new ProductoNegocio();
            List<Producto> lista = negocio.listarActivo();
            return lista.Find(x => x.Id == id);
        }

        // POST: api/Productos
        public void Post([FromBody] ProductoDto nuevoDto)
        {
            ProductoNegocio negocio = new ProductoNegocio();
            Producto nuevo = new Producto();

            // Mapeo los datos del DTO al Producto real
            nuevo.Nombre = nuevoDto.Nombre;
            nuevo.Codigo = nuevoDto.Codigo;
            nuevo.Descripcion = nuevoDto.Descripcion;
            nuevo.Precio = nuevoDto.Precio;
            nuevo.Stock = nuevoDto.Stock;
            nuevo.Marca = new Marca { Id = nuevoDto.IdMarca };
            nuevo.Categoria = new Categoria { Id = nuevoDto.IdCategoria };
            nuevo.Activo = true;

            // Ahora las imagenes
            nuevo.Imagenes = new List<Imagen>();
            if (nuevoDto.Imagenes != null)
            {
                foreach (var url in nuevoDto.Imagenes)
                {
                    nuevo.Imagenes.Add(new Imagen { ImagenUrl = url });
                }
            }
            negocio.agregar(nuevo);
        }

        // PUT: api/Productos/5
        public void Put(int id, [FromBody] ProductoDto nuevoDto)
        {
            ProductoNegocio negocio = new ProductoNegocio();
            Producto modificado = new Producto();

            modificado.Id = id;
            modificado.Nombre = nuevoDto.Nombre;
            modificado.Codigo = nuevoDto.Codigo;
            modificado.Descripcion = nuevoDto.Descripcion;
            modificado.Precio = nuevoDto.Precio;
            modificado.Stock = nuevoDto.Stock;
            modificado.Marca = new Marca { Id = nuevoDto.IdMarca };
            modificado.Categoria = new Categoria { Id = nuevoDto.IdCategoria };

            // Mapeo las imagenes
            modificado.Imagenes = new List<Imagen>();
            if (nuevoDto.Imagenes != null)
            {
                foreach (var url in nuevoDto.Imagenes)
                {
                    modificado.Imagenes.Add(new Imagen { ImagenUrl = url });
                }
            }
            negocio.modificar(modificado);
        }

        // DELETE: api/Productos/5
        public void Delete(int id)
        {
            ProductoNegocio negocio = new ProductoNegocio();
            negocio.actualizarEstado(id, false);
        }
    }
}

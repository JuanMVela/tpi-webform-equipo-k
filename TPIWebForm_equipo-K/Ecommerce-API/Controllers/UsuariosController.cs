using dominio;
using Ecommerce_API.Models;
using negocio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Ecommerce_API.Controllers
{
    public class UsuariosController : ApiController
    {
        // GET:
        public IEnumerable<Usuario> Get()
        {
            UsuarioNegocio negocio = new UsuarioNegocio();
            return negocio.listarTodos();
        }

        // GET: 
        public Usuario Get(int id)
        {
            UsuarioNegocio negocio = new UsuarioNegocio();
            List<Usuario> lista = negocio.listarTodos();
            return lista.Find(x => x.Id == id);
        }

        // POST:
        public void Post([FromBody] UsuarioDto nuevoDto)
        {
            UsuarioNegocio negocio = new UsuarioNegocio();

            Usuario nuevo = new Usuario();
            nuevo.Nombre = nuevoDto.Nombre;
            nuevo.Apellido = nuevoDto.Apellido;
            nuevo.Email = nuevoDto.Email;
            nuevo.Telefono = nuevoDto.Telefono;
            nuevo.PasswordHash = nuevoDto.Password;
            nuevo.Perfil = new Perfil { Id = 1 }; // 1 = Cliente
            nuevo.Activo = true;

            negocio.agregar(nuevo);
        }

        // PUT:
        public void Put(int id, [FromBody] UsuarioDto modificarDto)
        {
            UsuarioNegocio negocio = new UsuarioNegocio();

            Usuario usuarioExistente = negocio.listarTodos().Find(x => x.Id == id);

            if (usuarioExistente != null)
            {
                usuarioExistente.Nombre = modificarDto.Nombre;
                usuarioExistente.Apellido = modificarDto.Apellido;
                usuarioExistente.Email = modificarDto.Email;
                usuarioExistente.Telefono = modificarDto.Telefono;

                negocio.actualizarDatosPersonales(usuarioExistente);
            }
        }

        // DELETE:
        public void Delete(int id)
        {
            UsuarioNegocio negocio = new UsuarioNegocio();
            negocio.cambiarEstado(id, false);
        }
    }
}

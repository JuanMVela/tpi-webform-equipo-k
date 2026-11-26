using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ecommerce_API.Models
{
    public class ProductoDto
    {
        public string Nombre { get; set; }
        public string Codigo { get; set; }
        public string Descripcion { get; set; }
        public decimal Precio { get; set; }
        public int Stock { get; set; }
        public int IdMarca { get; set; }
        public int IdCategoria { get; set; }
        public List<string> Imagenes { get; set; }
    }
}
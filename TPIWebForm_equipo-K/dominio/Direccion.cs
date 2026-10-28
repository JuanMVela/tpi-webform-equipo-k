using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dominio
{
    internal class Direccion
    {
        public int Id { get; set; }
        public int IdUsuario { get; set; }
        public string Calle { get; set; }
        public string Numero { get; set; }
        public string Localidad { get; set; }
        public string CodigoPostal { get; set; }
        public string Observaciones { get; set; }
        public bool Activo { get; set; }
    }
}

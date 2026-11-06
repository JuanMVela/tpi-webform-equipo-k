using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dominio
{
    public class FormaEntrega
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public bool RequiereDireccion { get; set; }
        public bool Activo { get; set; }

        // Devuelve el nombre de la forma de entrega
        public override string ToString()
        {
            return Nombre;
        }
    }
}

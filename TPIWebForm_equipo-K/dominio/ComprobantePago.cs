using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dominio
{
    // Representa el comprobante cargado para un pedido.
    public class ComprobantePago
    {
        public int Id { get; set; }
        public int IdPedido { get; set; }
        public string NombreArchivo { get; set; }
        public string TipoContenido { get; set; }
        public byte[] Archivo { get; set; }
        public DateTime FechaCarga { get; set; }
        public string Estado { get; set; }
    }
}

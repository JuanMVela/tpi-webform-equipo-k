using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dominio
{
    // Representa una compra y sus productos.
    public class Pedido
    {
        public int Id { get; set; }
        public Usuario Cliente { get; set; }
        public DateTime FechaCreacion { get; set; }
        public decimal Total { get; set; }
        public FormaPago FormaPago { get; set; }
        public FormaEntrega FormaEntrega { get; set; }
        public Direccion Direccion { get; set; }
        public int IdEstado { get; set; }

        public List<DetallePedido> Detalles { get; set; }

        public Pedido()
        {
            Detalles = new List<DetallePedido>();
        }
    }
}
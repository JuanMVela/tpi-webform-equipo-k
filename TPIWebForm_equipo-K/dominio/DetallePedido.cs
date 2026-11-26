using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dominio
{
    // Representa un producto dentro de un pedido.
    public class DetallePedido
    {
        public Producto Producto { get; set; }
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
        // Calculamos el importe de este producto.
        public decimal Subtotal
        {
            get
            {
                return PrecioUnitario * Cantidad;
            }
        }
    }
}

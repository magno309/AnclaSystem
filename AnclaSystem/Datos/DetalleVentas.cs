using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelo
{
    public class DetalleVentas
    {
        public int ID_PROD { get; set; }
        public int ID_VENT { get; set; }
        public String NOMBRE_AUX { get; set; }
        public int CANTIDAD { get; set; }
        public double PRECIO_VENTA { get; set; }
        public double SUBTOTAL { get; set; }
    }
}

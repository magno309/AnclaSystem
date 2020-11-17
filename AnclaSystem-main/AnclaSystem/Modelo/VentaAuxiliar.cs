using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelo
{
    /// <summary>
    /// POJO VENTA AUXILIAR, PARA AYUDAR A DESCONTAR INVENTARIO
    /// </summary>
    public class VentaAuxiliar
    {
        public int ID_inv { get; set; }
        public int cantidad { get; set; }
        public int stock_actual { get; set; }
        public int cant { get; set; }

        public VentaAuxiliar(int cant, int iD_inv, int cantidad, int stock_actual)
        {
            this.cant = cant;
            this.ID_inv = iD_inv;
            this.cantidad = cantidad;
            this.stock_actual = stock_actual;
        }
    }
}

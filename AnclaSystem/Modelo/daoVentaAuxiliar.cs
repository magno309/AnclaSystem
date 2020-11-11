using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelo
{
    public class daoVentaAuxiliar
    {
        public int ID_inv { get; set; }
        public int cantidad { get; set; }
        public int stock_actual { get; set; }
        public int cant { get; set; }

        public daoVentaAuxiliar(int cant, int iD_inv, int cantidad, int stock_actual)
        {
            this.cant = cant;
            this.ID_inv = iD_inv;
            this.cantidad = cantidad;
            this.stock_actual = stock_actual;
        }
    }
}

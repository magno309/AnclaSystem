using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelo
{
    public class Ventas
    {
        public int ID { get; set; }
        public double TOTAL { get; set; }
        public String FECHA{ get; set; }
        public int ID_CAJERO { get; set; }

        public String NOMBRE_CAJERO { get; set; }

        public Ventas() { }

        public Ventas(int iD, double tOTAL, string fECHA, int iD_CAJERO)
        {
            ID = iD;
            TOTAL = tOTAL;
            FECHA = fECHA;
            ID_CAJERO = iD_CAJERO;
        }
    }
}

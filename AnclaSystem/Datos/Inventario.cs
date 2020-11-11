using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datos
{
    /// <summary>
    /// POJO INVENTARIO
    /// </summary>
    public class Inventario
    {
        public int ID { get; set; }
        public String nombre { get; set; }
        public String unidad { get; set; }
        public int stock { get; set; }
        public Boolean descontinuado { get; set; }
    }
}

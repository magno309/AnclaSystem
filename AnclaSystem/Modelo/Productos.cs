using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelo
{
    /// <summary>
    /// POJO PRODUCTOS
    /// </summary>
    public class Productos
    {
        public int ID { get; set; }

        public String nombre { get; set; }

        public double precio { get; set; }

        public Boolean descontinuado { get; set; }

        public Productos(int iD, string nombre, double precio, bool descontinuado)
        {
            ID = iD;
            this.nombre = nombre;
            this.precio = precio;
            this.descontinuado = descontinuado;
        }

        public Productos(int iD, string nombre, double precio)
        {
            ID = iD;
            this.nombre = nombre;
            this.precio = precio;
        }

        public Productos(string nombre, double precio, bool descontinuado)
        {
            this.nombre = nombre;
            this.precio = precio;
            this.descontinuado = descontinuado;
        }
    }
}

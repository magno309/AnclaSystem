using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelo
{
    public class Ingrediente
    {
        public int IdIngrediente { get; }
        public string Nombre { get; set; }
        public string Unidad { get; set; }
        public int Stock { get; set; }
        public bool Descontinuado { get; set; }

        public Ingrediente(int idIngrediente, string nombre, string unidad, int stock, bool descontinuado)
        {
            IdIngrediente = idIngrediente;
            Nombre = nombre;
            Unidad = unidad;
            Stock = stock;
            Descontinuado = descontinuado;
        }
    }
}

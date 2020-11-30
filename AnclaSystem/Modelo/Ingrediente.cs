using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelo
{
    public class Ingrediente
    {
        public int IdIngrediente { get; set; }
        public string Nombre { get; set; }
        public string Unidad { get; set; }
        public int Stock { get; set; }
        public bool Descontinuado { get; set; }

        public Ingrediente() { }

        public Ingrediente(int idIngrediente, string nombre, string unidad, int stock, bool descontinuado)
        {
            IdIngrediente = idIngrediente;
            Nombre = nombre;
            Unidad = unidad;
            Stock = stock;
            Descontinuado = descontinuado;
        }

        public Ingrediente(string nombre, string unidad, int stock, bool descontinuado)
        {
            Nombre = nombre;
            Unidad = unidad;
            Stock = stock;
            Descontinuado = descontinuado;
        }

        public override bool Equals(object obj)
        {
            return obj is Ingrediente ingrediente &&
                   IdIngrediente == ingrediente.IdIngrediente &&
                   Nombre == ingrediente.Nombre &&
                   Unidad == ingrediente.Unidad &&
                   Stock == ingrediente.Stock &&
                   Descontinuado == ingrediente.Descontinuado;
        }
    }
}

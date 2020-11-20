using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Modelo;
using MySql.Data.MySqlClient;

namespace Datos
{
    public class daoIngredientes
    {

        public List<Ingrediente> obtenerTodos() {
            List<Ingrediente> listaIngredientes = new List<Ingrediente>();
            Conexion cn = new Conexion();
            string query = "SELECT * FROM INVENTARIO WHERE DESCONTINUADO = 0;";
            foreach (List<object> fila in cn.ejecutarConsulta(query)) {
                listaIngredientes.Add(
                        new Ingrediente(
                                int.Parse(fila[0].ToString()),
                                fila[1].ToString(),
                                fila[2].ToString(),
                                int.Parse(fila[3].ToString()),
                                bool.Parse(fila[4].ToString())
                            )
                    );
            }
            return listaIngredientes;
        }

    }
}

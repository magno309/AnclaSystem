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

        public bool agregar(Ingrediente ingrediente)
        {
            Conexion cn = new Conexion();
            try
            {
                string query = "Insert into INVENTARIO (NOMBRE, UNIDAD, STOCK, DESCONTINUADO) values (@NOMBRE, @UNIDAD, @STOCK, @DESCONTINUADO);";
                MySqlCommand comando = new MySqlCommand(query);
                comando.Parameters.AddWithValue("NOMBRE", ingrediente.Nombre);
                comando.Parameters.AddWithValue("UNIDAD", ingrediente.Unidad);
                comando.Parameters.AddWithValue("STOCK", ingrediente.Stock);
                comando.Parameters.AddWithValue("DESCONTINUADO", ingrediente.Descontinuado);
                return cn.ejecutarSentencia(comando);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public bool descontinuar(int idIngregiente)
        {
            Conexion cn = new Conexion();
            try
            {
                string query = "Update INVENTARIO set DESCONTINUADO = 0 where ID = @ID_INGREDIENTE;";
                MySqlCommand comando = new MySqlCommand(query);
                comando.Parameters.AddWithValue("ID_INGREDIENTE", idIngregiente);
                return cn.ejecutarSentencia(comando);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public bool actualizar(Ingrediente ingrediente)
        {
            Conexion cn = new Conexion();
            try
            {
                string query = "Update INVENTARIO set NOMBRE = @NOMBRE, UNIDAD = @UNIDAD, STOCK = @STOCK, DESCONTINUADO = @DESCONTINUADO where ID = @ID_INGREDIENTE;";
                MySqlCommand comando = new MySqlCommand(query);
                comando.Parameters.AddWithValue("ID_INGREDIENTE", ingrediente.IdIngrediente);
                comando.Parameters.AddWithValue("NOMBRE", ingrediente.Nombre);
                comando.Parameters.AddWithValue("UNIDAD", ingrediente.Unidad);
                comando.Parameters.AddWithValue("STOCK", ingrediente.Stock);
                comando.Parameters.AddWithValue("DESCONTINUADO", ingrediente.Descontinuado);
                return cn.ejecutarSentencia(comando);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public List<Ingrediente> obtenerTodos() {
            List<Ingrediente> listaIngredientes = new List<Ingrediente>();
            Conexion cn = new Conexion();
            try
            {
                string query = "Select * from INVENTARIO where DESCONTINUADO = 0;";
                foreach (List<object> fila in cn.ejecutarConsulta(query))
                {
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
            catch (Exception e)
            {
                throw e;
            }                
        }

        public Dictionary<Ingrediente, int> obtenerTodosPorProducto(int idProducto)
        {
            Dictionary<Ingrediente, int> listaIngredientes = new Dictionary<Ingrediente, int>();
            Conexion cn = new Conexion();
            try
            {
                MySqlCommand cmd = new MySqlCommand();
                cmd.CommandText = "SELECT i.ID, i.NOMBRE, i.UNIDAD, i.STOCK, i.DESCONTINUADO, dp.CANTIDAD FROM INVENTARIO i JOIN DETALLE_PRODUCTOS dp " +
                                "WHERE i.ID = dp.ID_INV AND i.DESCONTINUADO = 0 AND dp.ID_PROD = @idProducto;";
                cmd.Parameters.AddWithValue("@idProducto", idProducto);
                foreach (List<object> fila in cn.ejecutarConsulta(cmd))
                {
                    listaIngredientes.Add(
                            new Ingrediente(
                                    int.Parse(fila[0].ToString()),
                                    fila[1].ToString(),
                                    fila[2].ToString(),
                                    int.Parse(fila[3].ToString()),
                                    bool.Parse(fila[4].ToString())
                                ),
                            int.Parse(fila[5].ToString())
                        );
                }
                return listaIngredientes;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

    }
}

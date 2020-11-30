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
                string query = "Insert into INVENTARIO (NOMBRE, UNIDAD, STOCK, DESCONTINUADO) values (upper(@NOMBRE), @UNIDAD, @STOCK, @DESCONTINUADO);";
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
                string query = "Update INVENTARIO set DESCONTINUADO = 1 where ID = @ID_INGREDIENTE;";
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
                string query = "Update INVENTARIO set NOMBRE = upper(@NOMBRE), UNIDAD = @UNIDAD, STOCK = @STOCK, DESCONTINUADO = @DESCONTINUADO where ID = @ID_INGREDIENTE;";
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

        public List<Ingrediente> obtenerTodos()
        {
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

        public List<Ingrediente> buscarPatron(String nombre, String unidad, int stock)
        {
            List<Ingrediente> listaIngredientes = new List<Ingrediente>();
            Conexion cn = new Conexion();
            try
            {
                string query = "select * from INVENTARIO where (lower(NOMBRE) LIKE lower(@NOMBRE) or UNIDAD like @UNIDAD or STOCK like @STOCK) and DESCONTINUADO = 0;";
                MySqlCommand comando = new MySqlCommand(query);
                comando.Parameters.AddWithValue("@NOMBRE", nombre.Equals("")? "%\"\"%" : "%"+nombre+"%");
                comando.Parameters.AddWithValue("@UNIDAD", unidad.Equals("")? "%\"\"%" : "%"+unidad+"%");
                comando.Parameters.AddWithValue("@STOCK", stock==-1? "%\"\"%" : "%"+stock+"%");
                
                foreach (List<object> fila in cn.ejecutarConsulta(comando))
                {
                    Ingrediente ingrediente = new Ingrediente();
                    ingrediente.IdIngrediente = int.Parse(fila[0].ToString());
                    ingrediente.Nombre = fila[1].ToString();
                    ingrediente.Unidad = fila[2].ToString();
                    ingrediente.Stock = int.Parse(fila[3].ToString());
                    ingrediente.Descontinuado = bool.Parse(fila[4].ToString());
                    listaIngredientes.Add(ingrediente);
                }

                return listaIngredientes;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public bool nombreDuplicado(string nombre)
        {
            Conexion cn = new Conexion();
            try
            {
                string query = "select * from INVENTARIO where lower(NOMBRE) LIKE lower(@NOMBRE)and DESCONTINUADO = 0;";
                MySqlCommand comando = new MySqlCommand(query);
                comando.Parameters.AddWithValue("@NOMBRE", nombre);                
                return cn.ejecutarConsulta(comando).Count>0;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public Dictionary<Ingrediente, int> ObtenerTodosPorProducto(int idProducto) {
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
            catch (Exception e) {
                throw e;
            }
        }

    }
}


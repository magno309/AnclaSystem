using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Modelo;
using MySql.Data.MySqlClient;

namespace Datos
{
    public class daoProducto
    {
        /// <summary>
        /// Metodo que se manda llamar para llenar la tabla de productos en registrar venta.
        /// </summary>
        /// <returns>Lista de productos que no estan descontinuados.</returns>
        public List<Productos> getProductosNoDescontinuados()
        {
            ///CREAR CONEXION, MODIFICARLA, USARLA
            String server = "localhost";
            String port = "8457";
            String database = "ANCLA";
            String uid = "root";
            String password = "root";
            string connectionString;
            connectionString = "SERVER=" + server + ";" + "DATABASE=" +
                                database + ";" + "UID=" + uid + ";" + "PASSWORD=" + password + ";";

            List<Productos> productos = new List<Productos>();
            MySqlConnection cn = new MySqlConnection(connectionString);

                try
                {                              
                cn.Open();
                    ///EJECUTAR COMANDO
                    string strSQL = "SELECT * FROM PRODUCTOS WHERE DESCONTINUADO = FALSE";
                    MySqlCommand comando = new MySqlCommand(strSQL, cn);
                    MySqlDataReader dr = comando.ExecuteReader();
                    while (dr.Read())
                    {
                    Productos producto = new Productos(dr.GetInt32("ID"), dr.GetString("NOMBRE"), dr.GetDouble("PRECIO"));
                    
                    productos.Add(producto);
                    }
                    comando.Dispose();
                    return productos;
                }
                catch (Exception e)
                {
                    throw e;
                }
                finally
                {
                    ///CERRAR CONEXION
                    cn.Close();
                    cn.Dispose();
                }
            }
        }
    }

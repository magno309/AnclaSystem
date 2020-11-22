using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Modelo;
using MySql.Data.MySqlClient;

namespace Datos
{
    /// <summary>
    /// CLASE ACCESO A DATOS PRODUCTO
    /// </summary>
    public class daoProducto
    { 
        /// <summary>
        /// Metodo que se manda llamar para llenar la tabla de productos en registrar venta.
        /// </summary>
        /// <returns>Lista de productos que no estan descontinuados.</returns>
        public List<Productos> getProductosNoDescontinuados()
        {
            List<Productos> productos = new List<Productos>();
            Conexion cn = new Conexion();

            try
            {
                ///EJECUTAR COMANDO SELECT PARA OBTENER PRODUCTOS NO DESCONTINUADOS
                string strSQL = "SELECT * FROM PRODUCTOS WHERE DESCONTINUADO = FALSE";
               
                foreach (List<object> fila in cn.ejecutarConsulta(strSQL))
                {
                    Productos producto = new Productos(int.Parse(fila[0]+""), fila[1]+"", double.Parse(fila[2]+""));
                    productos.Add(producto);
                }

                return productos;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public bool AgregarProducto(Productos nuevo, Dictionary<int, int> listaIngredientes) {
            MySqlConnection cn = new Conexion().getConexion();
            MySqlTransaction transaction = cn.BeginTransaction();
            try {
                MySqlCommand insertarProducto = new MySqlCommand();
                insertarProducto.Connection = cn;
                insertarProducto.Transaction = transaction;
                insertarProducto.CommandText = "INSERT INTO PRODUCTOS (NOMBRE, PRECIO, DESCONTINUADO) VALUES " +
                    "(@Nombre, @Precio, @Descontinuado);";
                insertarProducto.Parameters.AddWithValue("@Nombre", nuevo.nombre);
                insertarProducto.Parameters.AddWithValue("@Precio", nuevo.precio);
                insertarProducto.Parameters.AddWithValue("@Descontinuado", nuevo.descontinuado);
                var idProducto = insertarProducto.ExecuteScalar();
                foreach (var map in listaIngredientes) {
                    MySqlCommand insertarDetalle = new MySqlCommand();
                    insertarDetalle.Connection = cn;
                    insertarDetalle.Transaction = transaction;
                    insertarDetalle.CommandText = "INSERT INTO DETALLES_PRODUCTO VALUES " +
                        "(@IdProducto, @IdIngrediente, @Cantidad);";
                    insertarDetalle.Parameters.AddWithValue("@IdProducto", idProducto);
                    insertarDetalle.Parameters.AddWithValue("@IdIngrediente", map.Key);
                    insertarDetalle.Parameters.AddWithValue("@Cantidad", map.Value);
                    insertarDetalle.ExecuteNonQuery();
                }
                transaction.Commit();
                return true;
            }
            catch (Exception e)
            {
                try
                {
                    transaction.Rollback();
                    return false;
                }
                catch (SqlException ex)
                {
                    if (transaction.Connection != null)
                    {
                        Console.WriteLine("An exception of type " + ex.GetType() +
                        " was encountered while attempting to roll back the transaction.");
                    }
                    return false;
                }
            }
            finally
            {
                cn.Close();
                cn.Dispose();
            }
        }
    }
}

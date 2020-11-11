using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Modelo;
using MySql.Data.MySqlClient;

namespace Datos
{
    /// <summary>
    /// Clase de acceso a datos para la entidad Venta.
    /// </summary>
    public class daoVentas
    {

        /// <summary>
        /// MODIFICAR CAMPOS PARA LA CONEXION A MYSQL
        /// </summary>
        static String server = "localhost";
        static String port = "8457";
        static String database = "ANCLA";
        static String uid = "root";
        static String password = "root";
        static string connectionString = "SERVER=" + server + ";" + "DATABASE=" +
                                database + ";" + "UID=" + uid + ";" + "PASSWORD=" + password + ";";

        /// <summary>
        /// Metodo que registra una venta en la base de datos. Sus detalles y descuenta del inventario los productos vendidos.
        /// </summary>
        /// <param name="nuevaVenta">pojo que contiene los datos de venta</param>
        /// <param name="detalleVenta">lista con los detalles de venta</param>
        /// <returns></returns>
        public bool agregarVenta(Ventas nuevaVenta, List<DetalleVentas> detalleVenta)
        {
            MySqlConnection cn = new MySqlConnection(connectionString);
            cn.Open();

            ///INICIAR TRANSACCION
            MySqlTransaction trans = cn.BeginTransaction();

            try
            {
                ///INSERTAR VENTA A LA BASE DE DATOS
                string strSQL = "insert into VENTAS(TOTAL, FECHA, ID_CAJERO) values (@TOTAL, @FECHA, @ID_CAJERO)";
                MySqlCommand comando = new MySqlCommand(strSQL, cn);
                comando.Parameters.AddWithValue("TOTAL", nuevaVenta.TOTAL);
                comando.Parameters.AddWithValue("FECHA", nuevaVenta.FECHA);
                comando.Parameters.AddWithValue("ID_CAJERO", nuevaVenta.ID_CAJERO);
                comando.ExecuteNonQuery();
                comando.Dispose();

                ///OBTENER EL ID DE LA VENTA RECIENTEMENTE REGISTRADA
                string strSQL2 = "select MAX(ID) from VENTAS";
                MySqlCommand comando2 = new MySqlCommand(strSQL2, cn);
                MySqlDataReader dr = comando2.ExecuteReader();

                if (dr.Read())
                {
                    int indice = dr.GetInt32("MAX(ID)");
                    comando2.Dispose();
                    dr.Dispose();

                    ///INSERTAR LOS DETALLES DE VENTA CON EL ID OBTENIDO CON ANTERIORIDAD
                    for (int i = 0; i < detalleVenta.Count; i++)
                    {
                        string strSQL1 = "insert into DETALLE_VENTAS values (@ID_PROD, @ID_VENT, @CANTIDAD, @PRECIO_VENTA)";
                        MySqlCommand comando1 = new MySqlCommand(strSQL1, cn);
                        comando1.Parameters.AddWithValue("ID_PROD", detalleVenta[i].ID_PROD);
                        comando1.Parameters.AddWithValue("ID_VENT", indice);
                        comando1.Parameters.AddWithValue("CANTIDAD", detalleVenta[i].CANTIDAD);
                        comando1.Parameters.AddWithValue("PRECIO_VENTA", detalleVenta[i].PRECIO_VENTA);
                        comando1.ExecuteNonQuery();
                        comando1.Dispose();
                    }
                }

                ///MODIFICAR EL INVENTARIO, DESCONTAR LOS INGREDIENTES DE LOS PRODUCTOS VENDIDOS
                List<List<VentaAuxiliar>> listas = new List<List<VentaAuxiliar>>(); //lista de listas porque no me deja tener dos datareaders
                MySqlDataReader dr1 = null;

                foreach (DetalleVentas prod in detalleVenta)
                {
                    ///OBTENER EL ID DEL INVENARIO, LA CANTIDAD DE INGREDIENTES Y EL STOCK ACTUAL DE CADA PRODUCTO VENDIDO
                    List<VentaAuxiliar> ingredientes = new List<VentaAuxiliar>();
                    string strSQL3 = "SELECT DP.ID_INV, DP.CANTIDAD, I.STOCK FROM DETALLE_PRODUCTOS AS DP JOIN PRODUCTOS AS P ON" +
                        " DP.ID_PROD = P.ID JOIN INVENTARIO AS I ON DP.ID_INV = I.ID WHERE DP.ID_PROD = @ID_PRODU";
                    MySqlCommand comando3 = new MySqlCommand(strSQL3, cn);
                    comando3.Parameters.AddWithValue("@ID_PRODU", prod.ID_PROD);
                    dr1 = comando3.ExecuteReader();

                    while (dr1.Read())
                    {
                        ingredientes.Add(new VentaAuxiliar(prod.CANTIDAD, dr1.GetInt32("ID_INV"), dr1.GetInt32("CANTIDAD"), dr1.GetInt32("STOCK")));
                    }

                    ///GUARDAR CADA DATO POR CADA PRODUCTO
                    listas.Add(ingredientes);
                    dr1.Dispose();
                }

                ///MODIFICAR EL INVENTARIO
                int nuevo_inv = 0;
                foreach (List<VentaAuxiliar> lista in listas)
                {
                    foreach (VentaAuxiliar aux in lista)
                    {
                        string strSQL4 = "UPDATE INVENTARIO SET STOCK = @DESC WHERE ID = @ID_INV";
                        MySqlCommand comando4 = new MySqlCommand(strSQL4, cn);

                        ///CALCULAR EL NUEVO STOCK
                        nuevo_inv = aux.stock_actual - (aux.cantidad * aux.cant);
                        if (nuevo_inv < 0)
                        {
                            nuevo_inv = 0;
                        }

                        comando4.Parameters.AddWithValue("DESC", nuevo_inv);
                        comando4.Parameters.AddWithValue("ID_INV", aux.ID_inv);
                        comando4.ExecuteNonQuery();
                        comando4.Dispose();
                    }
                }

                ///TERMINAR TRANSACCION
                trans.Commit();
                return true;
            }
            catch (Exception ex)
            {
                ///SI HAY ERROR DESHACER LO QUE SE LLEVARA EN LA TRANSACCION Y LANZAR EXCEPCION
                trans.Rollback();
                throw ex;
            }
            finally
            {
                ///CERRAR TODO
                cn.Close();
                cn.Dispose();
            }
        }

        public int getMax()
        {
            int max = 0;
            /// CREAR LA CONEXIÓN, CONFIGURAR Y ABRIRLA
            MySqlConnection cn = new MySqlConnection(connectionString);

            try
            {
                cn.Open();

                ///OBTENER EL ID DE LA VENTA RECIENTEMENTE REGISTRADA
                string strSQL2 = "select MAX(ID) from VENTAS";
                MySqlCommand comando2 = new MySqlCommand(strSQL2, cn);
                MySqlDataReader dr = comando2.ExecuteReader();

                if (dr.Read())
                {
                    max = dr.GetInt32("MAX(ID)");
                    comando2.Dispose();
                }
                dr.Dispose();
                return max;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                cn.Close();
                cn.Dispose();
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Modelo;
using MySql.Data.MySqlClient;

namespace Modelo
{
    public class daoVentas
    {
        public bool agregarVenta(Ventas nuevaVenta, List<DetalleVentas> detalleVenta)
        {
            MySqlConnection conexion = new MySqlConnection();
            MySqlTransaction trans = conexion.BeginTransaction();
            try
            {
                String server = "25.89.125.13";
                String database = "ANCLA";
                String uid = "remoto";
                String password = "remoto1";
                String port = "8457";
                String connectionString = "SERVER=" + server + "; PORT =" + port + ";" + "DATABASE="
                    + database + ";" + "UID=" + uid + ";" + "PASSWORD=" + password + ";";
                conexion.Open();
                string strSQL = "insert into VENTAS values (ID, @TOTAL, @FECHA)";
                MySqlCommand comando = new MySqlCommand(strSQL, conexion);
                comando.Parameters.AddWithValue("TOTAL", nuevaVenta.TOTAL);
                comando.Parameters.AddWithValue("FECHA", nuevaVenta.FECHA);
                comando.ExecuteNonQuery();
                comando.Dispose();

                string strSQL2 = "select MAX(ID) from VENTAS";
                MySqlCommand comando2 = new MySqlCommand(strSQL2, conexion);
                MySqlDataReader dr = comando2.ExecuteReader();
                int indice = dr.GetInt32("MAX(ID)");
                comando2.Dispose();

                for (int i = 0; i < detalleVenta.Count; i++)
                {
                    string strSQL1 = "insert into DETALLE_VENTAS values (@ID_PROD, @ID_VENT, @CANTIDAD, @PRECIO_VENTA)";
                    MySqlCommand comando1 = new MySqlCommand(strSQL1, conexion);
                    comando1.Parameters.AddWithValue("ID_PROD", detalleVenta[i].ID_PROD);
                    comando1.Parameters.AddWithValue("ID_VENT", indice);
                    comando1.Parameters.AddWithValue("CANTIDAD", detalleVenta[i].CANTIDAD);
                    comando1.Parameters.AddWithValue("PRECIO_VENTA", detalleVenta[i].PRECIO_VENTA);
                    comando1.ExecuteNonQuery();
                    comando1.Dispose();
                }

                List<Inventario> ingredientes = new List<Inventario>();
                foreach (DetalleVentas prod in detalleVenta)
                {
                    string strSQL3 = "SELECT DP.ID_INV, DP.CANT, I.STOCK FROM DETALLE_PRODUCTOS AS DP JOIN PRODUCTOS AS P ON" +
                        " DP.ID_PROD = P.ID JOIN INVENTARIO AS I ON DP.ID_INV = I.ID WHERE DP.ID_PROD = @ID_PROD";
                    MySqlCommand comando3 = new MySqlCommand(strSQL3, conexion);
                    MySqlDataReader dr1 = comando3.ExecuteReader();

                    while (dr.Read())
                    {
                        int ID_inv = dr.GetInt32("ID_INV");
                        int cantidad = dr.GetInt32("CANT");
                        int stock_actual = dr.GetInt32("STOCK");

                        string strSQL4 = "UPDATE TABLE INVENTARIO SET STOCK = @DESC WHERE ID = @ID_INV";
                        MySqlCommand comando4 = new MySqlCommand(strSQL4, conexion);
                        comando4.Parameters.AddWithValue("DESC", stock_actual - (cantidad * prod.CANTIDAD));
                        comando4.Parameters.AddWithValue("ID_INV", ID_inv);
                        comando4.ExecuteNonQuery();
                        comando4.Dispose();
                    }
                }

                trans.Commit();
                conexion.Close();
                return true;
            }
            catch (Exception e)
            {
                trans.Rollback();
                conexion.Close();
                return false;
            }
        }

    }
}



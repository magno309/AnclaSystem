using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Modelo;
using MySql.Data.MySqlClient;

namespace Datos
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

                string strSQL2 = "select MAX(ID) from VENTAS";
                MySqlCommand comando2 = new MySqlCommand(strSQL2, conexion);
                MySqlDataReader dr = comando2.ExecuteReader();
                int indice = dr.GetInt32("MAX(ID)");

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

                comando.Dispose();
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



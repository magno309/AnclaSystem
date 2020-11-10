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
        public bool agregarVenta(Ventas nuevaVenta, List<DetalleVentas> detalles)
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
                trans.Commit();
                conexion.Close();
                agregarDetalles(detalles);
                return true;
            }
            catch (Exception e)
            {
                trans.Rollback();
                conexion.Close();
                return false;
            }
        }
        public bool agregarDetalles(List<DetalleVentas> detalleVenta)
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

                for (int i = 0; i < detalleVenta.Count; i++)
                {
                    string strSQL = "insert into DETALLE_VENTAS values (@ID_PROD, @ID_VENT, @CANTIDAD, @PRECIO_VENTA)";
                    MySqlCommand comando = new MySqlCommand(strSQL, conexion);
                    comando.Parameters.AddWithValue("ID_PROD", detalleVenta[i].ID_PROD);
                    comando.Parameters.AddWithValue("ID_VENT", obtenerUltimaVenta());
                    comando.Parameters.AddWithValue("CANTIDAD", detalleVenta[i].CANTIDAD);
                    comando.Parameters.AddWithValue("PRECIO_VENTA", detalleVenta[i].PRECIO_VENTA);
                    comando.ExecuteNonQuery();
                    comando.Dispose();
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
        public int obtenerUltimaVenta()
        {
            MySqlConnection conexion = new MySqlConnection();
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
                string strSQL = "select MAX(ID) from VENTAS";
                MySqlCommand comando = new MySqlCommand(strSQL, conexion);
                MySqlDataReader dr = comando.ExecuteReader();
                int indice = dr.GetInt32("MAX(ID)");
                comando.Dispose();
                conexion.Close();
                return indice;
            }
            catch (Exception e)
            {
                conexion.Close();
                return -1;
            }
        }
    }
}



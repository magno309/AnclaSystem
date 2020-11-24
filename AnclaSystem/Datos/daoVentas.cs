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
        /// Metodo que registra una venta en la base de datos. Sus detalles y descuenta del inventario los productos vendidos.
        /// </summary>
        /// <param name="nuevaVenta">pojo que contiene los datos de venta</param>
        /// <param name="detalleVenta">lista con los detalles de venta</param>
        /// <returns></returns>
        public bool agregarVenta(Ventas nuevaVenta, List<DetalleVentas> detalleVenta)
        {
            MySqlConnection cn = new Conexion().getConexion(); //aqui ya se abre

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

        /// <summary>
        /// Obtener el ID maximo
        /// </summary>
        /// <returns></returns>
        public int getMax()
        {            
            Conexion cn = new Conexion();
            try
            {
                string strSQL = "select MAX(ID) from VENTAS;";
                List<List<object>> max = cn.ejecutarConsulta(strSQL);
                return Convert.ToInt32(max.ElementAt(0).ElementAt(0));
            }
            catch (Exception e)
            {
                return 0;
            }
        }

        public List<Ventas> obtenerTodos() {
            List<Ventas> listaVentas = new List<Ventas>();
            Conexion cn = new Conexion();
            
            try
            {
                string query = "select V.ID, V.TOTAL,V.FECHA,V.ID_CAJERO,U.NOMBRE from VENTAS AS V JOIN USUARIOS AS U ON V.ID_CAJERO=U.ID;";
                foreach(List<object> l in cn.ejecutarConsulta(query)){
                    Ventas nueva = new Ventas();
                    nueva.ID = int.Parse(l[0].ToString());
                    nueva.TOTAL = (double) decimal.Parse(l[1].ToString());
                    nueva.FECHA = l[2].ToString();
                    nueva.ID_CAJERO = int.Parse(l[3].ToString());
                    nueva.NOMBRE_CAJERO=l[4].ToString();
                    listaVentas.Add(nueva);
                }
                return listaVentas;
            }
            catch (Exception e) {
                Console.WriteLine("Error: " + e.Message);
                return null;
            }
        }

        public List<Ventas> obtenerTodos(int ID_CAJERO)
        {
            List<Ventas> listaVentas = new List<Ventas>();
            Conexion cn = new Conexion();

            try
            {
                string query = "select V.ID, V.TOTAL,V.FECHA,V.ID_CAJERO,U.NOMBRE from VENTAS AS V JOIN USUARIOS AS U ON V.ID_CAJERO=U.ID WHERE V.ID_CAJERO=" + ID_CAJERO + ";";
                foreach (List<object> l in cn.ejecutarConsulta(query))
                {
                    Ventas nueva = new Ventas();
                    nueva.ID = int.Parse(l[0].ToString());
                    nueva.TOTAL = (double)decimal.Parse(l[1].ToString());
                    nueva.FECHA = l[2].ToString();
                    nueva.ID_CAJERO = int.Parse(l[3].ToString());
                    nueva.NOMBRE_CAJERO = l[4].ToString();
                    listaVentas.Add(nueva);
                }
                return listaVentas;
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: " + e.Message);
                return null;
            }
        }

        public List<Ventas> obtenerTodos(String FECHA)
        {
            List<Ventas> listaVentas = new List<Ventas>();
            Conexion cn = new Conexion();

            try
            {
                string query = "select V.ID, V.TOTAL,V.FECHA,V.ID_CAJERO,U.NOMBRE from VENTAS AS V JOIN USUARIOS AS U ON V.ID_CAJERO=U.ID WHERE DATE(V.FECHA)='"+FECHA+"';";
                foreach (List<object> l in cn.ejecutarConsulta(query))
                {
                    Ventas nueva = new Ventas();
                    nueva.ID = int.Parse(l[0].ToString());
                    nueva.TOTAL = (double)decimal.Parse(l[1].ToString());
                    nueva.FECHA = l[2].ToString();
                    nueva.ID_CAJERO = int.Parse(l[3].ToString());
                    nueva.NOMBRE_CAJERO = l[4].ToString();
                    listaVentas.Add(nueva);
                }
                return listaVentas;
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: " + e.Message);
                return null;
            }
        }

        public double obtenerTotalVentasPorFecha(string fechaInicio, string fechaFin)
        {
            List<Ventas> listaVentas = new List<Ventas>();
            Conexion cn = new Conexion();
            try
            {
                MySqlCommand cmd = new MySqlCommand();
                cmd.CommandText = "select SUM(TOTAL) from VENTAS where FECHA between @FechaInicio and @FechaFin;";
                cmd.Parameters.AddWithValue("@FechaInicio", fechaInicio);
                cmd.Parameters.AddWithValue("@FechaFin", fechaFin);
                List<List<object>> resultado = cn.ejecutarConsulta(cmd);
                if (resultado.Count >= 1) {
                    List<object> fila = resultado[0];
                    return double.Parse(fila[0].ToString());
                }
                return -1;
            }
            catch (Exception e)
            {
                return -1;
            }
        }

        public List<DetalleVentas> detallesUnaVenta(int ID)
        {
            List<DetalleVentas> listaDetalles = new List<DetalleVentas>();
            Conexion cn = new Conexion();
            try
            {
                string query = "select DV.ID_VENT,DV.ID_PROD, DV.CANTIDAD, DV.PRECIO_VENTA, P.NOMBRE from DETALLE_VENTAS AS DV JOIN VENTAS AS V ON DV.ID_VENT=V.ID JOIN PRODUCTOS AS P ON DV.ID_PROD=P.ID WHERE V.ID= "+ID+";";
                foreach (List<object> l in cn.ejecutarConsulta(query))
                {
                    DetalleVentas nueva = new DetalleVentas();
                    nueva.ID_VENT = int.Parse(l[0].ToString());
                    nueva.ID_PROD= int.Parse(l[1].ToString());
                    nueva.CANTIDAD = int.Parse(l[2].ToString());
                    nueva.PRECIO_VENTA = (double)decimal.Parse(l[3].ToString());
                    nueva.NOMBRE_AUX = l[4].ToString();
                    listaDetalles.Add(nueva);
                }
                return listaDetalles;
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: " + e.Message);
                return null;
            }
        }
        public bool eliminarDetalle(DetalleVentas detalle)
        {
            MySqlConnection cn = new Conexion().getConexion();
            MySqlTransaction trans = cn.BeginTransaction();
            try
            {
                string strSQL = "DELETE FROM DETALLE_VENTAS WHERE ID_VENT=@ID_VENT AND ID_PROD=@ID_PROD;";
                MySqlCommand cmd = new MySqlCommand(strSQL,cn);
                cmd.Parameters.AddWithValue("@ID_VENT", detalle.ID_VENT);
                cmd.Parameters.AddWithValue("@ID_PROD", detalle.ID_PROD);
                cmd.ExecuteNonQuery();
                cmd.Dispose();

                ///MODIFICAR EL INVENTARIO, DESCONTAR LOS INGREDIENTES DE LOS PRODUCTOS VENDIDOS
                List<List<VentaAuxiliar>> listas = new List<List<VentaAuxiliar>>(); //lista de listas porque no me deja tener dos datareaders
                MySqlDataReader dr1 = null;

                ///OBTENER EL ID DEL INVENARIO, LA CANTIDAD DE INGREDIENTES Y EL STOCK ACTUAL DE CADA PRODUCTO VENDIDO
                List<VentaAuxiliar> ingredientes = new List<VentaAuxiliar>();
                string strSQL3 = "SELECT DP.ID_INV, DP.CANTIDAD, I.STOCK FROM DETALLE_PRODUCTOS AS DP JOIN PRODUCTOS AS P ON" +
                    " DP.ID_PROD = P.ID JOIN INVENTARIO AS I ON DP.ID_INV = I.ID WHERE DP.ID_PROD = @ID_PRODU";
                MySqlCommand comando3 = new MySqlCommand(strSQL3, cn);
                comando3.Parameters.AddWithValue("@ID_PRODU", detalle.ID_PROD);
                dr1 = comando3.ExecuteReader();

                while (dr1.Read())
                {
                    ingredientes.Add(new VentaAuxiliar(detalle.CANTIDAD, dr1.GetInt32("ID_INV"), dr1.GetInt32("CANTIDAD"), dr1.GetInt32("STOCK")));
                }

                ///GUARDAR CADA DATO POR CADA PRODUCTO
                listas.Add(ingredientes);
                dr1.Close();
                dr1.Dispose();
                


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

        public bool modificarDetalles(List<DetalleVentas> detalles, int ID_VENT, double nuevoTotal)
        {
            List<DetalleVentas> listaAux = detallesUnaVenta(ID_VENT);
            MySqlConnection cn = new Conexion().getConexion(); //aqui ya se abre la conexion
            double total_actual = 0;

            string str = "SELECT SUM(CANTIDAD*PRECIO_VENTA) AS TOTAL FROM DETALLE_VENTAS WHERE ID_VENT=@ID_VENT;";
            MySqlCommand com = new MySqlCommand(str, cn);
            com.Parameters.AddWithValue("ID_VENT", ID_VENT);
            com.Dispose();
            MySqlDataReader dr1 = com.ExecuteReader();
            dr1.Read();
            if (listaAux.Count > 0)
            {
                total_actual = dr1.GetDouble("TOTAL");
            }
            dr1.Close();
            dr1.Dispose();
            nuevoTotal += total_actual;

            //YA QUE SE MODIFICO LA VENTA SE DEBE MODIFICAR EL TOTAL DE DICHA VENTA
            string strSQL = "UPDATE VENTAS SET TOTAL=@TOTAL WHERE ID=@ID_VENT;";
            MySqlCommand comando = new MySqlCommand(strSQL, cn);
            comando.Parameters.AddWithValue("TOTAL", nuevoTotal);
            comando.Parameters.AddWithValue("ID_VENT", ID_VENT);
            comando.ExecuteNonQuery();
            comando.Dispose();

            foreach (DetalleVentas dt_nuevo in detalles)
            {
                bool existe = false;
                if (listaAux.Count>0)
                {
                    foreach (DetalleVentas dt in listaAux)
                    {
                        if (dt.ID_PROD == dt_nuevo.ID_PROD)
                        {
                            existe = true;
                            break;
                        }
                    }
                }
                if (!existe)
                {
                    ///INICIAR TRANSACCION
            MySqlTransaction trans = cn.BeginTransaction();
                    try
            {

                        //SI EL DETALLE DE VENTA ES NUEVO SE AGREGA EN LA BASE DE DATOS
                        string strSQL1 = "insert into DETALLE_VENTAS values (@ID_PROD, @ID_VENT, @CANTIDAD, @PRECIO_VENTA)";
                        MySqlCommand comando1 = new MySqlCommand(strSQL1, cn);
                        comando1.Parameters.AddWithValue("ID_PROD", dt_nuevo.ID_PROD);
                        comando1.Parameters.AddWithValue("ID_VENT", ID_VENT);
                        comando1.Parameters.AddWithValue("CANTIDAD", dt_nuevo.CANTIDAD);
                        comando1.Parameters.AddWithValue("PRECIO_VENTA", dt_nuevo.PRECIO_VENTA);
                        comando1.ExecuteNonQuery();
                        comando1.Dispose();

                ///MODIFICAR EL INVENTARIO, DESCONTAR LOS INGREDIENTES DE LOS PRODUCTOS VENDIDOS
                List<List<VentaAuxiliar>> listas = new List<List<VentaAuxiliar>>(); //lista de listas porque no me deja tener dos datareaders
                

                    ///OBTENER EL ID DEL INVENARIO, LA CANTIDAD DE INGREDIENTES Y EL STOCK ACTUAL DE CADA PRODUCTO VENDIDO
                    List<VentaAuxiliar> ingredientes = new List<VentaAuxiliar>();
                    string strSQL3 = "SELECT DP.ID_INV, DP.CANTIDAD, I.STOCK FROM DETALLE_PRODUCTOS AS DP JOIN PRODUCTOS AS P ON" +
                        " DP.ID_PROD = P.ID JOIN INVENTARIO AS I ON DP.ID_INV = I.ID WHERE DP.ID_PROD = @ID_PRODU";
                    MySqlCommand comando3 = new MySqlCommand(strSQL3, cn);
                    comando3.Parameters.AddWithValue("@ID_PRODU", dt_nuevo.ID_PROD);
                    dr1 = comando3.ExecuteReader();

                    while (dr1.Read())
                    {
                        ingredientes.Add(new VentaAuxiliar(dt_nuevo.CANTIDAD, dr1.GetInt32("ID_INV"), dr1.GetInt32("CANTIDAD"), dr1.GetInt32("STOCK")));
                    }

                    ///GUARDAR CADA DATO POR CADA PRODUCTO
                    listas.Add(ingredientes);
                        dr1.Close();
                        dr1.Dispose();

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
            }
            catch (Exception ex)
            {
                ///SI HAY ERROR DESHACER LO QUE SE LLEVARA EN LA TRANSACCION Y LANZAR EXCEPCION
                trans.Rollback();
                       
                throw ex;
            }
                }
                else
                {
                    ///INICIAR TRANSACCION
                    MySqlTransaction trans = cn.BeginTransaction();
                    try
                    {

                        //SI EL DETALLE DE VENTA ESTA REGISTRADO SE MODIFICA EN LA BASE DE DATOS
                        string strSQL1 = "UPDATE DETALLE_VENTAS SET CANTIDAD=@CANTIDAD WHERE ID_VENT=@ID_VENT AND ID_PROD=@ID_PROD;";
                        MySqlCommand comando1 = new MySqlCommand(strSQL1, cn);
                        comando1.Parameters.AddWithValue("CANTIDAD", dt_nuevo.CANTIDAD);
                        comando1.Parameters.AddWithValue("ID_VENT", dt_nuevo.ID_VENT);
                        comando1.Parameters.AddWithValue("ID_PROD", dt_nuevo.ID_PROD);
                        comando1.ExecuteNonQuery();
                        comando1.Dispose();

                        ///MODIFICAR EL INVENTARIO, DESCONTAR LOS INGREDIENTES DE LOS PRODUCTOS VENDIDOS
                        List<List<VentaAuxiliar>> listas = new List<List<VentaAuxiliar>>(); //lista de listas porque no me deja tener dos datareaders

                        ///OBTENER EL ID DEL INVENARIO, LA CANTIDAD DE INGREDIENTES Y EL STOCK ACTUAL DE CADA PRODUCTO VENDIDO
                        List<VentaAuxiliar> ingredientes = new List<VentaAuxiliar>();
                        string strSQL3 = "SELECT DP.ID_INV, DP.CANTIDAD, I.STOCK FROM DETALLE_PRODUCTOS AS DP JOIN PRODUCTOS AS P ON" +
                            " DP.ID_PROD = P.ID JOIN INVENTARIO AS I ON DP.ID_INV = I.ID WHERE DP.ID_PROD = @ID_PRODU";
                        MySqlCommand comando3 = new MySqlCommand(strSQL3, cn);
                        comando3.Parameters.AddWithValue("@ID_PRODU", dt_nuevo.ID_PROD);
                        dr1 = comando3.ExecuteReader();

                        while (dr1.Read())
                        {
                            ingredientes.Add(new VentaAuxiliar(dt_nuevo.CANTIDAD, dr1.GetInt32("ID_INV"), dr1.GetInt32("CANTIDAD"), dr1.GetInt32("STOCK")));
                        }
                        dr1.Close();
                        dr1.Dispose();
                        ///GUARDAR CADA DATO POR CADA PRODUCTO
                        listas.Add(ingredientes);

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
                    }
                    catch (Exception ex)
                    {
                        ///SI HAY ERROR DESHACER LO QUE SE LLEVARA EN LA TRANSACCION Y LANZAR EXCEPCION
                        trans.Rollback();
                        throw ex;
                    }
                    
                }
            }
                ///CERRAR TODO
                cn.Close();
                cn.Dispose();
            return true;
        }
    }
}
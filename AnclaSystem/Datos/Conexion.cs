using System;
using System.Data;
using MySql.Data.MySqlClient;

namespace Datos
{
    public class Conexion
    {
        //Variable que administra la conexion a la base de datos 
        static MySqlConnection conexion;

        //Establece la conexión a la BD de acuerdo a un String que indica el servidor y el nombre de esta
        //Además de el nombre y la contraseña del usuario y el puerto.
        public static bool conectar()
        {
            try
            {
                if (conexion == null || conexion.State != ConnectionState.Open)
                {
                    conexion = new MySqlConnection();
                    //conexion.ConnectionString = "Server=localhost;Database=world;uid=root;pwd=root";
                    String server = "25.89.125.13";
                    String database = "ANCLA";
                    String uid = "remoto";
                    String password = "remoto1";
                    String port = "8457";
                    String connectionString = "SERVER=" + server + "; PORT =" + port + ";" + "DATABASE="
                        + database + ";" + "UID=" + uid + ";" + "PASSWORD=" + password + ";";
                    conexion.Open();
                }
                return true;
            }
            catch (MySqlException ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
            finally
            {
                desconectar();
            }

        }

        //Metodo que asigna los datos de la BD a un datatable usando un MySQLDataAdapter como
        //intermediario entre los datos contenidos en BD y la nueva datatable.
        public static DataTable ejecutarConsulta(MySqlCommand consulta)
        {
            if (conectar())
            {
                consulta.Connection = conexion;
                MySqlDataAdapter adaptador = new MySqlDataAdapter(consulta);
                DataTable tabla = new DataTable();
                adaptador.Fill(tabla);
                return tabla;
            }
            return null;
        }


        //Ejecuta las instrucciones INSERT, UPDATE o DELETE y asigna el valor retornado por 
        //ExecuteScalar() o ExecuteNonQuery() a la variable valor. 
        //ExecuteScalar() retorna un object y recupera la primera columna de la primera fila.
        //ExecuteNonQuery() se usa para ejecutar instrucciones de SQL que no devolverán un valor.
        public static int ejecutarSentencia(MySqlCommand sentencia, bool esInsertar)
        {
            int valor = 0;
            if (conectar())
            {
                sentencia.Connection = conexion;
                if (esInsertar)
                    valor = int.Parse(sentencia.ExecuteScalar().ToString());
                else
                    valor = sentencia.ExecuteNonQuery();
            }
            return valor;
        }

        //Cierra la conexion a la BD
        public static void desconectar()
        {
            if (conexion != null && conexion.State == ConnectionState.Open)
            {
                conexion.Close();
                conexion.Dispose();
            }
        }

    }
}
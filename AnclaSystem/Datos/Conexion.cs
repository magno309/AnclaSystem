using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace Datos {
    public class Conexion {
        private MySqlConnection connection;
        private string server;
        private string port;
        private string database;
        private string uid;
        private string password;

        public Conexion() {
            Inicializar();
        }

        private void Inicializar() {
            server = "remotemysql.com";
            port = "3306";
            database = "xpycSmCcRA";
            uid = "xpycSmCcRA";
            password = "XXcAGrUXKg";
            string connectionString;
            connectionString = "SERVER=" + server + ";PORT=" + port + ";" + "DATABASE=" +
                                database + ";" + "UID=" + uid + ";" + "PWD=" + password + ";AllowUserVariables=True";

            //"server=25.89.125.13;port=8457;uid=remoto;pwd=remoto1;database=ANCLA;Allow User Variables=True";



            connection = new MySqlConnection(connectionString);
        }

        private bool OpenConnection() {
            try {
                connection.Open();
                return true;
            }
            catch (MySqlException ex) {

                Console.WriteLine(ex.Message);

                //When handling errors, you can your application's response based 
                //on the error number.
                //The two most common error numbers when connecting are as follows:
                //0: Cannot connect to server.
                //1045: Invalid user name and/or password.
                switch (ex.Number) {

                    case 0:
                        //MessageBox.Show("Cannot connect to server.  Contact administrator");
                        break;

                    case 1045:
                        //MessageBox.Show("Invalid username/password, please try again");
                        break;
                }
                return false;
            }
        }

        //Close connection
        private bool CloseConnection() {
            try {
                connection.Close();
                connection.Dispose();
                return true;
            }
            catch (MySqlException ex) {
                //MessageBox.Show(ex.Message);
                return false;
            }
        }

        public bool ProbarConexion() {
            if (OpenConnection()) {
                CloseConnection();
                return true;
            }
            else {
                return false;
            }
        }

        public List<List<object>> ejecutarConsulta(string query) {
            List<List<object>> lista = new List<List<object>>();
            if (this.OpenConnection()) {
                MySqlCommand cmd = new MySqlCommand(query, connection);
                MySqlDataReader dataReader = cmd.ExecuteReader();
                int index = 0;
                while (dataReader.Read()) {
                    lista.Add(new List<object>());
                    for (int i = 0; i < dataReader.FieldCount; i++) {
                        lista[index].Add(dataReader[dataReader.GetName(i)]);
                    }
                    index++;
                }
                dataReader.Close();
                this.CloseConnection();
            }
            return lista;
        }

        /// <summary>
        /// Sobrecarga del método ejecutarConsulta. Para aquellas consultas que incluyan parametros en el comando.
        /// Estos serán configurados desde el DAO. y se enviaran mediante el MySqlCommand
        /// </summary>
        /// <param name="cmd">Comando de consulta con parametros</param>
        /// <returns>Una lista de un arreglo de objetos, cada Lista contenida en la Lista principal, contiene los campos de un solo registro.</returns>
        public List<List<object>> ejecutarConsulta(MySqlCommand cmd) {
            List<List<object>> lista = new List<List<object>>();
            if (this.OpenConnection()) {
                //MySqlCommand cmd = new MySqlCommand(query, connection);
                cmd.Connection = connection;        // Esta línea sustituye a la de arriba
                MySqlDataReader dataReader = cmd.ExecuteReader();
                int index = 0;
                while (dataReader.Read()) {
                    lista.Add(new List<object>());
                    for (int i = 0; i < dataReader.FieldCount; i++) {
                        lista[index].Add(dataReader[dataReader.GetName(i)]);
                    }
                    index++;
                }
                dataReader.Close();
                this.CloseConnection();
            }
            return lista;
        }

        public bool ejecutarSentencia(MySqlCommand cmd) {
            if (this.OpenConnection()) {
                cmd.Connection = connection;
                cmd.ExecuteNonQuery();
                this.CloseConnection();
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool EjecutarTransaccion(List<MySqlCommand> listaCmd)
        {
            if (this.OpenConnection())
            {
                MySqlTransaction transaction = connection.BeginTransaction();
                try
                {
                    foreach (MySqlCommand cmd in listaCmd)
                    {
                        cmd.Connection = connection;
                        cmd.Transaction = transaction;
                        cmd.ExecuteNonQuery();
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
                    this.CloseConnection();
                }
            }
            else {
                return false;
            }
        }

        /// <summary>
        /// Para las transacciones
        /// </summary>
        /// <returns></returns>
       public MySqlConnection getConexion()
        {
            if (this.OpenConnection())
            {
                return this.connection;
            }
            else
            {
                return null;
            }
        }
    }
}

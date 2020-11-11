using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;


namespace Datos
{
    public class Conexion
    {
        private MySqlConnection connection;
        private string server;
        private string port;
        private string database;
        private string uid;
        private string password;

        public Conexion() {
            Inicializar();
        }

        private void Inicializar()
        {
            server = "25.89.125.13";
            port = "8457";
            database = "ANCLA";
            uid = "remoto";
            password = "remoto1";
            string connectionString;
            connectionString = "SERVER=" + server + ";" + "PORT=" + port + ";" + "DATABASE=" +
            database + ";" + "UID=" + uid + ";" + "PASSWORD=" + password + ";";

            connection = new MySqlConnection(connectionString);
        }

        private bool OpenConnection()
        {
            try
            {
                connection.Open();
                return true;
            }
            catch (MySqlException ex)
            {

                Console.WriteLine(ex.Message);

                //When handling errors, you can your application's response based 
                //on the error number.
                //The two most common error numbers when connecting are as follows:
                //0: Cannot connect to server.
                //1045: Invalid user name and/or password.
                switch (ex.Number)
                {
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
        private bool CloseConnection()
        {
            try
            {
                connection.Close();
                return true;
            }
            catch (MySqlException ex)
            {
                //MessageBox.Show(ex.Message);
                return false;
            }
        }

        public bool ProbarConexion()
        {
            if (OpenConnection())
            {
                CloseConnection();
                return true;
            }
            else
            {
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
                        lista[index].Add(dataReader[ dataReader.GetName(i) ]);
                    }
                    index++;
                }
                dataReader.Close();
                this.CloseConnection();
            }
            return lista;
        }
    }
}

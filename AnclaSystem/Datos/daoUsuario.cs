using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Datos;
using System.Data;
using MySql.Data.MySqlClient;

namespace Modelo {
    public class daoUsuario {
        //Conexión con la BD

        /// <summary>
        /// Este método inserta un objeto usuario en la base de datos.
        /// </summary>
        /// <param name="obj"> Objeto de tipo Usuario. No es necesario que contenga un ID </param>
        /// <returns>Verdadero o falso de acuerdo a si se logro o no insertar el objeto</returns>    
        public bool agregar(Usuario obj) {
            return false;
        }


        /// <summary>
        /// Este método actualiza un registro de la base de datos, con información cargada en el objeto Usuario.
        /// En relación al 'id' cargado en el objeto.
        /// </summary>
        /// <param name="obj">Objeto de tipo Usuario. Es necesario que contenga un valor en el atributo 'id'</param>
        /// <returns>Verdadero o falso de acuerdo a si se logro o no actualizar el objeto</returns>
        public bool editar(Usuario obj) {
            return false;
        }

        /// <summary>
        /// Este método asume la eliminación de un usuario, como una marca en el atributo 'esActivo' = false. 
        /// No elimina el registro del sistema.
        /// </summary>
        /// <param name="id">Identificador del registro Usuario a eliminar</param>
        /// <returns>Verdadero o falso de acuerdo a si se logro o no modificar el objeto</returns>
        public bool eliminar(int id) {
            return false;
        }

        /// <summary>
        /// Este método busca un registro de Usuario en la base de datos, de acuerdo a un identificador.
        /// </summary>
        /// <param name="id">Identificador del registro Usuario a buscar</param>
        /// <returns>Objeto de tipo Usuario con el resultado de la busqueda. Estará vacío si no se encuentra.</returns>
        public Usuario buscarUno(int id) {
            Usuario obj = new Usuario();

            return obj;
        }

        /// <summary>
        /// Este método busca un registro de Usuario en la base de datos, de acuerdo a un identificador.
        /// </summary>
        /// <param name="nombre_usuario">nombre de usuario del registro Usuario a buscar</param>
        /// <returns>Objeto de tipo Usuario con el resultado de la busqueda. Estará vacío si no se encuentra.</returns>
        public Usuario buscarUno(String nombre_usuario) {
            Usuario obj = new Usuario();
            try {
                Conexion con = new Conexion();
                MySqlCommand comando = new MySqlCommand();
                comando.CommandText = "Select * from USUARIOS where NOMBRE_USUARIO = @nomU;";
                comando.Parameters.AddWithValue("@nomU", nombre_usuario);
                List<List<Object>> ret = con.ejecutarConsulta(comando);
                if (ret.Count == 1) {
                    List<Object> row = ret[0];
                    obj.id = int.Parse(row[0].ToString());
                    obj.nombre = row[1].ToString();
                    obj.nombre_usuario = row[2].ToString();
                    obj.contrasenia = row[3].ToString();
                    obj.esAdmin = bool.Parse(row[4].ToString());
                    obj.esActivo = bool.Parse(row[5].ToString());
                }

            }
            catch (Exception e) {
                throw e;
            }

            return obj;
        }

        /// <summary>
        /// Consulta el conjunto de registros de usuaruios que se encuntran actualmente en la base de datos.
        /// </summary>
        /// <returns>Retorna una lista de usuarios (List<Usuario>) con el contenido encontrado.</returns>
        public List<Usuario> buscarTodos() {
            List<Usuario> lista = new List<Usuario>();

            return lista;
        }
    }
}

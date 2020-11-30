using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Modelo;
using System.Data;
using MySql.Data.MySqlClient;

namespace Datos {
    public class daoUsuario {
        //Conexión con la BD

        /// <summary>
        /// Este método inserta un objeto usuario en la base de datos.
        /// </summary>
        /// <param name="obj"> Objeto de tipo Usuario. No es necesario que contenga un ID </param>
        /// <returns>Verdadero o falso de acuerdo a si se logro o no insertar el objeto</returns>    
        public bool agregar(Usuario obj) {
            bool i = false;
            try {
                MySqlCommand comando = new MySqlCommand();
                comando.CommandText = "insert into USUARIOS (NOMBRE, NOMBRE_USUARIO, CONTRASENA, ES_ADMIN, ACTIVO) " +
                    " values (@nombre, @nombre_usuario, @pass, @esAdmin, @activo);";
                comando.Parameters.AddWithValue("@nombre", obj.nombre);
                comando.Parameters.AddWithValue("@nombre_usuario", obj.nombre_usuario);
                comando.Parameters.AddWithValue("@pass", obj.contrasenia);
                comando.Parameters.AddWithValue("@esAdmin", obj.esAdmin);
                comando.Parameters.AddWithValue("@activo", obj.esActivo);

                Conexion con = new Conexion();
                i = con.ejecutarSentencia(comando);
            } catch (MySqlException ex) {
                throw ex;
            }
            catch (Exception ex) {
                throw ex;
            }
            return i;
        }


        /// <summary>
        /// Este método actualiza un registro de la base de datos, con información cargada en el objeto Usuario.
        /// En relación al 'id' cargado en el objeto.
        /// </summary>
        /// <param name="obj">Objeto de tipo Usuario. Es necesario que contenga un valor en el atributo 'id'</param>
        /// <returns>Verdadero o falso de acuerdo a si se logro o no actualizar el objeto</returns>
        public bool editar(Usuario obj) {
            bool i = false;
            try {
                MySqlCommand comando = new MySqlCommand();
                comando.CommandText = "update USUARIOS set NOMBRE = @nombre, NOMBRE_USUARIO = @nombre_usuario, CONTRASENA=@pass, ES_ADMIN= @esAdmin, ACTIVO = @activo where ID = @id;"; 
                comando.Parameters.AddWithValue("@nombre", obj.nombre);
                comando.Parameters.AddWithValue("@nombre_usuario", obj.nombre_usuario);
                comando.Parameters.AddWithValue("@pass", obj.contrasenia);
                comando.Parameters.AddWithValue("@esAdmin", obj.esAdmin);
                comando.Parameters.AddWithValue("@activo", obj.esActivo);
                comando.Parameters.AddWithValue("@id", obj.id);
                Conexion con = new Conexion();
                i = con.ejecutarSentencia(comando);
            } catch(Exception e) {
                throw e;
            }
            return i;
        }

        /// <summary>
        /// Este método asume la eliminación de un usuario, como una marca en el atributo 'esActivo' = false. 
        /// No elimina el registro del sistema.
        /// </summary>
        /// <param name="id">Identificador del registro Usuario a eliminar</param>
        /// <returns>Verdadero o falso de acuerdo a si se logro o no modificar el objeto</returns>
        public bool eliminar(int id) {
            bool i = false;
            try {
                Conexion con = new Conexion();
                MySqlCommand comando = new MySqlCommand();
                comando.CommandText = "update USUARIOS set ACTIVO = 0 where ID = @id;";
                comando.Parameters.AddWithValue("@id", id);
                i = con.ejecutarSentencia(comando);
            } catch(Exception e) {
                throw e;
            }
            return i;
        }

        /// <summary>
        /// Este método busca un registro de Usuario en la base de datos, de acuerdo a un identificador.
        /// </summary>
        /// <param name="id">Identificador del registro Usuario a buscar</param>
        /// <returns>Objeto de tipo Usuario con el resultado de la busqueda. Estará vacío si no se encuentra.</returns>
        public Usuario buscarUno(int id) {
            Usuario obj = new Usuario();
            try {
                Conexion con = new Conexion();
                MySqlCommand comando = new MySqlCommand();
                comando.CommandText = "Select * from USUARIOS where ID = @id;";
                comando.Parameters.AddWithValue("@id", id);
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
            catch(Exception e) {
                throw e;
            }
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
        /// Buscar coincidencia del texto introducido, con el campo nombre y nombre usuario
        /// </summary>
        /// <param name="parcial">Texto a buscar</param>
        /// <returns>Lista con los usuarios encontrados</returns>
        public List<Usuario> buscarPatron(String parcial) {
            List<Usuario> lista = new List<Usuario>();
            
            try {
                Conexion con = new Conexion();
                MySqlCommand comando = new MySqlCommand();
                comando.CommandText = "select ID, NOMBRE, NOMBRE_USUARIO, ES_ADMIN from USUARIOS where (NOMBRE like @patron or NOMBRE_USUARIO like @patron) and ACTIVO = 1;";
                comando.Parameters.AddWithValue("@patron", "%" +  parcial + "%");
                List<List<Object>> ret = con.ejecutarConsulta(comando);
                foreach (List<Object> registro in ret) {
                    Usuario obj  = new Usuario();
                    obj.id = int.Parse(registro[0].ToString());
                    obj.nombre = registro[1].ToString();
                    obj.nombre_usuario = registro[2].ToString();
                    obj.esAdmin = bool.Parse(registro[3].ToString());
                    lista.Add(obj);
                }
            }
            catch (Exception e) {
                throw e;
            }
            return lista;
        }

        /// <summary>
        /// Consulta el conjunto de registros de usuaruios que se encuntran actualmente en la base de datos.
        /// </summary>
        /// <returns>Retorna una lista de usuarios (List<Usuario>) con el contenido encontrado.</returns>
        public List<Usuario> buscarTodos() {
            List<Usuario> lista = new List<Usuario>();
            
            try {
                Conexion con = new Conexion();
                MySqlCommand comando = new MySqlCommand();
                comando.CommandText = "Select ID, NOMBRE, NOMBRE_USUARIO, ES_ADMIN from USUARIOS where ACTIVO = 1;";
                List<List<Object>> ret = con.ejecutarConsulta(comando);

                foreach (List<Object> registro in ret) {
                    Usuario obj = new Usuario();
                    obj.id = int.Parse(registro[0].ToString());
                    obj.nombre = registro[1].ToString();
                    obj.nombre_usuario = registro[2].ToString();
                    obj.esAdmin = bool.Parse(registro[3].ToString());
                    lista.Add(obj);
                }
            }
            catch (Exception e) {
                throw e;
            }
            return lista;
        }
    }
}

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
    /// Clase de acceso a datos para la entidad Proveedor
    /// </summary>
    public class daoProveedor
    {
        /// <summary>
        /// Metodo que agrega un proveedor a la tabla proveedores de a base de datos Ancla.
        /// </summary>
        /// <param name="proveedor">Objeto de la clase Proveedor que contiene los datos a agregar en el registro.</param>
        /// <returns>Retorna verdadero si se pudo agregar el proveedor, lanza una excepcion si algo sale mal.</returns>
        public Boolean agregaProveedor(Proveedores proveedor)
        {
            Conexion cn = new Conexion();
            try
            {
                string strSQL = "INSERT INTO PROVEEDORES (NOMBRE_EMPRESA, NOMBRE_CONTACTO, TELEFONO, CORREO, DIRECCION)" +
                    " VALUES (@N_EMPRESA, @N_CONTACTO, @TELEFONO, @CORREO, @DIRECCION)";
                MySqlCommand comando = new MySqlCommand(strSQL);
                comando.Parameters.AddWithValue("N_EMPRESA", proveedor.NombreEmpresa);
                comando.Parameters.AddWithValue("N_CONTACTO", proveedor.NombreContacto);
                comando.Parameters.AddWithValue("TELEFONO", proveedor.Telefono);
                comando.Parameters.AddWithValue("CORREO", proveedor.Correo);
                comando.Parameters.AddWithValue("DIRECCION", proveedor.Direccion);
                if (cn.ejecutarSentencia(comando))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Metodo que elimina un proveedor de la tabla proveedores de la base de datos Ancla de acuerdo
        /// al id del proveedor brindado.
        /// </summary>
        /// <param name="idProveedor">Identificador del proveedor a eliminar.</param>
        /// <returns>retorna verdadero si elimino el proveedor, lanza una excepcion si algo salio mal.</returns>
        public Boolean eliminarProveedor(int idProveedor)
        {
            Conexion cn = new Conexion();
            try
            {
                ///EJECUTAR COMANDO
                string strSQL = "DELETE FROM PROVEEDORES WHERE ID = @ID_PROVEEDOR";
                MySqlCommand comando = new MySqlCommand(strSQL);
                comando.Parameters.AddWithValue("ID_PROVEEDOR", idProveedor);
                if (cn.ejecutarSentencia(comando))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Metodo que modifica un proveedor de la tabla Proveedores en la base de datos Ancla
        /// </summary>
        /// <param name="producto">Objeto de la clase Proveedor que contiene los nuevos datos para el registro con el id descrito.</param>
        /// <returns>Retorna verdadero si logró modificar el registro, lanza una excepcion si algo sale mal</returns>
        public Boolean modificarProveedor(Proveedores proveedor)
        {
            Conexion cn = new Conexion();

            try
            {
                ///EJECUTAR COMANDO
                string strSQL = "UPDATE PROVEEDORES SET NOMBRE_EMPRESA = @N_EMPRESA, NOMBRE_CONTACTO = @N_CONTACTO, TELEFONO = @TELEFONO, " +
                    "CORREO = @CORREO, DIRECCION = @DIRECCION WHERE ID = @ID_PROVEEDOR";
                MySqlCommand comando = new MySqlCommand(strSQL);
                comando.Parameters.AddWithValue("N_EMPRESA", proveedor.NombreEmpresa);
                comando.Parameters.AddWithValue("N_CONTACTO", proveedor.NombreContacto);
                comando.Parameters.AddWithValue("TELEFONO", proveedor.Telefono);
                comando.Parameters.AddWithValue("CORREO", proveedor.Correo);
                comando.Parameters.AddWithValue("DIRECCION", proveedor.Direccion);
                comando.Parameters.AddWithValue("ID_PROVEEDOR", proveedor.ID);

                if (cn.ejecutarSentencia(comando))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Metodo que obtiene todos los registros en la tabla proveedores.
        /// </summary>
        /// <returns>Retorna la lista con los proveedores obtenidos, lanza una excepcion si algo sale mal.</returns>
        public List<Proveedores> devolverProveedores()
        {
            List<Proveedores> proveedores = new List<Proveedores>();
            Conexion cn = new Conexion();

            try
            {
                string strSQL = "SELECT * FROM PROVEEDORES";
                MySqlCommand comando = new MySqlCommand(strSQL);

                foreach (List<object> fila in cn.ejecutarConsulta(strSQL))
                {
                    Proveedores proveedor = new Proveedores();
                    proveedor.ID = int.Parse(fila[0]+"");
                    proveedor.NombreEmpresa = fila[1] + "";
                    proveedor.NombreContacto = fila[2] + "";
                    proveedor.Telefono = fila[3] + "";
                    proveedor.Correo = fila[4] + "";
                    proveedor.Direccion = fila[5] + "";

                    proveedores.Add(proveedor);
                }

                return proveedores;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Metodo para buscar incidencias de proveedores con el nombre de empresa o de contacto que el usuario indique
        /// </summary>
        /// <param name="clave">Nombre de contacto o empresa para buscar</param>
        /// <returns>Lista de proveedores que sus nombres de empresa o contacto coinciden con clave</returns>
        public List<Proveedores> buscarProveedor(String clave)
        {
            List<Proveedores> proveedores = new List<Proveedores>();
            Conexion cn = new Conexion();

            try
            {
                string strSQL = "SELECT * FROM PROVEEDORES WHERE (NOMBRE_EMPRESA LIKE '%@KEY%' OR NOMBRE_CONTACTO LIKE '%@KEY%')";
                MySqlCommand comando = new MySqlCommand(strSQL);
                comando.Parameters.AddWithValue("KEY", clave);

                foreach (List<object> fila in cn.ejecutarConsulta(comando))
                {
                    Proveedores proveedor = new Proveedores();
                    proveedor.ID = int.Parse(fila[0] + "");
                    proveedor.NombreEmpresa = fila[1] + "";
                    proveedor.NombreContacto = fila[2] + "";
                    proveedor.Telefono = fila[3] + "";
                    proveedor.Correo = fila[4] + "";
                    proveedor.Direccion = fila[5] + "";

                    proveedores.Add(proveedor);
                }

                return proveedores;
            }
            catch (Exception e)
            {
                throw e;
            }
        } 
    }
}

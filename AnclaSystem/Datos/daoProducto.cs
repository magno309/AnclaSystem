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
    /// CLASE ACCESO A DATOS PRODUCTO
    /// </summary>
    public class daoProducto
    { 
        /// <summary>
        /// Metodo que se manda llamar para llenar la tabla de productos en registrar venta.
        /// </summary>
        /// <returns>Lista de productos que no estan descontinuados.</returns>
        public List<Productos> getProductosNoDescontinuados()
        {
            List<Productos> productos = new List<Productos>();
            Conexion cn = new Conexion();

            try
            {
                ///EJECUTAR COMANDO SELECT PARA OBTENER PRODUCTOS NO DESCONTINUADOS
                string strSQL = "SELECT * FROM PRODUCTOS WHERE DESCONTINUADO = FALSE";
               
                foreach (List<object> fila in cn.ejecutarConsulta(strSQL))
                {
                    Productos producto = new Productos(int.Parse(fila[0]+""), fila[1]+"", double.Parse(fila[2]+""));
                    productos.Add(producto);
                }

                return productos;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public bool AgregarProducto(Productos nuevo) {
            return false;
        }
    }
}

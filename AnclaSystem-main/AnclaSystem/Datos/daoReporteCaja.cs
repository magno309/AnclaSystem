using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Modelo;

namespace Datos
{
    public class daoReporteCaja
    {
        public bool insertarReporteCaja(ReporteCaja nuevo) {
            MySqlCommand cmd = new MySqlCommand();
            cmd.CommandText = "insert into REPORTES_CAJA (ID_USUARIO, FECHA_APERTURA, FECHA_CIERRE, IMPORTE_APERTURA, IMPORTE_CIERRE, TOTAL_VENTAS, FALTANTE) values " +
                "(@ID_USUARIO, @FECHA_APERTURA, @FECHA_CIERRE, @IMPORTE_APERTURA, @IMPORTE_CIERRE, @TOTAL_VENTAS, @FALTANTE);";
            cmd.Parameters.AddWithValue("@ID_USUARIO", nuevo.IdUsuario);
            cmd.Parameters.AddWithValue("@FECHA_APERTURA", nuevo.FechaApertura);
            cmd.Parameters.AddWithValue("@FECHA_CIERRE", nuevo.FechaCierre);
            cmd.Parameters.AddWithValue("@IMPORTE_APERTURA", nuevo.ImporteApertura);
            cmd.Parameters.AddWithValue("@IMPORTE_CIERRE", nuevo.ImporteCierre);
            cmd.Parameters.AddWithValue("@TOTAL_VENTAS", nuevo.TotalVentas);
            cmd.Parameters.AddWithValue("@FALTANTE", nuevo.Faltante);
            return new Conexion().ejecutarSentencia(cmd);

        }

        public List<ReporteCaja> obtenerTodos() {
            List<ReporteCaja> listaReportes = new List<ReporteCaja>();
            Conexion cn = new Conexion();
            try
            {
                string query = "select * from REPORTE_CAJA;";
                foreach (List<object> l in cn.ejecutarConsulta(query))
                {
                    ReporteCaja nueva = new ReporteCaja(
                        int.Parse(l[0].ToString()),
                        int.Parse(l[1].ToString()),
                        l[2].ToString(),
                        l[3].ToString(),
                        double.Parse(l[4].ToString()),
                        double.Parse(l[5].ToString()),
                        double.Parse(l[6].ToString()),
                        double.Parse(l[7].ToString())
                        );
                    listaReportes.Add(nueva);
                }
                return listaReportes;
            }
            catch (Exception e)
            {
                return null;
            }
        }
    }
}

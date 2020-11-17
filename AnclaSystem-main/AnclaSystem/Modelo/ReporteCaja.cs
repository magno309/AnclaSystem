using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelo
{
    public class ReporteCaja
    {
        public int IdReporte { get; }
        public int IdUsuario { get; }
        public string FechaApertura { get; }
        public string FechaCierre { get; }
        public double ImporteApertura { get; }
        public double ImporteCierre { get; }
        public double TotalVentas { get; }
        public double Faltante { get; }

        public ReporteCaja(int idReporte, int idUsuario, string fechaApertura, string fechaCierre, double importeApertura, double importeCierre, double totalVentas, double faltante)
        {
            IdReporte = idReporte;
            IdUsuario = idUsuario;
            FechaApertura = fechaApertura;
            FechaCierre = fechaCierre;
            ImporteApertura = importeApertura;
            ImporteCierre = importeCierre;
            TotalVentas = totalVentas;
            Faltante = faltante;
        }

        public ReporteCaja(int idUsuario, string fechaApertura, string fechaCierre, double importeApertura, double importeCierre, double totalVentas, double faltante)
        {
            IdUsuario = idUsuario;
            FechaApertura = fechaApertura;
            FechaCierre = fechaCierre;
            ImporteApertura = importeApertura;
            ImporteCierre = importeCierre;
            TotalVentas = totalVentas;
            Faltante = faltante;
        }
    }
}

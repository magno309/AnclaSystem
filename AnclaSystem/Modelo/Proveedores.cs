using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelo
{
    public class Proveedores
    {
        public int ID { get; set; }
        public String NombreEmpresa { get; set; }
        public String NombreContacto { get; set; }
        public String Telefono { get; set; }
        public String Correo { get; set; }
        public String Direccion { get; set; }

        public Proveedores()
        {
        }

        public override bool Equals(object obj)
        {
            return obj is Proveedores proveedores &&
                   NombreEmpresa == proveedores.NombreEmpresa &&
                   NombreContacto == proveedores.NombreContacto &&
                   Telefono == proveedores.Telefono &&
                   Correo == proveedores.Correo &&
                   Direccion == proveedores.Direccion;
        }
    }
}

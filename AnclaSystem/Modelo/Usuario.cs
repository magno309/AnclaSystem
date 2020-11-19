using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Modelo {
    public class Usuario {
        //Atributos del objeto usuario, de acuerdo a la base de datos.
        public int id { get; set; }
        public string nombre { get; set; }
        public string nombre_usuario { get; set; }
        public string contrasenia { get; set; }
        public bool esAdmin { get; set; }
        public bool esActivo { get; set; }

        public Usuario() {

        }

        public Usuario(int id, string nombre, string nombre_usuario, string contrasenia, bool esAdmin, bool esActivo) {
            this.id = id;
            this.nombre = nombre;
            this.nombre_usuario = nombre_usuario;
            this.contrasenia = contrasenia;
            this.esAdmin = esAdmin;
            this.esActivo = esActivo;
        }
        public Usuario(string nombre, string nombre_usuario, string contrasenia, bool esAdmin, bool esActivo) {
            this.nombre = nombre;
            this.nombre_usuario = nombre_usuario;
            this.contrasenia = contrasenia;
            this.esAdmin = esAdmin;
            this.esActivo = esActivo;
        }

    }
}

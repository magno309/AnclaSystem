using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Datos;
using Modelo;

namespace Vista
{
    public partial class frmInicioSesion : Form
    {
        public frmInicioSesion()
        {
            InitializeComponent();
        }

        private void btnIngresar_Click(object sender, EventArgs e)
        {
            string usr = txtUsuario.Text;
            string psw = txtContrasenia.Text;

            daoUsuario dao = new daoUsuario();
            try {
                Usuario inicio = dao.buscarUno(usr);
                if(inicio.contrasenia != psw || !inicio.esActivo) {     
                    MessageBox.Show("Usuario o contraseña incorrectos");
                } else {
                    Properties.Settings.Default.idUsuarioL = inicio.id;
                    Properties.Settings.Default.nombreL = inicio.nombre;
                    Properties.Settings.Default.nombreUsuarioL = inicio.nombre_usuario;
                    Properties.Settings.Default.esAdmin = inicio.esAdmin;
                    MessageBox.Show("Bienvenido " + inicio.nombre + "\n" + Properties.Settings.Default.idUsuarioL);
                    MenuPrincipal.frmMenu vista = new MenuPrincipal.frmMenu();
                    vista.Show();
                    this.Hide();
                }
            }
            catch (Exception ex) {
                MessageBox.Show("Error: " + ex.Message);
            }
            
        }
    }
}

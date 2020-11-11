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

namespace Vista
{
    public partial class frmCaja : Form
    {
        private string fechaIncio="";

        public frmCaja()
        {
            InitializeComponent();
            //posible solución para guardar fecha
            fechaIncio = Datos.Properties.Settings.Default.CajaFechaInicio;
            if (fechaIncio.Equals(""))
            {
                fechaIncio = DateTime.Now.ToString();
                btnAbrirCaja.Text = "Abrir Caja";
                this.Text = "Abrir Caja";
            }            
            lblFecha.Text = fechaIncio;
            lblUsuario.Text = "Cajero_1"; //probablemente obtener de Datos.Properties.Settings.Default.Usuario;
        }

        private void txtEfectivo_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }

            if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
            {
                e.Handled = true;
            }
        }

        private void btnAbrirCaja_Click(object sender, EventArgs e)
        {

        }
    }
}

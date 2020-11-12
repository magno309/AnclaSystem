using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Vista
{
    public partial class frmCaja : Form
    {
        private string fechaApertura;
        private string usuarioApertura;
        private decimal efectivoApertura;
        private bool cajaAbierta;

        public frmCaja()
        {
            InitializeComponent();
            fechaApertura = Properties.Settings.Default.cajaFechaApertura;
            usuarioApertura = Properties.Settings.Default.cajaUsuarioApertura;
            if (string.IsNullOrEmpty(fechaApertura) && string.IsNullOrEmpty(usuarioApertura))
            {
                fechaApertura = DateTime.Now.ToString();
                usuarioApertura = Properties.Settings.Default.nombreUsuarioL;     
                this.Text = "Apertura de caja";
                btnAbrirCaja.Text = "Abrir caja";                
                cajaAbierta = false;
            }
            else
            {
                this.Text = "Cierre de caja";
                btnAbrirCaja.Text = "Cerrar caja";
                cajaAbierta = true;
            }
            lblFecha.Text = fechaApertura;
            lblUsuario.Text = usuarioApertura;
        }

        private void btnAbrirCaja_Click(object sender, EventArgs e)
        {
            errorProviderEfectivo.SetError(txtEfectivo, "");
            if (decimal.TryParse(txtEfectivo.Text.Trim(), out efectivoApertura))
            {                
                if (cajaAbierta == false)
                {
                    Properties.Settings.Default.cajaFechaApertura = fechaApertura;
                    Properties.Settings.Default.cajaUsuarioApertura = usuarioApertura;
                    Properties.Settings.Default.cajaEfectivoApertura = efectivoApertura;
                    Properties.Settings.Default.Save();                    
                    MessageBox.Show("Apertura de caja realizada exitosamente");
                    this.Close();
                }
                else
                {
                    //generar reporte de cierre de caja
                    Properties.Settings.Default.cajaFechaApertura = "";
                    Properties.Settings.Default.cajaUsuarioApertura = "";
                    Properties.Settings.Default.cajaEfectivoApertura = 0;
                    Properties.Settings.Default.Save();                    
                    MessageBox.Show("Cierre de caja realizado exitosamente");
                    this.Close();
                }
            }
            else
            {
                errorProviderEfectivo.SetError(txtEfectivo, "Ingresa una cantidad correcta");
            }
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

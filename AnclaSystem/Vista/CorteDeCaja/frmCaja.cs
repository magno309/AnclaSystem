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
        private double efectivoApertura;
        private bool cajaAbierta;

        public frmCaja()
        {
            InitializeComponent();            
            fechaApertura = Properties.Settings.Default.CajaFechaApertura;
            usuarioApertura = Properties.Settings.Default.CajaUsuarioApertura;
            if (fechaApertura.Equals("") && usuarioApertura.Equals(""))
            {
                fechaApertura = DateTime.Now.ToString();
                usuarioApertura = "Cajero_1"; //Properties.Settings.Default.Usuario;     
                this.Text = "Apertura de caja";
                btnAbrirCaja.Text = "Abrir caja";
                lblFecha.Text = "Fecha:";
                cajaAbierta = false;
            }
            else
            {                
                this.Text = "Cierre de caja";
                btnAbrirCaja.Text = "Cerrar caja";
                cajaAbierta = true;
            }
            lblFechaHora.Text = fechaApertura;
            lblUsuario.Text = usuarioApertura;             
        }

        private void btnAbrirCaja_Click(object sender, EventArgs e)
        {            
            if (double.TryParse(txtEfectivo.Text.Trim(), out efectivoApertura))
            {
                errorProviderEfectivo.SetError(txtEfectivo, "");
                if (cajaAbierta == false)
                {
                    Properties.Settings.Default.CajaFechaApertura = fechaApertura;
                    Properties.Settings.Default.CajaUsuarioApertura = usuarioApertura;
                    Properties.Settings.Default.CajaEfectivoApertura = efectivoApertura;
                    Properties.Settings.Default.Save();
                    //generar notificación de operación exitosa
                    MessageBox.Show("apertura");
                    this.Close();
                }
                else
                {
                    //generar reporte de cierre de caja
                    Properties.Settings.Default.CajaFechaApertura = "";
                    Properties.Settings.Default.CajaUsuarioApertura = "";
                    Properties.Settings.Default.CajaEfectivoApertura = 0;
                    Properties.Settings.Default.Save();
                    //generar notificación de operación exitosa
                    MessageBox.Show("cierre");
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
    }
}

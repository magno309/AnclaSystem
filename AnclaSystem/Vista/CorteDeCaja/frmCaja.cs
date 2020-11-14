using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Vista.CorteDeCaja;
using Datos;
using Modelo;

namespace Vista
{
    public partial class frmCaja : Form
    {
        private string fechaApertura;
        private string usuarioApertura;
        private decimal efectivoIngresado;
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
            if (decimal.TryParse(txtEfectivo.Text.Trim(), out efectivoIngresado))
            {                
                if (cajaAbierta == false)
                {
                    Properties.Settings.Default.cajaFechaApertura = fechaApertura;
                    Properties.Settings.Default.cajaUsuarioApertura = usuarioApertura;
                    Properties.Settings.Default.cajaEfectivoApertura = efectivoIngresado;
                    Properties.Settings.Default.cajaAbierta = true;
                    Properties.Settings.Default.Save();                    
                    MessageBox.Show("Apertura de caja realizada exitosamente");
                    this.Close();
                }
                else
                {
                    //generar reporte de cierre de caja
                    Properties.Settings.Default.cajaFechaCierre = DateTime.Now.ToString();
                    Properties.Settings.Default.cajaEfectivoCierre = efectivoIngresado;
                    generarReporte();
                    Properties.Settings.Default.cajaFechaApertura = "";
                    Properties.Settings.Default.cajaFechaCierre = "";
                    Properties.Settings.Default.cajaUsuarioApertura = "";
                    Properties.Settings.Default.cajaEfectivoApertura = 0;
                    Properties.Settings.Default.cajaAbierta = false;
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

        private void generarReporte()
        {
            Properties.Settings.Default.cajaEfectivoTotal = (decimal) new daoVentas().obtenerTotalVentasPorFecha(DateTime.Parse(DateTime.Today.ToString("MM-dd-yyyy")));
            ReporteCaja nuevo = new ReporteCaja(
                    Properties.Settings.Default.idUsuarioL,
                    Properties.Settings.Default.cajaFechaApertura,
                    Properties.Settings.Default.cajaFechaCierre,
                    (double)Properties.Settings.Default.cajaEfectivoApertura,
                    (double)Properties.Settings.Default.cajaEfectivoCierre,
                    (double)Properties.Settings.Default.cajaEfectivoTotal,
                    ((double)Properties.Settings.Default.cajaEfectivoTotal - (double)Properties.Settings.Default.cajaEfectivoCierre)
                );
            if (new daoReporteCaja().insertarReporteCaja(nuevo))
            {
                frmReporteCaja frm = new frmReporteCaja();
                frm.ShowDialog();
            }
            else {
                MessageBox.Show(this, "Error al cerrar caja!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

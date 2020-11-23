using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Vista.MenuPrincipal
{
    public partial class frmMenu : Form
    {
        public frmMenu()
        {
            InitializeComponent();
        }

        private void btnAgregarVenta_Click(object sender, EventArgs e)
        {
            if (Properties.Settings.Default.cajaAbierta)
            {
                frmVenta frm = new frmVenta("Agregar", -1);
                frm.Show();
            }
            else {
                MessageBox.Show(this, "Debe realizar una apertura de caja!", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);

            }
        }


        private void frmMenu_Load(object sender, EventArgs e)
        {
            lblModo.Text = Properties.Settings.Default.esAdmin ? "Administrador" : "Usuario";
        }

        private void btnCorteDeCaja_Click(object sender, EventArgs e)
        {
            frmCaja frm = new frmCaja();
            frm.ShowDialog();
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            Vista.GestionDeVentas.frmListaVentas frm = new GestionDeVentas.frmListaVentas();
            frm.Show();
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            GestionDeProducto.frmCatalogoProducto frm = new GestionDeProducto.frmCatalogoProducto();
            frm.Show();
        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            frmInventario frm = new frmInventario();
            frm.Show();
        }

        private void toolStripButton5_Click(object sender, EventArgs e)
        {
            frmDirectorioProveedores frm = new frmDirectorioProveedores();
            frm.Show();
        }

        private void reportesDeCajaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CorteDeCaja.frmListaReportesCaja frm = new CorteDeCaja.frmListaReportesCaja();
            frm.Show();
        }

        private void frmMenu_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (CloseCancel() == System.Windows.Forms.DialogResult.Yes)
            {
                Dispose(true);
                Application.Exit();
            }
            else
            {
                e.Cancel = true;
            }
        }

        public static DialogResult CloseCancel()
        {
            const string message = "¿Está seguro que desea salir de la aplicación?";
            const string caption = "Salir";
            var result = MessageBox.Show(message, caption,
                                         MessageBoxButtons.YesNo,
                                         MessageBoxIcon.Question);

            return result;
        }

        private void salirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}

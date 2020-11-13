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
                frmVenta frm = new frmVenta();
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

        }
    }
}

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
            //Si ya se abrió caja
            frmVenta frm = new frmVenta();
            frm.Show();
        }
    }
}

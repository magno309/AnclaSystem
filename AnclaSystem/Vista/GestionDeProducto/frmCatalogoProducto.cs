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

namespace Vista.GestionDeProducto
{
    public partial class frmCatalogoProducto : Form
    {

        List<Productos> listaProductos;

        public frmCatalogoProducto()
        {
            InitializeComponent();
        }

        private void frmCatalogoProducto_Activated(object sender, EventArgs e)
        {
            listaProductos = new daoProducto().getProductosNoDescontinuados();
            dgvProductos.Rows.Clear();
            dgvProductos.DataSource = listaProductos;
            dgvProductos.Columns[0].Visible = false;
            dgvProductos.Columns[3].Visible = false;
            dgvProductos.Columns[1].HeaderText = "PRODUCTO";
            dgvProductos.Columns[2].HeaderText = "PRECIO";
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            frmDetalleProducto frm = new frmDetalleProducto(1);
            frm.ShowDialog();
        }
    }
}

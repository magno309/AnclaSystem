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
            dgvProductos.DataSource = listaProductos;
            dgvProductos.Columns[0].Visible = false;
            dgvProductos.Columns[3].Visible = false;
            dgvProductos.Columns[1].HeaderText = "PRODUCTO";
            dgvProductos.Columns[2].HeaderText = "PRECIO";
            dgvProductos.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgvProductos.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            frmDetalleProducto frm = new frmDetalleProducto(1);
            frm.ShowDialog();
        }

        private void btnModificar_Click(object sender, EventArgs e)
        {
            Productos seleccionado = new Productos(
                    (int)dgvProductos.SelectedRows[0].Cells[0].Value,
                    (string)dgvProductos.SelectedRows[0].Cells[1].Value,
                    (double)dgvProductos.SelectedRows[0].Cells[2].Value,
                    (bool)dgvProductos.SelectedRows[0].Cells[3].Value
                );
            frmDetalleProducto frm = new frmDetalleProducto(2, seleccionado);
            frm.ShowDialog();
        }

        private void dgvProductos_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            
        }
    }
}

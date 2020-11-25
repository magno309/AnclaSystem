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

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (dgvProductos.SelectedRows.Count > 0) {
                int index = dgvProductos.SelectedRows[0].Index;
                DialogResult res = MessageBox.Show(this, "¿Deseea eliminar el producto " + (string)dgvProductos.Rows[index].Cells[1].Value + "?",
                    "Eliminar producto", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (res == DialogResult.Yes) {
                    if (new daoProducto().EliminarProducto((int)dgvProductos.Rows[index].Cells[0].Value))
                    {
                        MessageBox.Show(this, "Producto eliminado correctamente!", "Eliminar producto", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else {
                        MessageBox.Show(this, "Error al eliminar producto!", "Eliminar producto", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            listaProductos = new daoProducto().getProductosNoDescontinuados(txtBuscar.Text + "%");
            dgvProductos.DataSource = listaProductos;
            dgvProductos.Columns[0].Visible = false;
            dgvProductos.Columns[3].Visible = false;
            dgvProductos.Columns[1].HeaderText = "PRODUCTO";
            dgvProductos.Columns[2].HeaderText = "PRECIO";
            dgvProductos.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgvProductos.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        }
    }
}

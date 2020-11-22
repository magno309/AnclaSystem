using Modelo;
using Datos;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Vista.GestionDeProducto
{
    public partial class frmDetalleProducto : Form
    {

        int modo;
        List<Ingrediente> listaIngredientes;
        public frmDetalleProducto(int modo)
        {
            InitializeComponent();
            listaIngredientes = new daoIngredientes().obtenerTodos();
            switch (modo) {
                case 1:
                    colIngrediente.DataSource = listaIngredientes;
                    colIngrediente.DisplayMember = "Nombre";
                    colIngrediente.ValueMember = "IdIngrediente";
                    this.Text = "Agregar producto";
                    btnAceptar.Text = "Agregar";
                    chbDescontinuado.Visible = false;
                    this.modo = modo;
                    break;
            }
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            switch (modo) {
                case 1:
                    if (!txtNombre.Text.Equals(""))
                    {
                        if (dgvIngredientes.Rows.Count >= 0)
                        {
                            Productos nuevo = new Productos(
                                    txtNombre.Text,
                                    double.Parse(txtPrecio.Text),
                                    false
                                );
                            Dictionary<int, int> listaIngredientes = new Dictionary<int, int>();

                            foreach (DataGridViewRow row in dgvIngredientes.Rows)
                            {
                                listaIngredientes.Add(int.Parse(row.Cells[0].Value.ToString()), int.Parse(row.Cells[1].Value.ToString()));
                            }
                            if (new daoProducto().AgregarProducto(nuevo, listaIngredientes))
                            {
                                MessageBox.Show(this, "Producto agregado correctamente!", "Producto agregado", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                            else {
                                MessageBox.Show(this, "Error al agregar producto!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }
                        }
                        else {
                            MessageBox.Show(this, "El producto debe tener al menos un ingrediente!", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                    else {
                        MessageBox.Show(this, "El producto debe tener un nombre!", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    break;
            }
        }

        private void txtPrecio_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }
            // only allow one decimal point
            if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
            {
                e.Handled = true;
            }
        }

        private void btnAgregarIngrediente_Click_1(object sender, EventArgs e)
        {
            dgvIngredientes.Rows.Add();
        }

        private void dgvIngredientes_DefaultValuesNeeded(object sender, DataGridViewRowEventArgs e)
        {
            
        }

        private void btnEliminarIngrediente_Click(object sender, EventArgs e)
        {
            if (dgvIngredientes.SelectedRows.Count >= 0) {
                DialogResult r = MessageBox.Show(this, "¿Deseea quitar ingrediente de la lista?",
                    "Quitar ingrediente", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (r == DialogResult.Yes) {
                    foreach (DataGridViewRow row in dgvIngredientes.SelectedRows) {
                        dgvIngredientes.Rows.Remove(row);
                    }
                }
            }
        }
    }
}

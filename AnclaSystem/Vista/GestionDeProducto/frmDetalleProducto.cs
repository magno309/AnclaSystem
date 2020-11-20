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

        public frmDetalleProducto(int modo)
        {
            InitializeComponent();
            cbIngredientes.ComboBox.DisplayMember = "NOMBRE";
            cbIngredientes.ComboBox.ValueMember = "ID";
            cbIngredientes.ComboBox.DataSource = new daoIngredientes().obtenerTodos();
            switch (modo) {
                case 1:
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
                            Dictionary<Ingrediente, int> listaIngredientes = new Dictionary<Ingrediente, int>();
                            


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

        private void btnAgregarIngrediente_Click(object sender, EventArgs e)
        {

        }
    }
}

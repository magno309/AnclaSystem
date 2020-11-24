using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Modelo;
using Datos;


namespace Vista.GestionDeVentas
{
    public partial class frmListaVentas : Form
    {
        daoVentas ventas;
        daoUsuario usuarios;
        int IDSeleccionado;

        public frmListaVentas()
        {
            ventas = new daoVentas();
            InitializeComponent();
            actualizarTabla();

        }



        private void dgvVentas_DoubleClick(object sender, EventArgs e)
        { 
        }

        private void dgvVentas_Click(object sender, EventArgs e)
        {
            IDSeleccionado = dgvVentas.CurrentCell.RowIndex;
        }

        private void btnModificar_Click(object sender, EventArgs e)
        {
            frmVenta modificar = new frmVenta("modificar", ventas.obtenerTodos()[IDSeleccionado].ID);
            modificar.Show();
            actualizarTabla();
        }

        private void btnVerDetalles_Click(object sender, EventArgs e)
        {
            frmVenta detalle = new frmVenta("detalles", ventas.obtenerTodos()[IDSeleccionado].ID);
            detalle.Show();
        }

        public void actualizarTabla()
        {
            dgvVentas.DataSource = null;
            dgvVentas.DataSource = ventas.obtenerTodos();
            dgvVentas.Columns[0].HeaderText = "No. VENTA";
            dgvVentas.Columns[3].Visible = false;
            cbCajero.DataSource = null;
            //cbCajero.DataSource = usuarios.buscarTodos();
        }

        private void cbCajero_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbCajero.SelectedIndex >= 0)
            {
                ventas.obtenerTodos(((Usuario)(cbCajero.Items[cbCajero.SelectedIndex])).id);
            }
            else
            {
                actualizarTabla();
            }
        }
    }
}

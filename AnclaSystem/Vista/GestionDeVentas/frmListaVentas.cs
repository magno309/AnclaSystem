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
        int IDSeleccionado;

        public frmListaVentas()
        {
            ventas = new daoVentas();
            InitializeComponent();
            dgvVentas.DataSource = null;
            dgvVentas.DataSource = ventas.obtenerTodos();
            dgvVentas.Columns[0].HeaderText = "No. VENTA";
            dgvVentas.Columns[3].Visible = false;

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
        }

        private void btnVerDetalles_Click(object sender, EventArgs e)
        {
            frmVenta detalle = new frmVenta("detalles", ventas.obtenerTodos()[IDSeleccionado].ID);
            detalle.Show();
        }
    }
}

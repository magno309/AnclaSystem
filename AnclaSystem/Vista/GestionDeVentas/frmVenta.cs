using Datos;
using Microsoft.VisualBasic;
using Modelo;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Vista
{
    public partial class frmVenta : Form
    {
        List<Productos> listaProductos;
        daoProducto prod;

        int IDseleccionado; //id del producto doble al dar clic
        int cantidad; //cantidad de productos
        public frmVenta()
        {
            InitializeComponent();
            listaProductos = new List<Productos>();
        }

        private void frmVenta_Load(object sender, EventArgs e)
        {
            //llenar tabla de productos
            listaProductos = prod.getProductosNoDescontinuados();
            dgvProductos.DataSource = listaProductos;

            //ocultar ID
            dgvProductos.Columns[0].Visible = false;
        }

        private void dgvProductos_DoubleClick(object sender, EventArgs e)
        {
            IDseleccionado = dgvProductos.CurrentCell.RowIndex;

            int cantidad = 0;
            while (!int.TryParse(Interaction.InputBox("Cantidad", "Cantidad", "1"), out cantidad) || cantidad < 1)
            {
                MessageBox.Show("Entrada invalida");
            }

        }
    }
}

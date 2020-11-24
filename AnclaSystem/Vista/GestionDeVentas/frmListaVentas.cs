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
        List<Usuario> listaUsuarios;
        List<Ventas> listaVentas;
        public frmListaVentas()
        {
            InitializeComponent();
            ventas = new daoVentas();
            usuarios = new daoUsuario();
            listaUsuarios = usuarios.buscarTodos();
            actualizarTabla();
            dtpFecha.MaxDate = DateTime.Now;
            cbCajero.SelectedIndex = 0;
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
            frmVenta modificar = new frmVenta("modificar", listaVentas[IDSeleccionado].ID);
            modificar.Show();
            listaVentas = ventas.obtenerTodos();
            dgvVentas.DataSource = null;
            dgvVentas.DataSource = listaVentas;
            dgvVentas.Columns[0].HeaderText = "No. VENTA";
            dgvVentas.Columns[3].Visible = false;
        }

        private void btnVerDetalles_Click(object sender, EventArgs e)
        {
            frmVenta detalle = new frmVenta("detalles", listaVentas[IDSeleccionado].ID);
            detalle.Show();
        }

        public void actualizarTabla()
        {
            listaVentas= ventas.obtenerTodos();
            dgvVentas.DataSource = null;
            dgvVentas.DataSource = listaVentas;
            dgvVentas.Columns[0].HeaderText = "No. VENTA";
            dgvVentas.Columns[3].Visible = false;
            Usuario aux = new Usuario();
            aux.nombre = "---TODOS---";
            listaUsuarios.Insert(0, aux);
            cbCajero.DataSource = listaUsuarios;
            cbCajero.ValueMember = "nombre";
        }

        private void cbCajero_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (((Usuario)(cbCajero.Items[cbCajero.SelectedIndex])).nombre.Equals("---TODOS---"))
            {
                listaVentas= ventas.obtenerTodos();
                dgvVentas.DataSource = null;
                dgvVentas.DataSource = listaVentas;
                dgvVentas.Columns[0].HeaderText = "No. VENTA";
                dgvVentas.Columns[3].Visible = false;
            }
            else
            {
                listaVentas= ventas.obtenerTodos(((Usuario)(cbCajero.Items[cbCajero.SelectedIndex])).id);
                dgvVentas.DataSource = null;
                dgvVentas.DataSource = listaVentas;
                dgvVentas.Columns[0].HeaderText = "No. VENTA";
                dgvVentas.Columns[3].Visible = false;
            }
        }

        private void dtpFecha_ValueChanged(object sender, EventArgs e)
        {
            if (((Usuario)(cbCajero.Items[cbCajero.SelectedIndex])).nombre.Equals("---TODOS---"))
            {

                listaVentas = ventas.obtenerTodos(dtpFecha.Value.ToString("yyyy-MM-dd"));
                dgvVentas.DataSource = null;
                dgvVentas.DataSource = listaVentas;
                dgvVentas.Columns[0].HeaderText = "No. VENTA";
                dgvVentas.Columns[3].Visible = false;
            }
            else
            {
                int ID_CAJ = ((Usuario)(cbCajero.Items[cbCajero.SelectedIndex])).id;
                listaVentas = ventas.obtenerTodos(dtpFecha.Value.ToString("yyyy-MM-dd"),ID_CAJ);
                dgvVentas.DataSource = null;
                dgvVentas.DataSource = listaVentas;
                dgvVentas.Columns[0].HeaderText = "No. VENTA";
                dgvVentas.Columns[3].Visible = false;
            }
        }
    }
}

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
using Modelo;
using Datos;

namespace Vista
{
    public partial class frmVenta : Form
    {
        List<Productos> listaProductos = new List<Productos>();
        List<DetalleVentas> listaDetalles = new List<DetalleVentas>();
        daoProducto prod = new daoProducto();
        daoVentas venta = new daoVentas();

        int IDseleccionado; //id del producto doble al dar clic
        int cantidad = 0; //cantidad de productos
        double total = 0; //total de la venta
        int cajeroID = 1; //cambiar cuando este el login
        public frmVenta()
        {
            InitializeComponent();
        }

        private void frmVenta_Load(object sender, EventArgs e)
        {
            //llenar tabla de productos
            try
            {
                listaProductos = prod.getProductosNoDescontinuados();
                dgvProductos.DataSource = listaProductos;

                //ocultar ID
                dgvProductos.Columns[0].Visible = false;
                //ocultar descontinuado
                dgvProductos.Columns[3].Visible = false;

                //poner nombre
                dgvProductos.Columns[1].HeaderText = "PRODUCTO";
                dgvProductos.Columns[1].HeaderText = "PRECIO";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void dgvProductos_DoubleClick(object sender, EventArgs e)
        {
            cantProd();
        }

        private void btnRegistrar_Click(object sender, EventArgs e)
        {
            registrarVenta();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            registrarVenta();
        }

        private void cantProd()
        {
            IDseleccionado = dgvProductos.CurrentCell.RowIndex;

            while (!int.TryParse(Interaction.InputBox("Cantidad", "Cantidad", "1"), out cantidad) || cantidad < 1)
            {
                MessageBox.Show("Entrada invalida");
            }
            if (cantidad > 0)
            {
                DetalleVentas detalle = new DetalleVentas();
                detalle.ID_PROD = listaProductos[IDseleccionado].ID;
                detalle.NOMBRE_AUX = listaProductos[IDseleccionado].nombre; //solo para mostrar en la tabla
                detalle.PRECIO_VENTA = listaProductos[IDseleccionado].precio;
                detalle.CANTIDAD = cantidad;
                detalle.SUBTOTAL = cantidad * listaProductos[IDseleccionado].precio;
                bool existe = false;
                foreach(DetalleVentas x in listaDetalles)
                {
                    if (x.ID_PROD==detalle.ID_PROD)
                    {
                        existe = true;
                        x.CANTIDAD += detalle.CANTIDAD;
                        break;
                    }
                }

                if (!existe)
                {
                    listaDetalles.Add(detalle);
                }

                dgvDetalle.DataSource = null;
                dgvDetalle.DataSource = listaDetalles;
                
                //ocultar IDS
                dgvDetalle.Columns[0].Visible = false;
                dgvDetalle.Columns[1].Visible = false;

                //poner nombres
                dgvDetalle.Columns[2].HeaderText = "PRODUCTO";
                dgvDetalle.Columns[4].HeaderText = "PRECIO";

                total += detalle.SUBTOTAL;
                lblTotal.Text = "$" + total;
            }
        }

        private void registrarVenta()
        {
            try
            {
                Ventas nuevaVenta = new Ventas();
                nuevaVenta.TOTAL = total;
                nuevaVenta.FECHA = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");
                nuevaVenta.ID_CAJERO = cajeroID;
                if (venta.agregarVenta(nuevaVenta, listaDetalles))
                {
                    MessageBox.Show("Venta registrada con exito");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void dgvProductos_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                cantProd();
                e.Handled = true;
            }
        }
    }
}

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
        String usuario = "NOMBRE USAURIO"; //es el autenticado
        public frmVenta()
        {
            InitializeComponent();
        }

        private void frmVenta_Load(object sender, EventArgs e)
        {
            iniciarForm();
        }

        public void iniciarForm()
        {
            try
            {
                //poner nombre usuario
                lblUsuario.Text = usuario;

                //poner fecha y hora de una vez
                lblNow.Text = DateTime.Now.ToString("yyyy-MM-dd hh:mm");

                //id venta
                lblVentaId.Text = (venta.getMax() + 1) + "";

                //llenar tabla de productos
                listaProductos = prod.getProductosNoDescontinuados();
                dgvProductos.DataSource = listaProductos;

                //ocultar ID
                dgvProductos.Columns[0].Visible = false;
                //ocultar descontinuado
                dgvProductos.Columns[3].Visible = false;

                //poner nombre
                dgvProductos.Columns[1].HeaderText = "PRODUCTO";
                dgvProductos.Columns[2].HeaderText = "PRECIO";
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

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            registrarVenta();
        }

        private void cantProd()
        {
            cantidad = 0;
            IDseleccionado = dgvProductos.CurrentCell.RowIndex;

            //VERIFICAR QUE LA ENTRADA DE CANTIDAD SEA MAYOR QUE 0
            while (true)
            {
                String input = Interaction.InputBox("Cantidad", "Cantidad", "1");
                if (input.Equals(""))
                {
                    break;
                }
                else if (!int.TryParse(input, out cantidad) || cantidad < 1)
                {
                    MessageBox.Show("Entrada invalida");
                }
                else
                {
                    break;
                }
            }
            if (cantidad > 0)
            {
                //CONSTRUIR EL DETALLE DE VENTA
                DetalleVentas detalle = new DetalleVentas();
                detalle.ID_PROD = listaProductos[IDseleccionado].ID;
                detalle.NOMBRE_AUX = listaProductos[IDseleccionado].nombre; //solo para mostrar en la tabla
                detalle.PRECIO_VENTA = listaProductos[IDseleccionado].precio;
                detalle.CANTIDAD = cantidad;
                detalle.SUBTOTAL = cantidad * listaProductos[IDseleccionado].precio;

                //VERIFICAR SI NO ES UN PRODUCTO REPETIDO
                bool existe = false;
                foreach (DetalleVentas x in listaDetalles)
                {
                    // SI ENCUENTRA EL PRODUCTO EN LA LISTA, SOLO ALTERA LA CANTIDAD Y SUBTOTAL
                    if (x.ID_PROD == detalle.ID_PROD)
                    {
                        existe = true;
                        x.CANTIDAD += detalle.CANTIDAD;
                        x.SUBTOTAL += x.PRECIO_VENTA * cantidad;
                        break;
                    }
                }

                //AGREGAR A LA LISTA DE DETALLE SI NO SE HA REGISTRADO EL PRODUCTO ANTES
                if (!existe)
                {
                    listaDetalles.Add(detalle);
                }

                //CALCULO DE TOTAL
                total += detalle.SUBTOTAL;
                lblTotal.Text = "$" + total;

                dgvDetalle.DataSource = null;
                dgvDetalle.DataSource = listaDetalles;

                //ocultar IDS
                dgvDetalle.Columns[0].Visible = false;
                dgvDetalle.Columns[1].Visible = false;

                //poner nombres
                dgvDetalle.Columns[2].HeaderText = "PRODUCTO";
                dgvDetalle.Columns[4].HeaderText = "PRECIO";

                erroP.SetError(btnReg, "");
            }
        }

        /// <summary>
        /// METODO PARA REGISTRAR LA VENTA
        /// </summary>
        private void registrarVenta()
        {
            if (total != 0)
            {
                erroP.SetError(btnReg, "");
                try
                {
                    this.ValidateChildren();
                    bool valido = true;

                    ///VERIFICA SI EL USUARIO INGRESO TODOS LOS DATOS CORRECTOS. NO CUENTA EL IMPORTE PORQUE NO SE REGISTRA EN LA BASE DE DATOS
                    foreach (Control item in this.Controls)
                    {
                        if (!erroP.GetError(item).Equals(""))
                        {
                            valido = false;
                            break;
                        }
                    }

                    ///SOLAMENTE REALIZAR LA VENTA SI NO HAY PROBLEMAS DE VALIDACION
                    if (valido)
                    {
                        Ventas nuevaVenta = new Ventas();
                        nuevaVenta.TOTAL = total;
                        nuevaVenta.FECHA = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");
                        nuevaVenta.ID_CAJERO = cajeroID;
                        if (venta.agregarVenta(nuevaVenta, listaDetalles))
                        {
                            MessageBox.Show("Venta registrada con exito");
                        }

                        ReloadForm();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else
            {
                erroP.SetError(btnReg, "No hay nada que registrar");
            }
        }

        /// <summary>
        /// PERMITE QUE SE CARGUE EL FORM PARA REGISTRAR OTRA VENTA
        /// </summary>
        private void ReloadForm()
        {
            listaProductos = new List<Productos>();
            listaDetalles = new List<DetalleVentas>();

            IDseleccionado = -1; //id del producto doble al dar clic
            cantidad = 0; //cantidad de productos
            total = 0; //total de la venta
            cajeroID = 1; //cambiar cuando este el login

            dgvDetalle.DataSource = null;
            dgvProductos.DataSource = null;
            iniciarForm();

            lblTotal.Text = "$ 0";
            lblCambio.Text = "$ 0";
            txtImporte.Text = "";
        }

        /// <summary>
        /// SI SE DA ENTER EN LA TABLA DE PRODUCTOS PUEDE REGISTRARSE EN LA VENTA
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvProductos_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                cantProd();
                e.Handled = true;
            }
        }

        /// <summary>
        /// PREGUNTAR AL USUARIO SI QUIERE SALIR DE LA VENTANA
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmVenta_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult dialogo = MessageBox.Show("¿Desea cerrar la ventana? Perderá los cambios.",
               "Cerrar el programa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dialogo == DialogResult.No)
            {
                e.Cancel = true;
            }
            else
            {
                e.Cancel = false;
            }
        }

        private void btnReg_Click(object sender, EventArgs e)
        {
            registrarVenta();
        }

        /// <summary>
        /// VALIDAD IMPORTE, QUE EL VALOR SEA MAYOR O IGUAL A LA VENTA
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtImporte_Validating(object sender, CancelEventArgs e)
        {
            if (txtImporte.Text.Equals("") || Convert.ToDouble(txtImporte.Text) < total)
            {
                erroP.SetError(txtImporte, "La cantidad debe ser mayor al total de la venta");
            }
            else
            {
                erroP.SetError(txtImporte, "");
            }
        }

        /// <summary>
        /// VALIDAR QUE EL DATO INGRESADO EN IMPORTE TENGA FORMATO DECIMAL(10,2)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtImporte_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (txtImporte.Text.Contains("."))
            {
                if (!char.IsDigit(e.KeyChar) && e.KeyChar != Convert.ToChar(Keys.Back))
                {
                    e.Handled = true;
                }
            }
            else
            {
                if (txtImporte.Text.Length != 8)
                {
                    if (!char.IsDigit(e.KeyChar) && e.KeyChar != Convert.ToChar(Keys.Back))
                    {
                        e.Handled = true;
                    }

                    if (e.KeyChar.Equals('.'))
                    {
                        e.Handled = false;
                    }
                }
                else
                {
                    if (e.KeyChar.Equals('.') || e.KeyChar == Convert.ToChar(Keys.Back))
                    {
                        e.Handled = false;
                    }
                    else
                    {
                        e.Handled = true;
                    }
                }
            }
        }

        /// <summary>
        /// METODO PARA CALCULAR EL CAMBIO
        /// </summary>
        public void calcularCambio()
        {
            double importe = 0;
            try
            {
                if (!txtImporte.Text.Equals(""))
                {
                    importe = Double.Parse(txtImporte.Text);
                }
                double cambio = importe - total;
                lblCambio.Text = "$ " + cambio;
            }
            catch
            {

            }
        }

        private void txtImporte_KeyUp(object sender, KeyEventArgs e)
        {
            calcularCambio();
        }

        private void toolStripLabel1_Click(object sender, EventArgs e)
        {

        }
    }
}

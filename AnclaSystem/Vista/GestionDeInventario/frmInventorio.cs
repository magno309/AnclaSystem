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

namespace Vista
{
    public partial class frmInventario : Form
    {
        private List<Ingrediente> lista;
        private daoIngredientes dao;
        private int indexFila;

        public frmInventario()
        {
            InitializeComponent();
            dao = new daoIngredientes();            
            llenarTabla();
            llenarComboUnidad();
            limpiar();
        }

        private void limpiar()
        {
            btnModificar.Enabled = false;
            btnEliminar.Enabled = false;
            btnLimpiar.Enabled = false;
            txtNombre.Text = "";
            txtStock.Text = "";
            cmbUnidad.SelectedIndex = 0;
        }

        private void llenarTabla()
        {
            try
            {
                lista = dao.obtenerTodos();
                dgvIngredientes.DataSource = lista;
                dgvIngredientes.Columns[0].Visible = false;
                dgvIngredientes.Columns[4].Visible = false;
                dgvIngredientes.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                dgvIngredientes.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                dgvIngredientes.Columns[2].Width = 100;                  
                dgvIngredientes.Columns[3].Width = 100;                
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ocurrió un error al cargar todos los ingredientes\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void llenarTabla(List<Ingrediente> listaIngredientes)
        {
            try
            {
                dgvIngredientes.DataSource = listaIngredientes;
                dgvIngredientes.Columns[0].Visible = false;
                dgvIngredientes.Columns[4].Visible = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ocurrió un error al cargar los ingredientes solicitados\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void llenarComboUnidad()
        {
            cmbUnidad.DataSource = new string[] { "SELECCIONAR UNIDAD", "PIEZAS", "GRAMOS", "MILILITROS" };
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            limpiar();
            llenarTabla();
        }

        private bool validarCampos()
        {
            errorProvider.SetError(txtNombre, "");
            errorProvider.SetError(txtStock, "");
            errorProvider.SetError(cmbUnidad, "");
            bool validos = true;
            if (string.IsNullOrEmpty(txtNombre.Text.Trim()))
            {
                errorProvider.SetError(txtNombre, "Ingresa un nombre");
                validos = false;
            }
            if (string.IsNullOrEmpty(txtStock.Text.Trim()))
            {
                errorProvider.SetError(txtStock, "Ingresa el stock");
                validos = false;
            }              
            else if (!(int.TryParse(txtStock.Text.Trim(), out int s) && s>=0))
            {
                errorProvider.SetError(txtStock, "Ingresa una cantidad mayor o igual a 0");
                validos = false;
            }
            if (cmbUnidad.SelectedIndex == 0)
            {
                errorProvider.SetError(cmbUnidad, "Selecciona una unidad");
                validos = false;
            }
            return validos;
        }

        private bool nombreDuplicado(string nombre)
        {
            bool duplicado = false;
            try
            {
                duplicado = dao.nombreDuplicado(nombre);
            }
            catch(Exception ex)
            {
                throw ex;
            }
            return duplicado;
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {            ;
            if (validarCampos())
            {
                string nombre = txtNombre.Text.Trim();
                string unidad = cmbUnidad.SelectedValue.ToString();
                int stock = int.Parse(txtStock.Text.Trim());
                bool duplicado = nombreDuplicado(nombre);
                bool agregarDuplicado = false;
                if (duplicado)
                {
                    if (MessageBox.Show("Un ingrediente con este nombre ya está registrado\n¿Desea registrarlo otra vez?", "Ingrediente duplicado", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        agregarDuplicado = true;
                    }
                }
                if(!duplicado || (duplicado && agregarDuplicado))
                {
                    Ingrediente nuevo = new Ingrediente(nombre, unidad, stock, false);
                    try
                    {
                        if (dao.agregar(nuevo))
                        {                            
                            llenarTabla();
                            MessageBox.Show("Nuevo ingrediente agregado con éxito", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            MessageBox.Show("No fue posible agregar el ingrediente\n", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Ocurrió un error al agregar el ingrediente\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                }                
            }           
        }        

        private void txtNombre_TextChanged(object sender, EventArgs e)
        {
            if (!errorProvider.GetError(txtNombre).Equals(""))
            {
                if (!string.IsNullOrEmpty(txtNombre.Text.Trim()))
                {
                    errorProvider.SetError(txtNombre, "");
                }
            }
        }

        private void dgvIngredientes_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            errorProvider.SetError(txtNombre, "");
            errorProvider.SetError(txtStock, "");
            errorProvider.SetError(cmbUnidad, "");
            btnModificar.Enabled = true;
            btnEliminar.Enabled = true;
            btnLimpiar.Enabled = true;
            indexFila = e.RowIndex;
            txtNombre.Text = lista[indexFila].Nombre;
            txtStock.Text = lista[indexFila].Stock.ToString();
            cmbUnidad.SelectedIndex = lista[indexFila].Unidad.Equals("PIEZAS") ? 1 : lista[indexFila].Unidad.Equals("GRAMOS") ? 2 : 3;
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                if(!string.IsNullOrEmpty(txtStock.Text.Trim()) && !(int.TryParse(txtStock.Text.Trim(), out int s) && s >= 0))
                {
                    errorProvider.SetError(txtStock, "Ingresa una cantidad mayor o igual a 0");
                }
                else
                {
                    btnLimpiar.Enabled = true;
                    string nombre = txtNombre.Text.Trim();
                    string unidad = (cmbUnidad.SelectedIndex == 0) ? "" : cmbUnidad.SelectedValue.ToString();
                    int stock = string.IsNullOrEmpty(txtStock.Text.Trim()) ? -1 : int.Parse(txtStock.Text.Trim());
                    List<Ingrediente> resultado = dao.buscarPatron(nombre, unidad, stock);
                    if (!resultado.Equals(null) && resultado.Count>0)
                    {
                        llenarTabla(resultado);
                    }
                    else
                    {
                        MessageBox.Show("No se encontró ningún ingrediente con los datos proporcionados", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }                
            }
            catch(Exception ex)
            {
                MessageBox.Show("Ocurrió un error al buscar el ingrediente\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void txtStock_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }           
        }

        private void txtStock_TextChanged(object sender, EventArgs e)
        {
            switch (errorProvider.GetError(txtStock))
            {
                case "Ingresa el stock":
                    if (!string.IsNullOrEmpty(txtStock.Text.Trim()))
                    {
                        errorProvider.SetError(txtStock, "");
                    }
                    break;
                case "Ingresa una cantidad mayor o igual a 0":
                    if (int.TryParse(txtStock.Text.Trim(), out int s) && s >= 0)
                    {
                        errorProvider.SetError(txtStock, "");
                    }
                    break;
            }
        }

        private void cmbUnidad_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!errorProvider.GetError(cmbUnidad).Equals(""))
            {
                if (cmbUnidad.SelectedIndex != 0)
                {
                    errorProvider.SetError(cmbUnidad, "");
                }
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("¿Estás seguro que deseas eliminar el ingrediente " +
                lista[indexFila].Nombre + " con unidad "+ lista[indexFila].Unidad + " y stock "+ lista[indexFila].Stock + "?", "Confirmación eliminación", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                try
                {
                    dao.descontinuar(lista[indexFila].IdIngrediente);                    
                    limpiar();
                    llenarTabla();
                    MessageBox.Show("Ingrediente eliminado con éxito", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ocurrió un error al eliminar el ingrediente\n " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnModificar_Click(object sender, EventArgs e)
        {
            if (validarCampos())
            {
                Ingrediente ingrediente = new Ingrediente();
                ingrediente.IdIngrediente = lista[indexFila].IdIngrediente;
                ingrediente.Nombre = txtNombre.Text.Trim();
                ingrediente.Unidad = cmbUnidad.SelectedValue.ToString();
                ingrediente.Stock = int.Parse(txtStock.Text.Trim());
                ingrediente.Descontinuado = lista[indexFila].Descontinuado;
                if (!ingrediente.Equals(lista[indexFila]))
                {
                    try
                    {
                        if (dao.actualizar(ingrediente))
                        {
                            llenarTabla();
                            limpiar();
                            MessageBox.Show("Ingrediente modificado con éxito", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);   
                        }
                        else
                        {
                            MessageBox.Show("No fue posible modificar el ingrediente\n", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                    catch(Exception ex)
                    {
                        MessageBox.Show("Ocurrió un error al modificar el ingrediente\n " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show("No se modificó ningún dato del ingrediente", "Datos duplicados", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }
    }
}

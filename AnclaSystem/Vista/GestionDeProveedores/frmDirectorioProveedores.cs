using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using Modelo;
using Datos;

namespace Vista
{
    /// <summary>
    /// CRUD Proveedores
    /// </summary>
    public partial class frmDirectorioProveedores : Form
    {
        int index = 0; //llevar el control del indice
        List<Proveedores> listaProveedores;
        daoProveedor daoProveedor;
        public frmDirectorioProveedores()
        {
            InitializeComponent();
            daoProveedor = new daoProveedor();
            cargarProveedores();
        }

        private void cargarProveedores()
        {
            listaProveedores = daoProveedor.devolverProveedores();
            dgvProveedores.DataSource = listaProveedores;

            //ocultar ID
            dgvProveedores.Columns[0].Visible = false;

            //poner nombres
            dgvProveedores.Columns[1].HeaderText = "Empresa";
            dgvProveedores.Columns[2].HeaderText = "Contacto";
        }


        /// <summary>
        /// Evita que se tecleen mas de 35 caracteres para el nombre de la empresa.
        /// Permite cualquier letra, caracter o numero.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtNombreEmpresa_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (txtNombreEmpresa.Text.Length >= 35)
            {
                if (e.KeyChar == Convert.ToChar(Keys.Back))
                {
                    e.Handled = false;
                }
                else
                {
                    e.Handled = true;
                }
            }
        }

        /// <summary>
        /// Evita que se tecleen mas de 35 caracteres para el nombre de contacto
        /// Solo letras y espacios para nombre
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtNombreContacto_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (txtNombreContacto.Text.Length < 35)
            {
                if (e.KeyChar != Convert.ToChar(Keys.Space) && !char.IsLetter(e.KeyChar) && e.KeyChar != Convert.ToChar(Keys.Back))
                {
                    e.Handled = true;
                }
            }
            else
            {
                if (e.KeyChar == Convert.ToChar(Keys.Back))
                {
                    e.Handled = false;
                }
                else
                {
                    e.Handled = true;
                }
            }
        }

        /// <summary>
        /// Permite solo numeros y que no se teclee mas de 15 caracteres para el campo telefono
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtTelefono_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (txtTelefono.Text.Length < 15)
            {
                if (!char.IsDigit(e.KeyChar) && e.KeyChar != Convert.ToChar(Keys.Back))
                {
                    e.Handled = true;
                }
            }
            else
            {
                if (e.KeyChar == Convert.ToChar(Keys.Back))
                {
                    e.Handled = false;
                }
                else
                {
                    e.Handled = true;
                }
            }
        }

        /// <summary>
        /// Filtra que en el campo de correo no se digiten mas de 30 caracteres
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtCorreo_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (txtDireccion.Text.Length >= 30)
            {
                if (e.KeyChar == Convert.ToChar(Keys.Back))
                {
                    e.Handled = false;
                }
                else
                {
                    e.Handled = true;
                }
            }
        }

        /// <summary>
        /// Filtra que no se escriban mas de 40 digitos para el campo direccion
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtDireccion_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (txtDireccion.Text.Length >= 40)
            {
                if (e.KeyChar == Convert.ToChar(Keys.Back))
                {
                    e.Handled = false;
                }
                else
                {
                    e.Handled = true;
                }
            }
        }

        /// <summary>
        /// Cuando se da doble clic a una celda, los textbox se cargan con esos datos del proveedor seleccionado
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvProveedores_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                //limpiar errorProvider
                foreach (Control item in gbDatos.Controls)
                {
                    errorP.SetError(item, "");
                }

                //indice actual
                index = e.RowIndex;

                btnModificar.Enabled = true;
                btnEliminar.Enabled = true;
                btnLimpiar.Enabled = true;

                txtNombreContacto.Text = listaProveedores[index].NombreContacto;
                txtNombreEmpresa.Text = listaProveedores[index].NombreEmpresa;
                txtTelefono.Text = listaProveedores[index].Telefono;
                txtCorreo.Text = listaProveedores[index].Correo;
                txtDireccion.Text = listaProveedores[index].Direccion;
            }
            catch (Exception)
            {

            }
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            limpiar();
        }

        /// <summary>
        /// Limpia la ventana y carga todos los proveedores sin filtros
        /// </summary>
        private void limpiar()
        {
            //limpiar errorProvider
            foreach (Control item in gbDatos.Controls)
            {
                errorP.SetError(item, "");
            }

            btnModificar.Enabled = false;
            btnEliminar.Enabled = false;
            btnLimpiar.Enabled = false;
            txtNombreContacto.Text = "";
            txtNombreEmpresa.Text = "";
            txtTelefono.Text = "";
            txtCorreo.Text = "";
            txtDireccion.Text = "";
            index = 0;
            cargarProveedores();
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            //limpiar errorProvider
            foreach (Control item in gbDatos.Controls)
            {
                errorP.SetError(item, "");
            }

            String keyContacto = txtNombreContacto.Text;
            String keyEmpresa = txtNombreEmpresa.Text;
            String keyTelefono = txtTelefono.Text;
            String keyCorreo = txtCorreo.Text;
            String keyDireccion = txtDireccion.Text;
            dgvProveedores.DataSource =  daoProveedor.buscarProveedor(keyContacto, keyEmpresa, 
                keyTelefono, keyDireccion, keyCorreo);
            btnLimpiar.Enabled = true;
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            Proveedores nuevoProveedor = new Proveedores();
            nuevoProveedor.NombreContacto = txtNombreContacto.Text;
            nuevoProveedor.NombreEmpresa = txtNombreEmpresa.Text;
            nuevoProveedor.Telefono = txtTelefono.Text;
            nuevoProveedor.Direccion = txtDireccion.Text;
            nuevoProveedor.Correo = txtCorreo.Text;

            //verifica si hay datos duplicados
            if (!listaProveedores[index].Equals(nuevoProveedor))
            {
                try
                {
                    //verifica que no haya campos vacios
                    if (this.valido())
                    {
                        //agrega proveedor y reestablece ventana
                        daoProveedor.agregaProveedor(nuevoProveedor);
                        MessageBox.Show("Nuevo proveedor agregado con exito", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        limpiar();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ocurrio un error al agregar proveedor\n" + ex.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                //si hay datos repetidos pide confirmacion para duplicar proveedor
                if (MessageBox.Show("Este proveedor ya esta registrado\n¿Desea registrarlo de nuevo?:",
                "Proveedor duplicado", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    try
                    {
                        //valida que no haya campos vacios por si acaso
                        if (this.valido())
                        {
                            //agrega nuevo proveedor
                            daoProveedor.agregaProveedor(nuevoProveedor);
                            MessageBox.Show("Nuevo proveedor agregado con exito", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            limpiar();
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Ocurrio un error al agregar proveedor\n" + ex.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        /// <summary>
        /// Metodo que valida los campos de la ventana, que no esten vacios y 
        /// que el correo tenga formato correcto solo cuando se agregue o modifique, no cuando se busque
        /// </summary>
        /// <returns></returns>
        private Boolean valido()
        {
            this.ValidateChildren();

            Boolean valido = true;

            ///verifica si los textbox estan vacios
            foreach (Control textbox in gbDatos.Controls)
            {
                if (string.IsNullOrEmpty(textbox.Text))
                {
                    errorP.SetError(textbox, "Este campo es obligatorio");
                }
                else if (!Regex.IsMatch(txtCorreo.Text, "[a-zA-Z]+[-_.a-zA-Z0-9]*[a-zA-Z0-9]+@([a-zA-Z]+[.][a-zA-Z]+)+$"))
                {
                    errorP.SetError(txtCorreo, "El email no cumple con el formato especificado");
                }
            }

            ///verifica si el error provider esta activo o no, si esta activo, aun no se validan los datos
            foreach (Control item in gbDatos.Controls)
            {
                if (!errorP.GetError(item).Equals(""))
                {
                    valido = false;
                    break;
                }
            }
            return valido;
        }
        
        private void btnEliminar_Click(object sender, EventArgs e)
        {
            //Pedir confirmacion        
            if (MessageBox.Show("¿Está seguro de que desea eliminar el Proveedor: \n" +
                listaProveedores[index].NombreEmpresa + "?",
                "Confirmación eliminación", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                try
                {
                    //eliminar y reestablecer ventana
                    daoProveedor.eliminarProveedor(listaProveedores[index].ID);
                    limpiar();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("No se pudo eliminar proveedor\n " + ex.Message, "Aviso",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnModificar_Click(object sender, EventArgs e)
        {
            Proveedores nuevoProveedor = new Proveedores();
            nuevoProveedor.NombreContacto = txtNombreContacto.Text;
            nuevoProveedor.NombreEmpresa = txtNombreEmpresa.Text;
            nuevoProveedor.Telefono = txtTelefono.Text;
            nuevoProveedor.Direccion = txtDireccion.Text;
            nuevoProveedor.Correo = txtCorreo.Text;

            //verifica si hay modificaciones
            if (!listaProveedores[index].Equals(nuevoProveedor))
            {
                try
                {
                    //modifica y reestablece ventana
                    nuevoProveedor.ID = listaProveedores[index].ID;
                    daoProveedor.modificarProveedor(nuevoProveedor);
                    MessageBox.Show("Proveedor modificado con exito", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    limpiar();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("No se pudo modificar proveedor\n " + ex.Message, "Aviso",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Datos duplicados", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}

using Datos;
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

namespace Vista.GestionDeUsuarios
{
    public partial class frmDirectorioUsuarios : Form
    {
        List<Usuario> lista;

        public frmDirectorioUsuarios()
        {
            InitializeComponent();
            cargarTabla();
        }

        public void cargarTabla() {
            lista = new daoUsuario().buscarTodos();   // Consultar los usuarios activos del sistema
            // Asignar la tabla
            dgvUsuarios.DataSource = lista;
            dgvUsuarios.AutoGenerateColumns = false;
            if (dgvUsuarios.Columns.Contains("id")) {
                dgvUsuarios.Columns.Remove("id");
                dgvUsuarios.Columns.Remove("contrasenia");
                dgvUsuarios.Columns.Remove("esActivo");
            }
            dgvUsuarios.Columns[0].HeaderText = "Nombre";
            dgvUsuarios.Columns[1].HeaderText = "Usuario";
            dgvUsuarios.Columns[2].HeaderText = "Es Admin";
            dgvUsuarios.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvUsuarios.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
        }

        public void cargarBusqueda(String patron) {
            try { 
                lista = new daoUsuario().buscarPatron(patron);   // Consultar los usuarios activos del sistema
               // Asignar la tabla
                dgvUsuarios.DataSource = lista;
                dgvUsuarios.AutoGenerateColumns = false;
                if (dgvUsuarios.Columns.Contains("id")) {
                    dgvUsuarios.Columns.Remove("id");
                    dgvUsuarios.Columns.Remove("contrasenia");
                    dgvUsuarios.Columns.Remove("esActivo");
                }
                dgvUsuarios.Columns[0].HeaderText = "Nombre";
                dgvUsuarios.Columns[1].HeaderText = "Usuario";
                dgvUsuarios.Columns[2].HeaderText = "Es Admin";
                dgvUsuarios.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                dgvUsuarios.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            } catch (Exception ex){
                MessageBox.Show(ex.Message);
            }
        }

        private void btnAgregar_Click(object sender, EventArgs e) {
            frmModificarUsuario form = new frmModificarUsuario(null);
            form.Text = "Agregar usuario nuevo";
            form.Show();
        }

        private void btnModificar_Click(object sender, EventArgs e) {
            if(dgvUsuarios.SelectedRows.Count == 0) {
                MessageBox.Show("Favor de seleccionar un registro para modificarlo.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            } else if(dgvUsuarios.SelectedRows.Count > 1) {
                MessageBox.Show("Seleccione solo un registro.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            } else {
                //String usr = dgvUsuarios.SelectedRows[0].Cells[1].Value.ToString();
                Usuario seleccionado = lista.ElementAt(dgvUsuarios.SelectedRows[0].Index);
                frmModificarUsuario form = new frmModificarUsuario(seleccionado);
                form.Text = "Modificar usuario";
                form.Show();
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e) {
            if (dgvUsuarios.SelectedRows.Count == 0) {
                MessageBox.Show("Favor de seleccionar un registro para modificarlo.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            } else if (dgvUsuarios.SelectedRows.Count > 1) {
                MessageBox.Show("Por su seguridad, solo se permite eliminar un registro a la vez.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            } else {
                String usr = dgvUsuarios.SelectedRows[0].Cells[1].Value.ToString();
                DialogResult midr = MessageBox.Show("Está seguro que desea eliminar el usuario " + usr + "?", "Confirmar eliminación", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (midr.Equals(DialogResult.Yes)) {
                    int idx = lista.ElementAt(dgvUsuarios.SelectedRows[0].Index).id;
                    bool res = new daoUsuario().eliminar(idx);
                    if (res) {
                        MessageBox.Show(usr + " se ha eliminado correctamente.", "Eliminar", MessageBoxButtons.OK, MessageBoxIcon.None);
                        cargarTabla();
                    } else
                        MessageBox.Show("Fallo al eliminar a " + usr, "Eliminar", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
                
        }

        private void btnBuscar_Click(object sender, EventArgs e) {
            try {
                String patron = txtBuscar.Text;
                cargarBusqueda(patron);

            }catch(Exception ex) {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void txtBuscar_TextChanged(object sender, EventArgs e) {
            if (txtBuscar.Text.Equals("")) {
                cargarTabla();
            }
        }
    }
}

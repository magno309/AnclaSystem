using Datos;
using Modelo;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Vista.GestionDeUsuarios
{
    /// <summary>
    /// Esta clase se emplea tanto para Agregar usuario como para Editar usuario
    /// </summary>
    public partial class frmModificarUsuario : Form
    {
        Usuario obj;    // Objeto con infomración precargada para editar
        int tipo = 0;       // Tipo 0 = Agregar; Tipo 1 = Editar

        public frmModificarUsuario(Usuario obj)
        {
            InitializeComponent();
            this.obj = obj;
            if (obj != null) {
                this.tipo = 1;
                btnGuardar.Text = "Guardar";
                txtNombre.Text = obj.nombre;
                txtUsuario.Text = obj.nombre_usuario;
                chbAdmin.Checked = obj.esAdmin;
            } else {
                btnGuardar.Text = "Agregar";
            }

        }

        private void btnGuardar_Click(object sender, EventArgs e) {
            //Validar coincidencia de contrasenias
            
            bool valido = true;
            this.ValidateChildren();
            foreach(Control c in this.Controls) {
                if(!errorP.GetError(c).Equals("")) {
                    valido = false;
                }
            }
            // Si los valores son valido, procede a agregarse al sistema
            if (valido) {
                String nom = txtNombre.Text;
                String usr = txtUsuario.Text;
                String psw = txtContrasenia.Text;
                bool esAdmin = chbAdmin.Checked;
                // Verifica el tipo de formulario
                if (tipo == 0) {                        // Agregar
                    Usuario nuevo = new Usuario(nom, usr, psw, esAdmin, true);
                    try {
                        bool agregado = new daoUsuario().agregar(nuevo);
                        if (agregado) {
                            MessageBox.Show("Registro agregado exitosamente!");
                            actualizarTabla();
                            this.Close();
                        }
                    } catch (MySqlException ex) {
                       if(ex.Number == 1062) {
                            MessageBox.Show("El usuario '" + usr + "' ya existe en el sistema.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    } catch (Exception ex) {
                        MessageBox.Show("Fallo al agregar usuario: \n" + ex, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                } else {                                // Editar
                    this.obj.nombre = nom;
                    this.obj.nombre_usuario = usr;
                    this.obj.contrasenia = psw;
                    this.obj.esAdmin = esAdmin;
                    this.obj.esActivo = true;
                    try {
                        bool editado = new daoUsuario().editar(obj);
                        if (editado) {
                            MessageBox.Show("Registro modificado exitosamente!");
                            actualizarTabla();
                            this.Close();
                        }
                    } catch(MySqlException ex) {
                        MessageBox.Show("Fallo al modificar usuario: \n" + ex, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    } catch(Exception ex) {
                        MessageBox.Show("Fallo al modificar usuario: \n" + ex, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
           
        }

        private void txtNombre_Validating(object sender, CancelEventArgs e) {
            if (txtNombre.Text.Equals("")) {
                errorP.SetError(txtNombre, "Este campo no puede estar vacío.");
            }
            else {
                errorP.SetError(txtNombre, "");
            }

        }

        private void txtUsuario_Validating(object sender, CancelEventArgs e) {
            if (txtUsuario.Text.Equals("")) {
                errorP.SetError(txtUsuario, "Este campo no puede estar vacío.");
            }
            else {
                errorP.SetError(txtUsuario, "");
            }
        }

        private void txtContrasenia_Validating(object sender, CancelEventArgs e) {
            if (txtContrasenia.Text.Equals("")) {
                errorP.SetError(txtContrasenia, "Este campo no puede estar vacío.");
            }
            else {
                errorP.SetError(txtContrasenia, "");
            }
        }

        private void txtConfirmar_Validating(object sender, CancelEventArgs e) {
            if (txtConfirmar.Text.Equals("")) {
                errorP.SetError(txtConfirmar, "Este campo no puede estar vacío.");
            } else if (!txtContrasenia.Text.Equals(txtConfirmar.Text)) {
                errorP.SetError(txtConfirmar, "Las contraseñas no coinciden.");
            } else {
                errorP.SetError(txtConfirmar, "");
            }
        }
        
        private void actualizarTabla() {
            foreach(Form f in Application.OpenForms) {
                //Validasi el formulario que contiene la tabla de usuarios, esta abierto
                if (f.Name.Equals("frmDirectorioUsuarios")) {
                    ((frmDirectorioUsuarios)f).cargarTabla();
                }
            }
        }
    }
}

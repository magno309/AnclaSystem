﻿namespace Vista.GestionDeUsuarios
{
    partial class frmModificarUsuario
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnModificar = new System.Windows.Forms.Button();
            this.txtNombre = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtUsuario = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtContrasenia = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtConfirmar = new System.Windows.Forms.TextBox();
            this.chbAdmin = new System.Windows.Forms.CheckBox();
            this.chbActivo = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // btnModificar
            // 
            this.btnModificar.Location = new System.Drawing.Point(182, 170);
            this.btnModificar.Name = "btnModificar";
            this.btnModificar.Size = new System.Drawing.Size(322, 30);
            this.btnModificar.TabIndex = 17;
            this.btnModificar.Text = "Modificar usuario";
            this.btnModificar.UseVisualStyleBackColor = true;
            // 
            // txtNombre
            // 
            this.txtNombre.Location = new System.Drawing.Point(182, 12);
            this.txtNombre.Name = "txtNombre";
            this.txtNombre.Size = new System.Drawing.Size(322, 26);
            this.txtNombre.TabIndex = 9;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(108, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(69, 20);
            this.label1.TabIndex = 10;
            this.label1.Text = "Nombre:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(109, 47);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(68, 20);
            this.label2.TabIndex = 13;
            this.label2.Text = "Usuario:";
            // 
            // txtUsuario
            // 
            this.txtUsuario.Location = new System.Drawing.Point(182, 44);
            this.txtUsuario.Name = "txtUsuario";
            this.txtUsuario.Size = new System.Drawing.Size(322, 26);
            this.txtUsuario.TabIndex = 11;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(81, 79);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(96, 20);
            this.label3.TabIndex = 15;
            this.label3.Text = "Contraseña:";
            // 
            // txtContrasenia
            // 
            this.txtContrasenia.Location = new System.Drawing.Point(182, 76);
            this.txtContrasenia.Name = "txtContrasenia";
            this.txtContrasenia.PasswordChar = '*';
            this.txtContrasenia.Size = new System.Drawing.Size(322, 26);
            this.txtContrasenia.TabIndex = 12;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(11, 111);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(166, 20);
            this.label4.TabIndex = 16;
            this.label4.Text = "Confirmar contraseña:";
            // 
            // txtConfirmar
            // 
            this.txtConfirmar.Location = new System.Drawing.Point(182, 108);
            this.txtConfirmar.Name = "txtConfirmar";
            this.txtConfirmar.PasswordChar = '*';
            this.txtConfirmar.Size = new System.Drawing.Size(322, 26);
            this.txtConfirmar.TabIndex = 14;
            // 
            // chbAdmin
            // 
            this.chbAdmin.AutoSize = true;
            this.chbAdmin.Location = new System.Drawing.Point(182, 140);
            this.chbAdmin.Name = "chbAdmin";
            this.chbAdmin.Size = new System.Drawing.Size(126, 24);
            this.chbAdmin.TabIndex = 18;
            this.chbAdmin.Text = "Administrador";
            this.chbAdmin.UseVisualStyleBackColor = true;
            // 
            // chbActivo
            // 
            this.chbActivo.AutoSize = true;
            this.chbActivo.Location = new System.Drawing.Point(314, 140);
            this.chbActivo.Name = "chbActivo";
            this.chbActivo.Size = new System.Drawing.Size(71, 24);
            this.chbActivo.TabIndex = 19;
            this.chbActivo.Text = "Activo";
            this.chbActivo.UseVisualStyleBackColor = true;
            // 
            // frmModificarUsuario
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(512, 213);
            this.Controls.Add(this.chbActivo);
            this.Controls.Add(this.chbAdmin);
            this.Controls.Add(this.btnModificar);
            this.Controls.Add(this.txtConfirmar);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtContrasenia);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtUsuario);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtNombre);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "frmModificarUsuario";
            this.Text = "ID Usuario: [ID]";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnModificar;
        private System.Windows.Forms.TextBox txtNombre;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtUsuario;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtContrasenia;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtConfirmar;
        private System.Windows.Forms.CheckBox chbAdmin;
        private System.Windows.Forms.CheckBox chbActivo;
    }
}